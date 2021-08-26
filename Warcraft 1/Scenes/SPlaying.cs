using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraft_1.Scenes
{
    class SPlaying : AScene
    {
        enum TextureSPlay
        {
            cur,
            Interface
        }

        public override void Load(GraphicsDeviceManager graphics, ContentManager Content)
        {
            click = Content.Load<SoundEffect>("button");

            Texture2D cursor = Content.Load<Texture2D>("cursor");

            Texture2D Interface = Content.Load<Texture2D>("Interface");

            components.AddRange(new Texture2D[] { cursor, Interface });
            SoundAdjust();
        }
        private void SoundAdjust()
        {
            soundInstance = click.CreateInstance();
            soundInstance.Volume = Logic_Classes.Settings.SFXVol ? 0.35f : 0.0f;
        }
        public override Scenes Update(GameTime gameTime)
        {
            return Scenes.nullscene;
        }

        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            SpriteBatch _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            _spriteBatch.Begin();

            _spriteBatch.Draw(components[(int)TextureSPlay.Interface], new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.Draw(components[(int)TextureSPlay.cur], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

            _spriteBatch.End();
        }
    }
}
