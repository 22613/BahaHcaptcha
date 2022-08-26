using System;
using System.IO;
using System.Windows.Forms;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BahaHcaptcha;

public partial class ConfigForm: Form {
    private bool _canExitApp;

    private readonly IServiceProvider _provider;
    private readonly Config _config;

    public ConfigForm(IServiceProvider provider, Config config) {
        InitializeComponent();

        _config = config;
        _provider = provider;

        proxyBar.Text = config.ExternalProxy;
        if(ushort.TryParse(config.ListenPort, out var port))
            listenPort.Value = port;
        clearCookie.Checked = config.ClearCookie;
    }

    private void Form_FormClosing(object sender, FormClosingEventArgs e) {
        if(_canExitApp) return;

        var proxy = proxyBar.Text ?? string.Empty;
        //var old_proxy = _configuration[Keys.Proxy] ?? string.Empty;

        var port = listenPort.Value.ToString();
        //if(!ushort.TryParse(_configuration[Keys.Listen], out var old_port))
        //    old_port = 5100;

        var clear = clearCookie.Checked;

        if(_config.ExternalProxy != proxy
            || _config.ListenPort != port
            || _config.ClearCookie != clear
        ) {
            using(var sw = new StreamWriter(Config.ConfigPath)) {
                var serializer = new YamlDotNet.Serialization.Serializer();
                serializer.Serialize(sw, new Config {
                    ListenPort = port,
                    ClearCookie = clear,
                    ExternalProxy = proxy,
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
