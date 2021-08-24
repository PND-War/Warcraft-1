using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Warcraft_1.Scenes
{
    enum SettingStates
    {
        SFX,
        SOUND
    }
    enum TextureSSettings
    {
        backgr,
        cur,
        setticon,
        ok,
        back,
        soundturn,
        musicturn,
        noicon,
        yesicon
    }
    class SSettings : AScene
    {
        public bool musicswitcher = false;
        public bool soundswitcher = false;

        public bool okAimed = false;
        public bool backAimed = false;


        Texture2D no;
        Texture2D yes;

        public SoundEffect click;
        public SoundEffectInstance soundInstance;

        public override void Load(GraphicsDeviceManager graphics, ContentManager Content)
        {
            no = Content.Load<Texture2D>("noicon");
            yes = Content.Load<Texture2D>("yesicon");

            Texture2D cursor = Content.Load<Texture2D>("cursor");
            Texture2D background = Content.Load<Texture2D>("background");

            Texture2D settingsicon = Content.Load<Texture2D>("settings");
            Texture2D ok = Content.Load<Texture2D>("ok");
            Texture2D back = Content.Load<Texture2D>("back");

            Texture2D soundturn = Logic_Classes.Settings.MusicVol ? yes : no;
            Texture2D musicturn = Logic_Classes.Settings.SFXVol ? yes : no;

            components.AddRange(new Texture2D[] { background, cursor, settingsicon, ok, back, soundturn, musicturn });

            click = Content.Load<SoundEffect>("button");
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
                    if (new Rectangle(new Point(863, 581), new Point(components[(int)TextureSSettings.ok].Width, components[(int)TextureSSettings.ok].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        soundInstance.Play();
                        return Scenes.mainmenu;
                    }
                    if (new Rectangle(new Point(962, 581), new Point(components[(int)TextureSSettings.back].Width, components[(int)TextureSSettings.back].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        soundInstance.Play();
                        return Scenes.mainmenu;
                    }
                    if (new Rectangle(new Point(1041, 502), new Point(components[(int)TextureSSettings.musicturn].Width, components[(int)TextureSSettings.musicturn].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        ChangeSett(SettingStates.SOUND);
                    }
                    if (new Rectangle(new Point(1041, 530), new Point(components[(int)TextureSSettings.soundturn].Width, components[(int)TextureSSettings.soundturn].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        ChangeSett(SettingStates.SFX);
                    }
            }
            return Scenes.nullscene;

        }
        private void CheckAim()
        {
            if (new Rectangle(new Point(863, 581), new Point(components[(int)TextureSSettings.ok].Width, components[(int)TextureSSettings.ok].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y)) okAimed = true;
            else okAimed = false;

            if (new Rectangle(new Point(962, 581), new Point(components[(int)TextureSSettings.back].Width, components[(int)TextureSSettings.back].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y)) backAimed = true;
            else backAimed = false;

            if (new Rectangle(new Point(1041, 502), new Point(components[(int)TextureSSettings.musicturn].Width, components[(int)TextureSSettings.musicturn].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y)) musicswitcher = true;
            else musicswitcher = false;

            if (new Rectangle(new Point(1041, 530), new Point(components[(int)TextureSSettings.soundturn].Width, components[(int)TextureSSettings.soundturn].Height)).Contains(Mouse.GetState().X, Mouse.GetState().Y)) soundswitcher = true;
            else soundswitcher = false;
        }

        private void ChangeSett(SettingStates state)
        {
            Logic_Classes.Settings.MusicVol = state == SettingStates.SOUND ? !Logic_Classes.Settings.MusicVol : Logic_Classes.Settings.MusicVol;
            Logic_Classes.Settings.SFXVol = state == SettingStates.SFX ? !Logic_Classes.Settings.SFXVol : Logic_Classes.Settings.SFXVol;

            components[(int)TextureSSettings.soundturn] = state == SettingStates.SOUND ? components[(int)TextureSSettings.soundturn] == yes ? no : yes : components[(int)TextureSSettings.soundturn];
            components[(int)TextureSSettings.musicturn] = state == SettingStates.SFX ? components[(int)TextureSSettings.musicturn] == yes ? no : yes : components[(int)TextureSSettings.musicturn];

            MediaPlayer.Volume = Logic_Classes.Settings.MusicVol ? 0.05f : 0.0f;
            soundInstance.Volume = Logic_Classes.Settings.SFXVol ? 0.35f : 0.0f;
        }

        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            SpriteBatch _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            _spriteBatch.Begin();

            _spriteBatch.Draw(components[(int)TextureSSettings.backgr], new Rectangle(0, 0, 1920, 1080), Color.White);
            _spriteBatch.Draw(components[(int)TextureSSettings.setticon], new Vector2(831, 434), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSSettings.ok], new Vector2(863, 581), null, Color.White, 0f, Vector2.Zero, okAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSSettings.back], new Vector2(962, 581), null, Color.White, 0f, Vector2.Zero, backAimed ? 1.007f : 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSSettings.soundturn], new Vector2(1041, 502), null, Color.White, 0f, Vector2.Zero, musicswitcher ? 1.027f : 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSSettings.musicturn], new Vector2(1041, 530), null, Color.White, 0f, Vector2.Zero, soundswitcher ? 1.027f : 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(components[(int)TextureSSettings.cur], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

            _spriteBatch.End();
        }

    }
}
