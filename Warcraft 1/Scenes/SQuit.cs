using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

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
        public bool tapped = false;

        public SoundEffect click;
        public Song bgsong;
        public SoundEffectInstance soundInstance;

        public bool yesAimed = false;
        public bool noAimed = false;

        public override void Load(GraphicsDeviceManager graphics, ContentManager Content)
        {
            _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            Texture2D cursor = Content.Load<Texture2D>("cursor");
            Texture2D background = Content.Load<Texture2D>("background");

            Texture2D sure = Content.Load<Texture2D>("sure");
            Texture2D yes = Content.Load<Texture2D>("yes");
            Texture2D no = Content.Load<Texture2D>("no");

            components.AddRange(new Texture2D[] { background, cursor, sure, yes, no });

            click = Content.Load<SoundEffect>("button");
            soundInstance = click.CreateInstance();
            soundInstance.Volume = 0.35f;

            bgsong = Content.Load<Song>("backgroundmusic");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.05f;

            MediaPlayer.Play(bgsong);
        }
        public override void Update(GameTime gameTime)
        {
            CheckPress();
            CheckAim();
        }

        private int CheckPress()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (!tapped)
                {
                    if (new Rectangle(new Point(962, 520), new Point(components[(int)TextureSQuit.no].Width, components[(int)TextureSQuit.no].Height)).Contains(mousePos.X, mousePos.Y)) {
                        soundInstance.Play();
                        return (int)Scenes.mainmenu;
                    }
                    if (new Rectangle(new Point(863, 520), new Point(components[(int)TextureSQuit.yes].Width, components[(int)TextureSQuit.yes].Height)).Contains(mousePos.X, mousePos.Y)) Exit();
                }
                tapped = true;
            }
            else tapped = false;

            return 0;
        }

        private void CheckAim()
        {
            if (new Rectangle(new Point(863, 520), new Point(components[1].Width, components[1].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y)) yesAimed = true;
            else yesAimed = false;

            if (new Rectangle(new Point(962, 520), new Point(components[2].Width, components[2].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y)) noAimed = true;
            else noAimed = false;
        }

        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            SpriteBatch _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            _spriteBatch.Begin();

            _spriteBatch.Draw(components[(int)TextureSQuit.back], new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.Draw(components[(int)TextureSQuit.sure], new Vector2(831, 392), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSQuit.yes], new Vector2(831, 455), null, Color.White, 0f, Vector2.Zero, yesAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSQuit.no], new Vector2(831, 518), null, Color.White, 0f, Vector2.Zero, noAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSQuit.cur], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

            _spriteBatch.End();
        }
    }
}
