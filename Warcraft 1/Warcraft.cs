using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

using Warcraft_1.Scenes;

namespace Warcraft_1
{
    public class Warcraft : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Settings settings;

        public SoundEffect click;
        public Song bgsong;
        public SoundEffectInstance soundInstance;

        public bool tapped = false;
        int currentState = 1;

        public bool soundsOn = true;
        public bool musicOn = true;

        //1
        public static Texture2D background;
        public Texture2D start;
        public Texture2D load;
        public Texture2D sett;
        public Texture2D exit;

        public bool startAimed = false;
        public bool loadAimed = false;
        public bool settingsAimed = false;
        public bool exitAimed = false;

        //2
        public Texture2D sure;
        public Texture2D yes;
        public Texture2D no;

        public bool yesAimed = false;
        public bool noAimed = false;

        //3
        public Texture2D settingsicon;
        public Texture2D ok;
        public Texture2D back;

        public Texture2D soundturn;
        public Texture2D musicturn;

        public bool musicswitcher = false;
        public bool soundswitcher = false;

        public bool okAimed = false;
        public bool backAimed = false;

        public Texture2D cursor;

        AScene scene;
        public Warcraft()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;


            _graphics.PreferredBackBufferHeight = 1920;
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.IsFullScreen = false;

            scene = new SMenu();
            scene.Load(_graphics, Content);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //_spriteBatch = new SpriteBatch(GraphicsDevice);

            ////for each scene
            //cursor = Content.Load<Texture2D>("cursor");
            //background = Content.Load<Texture2D>("background");
            ////for each scene

            ////main menu scene
            //start = Content.Load<Texture2D>("start");
            //load = Content.Load<Texture2D>("load");
            //sett = Content.Load<Texture2D>("settingsbut");
            //exit = Content.Load<Texture2D>("exit");
            ////main menu scene

            ////exiting scene
            //sure = Content.Load<Texture2D>("sure");
            //yes = Content.Load<Texture2D>("yes");
            //no = Content.Load<Texture2D>("no");
            ////exiting scene

            ////settings scene
            //settingsicon = Content.Load<Texture2D>("settings");
            //ok = Content.Load<Texture2D>("ok");
            //back = Content.Load<Texture2D>("back");
            //soundturn = Content.Load<Texture2D>("yesicon");
            //musicturn = Content.Load<Texture2D>("yesicon");
            ////settings scene

            ////other
            //click = Content.Load<SoundEffect>("button");
            //soundInstance = click.CreateInstance();
            //soundInstance.Volume = 0.35f;

            //bgsong = Content.Load<Song>("backgroundmusic");

            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Volume = 0.05f;

            //MediaPlayer.Play(bgsong);
            ////other
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

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
