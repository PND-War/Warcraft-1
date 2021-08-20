using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace Warcraft_1.Scenes
{
    enum Scenes
    {
        mainmenu,
        startgame,
        loadgame,
        settings,
        quitwindow
    }
    abstract class AScene
    {
        protected SpriteBatch _spriteBatch;
        public List<Texture2D> components = new List<Texture2D>() { };
        public abstract void Load(GraphicsDeviceManager graphics, ContentManager Content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GraphicsDeviceManager graphics, GameTime gameTime);
    }
}
