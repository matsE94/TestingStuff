using Mats.Edvardsen.TestingStuff.Data;
using Mats.Edvardsen.TestingStuff.Data.Settings;

namespace Mats.Edvardsen.TestingStuff.Web.Settings;

public class ApplicationSettings
{
    public MySettings MySettings { get; set; }
    public DatabaseSettings DatabaseSettings { get; set; }
}