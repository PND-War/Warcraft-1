using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Warcraft_1.Scenes
{
    enum TextureSQuit
    {
        back,
        cur,
        sure,
        yes,
        no
    }
    class SQuit : AScene
    {
        public bool yesAimed = false;
        public bool noAimed = false;

        public override void Load(GraphicsDeviceManager graphics, ContentManager Content)
        {
            click = Content.Load<SoundEffect>("button");

            Texture2D cursor = Content.Load<Texture2D>("cursor");
            Texture2D background = Content.Load<Texture2D>("background");

            Texture2D sure = Content.Load<Texture2D>("sure");
            Texture2D yes = Content.Load<Texture2D>("yes");
            Texture2D no = Content.Load<Texture2D>("no");

            components.AddRange(new Texture2D[] { background, cursor, sure, yes, no });
            SoundAdjust();
        }

        private void SoundAdjust()
        {
            soundInstance = click.CreateInstance();
            soundInstance.Volume = Logic_Classes.Settings.SFXVol ? 0.35f : 0.0f;
        }
        public override Scenes Update(GameTime gameTime)
        {
            CheckAim();

            return CheckPress();
        }

        private Scenes CheckPress()
        {
            if (Logic_Classes.MouseInterpretator.GetPressed(Logic_Classes.MouseButton.Left))
            {
                if (new Rectangle(new Point(962, 520), new Point(components[(int)TextureSQuit.no].Width, components[(int)TextureSQuit.no].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    soundInstance.Play();
                    return Scenes.mainmenu;
                }
                if (new Rectangle(new Point(863, 520), new Point(components[(int)TextureSQuit.yes].Width, components[(int)TextureSQuit.yes].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y)) { Environment.Exit(0); }
            }
            return Scenes.nullscene;
        }

        private void CheckAim()
        {
            if (new Rectangle(new Point(863, 520), new Point(components[(int)TextureSQuit.no].Width, components[(int)TextureSQuit.no].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y)) yesAimed = true;
            else yesAimed = false;

            if (new Rectangle(new Point(962, 520), new Point(components[(int)TextureSQuit.yes].Width, components[(int)TextureSQuit.yes].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y)) noAimed = true;
            else noAimed = false;
        }

        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            SpriteBatch _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            _spriteBatch.Begin();

            _spriteBatch.Draw(components[(int)TextureSQuit.back], new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.Draw(components[(int)TextureSQuit.sure], new Vector2(831, 459), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSQuit.yes], new Vector2(863, 520), null, Color.White, 0f, Vector2.Zero, yesAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSQuit.no], new Vector2(962, 520), null, Color.White, 0f, Vector2.Zero, noAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSQuit.cur], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

            _spriteBatch.End();
        }
    }
}