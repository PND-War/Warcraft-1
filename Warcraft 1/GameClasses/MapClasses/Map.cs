using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Runtime.Serialization;

namespace Warcraft_1.GameClasses
{
    enum CameraMaxVal
    {
        X = 44,
        Y = 31
    }
    [DataContract]
    class Map
    {
        public Texture2D maptiles;
        public Texture2D buildingtiles;
        public Logic_Classes.Group group;
        public ContentManager Content;
        public SoundEffectInstance soundInstance;
        public Point Camera = new Point(0, mapSize - (int)CameraMaxVal.Y);
        public const int fieldPixelSize = 32;
        public const int mapSize = 100;
        [DataMember]
        public Field[,] map { get; set; }
        [DataMember]
        public int Wood;
        [DataMember]
        public int Gold;
        [DataMember]
        public System.Collections.Generic.List<Units.HumWorker> units;
        [DataMember]
        public System.Collections.Generic.List<Point> unitsCords;
        public Map()
        {
            map = new Field[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    map[i, j] = new Field();
                }
            }
            group = new Logic_Classes.Group();
            units = new System.Collections.Generic.List<Units.HumWorker>();
            unitsCords = new System.Collections.Generic.List<Point>();
        }
        public void CreateUnit(int x, int y, Units.Race race, Units.Role role)
        {
            bool human = true;
            if (race == Units.Race.ORC)
            {
                human = false;
            }
            switch (role)
            {
                case Units.Role.WORKER:
                    if (human)
                    {
                        map[x, y].unit = new Units.HumWorker();
                    }
                    else
                    {
                        map[x, y].unit = new Units.OrcWorker();
                    }
                    break;
                case Units.Role.WARRIOR:
                    if (human)
                    {
                        map[x, y].unit = new Units.HumWarrior();
                    }
                    else
                    {
                        map[x, y].unit = new Units.OrcWarrior();
                    }
                    break;
                case Units.Role.NONE:
                    break;
            }
            if (map[x, y].unit != null)
            {
                map[x, y].unit.positionToMove = new Point(x, y);
                if (human)
                {
                    CheckAround(x, y);
                }
            }
        }
        public void CheckAround(int x, int y)
        {
            const int checkRad = 5;
            for (int i = checkRad * -1; i <= checkRad; i++)
            {
                for (int j = checkRad * -1; j <= checkRad; j++)
                {
                    int c = 0;
                    if (i == checkRad * -1)
                        c++;
                    if (j == checkRad * -1)
                        c++;
                    if (i == checkRad)
                        c++;
                    if (j == checkRad)
                        c++;
                    if (c != 2 && x + i < 100 && x + i >= 0 && y + j < 100 && y + j >= 0)
                        map[x + i, y + j].cheked = true;
                }
            }
        }

        public void Read(string path)
        {
            try
            {
                Map tmp = JsonConvert.DeserializeObject<Map>(File.ReadAllText(path));
                this.map = tmp.map;
                this.Gold = tmp.Gold;
                this.Wood = tmp.Wood;
                if (tmp.units != null)
                {
                    for (int i = 0; i < tmp.units.Count; i++)
                    {
                        map[tmp.unitsCords[i].X, tmp.unitsCords[i].Y].unit = tmp.units[i];
                    }
                }
                if (path == "map.wc")
                    CreateUnit(9, 90, Units.Race.HUMAN, Units.Role.WORKER);


            }
            catch (Exception)
            {
                Map tmp = JsonConvert.DeserializeObject<Map>(File.ReadAllText("map.wc"));
                this.map = tmp.map;
                this.Gold = tmp.Gold;
                this.Wood = tmp.Wood;
                CreateUnit(9, 90, Units.Race.HUMAN, Units.Role.WORKER);
            }

        }
        public void Save(string path)
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j].unit != null)
                    {
                        map[i, j].unit.IsLoaded = false;
                        units.Add(map[i, j].unit as Units.HumWorker);
                        unitsCords.Add(new Point(i, j));
                    }
                }
            }
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(this));
            }
            else
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(this));
            }

        }

        public void DrawMini(SpriteBatch spriteBatch, Texture2D pixel)
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    spriteBatch.Draw(pixel, new Rectangle(45 + (i * 4), 45 + (j * 4), 4, 4), map[i, j].GetFieldColor());
                }
            }
            for (int i = 0; i < (int)CameraMaxVal.X; i++)
            {
                for (int j = 0; j < (int)CameraMaxVal.Y; j++)
                {
                    if (i == (int)CameraMaxVal.X - 1 || j == (int)CameraMaxVal.Y - 1 || i == 0 || j == 0)
                    {
                        spriteBatch.Draw(pixel, new Rectangle(45 + (i * 4) + Camera.X * 4, 45 + (j * 4) + Camera.Y * 4, 4, 4), Color.White);
                    }
                }
            }
        }
        public void DrawMain(SpriteBatch spriteBatch, Texture2D pixel)
        {
            for (int i = 0 + Camera.X; i < (int)CameraMaxVal.X + Camera.X; i++)
            {
                for (int j = 0 + Camera.Y; j < (int)CameraMaxVal.Y + Camera.Y; j++)
                {
                    if (map[i, j].cheked) spriteBatch.Draw(map[i, j].terrain == TypeOfTerrain.Mine ? buildingtiles : maptiles, new Rectangle(494 + (i - +Camera.X) * fieldPixelSize, 44 + (j - +Camera.Y) * fieldPixelSize, fieldPixelSize, fieldPixelSize), map[i, j].GetFieldTerrain(), Color.White);
                    else spriteBatch.Draw(maptiles, new Rectangle(494 + (i - +Camera.X) * fieldPixelSize, 44 + (j - +Camera.Y) * fieldPixelSize, fieldPixelSize, fieldPixelSize), Logic_Classes.GroundSprite.GetRecquiredSprite(74), Color.White);

                    if (map[i, j].unit != null && !map[i, j].unit.IsMoving && map[i, j].cheked)
                    {
                        map[i, j].unit.Load(Content);
                        spriteBatch.Draw(map[i, j].unit.Texture, new Rectangle(494 + (i - +Camera.X) * fieldPixelSize, 44 + (j - +Camera.Y) * fieldPixelSize, fieldPixelSize, fieldPixelSize), map[i, j].unit.Rect, Color.White);
                    }
                }
            }
            for (int i = 0 + Camera.X; i < (int)CameraMaxVal.X + Camera.X; i++)
            {
                for (int j = 0 + Camera.Y; j < (int)CameraMaxVal.Y + Camera.Y; j++)
                {
                    if (map[i, j].cheked)
                    {
                        if (map[i, j].unit != null && map[i, j].unit.IsMoving)
                        {
                            map[i, j].unit.Load(Content);
                            spriteBatch.Draw(map[i, j].unit.Texture, new Rectangle(map[i, j].unit.positionInMoving.X, map[i, j].unit.positionInMoving.Y, fieldPixelSize, fieldPixelSize), map[i, j].unit.Rect, Color.White);
                        }
                    }

                }
            }
            if (group.FocusedUnit != new Point(-1, -1))
            {
                for (int i = 0; i < (int)fieldPixelSize; i++)
                {
                    for (int j = 0; j < (int)fieldPixelSize; j++)
                    {
                            if (i == fieldPixelSize - 1 || j == fieldPixelSize - 1 || i == 0 || j == 0)
                            {
                                spriteBatch.Draw(pixel, new Rectangle(494 + (i) + (group.FocusedUnit.X - Camera.X) * fieldPixelSize, 44 + (j) + (group.FocusedUnit.Y - Camera.Y) * fieldPixelSize, 2, 2), Color.Lime);
                            }
                            

                    }
                }
            }
        }
        public void Update(GameTime gameTime)
        {
            Point MouseCoords = new Point(-1, -1);

            if (Logic_Classes.MouseInterpretator.GetPressed(Logic_Classes.MouseButton.Left))
            {
                Logic_Classes.MouseInterpretator.ResetInter();
                MouseCoords = new Point((Mouse.GetState().Position.X - 494) / fieldPixelSize + Camera.X, (Mouse.GetState().Position.Y - 44) / fieldPixelSize + Camera.Y);
                if ((MouseCoords.X > 0 && MouseCoords.X < 100) && (MouseCoords.Y > 0 && MouseCoords.Y < 100) && map[MouseCoords.X, MouseCoords.Y].unit != null) group.FocusedUnit = MouseCoords;
                else if ((MouseCoords.X > 0 && MouseCoords.X < 100) && (MouseCoords.Y > 0 && MouseCoords.Y < 100)) group.FocusedUnit = new Point(-1, -1);

            }
            else if (Logic_Classes.MouseInterpretator.GetPressed(Logic_Classes.MouseButton.Right))
            {
                if (group.FocusedUnit.X != -1 && map[group.FocusedUnit.X, group.FocusedUnit.Y].unit != null && !map[group.FocusedUnit.X, group.FocusedUnit.Y].unit.IsMoving && new Rectangle(494, 44, 1382, 992).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    MouseCoords = new Point((Mouse.GetState().Position.X - 494) / fieldPixelSize + Camera.X, (Mouse.GetState().Position.Y - 44) / fieldPixelSize + Camera.Y);
                    if (map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Tree && map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Water && map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Mine)
                    {
                        try
                        {
                            map[group.FocusedUnit.X, group.FocusedUnit.Y].unit.positionToMove = MouseCoords;
                            if (map[group.FocusedUnit.X, group.FocusedUnit.Y].unit.action != null)
                            {
                                soundInstance = map[group.FocusedUnit.X, group.FocusedUnit.Y].unit.action.CreateInstance();
                                soundInstance.Volume = Logic_Classes.Settings.SFXVol ? 0.35f : 0.0f;
                            }
                        }
                        catch (Exception) { }
                    }
                }
                else if (group.FocusedUnit.X != -1 && map[group.FocusedUnit.X, group.FocusedUnit.Y].unit != null && !map[group.FocusedUnit.X, group.FocusedUnit.Y].unit.IsMoving && new Rectangle(45, 45, 400, 400).Contains(Mouse.GetState().X, Mouse.GetState().Y))
                {
                    MouseCoords = new Point((Mouse.GetState().Position.X - 45) / 4, (Mouse.GetState().Position.Y - 45) / 4);
                    if (map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Tree && map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Water && map[MouseCoords.X, MouseCoords.Y].terrain != TypeOfTerrain.Mine)
                    {
                        try
                        {
                            map[group.FocusedUnit.X, group.FocusedUnit.Y].unit.positionToMove = MouseCoords;
                            if (map[group.FocusedUnit.X, group.FocusedUnit.Y].unit.action != null)
                            {
                                soundInstance = map[group.FocusedUnit.X, group.FocusedUnit.Y].unit.action.CreateInstance();
                                soundInstance.Volume = Logic_Classes.Settings.SFXVol ? 0.35f : 0.0f;
                            }
                        }
                        catch (Exception) { }
                    }
                }
            }
            for (int i = 0 + Camera.X; i < (int)CameraMaxVal.X + Camera.X; i++)
            {
                for (int j = 0 + Camera.Y; j < (int)CameraMaxVal.Y + Camera.Y; j++)
                {
                    if (map[i, j].unit != null)
                    {
                        map[i, j].unit.Update(gameTime);
                    }
                }
            }
        }
    }
}
