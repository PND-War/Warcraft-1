using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using System.Threading.Tasks;
using Warcraft_1.GameClasses;
using Warcraft_1.Units;

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

        private SpriteFont font;

        enum TextureSPlay
        {
            cur,
            Interface,
            Pixel,
            Menu,
            ProfileEmpty,
            Frame,
            Health,
            Resourses,
            BtnAttack,
            BtnWalk
        }
        public override void Load(GraphicsDeviceManager graphics, ContentManager Content)
        {
            if (!Logic_Classes.UnitsTextures.IsLoaded) Logic_Classes.UnitsTextures.Load(Content);

            font = Content.Load<SpriteFont>("Fonts/Font");

            emptyButton = Content.Load<Texture2D>("Textures/UI/ButtonEmpty");
            profile = Content.Load<Texture2D>("Textures/UI/ProfileEmpty");

            moveButton = emptyButton;
            defenseButton = emptyButton;
            attackButton = emptyButton;
            somethingButton = emptyButton;

            map.maptiles = Content.Load<Texture2D>("Textures/Game/groundcells");
            map.buildingtiles = Content.Load<Texture2D>("Textures/Game/buildingtiles");

            click = Content.Load<SoundEffect>("Sounds/button");
            //map.Save("map.wc");
            Texture2D Resourses = Content.Load<Texture2D>("Textures/Game/ResourceIcons");

            Texture2D cursor = Content.Load<Texture2D>("Textures/UI/cursor");
            Texture2D Interface = Content.Load<Texture2D>("Textures/UI/Interface");
            Texture2D Menu = Content.Load<Texture2D>("Textures/UI/MenuButton");

            Texture2D ProfileEmpty = Content.Load<Texture2D>("Textures/UI/ProfileEmpty");
            Texture2D Frame = Content.Load<Texture2D>("Textures/UI/Frame");
            Texture2D Health = Content.Load<Texture2D>("Textures/UI/HEALTH");


            //Texture2D BtnAttack = Content.Load<Texture2D>("Textures/Game/ButtonAttack");
            //Texture2D BtnWalk = Content.Load<Texture2D>("Textures/Game/ButtonWalk");

            Texture2D Pixel = new Texture2D(graphics.GraphicsDevice, 1, 1);
            Pixel.SetData<Color>(new Color[1] { Color.White });

            components.AddRange(new Texture2D[] { cursor, Interface, Pixel, Menu, ProfileEmpty, Frame, Health, Resourses, });
            this.map.Content = Content;
            SoundAdjust();
        }
        private void SoundAdjust()
        {
            soundInstance = click.CreateInstance();
            soundInstance.Volume = Logic_Classes.Settings.SFXVol ? 0.35f : 0.0f;
        }
        public override Scenes Update(GameTime gameTime)
        {
            map.Update(gameTime);
            CheckMapMove();
            CheckFocusMove();
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
                    map.Camera = new Point(Mouse.GetState().X < 400 - (((int)CameraMaxVal.X - 10) * 4) ? (Mouse.GetState().X - 45) / 4 : Map.mapSize - (int)CameraMaxVal.X, Mouse.GetState().Y < 400 - (((int)CameraMaxVal.Y - 8) * 4) ? (Mouse.GetState().Y - 45) / 4 : Map.mapSize - (int)CameraMaxVal.Y);
                }
            }

            return Scenes.nullscene;
        }

        private void CheckFocusMove()
        {
            if (map.group.FocusedUnit.X != -1 && map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit != null && map.group.FocusedUnit != map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit.positionToMove && !map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit.IsMoving && (map.group.WoodOntain || map.group.GoldOntain))
            {
                Task task = new Task(() => MoveFocusUnitToObtain(map.group.WoodOntain));
                task.Start();
            }
            else if (map.group.FocusedUnit.X != -1 && map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit != null && map.group.FocusedUnit != map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit.positionToMove && !map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit.IsMoving)
            {
                Task task = new Task(() => MoveFocusUnit());
                task.Start();
            }
            else if(map.group.FocusedUnit.X != -1 && map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit == null)
            {
                map.group.ChangePoint(-1, -1);
            }
        }
        private void MoveFocusUnit()
        {
            //AUnit aUnit = AUnit.DeepCopy(map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit);
            AUnit aUnit = map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit;
            Point Pos = map.group.FocusedUnit;
            map.group.ChangePoint(-1, -1);
            Logic_Classes.DIRS direction = Logic_Classes.DIRS.NONE;
            do
            {
                Point newPos = new Point(Pos.X, Pos.Y);

                bool up = false;
                bool left = false;
                bool down = false;
                bool right = false;
                if (Pos.X < aUnit.positionToMove.X && map.map[Pos.X + 1, Pos.Y].terrain != TypeOfTerrain.Tree && map.map[Pos.X + 1, Pos.Y].terrain != TypeOfTerrain.Water && map.map[Pos.X + 1, Pos.Y].terrain != TypeOfTerrain.Mine)
                {
                    newPos.X++; right = true;
                }
                else if (Pos.X > aUnit.positionToMove.X && map.map[Pos.X - 1, Pos.Y].terrain != TypeOfTerrain.Tree && map.map[Pos.X - 1, Pos.Y].terrain != TypeOfTerrain.Water && map.map[Pos.X - 1, Pos.Y].terrain != TypeOfTerrain.Mine)
                {
                    newPos.X--; left = true;
                }
                int n = right ? 1 : 0 + (left ? -1 : 0);
                if (Pos.Y < aUnit.positionToMove.Y && map.map[Pos.X+n, Pos.Y + 1].terrain != TypeOfTerrain.Tree && map.map[Pos.X + n, Pos.Y + 1].terrain != TypeOfTerrain.Water && map.map[Pos.X + n, Pos.Y + 1].terrain != TypeOfTerrain.Mine)
                {
                    newPos.Y++; down = true;
                }
                else if (Pos.Y > aUnit.positionToMove.Y && map.map[Pos.X+n, Pos.Y - 1].terrain != TypeOfTerrain.Tree && map.map[Pos.X + n, Pos.Y - 1].terrain != TypeOfTerrain.Water && map.map[Pos.X + n, Pos.Y - 1].terrain != TypeOfTerrain.Mine)
                {
                    newPos.Y--; up = true;
                }

                if (!up && !left && !down && !right)
                {
                    aUnit.positionToMove = Pos;
                    aUnit.IsMoving = false;
                    aUnit.UpdateAnim(false, direction != Logic_Classes.DIRS.NONE ? direction : Logic_Classes.DIRS.DOWN);
                    aUnit.Frame = 0;
                    map.map[Pos.X, Pos.Y].ClearUnitPlace();
                    map.map[Pos.X, Pos.Y].PlaceAUnit(aUnit);
                    return;
                }
                else
                {
                    map.map[newPos.X, newPos.Y].PlaceAUnit(aUnit);
                    map.map[Pos.X, Pos.Y].ClearUnitPlace();
                    aUnit.positionInMoving = new Point(494 + (Pos.X - map.Camera.X) * Map.fieldPixelSize, 44 + (Pos.Y - map.Camera.Y) * Map.fieldPixelSize);
                    aUnit.IsMoving = true;


                    direction = Logic_Classes.DIRS.NONE;
                    if (up && right)
                    {
                        direction = Logic_Classes.DIRS.UPRIGHT;
                    }
                    else if (up && left)
                    {
                        direction = Logic_Classes.DIRS.UPLEFT;
                    }
                    else if (up && left)
                    {
                        direction = Logic_Classes.DIRS.UPLEFT;
                    }
                    else if (down && right)
                    {
                        direction = Logic_Classes.DIRS.DOWNRIGHT;
                    }
                    else if (down && left)
                    {
                        direction = Logic_Classes.DIRS.DOWNLEFT;
                    }
                    else if (up)
                    {
                        direction = Logic_Classes.DIRS.UP;
                    }
                    else if (down)
                    {
                        direction = Logic_Classes.DIRS.DOWN;
                    }
                    else if (left)
                    {
                        direction = Logic_Classes.DIRS.LEFT;
                    }
                    else if (right)
                    {
                        direction = Logic_Classes.DIRS.RIGHT;
                    }

                    Point moveAdd = new Point();
                    switch (direction)
                    {
                        case Logic_Classes.DIRS.UP:
                            moveAdd = new Point(0, -1);
                            break;
                        case Logic_Classes.DIRS.RIGHT:
                            moveAdd = new Point(1, 0);
                            break;
                        case Logic_Classes.DIRS.DOWN:
                            moveAdd = new Point(0, 1);
                            break;
                        case Logic_Classes.DIRS.LEFT:
                            moveAdd = new Point(-1, 0);
                            break;
                        case Logic_Classes.DIRS.UPLEFT:
                            moveAdd = new Point(-1, -1);
                            break;
                        case Logic_Classes.DIRS.UPRIGHT:
                            moveAdd = new Point(1, -1);
                            break;
                        case Logic_Classes.DIRS.DOWNLEFT:
                            moveAdd = new Point(-1, 1);
                            break;
                        case Logic_Classes.DIRS.DOWNRIGHT:
                            moveAdd = new Point(1, 1);
                            break;
                    }
                    map.CheckAround(newPos.X, newPos.Y);
                    for (int i = 0; i < 160; i++)
                    {
                        if (i % 5 == 0)
                        {
                            aUnit.positionInMoving = new Point(494 + (Pos.X - map.Camera.X) * Map.fieldPixelSize + (moveAdd.X * (i / 5 + 1)), 44 + (Pos.Y - map.Camera.Y) * Map.fieldPixelSize + (moveAdd.Y * (i / 5 + 1)));
                            
                            if(aUnit is HumWorker hum)
                            {
                                aUnit.UpdateAnim(true, direction, hum.IsCarryingWood, hum.IsCarryingGold);
                            }
                            else if (aUnit is OrcWorker orc)
                            {
                                aUnit.UpdateAnim(true, direction, orc.IsCarryingWood, orc.IsCarryingGold);
                            }
                            else
                            {
                                aUnit.UpdateAnim(true, direction);
                            }
                        }
                        Thread.Sleep(1);
                    }
                    Pos = newPos;
                }




            } while (aUnit.positionToMove != Pos);
            aUnit.IsMoving = false;
            aUnit.UpdateAnim(false, direction != Logic_Classes.DIRS.NONE ? direction : Logic_Classes.DIRS.DOWN);
            aUnit.Frame = 0;
            map.map[Pos.X, Pos.Y].ClearUnitPlace();
            map.map[Pos.X, Pos.Y].PlaceAUnit(aUnit);
            GC.Collect();

        }
        private void MoveFocusUnitToObtain(bool wood)
        {
            Point pos = new Point(map.group.FocusedUnit.X, map.group.FocusedUnit.Y);
            HumWorker aUnit = map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit as HumWorker;
            MoveFocusUnit();
            aUnit.IsMoving = true;
            TypeOfTerrain type = wood ? TypeOfTerrain.Tree : TypeOfTerrain.Mine;
            bool can = false;
            for(int i = pos.X-1; i <= pos.X+1; i++)
            {
                for (int j = pos.Y - 1; j <= pos.Y + 1; j++)
                {
                    if(map.map[i,j].terrain == type)
                    {
                        can = true;
                    }
                }
            }
            if(can)
            {
                aUnit.UpdateAnim(false, Logic_Classes.DIRS.NONE);
                Thread.Sleep(5000);
                aUnit.IsCarryingWood = wood;
                aUnit.IsCarryingGold = !wood;
                aUnit.UpdateAnim(false, Logic_Classes.DIRS.DOWN);
            }
            aUnit.IsMoving = false;
        }
        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            //map.Read("map.wc");
            SpriteBatch _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            _spriteBatch.Begin();

            map.DrawMain(_spriteBatch, components[(int)TextureSPlay.Pixel]);
            _spriteBatch.Draw(components[(int)TextureSPlay.Interface], new Rectangle(0, 0, 1920, 1080), Color.White);

            map.DrawMini(_spriteBatch, components[(int)TextureSPlay.Pixel]);

            _spriteBatch.Draw(profile, new Vector2(40, 471), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(moveButton, new Vector2(40, 661), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(defenseButton, new Vector2(250, 661), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(attackButton, new Vector2(40, 791), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(somethingButton, new Vector2(250, 791), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            _spriteBatch.Draw(somethingButton, new Vector2(250, 791), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            _spriteBatch.Draw(components[(int)TextureSPlay.Menu], new Vector2(40, 952), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

           


            if (Logic_Classes.UnitsTextures.IsLoaded && map.group.FocusedUnit.X != -1 && map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit != null)
            {
                string UnitName;
                Logic_Classes.UnitNames.UnitName.TryGetValue((int)map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit.GetRole(), out UnitName);

                _spriteBatch.Draw(components[(int)TextureSPlay.Frame], new Rectangle(83, 500, 156, 113), Color.White);
                _spriteBatch.Draw(components[(int)TextureSPlay.Health], new Rectangle(253, 555, 156, 58), Color.White);
                _spriteBatch.Draw(Logic_Classes.UnitsTextures.Icons, new Rectangle(88, 505, Logic_Classes.IconSprite.XScale, Logic_Classes.IconSprite.YScale), Logic_Classes.IconSprite.GetTextureBounds(map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit.GetRace(), map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit.GetRole()), Color.White);
                _spriteBatch.DrawString(font, UnitName, new Vector2(251, 500), new Color(54, 26, 32, 255));

                for (int i = 0; i < map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit.GetCurHP() / map.map[map.group.FocusedUnit.X, map.group.FocusedUnit.Y].unit.GetMaxHP() * 145; i++)
                {
                    _spriteBatch.Draw(components[(int)TextureSPlay.Pixel], new Rectangle(259 + i, 588, 1, 20), Color.Green);
                }

            }
            _spriteBatch.DrawString(font, map.Wood.ToString(), new Vector2(173, 7), Color.Black);
            _spriteBatch.DrawString(font, map.Gold.ToString(), new Vector2(318, 7), Color.Black);

            _spriteBatch.DrawString(font, map.Wood.ToString(), new Vector2(170, 5), Color.White);
            _spriteBatch.DrawString(font, map.Gold.ToString(), new Vector2(315, 5), Color.White);

            

            _spriteBatch.Draw(components[(int)TextureSPlay.Resourses], new Rectangle(120, 5, 42, 29), new Rectangle(0, 0, 42, 29), Color.White);
            _spriteBatch.Draw(components[(int)TextureSPlay.Resourses], new Rectangle(280, 5, 27, 29), new Rectangle(42, 0, 27, 29), Color.White);

            //_spriteBatch.DrawString(font, map.Wood.ToString(), new Vector2(170, 5), Color.White);
            //_spriteBatch.DrawString(font, map.Gold.ToString(), new Vector2(315, 5), Color.White);

            _spriteBatch.Draw(components[(int)TextureSPlay.cur], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);

            _spriteBatch.End();
        }
    }
}
