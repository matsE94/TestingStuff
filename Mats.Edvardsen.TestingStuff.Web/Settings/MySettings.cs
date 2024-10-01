namespace Mats.Edvardsen.TestingStuff.Web.Settings;

public class MySettings
{
    public const string Key = "MySettings";

    public string Greeting { get; set; } = string.Empty;
    public int Number { get; set; }
}