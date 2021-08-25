using Newtonsoft.Json;

namespace Warcraft_1.Logic_Classes
{
    public static class Settings
    {
        [JsonProperty ("music")]
        public static bool MusicVol { get; set; } = true;
        [JsonProperty ("sound")]
        public static bool SFXVol { get; set; } = true;

    }
}
