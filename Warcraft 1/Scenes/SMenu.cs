using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Warcraft_1.Scenes
{
    class SMenu : AScene
    {
        public override void Load(GraphicsDeviceManager graphics, ContentManager Content)
        {
            _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            Texture2D cursor = Content.Load<Texture2D>("cursor");
            Texture2D background = Content.Load<Texture2D>("background");

            Texture2D start = Content.Load<Texture2D>("start");
            Texture2D load = Content.Load<Texture2D>("load");
            Texture2D sett = Content.Load<Texture2D>("settingsbut");
            Texture2D exit = Content.Load<Texture2D>("exit");

            components.AddRange(new Texture2D[]{cursor, background});
        }
        public override void Update(GameTime gameTime)
        {
         
        }

        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            SpriteBatch _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            _spriteBatch.Begin();
            _spriteBatch.Draw(components[1], new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.Draw(components[0], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);
            _spriteBatch.End();

            //_spriteBatch.Draw(start, new Vector2(831, 392), null, Color.White, 0f, Vector2.Zero, startAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);
            //_spriteBatch.Draw(load, new Vector2(831, 455), null, Color.White, 0f, Vector2.Zero, loadAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);
            //_spriteBatch.Draw(sett, new Vector2(831, 518), null, Color.White, 0f, Vector2.Zero, settingsAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);
            //_spriteBatch.Draw(exit, new Vector2(831, 626), null, Color.White, 0f, Vector2.Zero, exitAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);

            

        }
    }
}
