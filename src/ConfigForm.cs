using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BahaHcaptcha {
    public partial class ConfigForm: Form {
        public const string ConfigPath = "appsettings.yaml";
        public static class Keys {
            public const string Proxy = "proxy";
            public const string Listen = "listen";
        }

        private bool _canExitApp;

        private readonly IServiceProvider _provider;
        private readonly IConfiguration _configuration;

        public ConfigForm(IServiceProvider provider, IConfiguration configuration) {
            InitializeComponent();
            _provider = provider;
            _configuration = configuration;
            proxyBar.Text = _configuration[Keys.Proxy];

            if(ushort.TryParse(_configuration[Keys.Listen], out var port))
                listenPort.Value = port;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e) {
            if(_canExitApp) return;

            var proxy = proxyBar.Text ?? string.Empty;
            var old_proxy = _configuration[Keys.Proxy] ?? string.Empty;

            var port = listenPort.Value;
            if(!ushort.TryParse(_configuration[Keys.Listen], out var old_port))
                old_port = 5100;

            if(old_proxy != proxy || old_port != port) {
                using(var sw = new StreamWriter(ConfigPath)) {
                    var serializer = new YamlDotNet.Serialization.Serializer();
                    serializer.Serialize(sw, new {
                        listen = port,
                        proxy
                    });
                }

                if(MessageBox.Show(this, "修改设置后需要重启程序才能生效!!\n\t要立即重启程序吗?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                    _canExitApp = true;
                    var hcaptcha = _provider.GetService<HcaptchaForm>();
                    hcaptcha.BeginInvoke(hcaptcha.Restart);
                }
            }
        }
    }
}
