using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;

namespace BahaHcaptcha {
    public partial class HcaptchaForm: Form {
        private const string _bahaHost = "https://ani.gamer.com.tw";

        private string _cookie;
        private string _useragent;
        private bool _canExitApp;
        private bool _isHcaptcha = true;

        private readonly HttpListener _httpListener = new();
        private readonly IServiceProvider _provider;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public HcaptchaForm(IServiceProvider provider, IConfiguration configuration, IHttpClientFactory httpClientFactory) {
            InitializeComponent();

            _provider = provider;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;

            Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            notifyIcon.Icon = Icon;
            webView.NavigationStarting += WebView_NavigationStarting;
            webView.NavigationCompleted += WebView_NavigationCompleted;
            if(_configuration[ConfigForm.Keys.Listen] is not { Length: > 0 } port) port = "5100";
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
                    var context = await _httpListener.GetContextAsync();
                    try {
                        var request = context.Request;
                        using var response = context.Response;
                        if(request.HttpMethod is "GET" or "POST") {
                            using var message = new HttpRequestMessage {
                                RequestUri = new Uri($"{_bahaHost}{request.RawUrl}"),
                                Headers = {
                                    { "User-Agent", _useragent },
                                    { "Accept", "*/*" },
                                    { "Cookie", _cookie },
                                },
                            };

                            if(request.HttpMethod is "GET") {
                                message.Method = HttpMethod.Get;
                            } else {
                                message.Method = HttpMethod.Post;
                                var data = new StringBuilder();
                                using var input = new StreamReader(request.InputStream);
                                while(await input.ReadLineAsync() is { } line) {
                                    data.AppendLine(line);
                                }
                                message.Content = new StringContent(data.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded");
                            }
                            using var client = _httpClientFactory.CreateClient("baha");
                            using var resp = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);
                            if(resp.IsSuccessStatusCode) {
                                foreach(var v in resp.Headers) {
                                    response.AddHeader(v.Key, string.Join(";", v.Value));
                                }
                                foreach(var v in resp.Content.Headers) {
                                    response.AddHeader(v.Key, string.Join(";", v.Value));
                                }
                                await resp.Content.CopyToAsync(response.OutputStream);
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
            });
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
            if(_configuration[ConfigForm.Keys.Proxy] is { Length: > 0 } proxy) {
                options.AdditionalBrowserArguments = $"--proxy-server={proxy}";
            }
            var env = await CoreWebView2Environment.CreateAsync(null, null, options);
            await webView.EnsureCoreWebView2Async(env);

            webView.CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
            webView.CoreWebView2.WebResourceResponseReceived += CoreWebView2_WebResourceResponseReceived;
            _useragent = webView.CoreWebView2.Settings.UserAgent;

            webView.CoreWebView2.Navigate($"{_bahaHost}/");
        }

        private void CoreWebView2_DocumentTitleChanged(object sender, object e) {
            Text = webView.CoreWebView2.DocumentTitle;
        }

        private void CoreWebView2_WebResourceResponseReceived(object sender, CoreWebView2WebResourceResponseReceivedEventArgs e) {
            if(GetValue("cookie", "cf_clearance") is { Length: > 0 } cookie) {
                _cookie = cookie;
                Debug.WriteLine($"Request Clearance Cookie: {_cookie}");
            }

            if(GetValue("set-cookie", "cf_clearance") is { Length: > 0 } setcookie) {
                _cookie = setcookie;
                Debug.WriteLine($"Response Clearance Cookie: {_cookie}");
            }

            string GetValue(string headerName, string key) {
                foreach(var header in e.Response.Headers) {
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
    }
}