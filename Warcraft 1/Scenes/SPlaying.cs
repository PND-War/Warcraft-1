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
        public Map map { get; private set; } = new Map();
        Texture2D emptyButton;

        Texture2D profile;

        Texture2D moveButton;
        Texture2D defenseButton;
        Texture2D attackButton;
        Texture2D somethingButton;

        enum TextureSPlay
        {
            cur,
            Interface,
            Pixel,
            Menu
        }
        public override void Load(GraphicsDeviceManager graphics, ContentManager Content)
        {
            emptyButton = Content.Load<Texture2D>("Textures/UI/ButtonEmpty");
            profile = Content.Load<Texture2D>("Textures/UI/ProfileEmpty");

            moveButton = emptyButton;
            defenseButton = emptyButton;
            attackButton = emptyButton;
            somethingButton = emptyButton;

            map.maptiles = Content.Load<Texture2D>("Textures/Game/groundcells");
            click = Content.Load<SoundEffect>("Sounds/button");
            //map.Save("map.wc");
            

            Texture2D cursor = Content.Load<Texture2D>("Textures/UI/cursor");
            Texture2D Interface = Content.Load<Texture2D>("Textures/UI/Interface");
            Texture2D Menu = Content.Load<Texture2D>("Textures/UI/MenuButton");

            Texture2D Pixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            Pixel.SetData<Color>(new Color[1] { Color.White });

            components.AddRange(new Texture2D[] { cursor, Interface, Pixel, Menu});
            SoundAdjust();
        }
        private void SoundAdjust()
        {
            soundInstance = click.CreateInstance();
            soundInstance.Volume = Logic_Classes.Settings.SFXVol ? 0.35f : 0.0f;
        }
        public override Scenes Update(GameTime gameTime)
        {
            CheckMapMove();
            return CheckPress();
        }
        private void CheckMapMove()
        {
            if ((Mouse.GetState().X == 0 || Keyboard.GetState().IsKeyDown(Keys.Left)) && map.Camera.X > 0)
            {
                map.Camera = new Point(map.Camera.X - 1, map.Camera.Y);
            }
            if ((Mouse.GetState().Y == 0 || Keyboard.GetState().IsKeyDown(Keys.Up)) && map.Camera.Y > 0)
            {
                map.Camera = new Point(map.Camera.X, map.Camera.Y - 1);
            }
            if ((Mouse.GetState().X == 1919 || Keyboard.GetState().IsKeyDown(Keys.Right)) && map.Camera.X < 56)
            {
                map.Camera = new Point(map.Camera.X + 1, map.Camera.Y);
            }
            if ((Mouse.GetState().Y == 1079 || Keyboard.GetState().IsKeyDown(Keys.Down)) && map.Camera.Y < 69)
            {
                map.Camera = new Point(map.Camera.X, map.Camera.Y + 1);
            }
        }
        private Scenes CheckPress()
        {
            if (Logic_Classes.MouseInterpretator.GetPressed(Logic_Classes.MouseButton.Left))
            {
                if (new Rectangle(40, 952, 410, 88).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    map.Save("save.wc");
                    return Scenes.mainmenu;
                }
            }
            //minimap interaction
            if (Logic_Classes.MouseInterpretator.GetPressedAllTime(Logic_Classes.MouseButton.Left))
            {
                if (new Rectangle(45, 45, 400, 400).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {
                   map.Camera = new Point(Mouse.GetState().X < 400-(((int)CameraMaxVal.X-10)*4) ? (Mouse.GetState().X-45)/4 : Map.mapSize - (int)CameraMaxVal.X, Mouse.GetState().Y < 400 - (((int)CameraMaxVal.Y-8) * 4) ?  (Mouse.GetState().Y-45)/4 : Map.mapSize - (int)CameraMaxVal.Y);
                }
            }
            
            return Scenes.nullscene;
        }

        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            map.Read("map.wc");
            SpriteBatch _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            _spriteBatch.Begin();


            map.DrawMain(_spriteBatch);
            _spriteBatch.Draw(components[(int)TextureSPlay.Interface], new Rectangle(0, 0, 1920, 1080), Color.White);

            map.DrawMini(_spriteBatch, components[(int)TextureSPlay.Pixel]);

            _spriteBatch.Draw(profile, new Vector2(40, 471), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(moveButton, new Vector2(40, 661), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(defenseButton, new Vector2(250, 661), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(attackButton, new Vector2(40, 791), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(somethingButton, new Vector2(250, 791), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(somethingButton, new Vector2(250, 791), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            _spriteBatch.Draw(components[(int)TextureSPlay.Menu], new Vector2(40, 952), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            _spriteBatch.Draw(components[(int)TextureSPlay.cur], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

            _spriteBatch.End();
        }
    }
}
