using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Warcraft_1.GameClasses;

namespace Warcraft_1.Scenes
{
    class SPlaying : AScene
    {
        Map map = new Map();

        enum TextureSPlay
        {
            cur,
            Interface,
            Pixel
        }
        public override void Load(GraphicsDeviceManager graphics, ContentManager Content)
        {
            click = Content.Load<SoundEffect>("Sounds/button");
            //map.Save("map.wc");
            map.Read();
            Texture2D cursor = Content.Load<Texture2D>("Textures/UI/cursor");
            Texture2D Interface = Content.Load<Texture2D>("Textures/UI/Interface");
            Texture2D Pixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            Pixel.SetData<Color>(new Color[1] { Color.White });

            components.AddRange(new Texture2D[] { cursor, Interface, Pixel });
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
            //map.Read();
            SpriteBatch _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            _spriteBatch.Begin();

            _spriteBatch.Draw(components[(int)TextureSPlay.Interface], new Rectangle(0, 0, 1920, 1080), Color.White);


            map.DrawMini(_spriteBatch, components[(int)TextureSPlay.Pixel]);//minimap

            _spriteBatch.Draw(components[(int)TextureSPlay.cur], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

            _spriteBatch.End();
        }
    }
}
