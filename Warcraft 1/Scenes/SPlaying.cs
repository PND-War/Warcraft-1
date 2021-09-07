using FindWay;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Warcraft_1.GameClasses.MapClasses;
using Warcraft_1.GameClasses.Units;
using Warcraft_1.SpriteAndUnits;

namespace Warcraft_1.Scenes
{
    class SPlaying : AScene
    {
        public Map map { get; private set; } = new Map();
        Texture2D emptyButton;

        Texture2D profile;

        public SoundEffectInstance unitInstance;
        Texture2D moveButton;
        Texture2D buildButton;
        Texture2D attackButton;
        Texture2D obtainButton;

        Vector2 attackCoords = new Vector2(40, 661);
        Vector2 moveCords = new Vector2(250, 661);
        Vector2 obtainCords = new Vector2(40, 791);
        Vector2 buildCords = new Vector2(250, 791);
        Point btnSize = new Point(200, 120);
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
            if (!UnitsTextures.IsLoaded) UnitsTextures.Load(Content);

            font = Content.Load<SpriteFont>("Fonts/Font");

            emptyButton = Content.Load<Texture2D>("Textures/UI/ButtonEmpty");
            profile = Content.Load<Texture2D>("Textures/UI/ProfileEmpty");

            moveButton = Content.Load<Texture2D>("Textures/UI/ButtonMove");
            buildButton = emptyButton;
            attackButton = Content.Load<Texture2D>("Textures/UI/ButtonAttack");
            obtainButton = emptyButton;

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
            Point MouseCoords = new Point(-1, -1);
            Logic_Classes.MouseInterpretator.GetPressed();
            if (Logic_Classes.MouseInterpretator.bt == Logic_Classes.MouseButton.Left)
            {
                MouseCoords = new Point((Mouse.GetState().Position.X - 494) / Map.fieldPixelSize + map.Camera.X, (Mouse.GetState().Position.Y - 44) / Map.fieldPixelSize + map.Camera.Y);
                if (map.buildMode && map.buildingType != BuildingType.None)
                {
                    Task build = new Task(() => Build());
                    build.Start();
                }
                else if (map.movingMode && map.group.FocusedObj.X != -1 && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null && !map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.IsMoving && new Rectangle(494, 44, 1382, 992).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    map.movingMode = false;
                    MouseCoords = new Point((Mouse.GetState().Position.X - 494) / Map.fieldPixelSize + map.Camera.X, (Mouse.GetState().Position.Y - 44) / Map.fieldPixelSize + map.Camera.Y);
                    if (map.map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Tree && map.map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Water && map.map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Mine && map.map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Building && map.map[MouseCoords.X, MouseCoords.Y].unit == null)
                    {
                        try
                        {
                            map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove = MouseCoords;
                            if (map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.action != null && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove != map.group.FocusedObj)
                            {
                                unitInstance = map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.action.CreateInstance();
                                unitInstance.Volume = Logic_Classes.Settings.SFXVol ? 0.35f : 0.0f;
                                unitInstance.Play();
                            }
                        }
                        catch (Exception) { }
                    }
                }
                else if (new Rectangle(40, 952, 410, 88).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    map.Save("save.wc");
                    soundInstance.Play();
                    return Scenes.mainmenu;
                }
                else if (new Rectangle((int)attackCoords.X, (int)attackCoords.Y, btnSize.X, btnSize.Y).Contains(Mouse.GetState().X, Mouse.GetState().Y) && map.group.FocusedObj.X != -1)
                {
                    if (map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null)
                    {
                        if (!map.buildMode)
                        {
                            map.attackMode = !map.attackMode;
                            map.movingMode = false;
                            map.obtainMode = false;
                        }
                        else
                            map.buildingType = BuildingType.MainBuild;
                    }
                    else if (map.group.buildingType != BuildingType.None)
                    {
                        Role role = Role.NONE;
                        switch (map.group.buildingType)
                        {
                            case BuildingType.MainBuild:
                                role = Role.WORKER;
                                break;
                            case BuildingType.Barracks:
                                role = Role.WARRIOR;
                                break;
                            case BuildingType.Farm:
                                break;
                            case BuildingType.None:
                                break;
                        }
                        if (role != Role.NONE && map.Gold >= UnitAssistance.GetCurrency(false, role) && map.Wood >= UnitAssistance.GetCurrency(true, role))
                        {
                            map.Gold -= UnitAssistance.GetCurrency(false, role);
                            map.Wood -= UnitAssistance.GetCurrency(true, role);
                            map.CreateUnit(map.group.FocusedObj.X - 1, map.group.FocusedObj.Y, Race.HUMAN, role);
                        }
                    }
                }
                else if (new Rectangle((int)moveCords.X, (int)moveCords.Y, btnSize.X, btnSize.Y).Contains(Mouse.GetState().X, Mouse.GetState().Y) && map.group.FocusedObj.X != -1)
                {
                    if (map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null)
                    {
                        if (!map.buildMode)
                        {
                            map.movingMode = !map.movingMode;
                            map.obtainMode = false;
                            map.attackMode = false;
                        }
                        else
                            map.buildingType = BuildingType.Barracks;
                    }

                }
                else if (new Rectangle((int)obtainCords.X, (int)obtainCords.Y, btnSize.X, btnSize.Y).Contains(Mouse.GetState().X, Mouse.GetState().Y) && map.group.FocusedObj.X != -1 && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit is HumWorker)
                {
                    if (!map.buildMode)
                    {
                        map.obtainMode = !map.obtainMode;
                        map.movingMode = false;
                        map.attackMode = false;
                    }
                    else
                        map.buildingType = BuildingType.Farm;
                }
                else if (new Rectangle((int)buildCords.X, (int)buildCords.Y, btnSize.X, btnSize.Y).Contains(Mouse.GetState().X, Mouse.GetState().Y) && map.group.FocusedObj.X != -1 && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit is HumWorker)
                {
                    map.buildMode = !map.buildMode;
                    map.movingMode = false;
                    map.attackMode = false;
                    map.obtainMode = false;
                    map.buildingType = BuildingType.None;
                }
                else if ((MouseCoords.X > 0 && MouseCoords.X < 100) && (MouseCoords.Y > 0 && MouseCoords.Y < 100) && map.map[MouseCoords.X, MouseCoords.Y].unit != null && !map.map[MouseCoords.X, MouseCoords.Y].unit.IsMoving) map.group.ChangePoint(MouseCoords);
                else if ((MouseCoords.X > 0 && MouseCoords.X < 100) && (MouseCoords.Y > 0 && MouseCoords.Y < 100) && map.map[MouseCoords.X, MouseCoords.Y].terrain == TypeOfTerrain.Building)
                {
                    map.group.ChangePoint(MouseCoords);
                    map.group.buildingType = map.map[MouseCoords.X, MouseCoords.Y].buildingType;
                    for (int i = MouseCoords.X - 2; i < MouseCoords.X + 2; i++)
                    {
                        bool can = false;
                        for (int j = MouseCoords.Y - 2; j < MouseCoords.Y + 2; j++)
                        {
                            if (map.map[i, j].buildingType == map.group.buildingType)
                            {
                                can = true;
                                map.group.ChangePoint(i, j);
                                map.group.buildingType = map.map[MouseCoords.X, MouseCoords.Y].buildingType;
                                break;
                            }

                        }
                        if (can)
                        {
                            break;
                        }
                    }
                }
                else if ((MouseCoords.X > 0 && MouseCoords.X < 100) && (MouseCoords.Y > 0 && MouseCoords.Y < 100))
                {
                    map.group.ChangePoint(-1, -1);
                    map.movingMode = false;
                    map.attackMode = false;
                    map.obtainMode = false;
                }
            }
            else if (Logic_Classes.MouseInterpretator.bt == Logic_Classes.MouseButton.Right)
            {

                MouseCoords = new Point((Mouse.GetState().Position.X - 494) / Map.fieldPixelSize + map.Camera.X, (Mouse.GetState().Position.Y - 44) / Map.fieldPixelSize + map.Camera.Y);
                if (map.buildMode && map.buildingType != BuildingType.None)
                {
                    map.buildMode = false;
                    map.buildingType = BuildingType.None;
                }
                else if (map.attackMode && map.map[MouseCoords.X, MouseCoords.Y].unit != null && map.map[MouseCoords.X, MouseCoords.Y].unit.GetRace() != Race.ORC)
                {
                    Task attack = new Task(() => Attack());
                    attack.Start();
                }
                else if (map.group.FocusedObj.X != -1 && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null && !map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.IsMoving && new Rectangle(494, 44, 1382, 992).Contains(Mouse.GetState().X, Mouse.GetState().Y) && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.GetRole() == Role.WORKER && (((HumWorker)map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit).IsCarryingGold || ((HumWorker)map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit).IsCarryingWood) && map.map[(Mouse.GetState().Position.X - 494) / Map.fieldPixelSize + map.Camera.X, (Mouse.GetState().Position.Y - 44) / Map.fieldPixelSize + map.Camera.Y].terrain == TypeOfTerrain.Building)
                {
                    if (map.map[MouseCoords.X, MouseCoords.Y].buildOf == Race.HUMAN && map.map[MouseCoords.X, MouseCoords.Y].buildingType == BuildingType.MainBuild)
                    {
                        map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove = MouseCoords;
                    }
                }
                else if (map.group.FocusedObj.X != -1 && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null && !map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.IsMoving && new Rectangle(494, 44, 1382, 992).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {

                    if (map.map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Tree && map.map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Water && map.map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Mine && map.map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Building && map.map[MouseCoords.X, MouseCoords.Y].unit == null)
                    {
                        try
                        {
                            map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove = MouseCoords;
                            if (map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.action != null && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove != map.group.FocusedObj)
                            {
                                unitInstance = map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.action.CreateInstance();
                                unitInstance.Volume = Logic_Classes.Settings.SFXVol ? 0.35f : 0.0f;
                                unitInstance.Play();
                            }
                        }
                        catch (Exception) { }
                    }
                    else if ((map.map[MouseCoords.X, MouseCoords.Y].terrain == TypeOfTerrain.Tree || map.map[MouseCoords.X, MouseCoords.Y].terrain == TypeOfTerrain.Mine) && (Keyboard.GetState().IsKeyDown(Keys.D) || map.obtainMode))
                    {
                        map.obtainMode = false;
                        try
                        {
                            map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove = MouseCoords;
                            if (map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.action != null && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove != map.group.FocusedObj)
                            {
                                unitInstance = map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.action.CreateInstance();
                                unitInstance.Volume = Logic_Classes.Settings.SFXVol ? 0.35f : 0.0f;
                                unitInstance.Play();
                            }
                            map.group.OntainChange(true, map.map[MouseCoords.X, MouseCoords.Y].terrain == TypeOfTerrain.Tree ? TypeOfTerrain.Tree : TypeOfTerrain.Mine);
                        }
                        catch (Exception) { }
                    }
                }
                else if (map.group.FocusedObj.X != -1 && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null && !map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.IsMoving && new Rectangle(45, 45, 400, 400).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    MouseCoords = new Point((Mouse.GetState().Position.X - 45) / 4, (Mouse.GetState().Position.Y - 45) / 4);
                    if (map.map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Tree && map.map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Water && map.map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Mine && map.map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Building)
                    {
                        try
                        {
                            map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove = MouseCoords;
                            if (map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.action != null && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove != map.group.FocusedObj)
                            {
                                unitInstance = map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.action.CreateInstance();
                                unitInstance.Volume = Logic_Classes.Settings.SFXVol ? 0.35f : 0.0f;
                                unitInstance.Play();
                            }
                        }
                        catch (Exception) { }
                    }
                }
            }
            else if (Logic_Classes.MouseInterpretator.GetPressedAllTime(Logic_Classes.MouseButton.Left))//minimap interaction
            {
                if (new Rectangle(45, 45, 400, 400).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    map.Camera = new Point(Mouse.GetState().X < 400 - (((int)CameraMaxVal.X - 10) * 4) ? (Mouse.GetState().X - 45) / 4 : Map.mapSize - (int)CameraMaxVal.X, Mouse.GetState().Y < 400 - (((int)CameraMaxVal.Y - 8) * 4) ? (Mouse.GetState().Y - 45) / 4 : Map.mapSize - (int)CameraMaxVal.Y);
                }
            }
            return Scenes.nullscene;
        }

        private void Build()
        {
            Point Pos = map.group.FocusedObj;
            int requireGold = BuildAssistance.GetCurrency(false, map.buildingType);
            int requireWood = BuildAssistance.GetCurrency(true, map.buildingType);
            BuildingType building = map.buildingType;
            int x = (Mouse.GetState().Position.X - 494) / Map.fieldPixelSize + map.Camera.X;
            int y = (Mouse.GetState().Position.Y - 44) / Map.fieldPixelSize + map.Camera.Y;
            if (map.Gold >= requireGold && map.Wood >= requireWood && map.CheckTerrainToBuild(x, y))
            {
                map.Gold -= requireGold;
                map.Wood -= requireWood;

                map.map[Pos.X, Pos.Y].unit.positionToMove = new Point(x, y);
                if (map.group.FocusedObj.X != -1 && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null && map.group.FocusedObj != map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove && !map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.IsMoving)
                {
                    CalculateWay CalcWay = new CalculateWay(new FField(map.group.FocusedObj.X, map.group.FocusedObj.Y), new FField(map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove.X, map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove.Y));

                    try
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            for (int j = 0; j < 100; j++)
                            {
                                CalcWay.map.map[j, i].Init((int)map.map[i, j].terrain);
                            }
                        }
                        List<System.Drawing.Point> Way = CalcWay.StartAlhorythm();

                        Task task = new Task(() => MoveFocusUnit(Way));
                        task.Start();
                        task.Wait();
                    }
                    catch
                    {
                        Task task = new Task(() => MoveFocusUnit());
                        task.Start();
                        task.Wait();
                    };

                }
                for (int i = x; i < x + 3; i++)
                {
                    for (int j = y; j < y + 3; j++)
                    {
                        map.map[i, j].terrain = TypeOfTerrain.Building;
                        map.map[i, j].buildOf = Race.HUMAN;
                        map.map[i, j].buildingType = building;
                        switch (building)
                        {
                            case BuildingType.MainBuild:
                                map.map[i, j].spriteId = 90 + ((i - x) + ((j - y) * 3));
                                break;
                            case BuildingType.Barracks:
                                map.map[i, j].spriteId = 99 + ((i - x) + ((j - y) * 3));
                                break;
                            case BuildingType.Farm:
                                map.map[i, j].spriteId = 108 + ((i - x) + ((j - y) * 3));
                                if (i == x && j == y)
                                    map.StartTimer(x, y);
                                break;
                            case BuildingType.None:
                                break;
                        }

                    }
                }


                map.buildMode = false;
                map.buildingType = BuildingType.None;
            }
            else
            {
                map.buildMode = false;
                map.buildingType = BuildingType.None;
            }
        }
        private void Attack()
        {
            //WIP
        }
        private void CheckFocusMove()
        {
            if (map.group.FocusedObj.X != -1 && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null && map.group.FocusedObj != map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove && !map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.IsMoving && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.GetRole() == Role.WORKER && (((HumWorker)map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit).IsCarryingGold || ((HumWorker)map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit).IsCarryingWood))
            {
                CalculateWay CalcWay = new CalculateWay(new FField(map.group.FocusedObj.X, map.group.FocusedObj.Y), new FField(map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove.X, map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove.Y));

                try
                {
                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            CalcWay.map.map[j, i].Init((int)map.map[i, j].terrain);
                        }
                    }
                    List<System.Drawing.Point> Way = CalcWay.StartAlhorythm();

                    Task task = new Task(() => MoveFocusUnitToGive(Way, ((HumWorker)map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit).IsCarryingWood));
                    task.Start();
                }
                catch
                {
                    Task task = new Task(() => MoveFocusUnitToGive(((HumWorker)map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit).IsCarryingWood));
                    task.Start();
                };
            }
            else if (map.group.FocusedObj.X != -1 && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null && map.group.FocusedObj != map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove && !map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.IsMoving && (map.group.WoodObtain || map.group.GoldOntain))
            {
                CalculateWay CalcWay = new CalculateWay(new FField(map.group.FocusedObj.X, map.group.FocusedObj.Y), new FField(map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove.X, map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove.Y));

                try
                {
                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            CalcWay.map.map[j, i].Init((int)map.map[i, j].terrain);
                        }
                    }
                    List<System.Drawing.Point> Way = CalcWay.StartAlhorythm();

                    Task task = new Task(() => MoveFocusUnitToObtain(Way, map.group.WoodObtain));
                    task.Start();
                }
                catch
                {
                    Task task = new Task(() => MoveFocusUnitToObtain(map.group.WoodObtain));
                    task.Start();
                };
            }
            else if (map.group.FocusedObj.X != -1 && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null && map.group.FocusedObj != map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove && !map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.IsMoving)
            {
                CalculateWay CalcWay = new CalculateWay(new FField(map.group.FocusedObj.X, map.group.FocusedObj.Y), new FField(map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove.X, map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.positionToMove.Y));

                try
                {
                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < 100; j++)
                        {
                            CalcWay.map.map[j, i].Init((int)map.map[i, j].terrain);
                        }
                    }
                    List<System.Drawing.Point> Way = CalcWay.StartAlhorythm();

                    Task task = new Task(() => MoveFocusUnit(Way));
                    task.Start();
                }
                catch
                {
                    Task task = new Task(() => MoveFocusUnit());
                    task.Start();
                };

            }
            else if (map.group.FocusedObj.X != -1 && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit == null && map.group.buildingType == BuildingType.None)
            {
                map.group.ChangePoint(-1, -1);
            }
        }
        private void MoveFocusUnit(List<System.Drawing.Point> Way)
        {
            List<Point> NewWay = new List<Point>();
            Point pos = map.group.FocusedObj;
            for (int i = 0; i < Way.Count; i++)
            {
                NewWay.Add(new Point(Way[i].X, Way[i].Y));
            }
            int CurrentStep = 0;

            AUnit aUnit = map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit;
            map.group.ChangePoint(-1, -1);
            DIRS direction = DIRS.NONE;

            do
            {

                bool up = false;
                bool left = false;
                bool down = false;
                bool right = false;
                if (NewWay[CurrentStep].X < NewWay[CurrentStep + 1].X)
                {
                    right = true;
                }
                else if (NewWay[CurrentStep].X > NewWay[CurrentStep + 1].X)
                {
                    left = true;
                }
                if (NewWay[CurrentStep].Y < NewWay[CurrentStep + 1].Y)
                {
                    down = true;
                }
                else if (NewWay[CurrentStep].Y > NewWay[CurrentStep + 1].Y)
                {
                    up = true;
                }

                if (!up && !left && !down && !right)
                {
                    aUnit.IsMoving = false;
                    aUnit.UpdateAnim(false, direction != DIRS.NONE ? direction : DIRS.DOWN);
                    aUnit.Frame = 0;
                    
                    return;
                }
                else
                {
                    map.map[NewWay[CurrentStep + 1].X, NewWay[CurrentStep + 1].Y].PlaceAUnit(aUnit);
                    map.map[NewWay[CurrentStep].X, NewWay[CurrentStep].Y].ClearUnitPlace();
                    aUnit.positionInMoving = new Point(494 + (NewWay[CurrentStep].X - map.Camera.X) * Map.fieldPixelSize, 44 + (NewWay[CurrentStep].Y - map.Camera.Y) * Map.fieldPixelSize);
                    aUnit.IsMoving = true;

                    direction = DIRS.NONE;
                    if (up && right)
                    {
                        direction = DIRS.UPRIGHT;
                    }
                    else if (up && left)
                    {
                        direction = DIRS.UPLEFT;
                    }
                    else if (up && left)
                    {
                        direction = DIRS.UPLEFT;
                    }
                    else if (down && right)
                    {
                        direction = DIRS.DOWNRIGHT;
                    }
                    else if (down && left)
                    {
                        direction = DIRS.DOWNLEFT;
                    }
                    else if (up)
                    {
                        direction = DIRS.UP;
                    }
                    else if (down)
                    {
                        direction = DIRS.DOWN;
                    }
                    else if (left)
                    {
                        direction = DIRS.LEFT;
                    }
                    else if (right)
                    {
                        direction = DIRS.RIGHT;
                    }

                    Point moveAdd = new Point();
                    switch (direction)
                    {
                        case DIRS.UP:
                            moveAdd = new Point(0, -1);
                            break;
                        case DIRS.RIGHT:
                            moveAdd = new Point(1, 0);
                            break;
                        case DIRS.DOWN:
                            moveAdd = new Point(0, 1);
                            break;
                        case DIRS.LEFT:
                            moveAdd = new Point(-1, 0);
                            break;
                        case DIRS.UPLEFT:
                            moveAdd = new Point(-1, -1);
                            break;
                        case DIRS.UPRIGHT:
                            moveAdd = new Point(1, -1);
                            break;
                        case DIRS.DOWNLEFT:
                            moveAdd = new Point(-1, 1);
                            break;
                        case DIRS.DOWNRIGHT:
                            moveAdd = new Point(1, 1);
                            break;
                    }
                    if (CurrentStep + 1 < NewWay.Count) map.CheckAround(NewWay[CurrentStep + 1].X, NewWay[CurrentStep + 1].Y);
                    for (int i = 0; i < 160; i++)
                    {
                        if (i % 5 == 0)
                        {
                            aUnit.positionInMoving = new Point(494 + (NewWay[CurrentStep].X - map.Camera.X) * Map.fieldPixelSize + (moveAdd.X * (i / 5 + 1)), 44 + (NewWay[CurrentStep].Y - map.Camera.Y) * Map.fieldPixelSize + (moveAdd.Y * (i / 5 + 1)));

                            if (aUnit is HumWorker hum)
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

                    CurrentStep++;
                }
            } while (CurrentStep + 1 != NewWay.Count);

            aUnit.positionToMove = NewWay.Last();
            map.map[pos.X, pos.Y].ClearUnitPlace();
            map.map[NewWay[CurrentStep].X, NewWay[CurrentStep].Y].PlaceAUnit(aUnit);
            aUnit.IsMoving = false;
            aUnit.UpdateAnim(false, direction != DIRS.NONE ? direction : DIRS.DOWN);
            aUnit.Frame = 0;
            GC.Collect();

        }
        private void MoveFocusUnit()
        {
            AUnit aUnit = map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit;
            Point Pos = map.group.FocusedObj;
            map.group.ChangePoint(-1, -1);
            DIRS direction = DIRS.NONE;
            do
            {
                Point newPos = new Point(Pos.X, Pos.Y);

                bool up = false;
                bool left = false;
                bool down = false;
                bool right = false;
                if (Pos.X < aUnit.positionToMove.X && map.map[Pos.X + 1, Pos.Y].terrain != TypeOfTerrain.Tree && map.map[Pos.X + 1, Pos.Y].terrain != TypeOfTerrain.Water && map.map[Pos.X + 1, Pos.Y].terrain != TypeOfTerrain.Mine && map.map[Pos.X + 1, Pos.Y].terrain != TypeOfTerrain.Building && map.map[Pos.X + 1, Pos.Y].unit == null)
                {
                    newPos.X++; right = true;
                }
                else if (Pos.X > aUnit.positionToMove.X && map.map[Pos.X - 1, Pos.Y].terrain != TypeOfTerrain.Tree && map.map[Pos.X - 1, Pos.Y].terrain != TypeOfTerrain.Water && map.map[Pos.X - 1, Pos.Y].terrain != TypeOfTerrain.Mine && map.map[Pos.X - 1, Pos.Y].terrain != TypeOfTerrain.Building && map.map[Pos.X - 1, Pos.Y].unit == null)
                {
                    newPos.X--; left = true;
                }
                int n = right ? 1 : 0 + (left ? -1 : 0);
                if (Pos.Y < aUnit.positionToMove.Y && map.map[Pos.X + n, Pos.Y + 1].terrain != TypeOfTerrain.Tree && map.map[Pos.X + n, Pos.Y + 1].terrain != TypeOfTerrain.Water && map.map[Pos.X + n, Pos.Y + 1].terrain != TypeOfTerrain.Mine && map.map[Pos.X + n, Pos.Y + 1].terrain != TypeOfTerrain.Building && map.map[Pos.X + n, Pos.Y + 1].unit == null)
                {
                    newPos.Y++; down = true;
                }
                else if (Pos.Y > aUnit.positionToMove.Y && map.map[Pos.X + n, Pos.Y - 1].terrain != TypeOfTerrain.Tree && map.map[Pos.X + n, Pos.Y - 1].terrain != TypeOfTerrain.Water && map.map[Pos.X + n, Pos.Y - 1].terrain != TypeOfTerrain.Mine && map.map[Pos.X + n, Pos.Y - 1].terrain != TypeOfTerrain.Building && map.map[Pos.X + n, Pos.Y - 1].unit == null)
                {
                    newPos.Y--; up = true;
                }

                if (!up && !left && !down && !right)
                {
                    aUnit.positionToMove = Pos;
                    aUnit.IsMoving = false;
                    aUnit.UpdateAnim(false, direction != DIRS.NONE ? direction : DIRS.DOWN);
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

                    direction = DIRS.NONE;
                    if (up && right)
                    {
                        direction = DIRS.UPRIGHT;
                    }
                    else if (up && left)
                    {
                        direction = DIRS.UPLEFT;
                    }
                    else if (up && left)
                    {
                        direction = DIRS.UPLEFT;
                    }
                    else if (down && right)
                    {
                        direction = DIRS.DOWNRIGHT;
                    }
                    else if (down && left)
                    {
                        direction = DIRS.DOWNLEFT;
                    }
                    else if (up)
                    {
                        direction = DIRS.UP;
                    }
                    else if (down)
                    {
                        direction = DIRS.DOWN;
                    }
                    else if (left)
                    {
                        direction = DIRS.LEFT;
                    }
                    else if (right)
                    {
                        direction = DIRS.RIGHT;
                    }

                    Point moveAdd = new Point();
                    switch (direction)
                    {
                        case DIRS.UP:
                            moveAdd = new Point(0, -1);
                            break;
                        case DIRS.RIGHT:
                            moveAdd = new Point(1, 0);
                            break;
                        case DIRS.DOWN:
                            moveAdd = new Point(0, 1);
                            break;
                        case DIRS.LEFT:
                            moveAdd = new Point(-1, 0);
                            break;
                        case DIRS.UPLEFT:
                            moveAdd = new Point(-1, -1);
                            break;
                        case DIRS.UPRIGHT:
                            moveAdd = new Point(1, -1);
                            break;
                        case DIRS.DOWNLEFT:
                            moveAdd = new Point(-1, 1);
                            break;
                        case DIRS.DOWNRIGHT:
                            moveAdd = new Point(1, 1);
                            break;
                    }
                    map.CheckAround(newPos.X, newPos.Y);
                    for (int i = 0; i < 160; i++)
                    {
                        if (i % 5 == 0)
                        {
                            aUnit.positionInMoving = new Point(494 + (Pos.X - map.Camera.X) * Map.fieldPixelSize + (moveAdd.X * (i / 5 + 1)), 44 + (Pos.Y - map.Camera.Y) * Map.fieldPixelSize + (moveAdd.Y * (i / 5 + 1)));

                            if (aUnit is HumWorker hum)
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
            aUnit.UpdateAnim(false, direction != DIRS.NONE ? direction : DIRS.DOWN);
            aUnit.Frame = 0;
            GC.Collect();

        }
        private void MoveFocusUnitToObtain(List<System.Drawing.Point> Way, bool wood)
        {
            HumWorker aUnit = map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit as HumWorker;
            MoveFocusUnit(Way);
            aUnit.IsMoving = true;
            TypeOfTerrain type = wood ? TypeOfTerrain.Tree : TypeOfTerrain.Mine;
            bool can = false;
            for (int i = aUnit.positionToMove.X - 1; i <= aUnit.positionToMove.X + 1; i++)
            {
                for (int j = aUnit.positionToMove.Y - 1; j <= aUnit.positionToMove.Y + 1; j++)
                {
                    if (map.map[i, j].terrain == type)
                    {
                        can = true;
                    }
                }
            }
            if (can)
            {
                aUnit.UpdateAnim(false, DIRS.NONE);
                Thread.Sleep(5000);
                aUnit.IsCarryingWood = wood;
                aUnit.IsCarryingGold = !wood;
                aUnit.UpdateAnim(false, DIRS.DOWN);
            }
            aUnit.IsMoving = false;
        }
        private void MoveFocusUnitToObtain(bool wood)
        {
            HumWorker aUnit = map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit as HumWorker;
            MoveFocusUnit();
            aUnit.IsMoving = true;
            TypeOfTerrain type = wood ? TypeOfTerrain.Tree : TypeOfTerrain.Mine;
            bool can = false;
            for (int i = aUnit.positionToMove.X - 1; i <= aUnit.positionToMove.X + 1; i++)
            {
                for (int j = aUnit.positionToMove.Y - 1; j <= aUnit.positionToMove.Y + 1; j++)
                {
                    if (map.map[i, j].terrain == type)
                    {
                        can = true;
                    }
                }
            }
            if (can)
            {
                aUnit.UpdateAnim(false, DIRS.NONE);
                Thread.Sleep(5000);
                aUnit.IsCarryingWood = wood;
                aUnit.IsCarryingGold = !wood;
                aUnit.UpdateAnim(false, DIRS.DOWN);
            }
            aUnit.IsMoving = false;
        }
        private void MoveFocusUnitToGive(List<System.Drawing.Point> Way, bool wood)
        {
            HumWorker aUnit = map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit as HumWorker;
            MoveFocusUnit(Way);
            aUnit.IsMoving = true;
            TypeOfTerrain type = wood ? TypeOfTerrain.Tree : TypeOfTerrain.Mine;
            bool can = false;
            for (int i = aUnit.positionToMove.X - 1; i <= aUnit.positionToMove.X + 1; i++)
            {
                for (int j = aUnit.positionToMove.Y - 1; j <= aUnit.positionToMove.Y + 1; j++)
                {
                    if (map.map[i, j].buildingType == BuildingType.MainBuild)
                    {
                        can = true;
                    }
                }
            }
            if (can)
            {
                aUnit.UpdateAnim(false, DIRS.NONE);
                Thread.Sleep(500);
                if (wood)
                {
                    map.Wood += 50;
                }
                else
                {
                    map.Gold += 50;
                }
                aUnit.IsCarryingWood = false;
                aUnit.IsCarryingGold = false;
                aUnit.UpdateAnim(false, DIRS.DOWN);
            }
            aUnit.IsMoving = false;
        }
        private void MoveFocusUnitToGive(bool wood)
        {
            HumWorker aUnit = map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit as HumWorker;
            MoveFocusUnit();
            aUnit.IsMoving = true;
            TypeOfTerrain type = wood ? TypeOfTerrain.Tree : TypeOfTerrain.Mine;
            bool can = false;
            for (int i = aUnit.positionToMove.X - 1; i <= aUnit.positionToMove.X + 1; i++)
            {
                for (int j = aUnit.positionToMove.Y - 1; j <= aUnit.positionToMove.Y + 1; j++)
                {
                    if (map.map[i, j].buildingType == BuildingType.MainBuild)
                    {
                        can = true;
                    }
                }
            }
            if (can)
            {
                aUnit.UpdateAnim(false, DIRS.NONE);
                Thread.Sleep(500);
                if (wood)
                {
                    map.Wood += 50;
                }
                else
                {
                    map.Gold += 50;
                }
                aUnit.IsCarryingWood = false;
                aUnit.IsCarryingGold = false;
                aUnit.UpdateAnim(false, DIRS.DOWN);
            }
            aUnit.IsMoving = false;
        }


        public override void Draw(GraphicsDeviceManager graphics, GameTime gameTime)
        {
            SpriteBatch _spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            _spriteBatch.Begin();

            map.DrawMain(_spriteBatch, components[(int)TextureSPlay.Pixel]);
            _spriteBatch.Draw(components[(int)TextureSPlay.Interface], new Rectangle(0, 0, 1920, 1080), Color.White);

            map.DrawMini(_spriteBatch, components[(int)TextureSPlay.Pixel]);

            _spriteBatch.Draw(profile, new Vector2(40, 471), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            if (!map.buildMode)
            {

                _spriteBatch.Draw(buildButton, buildCords, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(obtainButton, obtainCords, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                if (map.group.FocusedObj.X != -1 && map.group.buildingType == BuildingType.None)
                {
                    _spriteBatch.Draw(moveButton, moveCords, null, !map.movingMode ? Color.White : Color.Gray, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                    _spriteBatch.Draw(attackButton, attackCoords, null, map.attackMode || Keyboard.GetState().IsKeyDown(Keys.A) ? Color.Gray : Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                    if (map.obtainMode || Keyboard.GetState().IsKeyDown(Keys.D))
                    {
                        _spriteBatch.Draw(UnitsTextures.Icons, new Rectangle(40, 791, 200, 120), new Rectangle(600, 616, 200, 120), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(UnitsTextures.Icons, new Rectangle(40, 791, 200, 120), new Rectangle(800, 616, 200, 120), Color.White);
                    }
                    if (map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.GetRole() == Role.WORKER)
                    {
                        _spriteBatch.Draw(UnitsTextures.Icons, new Rectangle(250, 791, 200, 120), new Rectangle(1000, 616, 200, 120), Color.White);
                    }
                }
                else
                {
                    _spriteBatch.Draw(obtainButton, moveCords, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                    _spriteBatch.Draw(obtainButton, attackCoords, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                    if (map.group.buildingType != BuildingType.None)
                    {
                        switch (map.group.buildingType)
                        {
                            case BuildingType.MainBuild:
                                _spriteBatch.Draw(UnitsTextures.Icons, new Rectangle((int)attackCoords.X, (int)attackCoords.Y + 8, IconSprite.XScale, IconSprite.YScale), IconSprite.GetTextureBounds(Race.HUMAN, Role.WORKER), Color.White);
                                _spriteBatch.DrawString(font, UnitAssistance.GetCurrency(true, Role.WORKER).ToString(), new Vector2((int)attackCoords.X + IconSprite.XScale + 5, (int)attackCoords.Y + 20), Color.SandyBrown);
                                _spriteBatch.DrawString(font, UnitAssistance.GetCurrency(false, Role.WORKER).ToString(), new Vector2((int)attackCoords.X + IconSprite.XScale + 5, (int)attackCoords.Y + 20 + IconSprite.YScale / 2), Color.Gold);
                                break;
                            case BuildingType.Barracks:
                                _spriteBatch.Draw(UnitsTextures.Icons, new Rectangle((int)attackCoords.X + 27, (int)attackCoords.Y + 8, IconSprite.XScale, IconSprite.YScale), IconSprite.GetTextureBounds(Race.HUMAN, Role.WARRIOR), Color.White);
                                break;
                            case BuildingType.Farm:
                                break;
                            case BuildingType.None:
                                break;
                        }
                    }
                }
            }
            else if (map.group.FocusedObj.X != -1)
            {
                _spriteBatch.Draw(obtainButton, moveCords, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(obtainButton, buildCords, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(obtainButton, attackCoords, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(obtainButton, obtainCords, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                if (map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.GetRole() == Role.WORKER)
                {
                    _spriteBatch.Draw(UnitsTextures.Icons, new Rectangle(40, 661, 200, 120), new Rectangle(0, 616, 200, 120), map.buildingType != BuildingType.MainBuild ? Color.White : Color.Gray);
                    _spriteBatch.DrawString(font, BuildAssistance.GetCurrency(true, BuildingType.MainBuild).ToString(), new Vector2((int)attackCoords.X + IconSprite.XScale - 10, (int)attackCoords.Y + 20), Color.SandyBrown);
                    _spriteBatch.DrawString(font, BuildAssistance.GetCurrency(false, BuildingType.MainBuild).ToString(), new Vector2((int)attackCoords.X + IconSprite.XScale - 10, (int)attackCoords.Y + 20 + IconSprite.YScale / 2), Color.Gold);

                    _spriteBatch.Draw(UnitsTextures.Icons, new Rectangle(250, 661, 200, 120), new Rectangle(200, 616, 200, 120), map.buildingType != BuildingType.MainBuild ? Color.White : Color.Gray);
                    _spriteBatch.DrawString(font, BuildAssistance.GetCurrency(true, BuildingType.Barracks).ToString(), new Vector2((int)moveCords.X + IconSprite.XScale - 10, (int)moveCords.Y + 20), Color.SandyBrown);
                    _spriteBatch.DrawString(font, BuildAssistance.GetCurrency(false, BuildingType.Barracks).ToString(), new Vector2((int)moveCords.X + IconSprite.XScale - 10, (int)moveCords.Y + 20 + IconSprite.YScale / 2), Color.Gold);

                    _spriteBatch.Draw(UnitsTextures.Icons, new Rectangle(40, 791, 200, 120), new Rectangle(400, 616, 200, 120), map.buildingType != BuildingType.MainBuild ? Color.White : Color.Gray);
                    _spriteBatch.DrawString(font, BuildAssistance.GetCurrency(true, BuildingType.Farm).ToString(), new Vector2((int)obtainCords.X + IconSprite.XScale - 10, (int)obtainCords.Y + 20), Color.SandyBrown);
                    _spriteBatch.DrawString(font, BuildAssistance.GetCurrency(false, BuildingType.Farm).ToString(), new Vector2((int)obtainCords.X + IconSprite.XScale - 10, (int)obtainCords.Y + 20 + IconSprite.YScale / 2), Color.Gold);

                    _spriteBatch.Draw(UnitsTextures.Icons, new Rectangle(250, 791, 200, 120), new Rectangle(600, 616, 200, 120), Color.White);  

                }
                else if (map.group.buildingType != BuildingType.None)
                {
                    switch (map.group.buildingType)
                    {
                        case BuildingType.MainBuild:
                            _spriteBatch.Draw(UnitsTextures.Icons, new Rectangle((int)attackCoords.X, (int)attackCoords.Y + 8, IconSprite.XScale, IconSprite.YScale), IconSprite.GetTextureBounds(Race.HUMAN, Role.WORKER), Color.Red);
                            _spriteBatch.DrawString(font, UnitAssistance.GetCurrency(true, Role.WORKER).ToString(), new Vector2((int)attackCoords.X + IconSprite.XScale + 5, (int)attackCoords.Y + 20), Color.SandyBrown);
                            _spriteBatch.DrawString(font, UnitAssistance.GetCurrency(false, Role.WORKER).ToString(), new Vector2((int)attackCoords.X + IconSprite.XScale + 5, (int)attackCoords.Y + 20 + IconSprite.YScale / 2), Color.Gold);
                            break;
                        case BuildingType.Barracks:
                            _spriteBatch.Draw(UnitsTextures.Icons, new Rectangle((int)attackCoords.X, (int)attackCoords.Y + 8, IconSprite.XScale, IconSprite.YScale), IconSprite.GetTextureBounds(Race.HUMAN, Role.WARRIOR), Color.White);
                            _spriteBatch.DrawString(font, UnitAssistance.GetCurrency(true, Role.WARRIOR).ToString(), new Vector2((int)attackCoords.X + IconSprite.XScale + 5, (int)attackCoords.Y + 20), Color.SandyBrown);
                            _spriteBatch.DrawString(font, UnitAssistance.GetCurrency(false, Role.WARRIOR).ToString(), new Vector2((int)attackCoords.X + IconSprite.XScale + 5, (int)attackCoords.Y + 20 + IconSprite.YScale / 2), Color.Gold);
                            break;
                        case BuildingType.Farm:
                            break;
                        case BuildingType.None:
                            break;
                    }
                }
            }
            else
            {
                map.buildMode = false;
                map.buildingType = BuildingType.None;
                _spriteBatch.Draw(moveButton, moveCords, null, !map.movingMode ? Color.White : Color.Gray, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(buildButton, buildCords, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(attackButton, attackCoords, null, map.attackMode || Keyboard.GetState().IsKeyDown(Keys.A) ? Color.Gray : Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                _spriteBatch.Draw(obtainButton, obtainCords, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
            }

            _spriteBatch.Draw(components[(int)TextureSPlay.Menu], new Vector2(40, 952), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            if (UnitsTextures.IsLoaded && map.group.FocusedObj.X != -1 && map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit != null)
            {
                try
                {
                    string UnitName;
                    UnitAssistance.UnitName.TryGetValue((int)map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.GetRole(), out UnitName);

                    _spriteBatch.Draw(components[(int)TextureSPlay.Frame], new Rectangle(83, 500, 156, 113), Color.White);
                    _spriteBatch.Draw(components[(int)TextureSPlay.Health], new Rectangle(253, 555, 156, 58), Color.White);
                    _spriteBatch.Draw(UnitsTextures.Icons, new Rectangle(88, 505, IconSprite.XScale, IconSprite.YScale), IconSprite.GetTextureBounds(map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.GetRace(), map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.GetRole()), Color.White);
                    _spriteBatch.DrawString(font, UnitName, new Vector2(251, 515), new Color(54, 26, 32, 255));

                    for (int i = 0; i < map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.GetCurHP() / map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].unit.GetMaxHP() * 145; i++)
                    {
                        _spriteBatch.Draw(components[(int)TextureSPlay.Pixel], new Rectangle(259 + i, 588, 1, 20), Color.Green);
                    }
                }
                catch (Exception) { }
            }
            else if (map.group.FocusedObj.X != -1 && map.group.buildingType != BuildingType.None)
            {
                try
                {
                    string BuildName;
                    BuildAssistance.BuildName.TryGetValue((int)map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].buildingType, out BuildName);

                    _spriteBatch.Draw(components[(int)TextureSPlay.Frame], new Rectangle(83, 500, 156, 113), Color.White);
                    _spriteBatch.Draw(UnitsTextures.Icons, new Rectangle(88, 505, IconSprite.XScale, IconSprite.YScale), IconSprite.GetTextureBounds(map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].buildOf, map.map[map.group.FocusedObj.X, map.group.FocusedObj.Y].buildingType), Color.White);
                    _spriteBatch.DrawString(font, BuildName, new Vector2(251, 515), new Color(54, 26, 32, 255));
                }
                catch (Exception) { }
            }
            _spriteBatch.DrawString(font, map.Wood.ToString(), new Vector2(173, 7), Color.Black);
            _spriteBatch.DrawString(font, map.Gold.ToString(), new Vector2(318, 7), Color.Black);

            _spriteBatch.DrawString(font, map.Wood.ToString(), new Vector2(170, 5), Color.White);
            _spriteBatch.DrawString(font, map.Gold.ToString(), new Vector2(315, 5), Color.White);

            _spriteBatch.Draw(components[(int)TextureSPlay.Resourses], new Rectangle(120, 5, 42, 29), new Rectangle(0, 0, 42, 29), Color.White);
            _spriteBatch.Draw(components[(int)TextureSPlay.Resourses], new Rectangle(280, 5, 27, 29), new Rectangle(42, 0, 27, 29), Color.White);




            _spriteBatch.Draw(components[(int)TextureSPlay.cur], new Vector2(Mouse.GetState().X, Mouse.GetState().Y), map.attackMode || Keyboard.GetState().IsKeyDown(Keys.A) ? Color.Red : map.obtainMode || Keyboard.GetState().IsKeyDown(Keys.D) ? Color.Yellow : Color.White);

            _spriteBatch.End();
        }
    }
}