using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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

        proxyBar.Text = _config.ExternalProxy;
        if(ushort.TryParse(_config.ListenPort, out var port))
            listenPort.Value = port;
        clearCookie.Checked = _config.ClearCookie;
        if(_config.BahaSearchReplaces is not null)
            bahaSearchReplaceBar.Lines = _config.BahaSearchReplaces.ToArray();
        if(_config.BilibiliSearchReplaces is not null)
            bilibiliSearchReplaceBar.Lines = _config.BilibiliSearchReplaces.ToArray();
    }

    private void Form_FormClosing(object sender, FormClosingEventArgs e) {
        if(_canExitApp) return;

        var proxy = proxyBar.Text ?? string.Empty;
        var port = listenPort.Value.ToString();
        var clear = clearCookie.Checked;
        var bahaSearchReplaces = bahaSearchReplaceBar.Lines;
        var bilibiliSearchReplaces = bilibiliSearchReplaceBar.Lines;

        if((_config.ExternalProxy ?? string.Empty) != proxy
            || (_config.ListenPort ?? "5100") != port
            || _config.ClearCookie != clear
            || !(_config.BahaSearchReplaces ?? Array.Empty<string>()).SequenceEqual(bahaSearchReplaces)
            || !(_config.BilibiliSearchReplaces ?? Array.Empty<string>()).SequenceEqual(bilibiliSearchReplaces)
        ) {
            using(var sw = new StreamWriter(Config.ConfigPath)) {
                var serializer = new YamlDotNet.Serialization.Serializer();
                serializer.Serialize(sw, new Config {
                    ListenPort = port,
                    ClearCookie = clear,
                    ExternalProxy = proxy,
                    BahaSearchReplaces = bahaSearchReplaces,
                    BilibiliSearchReplaces = bilibiliSearchReplaces,
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
