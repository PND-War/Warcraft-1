using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Warcraft_1.Scenes
{
    enum TextureSMenu
    {
        back,
        cur,
        start,
        load,
        sett,
        exit
    }

    class SMenu : AScene
    {
        public bool tapped = false;

        public SoundEffect click;
        public Song bgsong;
        public SoundEffectInstance soundInstance;

        public bool startAimed = false;
        public bool loadAimed = false;
        public bool settingsAimed = false;
        public bool exitAimed = false;

        public override void Load(GraphicsDeviceManager graphics, ContentManager Content)
        {
            _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            Texture2D cursor = Content.Load<Texture2D>("cursor");
            Texture2D background = Content.Load<Texture2D>("background");

            Texture2D start = Content.Load<Texture2D>("start");
            Texture2D load = Content.Load<Texture2D>("load");
            Texture2D sett = Content.Load<Texture2D>("settingsbut");
            Texture2D exit = Content.Load<Texture2D>("exit");

            components.AddRange(new Texture2D[] { background, cursor, start, load, sett, exit });

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
                    if (new Rectangle(831, 392, 261, 64).Contains(Mouse.GetState().X, Mouse.GetState().Y)) {
                        soundInstance.Play();
                        return (int)Scenes.startgame;       //currentState
                    }
                    if (new Rectangle(831, 455, 261, 64).Contains(Mouse.GetState().X, Mouse.GetState().Y)) {
                        soundInstance.Play();
                        return (int)Scenes.loadgame;
                    }
                    if (new Rectangle(831, 518, 261, 64).Contains(Mouse.GetState().X, Mouse.GetState().Y)) {
                        soundInstance.Play();
                        return (int)Scenes.settings;
                    }
                    if (new Rectangle(831, 626, 261, 64).Contains(Mouse.GetState().X, Mouse.GetState().Y)) {
                        soundInstance.Play();
                        return (int)Scenes.quitwindow;
                    }
                }
                tapped = true;
            } else tapped = false;

            return 0;
        }
        private void CheckAim()
        {
            if (new Rectangle(831, 392, 261, 64).Contains(Mouse.GetState().X, Mouse.GetState().Y)) startAimed = true;
            else startAimed = false;

            if (new Rectangle(831, 455, 261, 64).Contains(Mouse.GetState().X, Mouse.GetState().Y)) loadAimed = true;
            else loadAimed = false;

            if (new Rectangle(831, 518, 261, 64).Contains(Mouse.GetState().X, Mouse.GetState().Y)) settingsAimed = true;
            else settingsAimed = false;

            if (new Rectangle(831, 626, 261, 64).Contains(Mouse.GetState().X, Mouse.GetState().Y)) exitAimed = true;
            else exitAimed = false;
        }

        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            SpriteBatch _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            _spriteBatch.Begin();

            _spriteBatch.Draw(components[(int)TextureSMenu.back], new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.Draw(components[(int)TextureSMenu.start], new Vector2(831, 392), null, Color.White, 0f, Vector2.Zero, startAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSMenu.load], new Vector2(831, 455), null, Color.White, 0f, Vector2.Zero, loadAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSMenu.sett], new Vector2(831, 518), null, Color.White, 0f, Vector2.Zero, settingsAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSMenu.exit], new Vector2(831, 626), null, Color.White, 0f, Vector2.Zero, exitAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSMenu.cur], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

            _spriteBatch.End();
        }
    }
}
