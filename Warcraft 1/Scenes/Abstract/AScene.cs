using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace Warcraft_1.Scenes
{
    abstract class AScene
    {
        protected SoundEffect click;
        protected SoundEffectInstance soundInstance;

        public List<Texture2D> components = new List<Texture2D>() { };
        public abstract void Load(GraphicsDeviceManager graphics, ContentManager Content);
        public abstract Scenes Update(GameTime gameTime);
        public abstract void Draw(GraphicsDeviceManager graphics, GameTime gameTime);
    }
}
