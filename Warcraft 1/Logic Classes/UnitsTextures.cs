using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Warcraft_1.Logic_Classes
{
    public static class UnitsTextures
    {
        public static bool IsLoaded = false;
        public static Texture2D Worker;
        public static Texture2D Icons;
        public static void Load(ContentManager Content)
        {
            IsLoaded = true;
            Worker = Content.Load<Texture2D>("Textures/Game/Worker");
            Icons = Content.Load<Texture2D>("Textures/Game/icons");
        }
    }
}
