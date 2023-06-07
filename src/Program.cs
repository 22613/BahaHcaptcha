using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BahaHcaptcha;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        if(!HttpListener.IsSupported)
        {
            MessageBox.Show("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
            return;
        }

        var mutexName = Application.ExecutablePath.Replace('\\', '_');
        var mutex = new Mutex(true, mutexName, out var createdNew);
        if(args.Contains("--restart"))
        {
            while(!createdNew)
            {
                mutex.Dispose();
                Task.Delay(500).Wait();
                mutex = new Mutex(true, mutexName, out createdNew);
            }
        }
        else if(!createdNew)
        {
            mutex.Dispose();
            MessageBox.Show("程序已经启动了");
            return;
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var host = Host.CreateDefaultBuilder(
                args
            ).ConfigureAppConfiguration(builder =>
            {
                builder.AddYamlFile(Config.ConfigPath, true);
            }).ConfigureServices(services =>
            {
                services.AddHttpClient("baha")
                    .SetHandlerLifetime(TimeSpan.FromMinutes(10))
                    .ConfigurePrimaryHttpMessageHandler(provider =>
                    {
                        var result = new HttpClientHandler
                        {
                            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                            UseCookies = false,
                        };
                        var config = provider.GetRequiredService<Config>();
                        if(config.ExternalProxy is { Length: > 0 } proxy)
                        {
                            result.Proxy = new WebProxy(proxy)
                            {
                                BypassProxyOnLocal = false
                            };
                        }
                        else
                            result.Proxy = null;
                        return result;
                    });
                services.AddSingleton<HcaptchaForm>();
                services.AddSingleton<ConfigForm>();
                services.AddSingleton(provider =>
                {
                    Config config = new();
                    provider.GetRequiredService<IConfiguration>().Bind(config);
                    return config;
                });
            }).Build();
        host.StartAsync().Wait();

        var mainForm = host.Services.GetRequiredService<HcaptchaForm>();
        Application.Run(mainForm);
        host.StopAsync().Wait();
        mutex.ReleaseMutex();
    }
}