using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;

namespace BahaHcaptcha {
    public partial class HcaptchaForm: Form {
        private const string _bahaHost = "https://ani.gamer.com.tw";
        private const string _bahaSearchUrl = "/search.php?kw=";

        private const string _dandanCasHost = "https://cas.dandanplay.net";
        private const string _biliSearchUrl = "/api/bilibili/search?keyword=";

        private string _cookie;
        private string _useragent;
        private bool _canExitApp;
        private bool _isHcaptcha = true;

        private readonly HttpListener _httpListener = new();
        private readonly IServiceProvider _provider;
        private readonly Config _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public HcaptchaForm(IServiceProvider provider, IHttpClientFactory httpClientFactory, Config config) {
            InitializeComponent();

            _config = config;
            _provider = provider;
            _httpClientFactory = httpClientFactory;

            Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.Icon = Icon;
            webView.NavigationStarting += WebView_NavigationStarting;
            webView.NavigationCompleted += WebView_NavigationCompleted;

            if(config.ListenPort is not { Length: > 0 } port) port = "5100";
            InitializeHttpListener($"http://localhost:{port}/");
            InitializeWebView2Async();
        }

        private Task InitializeHttpListener(string prefix) {
            _httpListener.Prefixes.Add(prefix);
            try {
                _httpListener.Start();
            } catch(Exception) {
            }
            return Task.Run(async () => {
                while(true) {
                    var context = await _httpListener.GetContextAsync().ConfigureAwait(false);
                    _ = ContextHandler(context).ConfigureAwait(false);
                }
            });

            static string Replace(string value, IEnumerable<string> replaces) {
                if(replaces is null || !replaces.Any()) return value;
                var sb = new StringBuilder(value);
                foreach(var r in replaces) {
                    var i = r.IndexOf('=');
                    if(i > 0) {
                        sb.Replace(r.Substring(0, i), r.Substring(i + 1));
                    }
                }
                return sb.ToString();
            }

            async Task ContextHandler(HttpListenerContext context) {
                try {
                    var request = context.Request;
                    using var response = context.Response;
                    if(request.HttpMethod is "GET" or "POST") {
                        using var message = new HttpRequestMessage {
                            Headers = {
                                { "User-Agent", _useragent },
                                { "Accept", "*/*" },
                                { "Cookie", _cookie },
                            },
                        };

                        if(request.HttpMethod is "GET") {
                            message.Method = HttpMethod.Get;
                            if(request.RawUrl.StartsWith(_bahaSearchUrl)) {
                                message.RequestUri = new Uri($"{_bahaHost}{_bahaSearchUrl}{Replace(request.QueryString["kw"], _config.BahaSearchReplaces)}");
                            } else if(request.RawUrl.StartsWith(_biliSearchUrl)) {
                                message.RequestUri = new Uri($"{_dandanCasHost}{_biliSearchUrl}{Replace(request.QueryString["keyword"], _config.BilibiliSearchReplaces)}");
                            } else {
                                message.RequestUri = new Uri($"{_bahaHost}{request.RawUrl}");
                            }
                        } else {
                            message.RequestUri = new Uri($"{_bahaHost}{request.RawUrl}");
                            message.Method = HttpMethod.Post;
                            var data = new StringBuilder();
                            using var input = new StreamReader(request.InputStream);
                            while(await input.ReadLineAsync().ConfigureAwait(false) is { } line) {
                                data.AppendLine(line);
                            }
                            message.Content = new StringContent(data.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
                        }
                        using var client = _httpClientFactory.CreateClient("baha");
                        using var resp = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                        if(resp.IsSuccessStatusCode) {
                            response.StatusCode = (int)resp.StatusCode;
                            foreach(var v in resp.Headers) {
                                response.AddHeader(v.Key, string.Join(";", v.Value));
                            }
                            foreach(var v in resp.Content.Headers) {
                                response.AddHeader(v.Key, string.Join(";", v.Value));
                            }
                            await resp.Content.CopyToAsync(response.OutputStream).ConfigureAwait(false);
                        } else if(resp.StatusCode is HttpStatusCode.Redirect) {
                            response.Redirect(resp.Headers.Location.AbsoluteUri);
                        } else {
                            Invoke(() => {
                                _isHcaptcha = true;
                                webView.CoreWebView2.Navigate(_bahaHost);
                                ShowActivate();
                            });
                            response.StatusCode = (int)resp.StatusCode;
                            response.Close(Array.Empty<byte>(), true);
                        }
                    } else {
                        response.StatusCode = 400;
                        response.Close(Array.Empty<byte>(), true);
                    }
                } catch(Exception) {

                }
            }
        }

        private void WebView_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e) {
            addressBar.Text = e.Uri;
        }

        private void WebView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e) {
            if(!e.IsSuccess) {
                var config = _provider.GetService<ConfigForm>();
                if(!config.Visible)
                    ShowMenuItem_Click(sender, e);
            } else if(_isHcaptcha) {
                HideToTray();
            }
        }

        private async void InitializeWebView2Async() {
            var options = new CoreWebView2EnvironmentOptions();
            if(_config.ExternalProxy is { Length: > 0 } proxy) {
                options.AdditionalBrowserArguments = $"--proxy-server={proxy}";
            }
            var env = await CoreWebView2Environment.CreateAsync(null, null, options).ConfigureAwait(false);
            await webView.EnsureCoreWebView2Async(env).ConfigureAwait(true);

            webView.CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
            webView.CoreWebView2.WebResourceResponseReceived += CoreWebView2_WebResourceResponseReceived;
            _useragent = webView.CoreWebView2.Settings.UserAgent;

            var cookies = await webView.CoreWebView2.CookieManager.GetCookiesAsync(_bahaHost).ConfigureAwait(true);
            foreach(var cookie in cookies) {
                if(cookie.Name == "cf_clearance") {
                    var match = Regex.Match(cookie.Value, @"-(\d{10})-\d-");
                    if(match.Success) {
                        if(long.TryParse(match.Groups[1].Value, out var seconds)) {
                            var dto = DateTimeOffset.FromUnixTimeSeconds(seconds).ToLocalTime();
                            if(DateTimeOffset.Now - dto > TimeSpan.FromHours(12)) {
                                webView.CoreWebView2.CookieManager.DeleteCookie(cookie);
                            }
                        }
                    }
                }
            }
            webView.CoreWebView2.Navigate($"{_bahaHost}/");
        }

        private void CoreWebView2_DocumentTitleChanged(object sender, object e) {
            Text = webView.CoreWebView2.DocumentTitle;
        }

        private void CoreWebView2_WebResourceResponseReceived(object sender, CoreWebView2WebResourceResponseReceivedEventArgs e) {
            if(GetValue(e.Request.Headers, "cookie", "cf_clearance") is { Length: > 0 } cookie) {
                _cookie = cookie;
                Debug.WriteLine($"cookie: {_cookie}");
            }

            if(GetValue(e.Response.Headers, "set-cookie", "cf_clearance") is { Length: > 0 } setcookie) {
                _cookie = setcookie;
                Debug.WriteLine($"set-cookie: {_cookie}");
            }
        }

        private static string GetValue(IEnumerable<KeyValuePair<string, string>> headers, string headerName, string key) {
            foreach(var header in headers) {
                if(header.Key == headerName) {
                    foreach(var value in header.Value.Split(';')) {
                        var separator = value.IndexOf('=');
                        if(separator > 0) {
                            if(value.Remove(separator).Trim() == key) {
                                return value.Trim();
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e) {
            if(_canExitApp) return;
            e.Cancel = true;
            HideToTray();
        }

        private void HideToTray() {
            Hide();
            _isHcaptcha = false;
            notifyIcon.ShowBalloonTip(2000, "提示", "程序已隐藏到托盘", ToolTipIcon.Info);
        }

        private void ShowMenuItem_Click(object sender, EventArgs e) {
            ShowActivate();
        }

        private void ShowActivate() {
            Show();
            Activate();
            BringToFront();
        }

        private void ConfigMenuItem_Click(object sender, EventArgs e) {
            var _configForm = _provider.GetService<ConfigForm>();
            if(Visible) {
                HideToTray();
            }
            if(!_configForm.Visible) {
                _configForm.ShowDialog();
            } else {
                _configForm.Activate();
                _configForm.BringToFront();
            }
        }
        private void ExitMenuItem_Click(object sender, EventArgs e) {
            _canExitApp = true;
            Close();
        }

        private bool _state_textBox1_selectAll = false;
        private void AddressBar_Enter(object sender, EventArgs e) {
            _state_textBox1_selectAll = true;
        }

        private void AddressBar_MouseUp(object sender, MouseEventArgs e) {
            if(_state_textBox1_selectAll) {
                _state_textBox1_selectAll = false;
                addressBar.SelectAll();
            }
        }

        public void Restart() {
            _canExitApp = true;
            Process.Start(Application.ExecutablePath, "--restart");
            Application.Exit();
        }

        private void HcaptchaForm_SizeChanged(object sender, EventArgs e) {
            var workHeight = Screen.PrimaryScreen.WorkingArea.Height;
            if(workHeight < Height) {
                Top = 0;
                Height = workHeight;
            }
        }
    }
}