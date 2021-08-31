using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraft_1.Logic_Classes
{
    public static class UnitsTextures
    {
        public static bool IsLoaded = false;
        public static Texture2D Worker;
        public static void Load(ContentManager Content)
        {
            IsLoaded = true;
            Worker = Content.Load<Texture2D>("Textures/Game/Worker");
        }
    }
}
