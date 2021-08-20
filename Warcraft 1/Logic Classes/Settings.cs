namespace Warcraft_1.Logic_Classes
{
    public class Settings
    {
        public bool MusicVol { get; set; }
        public bool SFXVol { get; set; }
        Settings()
        {
            MusicVol = true;
            SFXVol = true;
        }
    }
}
