using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Warcraft_1.Logic_Classes
{
    public static class Configuration
    {
        public static void SettingsAdjust()
        {
            dynamic Config = JsonConvert.DeserializeObject(File.ReadAllText("settings.json"));

            string musicx = $"{Config["music"]}";
            Settings.MusicVol = Convert.ToBoolean(musicx);

            string soundsx = $"{Config["sound"]}";
            Settings.SFXVol = Convert.ToBoolean(soundsx);
        }

        public static void JsonDataSetter()
        {
            string outputConfig = "{\"music\":" + Settings.MusicVol.ToString().ToLower() + ",\"sound\": " + Settings.SFXVol.ToString().ToLower() + "}";
            File.WriteAllText("settings.json", outputConfig);
        }
    }
}
