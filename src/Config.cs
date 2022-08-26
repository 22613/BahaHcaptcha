namespace BahaHcaptcha;
public class Config {
    public const string ConfigPath = "appsettings.yaml";

    public string ExternalProxy { get; set; }
    public string ListenPort { get; set; }
    public bool ClearCookie { get; set; }
}
