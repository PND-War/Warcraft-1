using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Warcraft_1.Scenes;
using System;
using System.Threading;
using System.IO;

namespace Warcraft_1
{
    public class Warcraft : Game
    {
        private GraphicsDeviceManager _graphics;
        //private SpriteBatch _spriteBatch;

        public SoundEffect click;
        public Song bgsong;
        public SoundEffectInstance soundInstance;

        public bool tapped = false;

        AScene scene;
        Scenes.Scenes currentState = Scenes.Scenes.mainmenu;

        public Warcraft()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            SoundsAdjust();
            scene = new SMenu();
            Logic_Classes.MouseInterpretator.Start();
        }

        protected override void Initialize()
        {
            scene.Load(_graphics, Content);
            base.Initialize();


            GraphicAdjust();
        }

        private void GraphicAdjust()
        {
            _graphics.PreferredBackBufferWidth = _graphics.GraphicsDevice.DisplayMode.Width;
            _graphics.PreferredBackBufferHeight = _graphics.GraphicsDevice.DisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
        }
        private void SoundsAdjust()
        {
            Logic_Classes.Configuration.SettingsAdjust();
            bgsong = Content.Load<Song>("Sounds/backgroundmusic");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = Logic_Classes.Settings.MusicVol ? 0.05f : 0.0f;

            MediaPlayer.Play(bgsong);
        }

        protected override void LoadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // TODO: Add your update logic here
            Scenes.Scenes outgoingScene = scene.Update(gameTime);

            if (outgoingScene != Scenes.Scenes.nullscene && outgoingScene != currentState)
            {
                currentState = outgoingScene;
                switch (outgoingScene)
                {
                    case Scenes.Scenes.startgame:
                        scene = new SPlaying();
                        if (File.Exists("map.wc"))
                        {
                            (scene as SPlaying).map.Read("map.wc");
                        }
                        break;
                    case Scenes.Scenes.mainmenu:
                        scene = new SMenu();
                        break;
                    case Scenes.Scenes.settings:
                        scene = new SSettings();
                        break;
                    case Scenes.Scenes.quitwindow:
                        scene = new SQuit();
                        break;
                    case Scenes.Scenes.loadgame:
                        scene = new SPlaying();
                        if (File.Exists("save.wc"))
                        {
                            (scene as SPlaying).map.Read("save.wc");
                        }
                        else
                        {
                            if (File.Exists("map.wc"))
                            {
                                (scene as SPlaying).map.Read("map.wc");
                            }
                        }
                        break;
                }
                Thread.Sleep(40);
                scene.Load(_graphics, Content);
                GC.Collect();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            scene.Draw(_graphics, gameTime);

            base.Draw(gameTime);
        }


    }
}