using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Content;
using System.Runtime.Serialization;
using Warcraft_1.GameClasses.Units;

namespace Warcraft_1.GameClasses.MapClasses
{
    [DataContract]
    class Map
    {
        public bool movingMode = false;
        public bool buildMode = false;
        public bool attackMode = false;
        public BuildingType buildingType = BuildingType.None;

        public Texture2D maptiles;
        public Texture2D buildingtiles;
        public Logic_Classes.Group group;
        public ContentManager Content;
        public Point Camera = new Point(0, mapSize - (int)CameraMaxVal.Y);
        public bool obtainMode = false;
        public const int fieldPixelSize = 32;
        public const int mapSize = 100;
        [DataMember]
        public Field[,] map { get; set; }
        [DataMember]
        public int Wood;
        [DataMember]
        public int Gold;
        [DataMember]
        public System.Collections.Generic.List<HumWorker> units;
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
            units = new System.Collections.Generic.List<HumWorker>();
            unitsCords = new System.Collections.Generic.List<Point>();
        }
        public void StartTimer(int x, int y)
        {
            if (map[x, y].buildingType == BuildingType.Farm)
            {
                map[x, y].timer = new System.Timers.Timer(1000);
                map[x, y].timer.Elapsed += (s, e) => { this.Gold++; this.Wood++; };
                map[x, y].timer.Start();
            }
        }
        public Point CheckPosAround(Point pos)
        {
            int step = 1;
            int x = 0;
            int y = 0;
            while(map[pos.X + x, pos.Y + y].terrain == TypeOfTerrain.Water || map[pos.X+x, pos.Y + y].terrain == TypeOfTerrain.Tree || map[pos.X + x, pos.Y + y].terrain == TypeOfTerrain.Mine || map[pos.X + x, pos.Y + y].terrain == TypeOfTerrain.Building || map[pos.X + x, pos.Y + y].unit != null)
            {
                if(step != 7)
                {
                    switch(step)
                    {
                        case 1:
                            if(pos.X + x+1 < 100)
                                x++;
                            break;
                        case 2:
                            if (pos.X - x > 0)
                                x *= -1;
                            break;
                        case 3:
                            if (pos.Y + y + 1 < 100)
                                y++;
                            break;
                        case 4:
                            if (pos.Y - y > 0)
                                y *=-1;
                            break;
                        case 5:
                            if (pos.Y + y + 1 < 100)
                                y--;
                            if (pos.X - x > 0)
                                x *= -1;
                            break;
                        case 6:
                            if (pos.Y - y > 0)
                                y *= -1;
                            break;
                    }
                    step++;
                }
                else
                {
                    y *= -1;
                    step = 1;
                }
            }
            return new Point(pos.X + x, pos.Y + y);
        }
        public void CreateUnit(int x, int y, Race race, Role role)
        {
            Point point = CheckPosAround(new Point(x, y));
            bool human = true;
            if (race == Race.ORC)
            {
                human = false;
            }
            switch (role)
            {
                case Role.WORKER:
                    if (human)
                    {
                        map[point.X, point.Y].unit = new HumWorker();
                    }
                    else
                    {
                        map[point.X, point.Y].unit = new OrcWorker();
                    }
                    break;
                case Role.WARRIOR:
                    if (human)
                    {
                        map[point.X, point.Y].unit = new HumWarrior();
                    }
                    else
                    {
                        map[point.X, point.Y].unit = new OrcWarrior();
                    }
                    break;
                case Role.NONE:
                    break;
            }
            if (map[point.X, point.Y].unit != null)
            {
                map[point.X, point.Y].unit.positionToMove = new Point(point.X, point.Y);
                if (human)
                {
                    CheckAround(point.X, point.Y);
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
            }
            catch (Exception)
            {
                Map tmp = JsonConvert.DeserializeObject<Map>(File.ReadAllText("map.wc"));
                this.map = tmp.map;
                this.Gold = tmp.Gold;
                this.Wood = tmp.Wood;
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
                        map[i, j].unit.IsMoving = false;
                        map[i, j].unit.positionToMove = new Point(i, j);
                        units.Add(map[i, j].unit as HumWorker);
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
                    if (map[i, j].cheked) spriteBatch.Draw((map[i, j].terrain == TypeOfTerrain.Mine || map[i, j].terrain == TypeOfTerrain.Building) ? buildingtiles : maptiles, new Rectangle(494 + (i - +Camera.X) * fieldPixelSize, 44 + (j - +Camera.Y) * fieldPixelSize, fieldPixelSize, fieldPixelSize), map[i, j].GetFieldTerrain(), Color.White);
                    else spriteBatch.Draw(maptiles, new Rectangle(494 + (i - +Camera.X) * fieldPixelSize, 44 + (j - +Camera.Y) * fieldPixelSize, fieldPixelSize, fieldPixelSize), SpriteAndUnits.GroundSprite.GetRecquiredSprite(74), Color.White);

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
            if (group.FocusedObj != new Point(-1, -1))
            {
                if(group.buildingType == BuildingType.None)
                {
                    for (int i = 0; i < (int)fieldPixelSize; i++)
                    {
                        for (int j = 0; j < (int)fieldPixelSize; j++)
                        {
                            if (i == fieldPixelSize - 1 || j == fieldPixelSize - 1 || i == 0 || j == 0)
                            {
                                spriteBatch.Draw(pixel, new Rectangle(494 + (i) + (group.FocusedObj.X - Camera.X) * fieldPixelSize, 44 + (j) + (group.FocusedObj.Y - Camera.Y) * fieldPixelSize, 2, 2), Color.Lime);
                            }


                        }
                    }
                }
                else
                {
                    for (int i = 0; i < (int)fieldPixelSize*3; i++)
                    {
                        for (int j = 0; j < (int)fieldPixelSize*3; j++)
                        {
                            if (i == fieldPixelSize*3 - 1 || j == fieldPixelSize*3 - 1 || i == 0 || j == 0)
                            {
                                spriteBatch.Draw(pixel, new Rectangle(494 + (i) + (group.FocusedObj.X - Camera.X) * fieldPixelSize, 44 + (j) + (group.FocusedObj.Y - Camera.Y) * fieldPixelSize, 2, 2), Color.Lime);
                            }


                        }
                    }
                }
            }
            if (buildMode && buildingType != BuildingType.None)
            {
                Point MouseCoords = new Point((Mouse.GetState().Position.X - 494) / fieldPixelSize + Camera.X, (Mouse.GetState().Position.Y - 44) / fieldPixelSize + Camera.Y);
                if ((MouseCoords.X > 0 && MouseCoords.X < 97) && (MouseCoords.Y > 0 && MouseCoords.Y < 97))
                {
                    for (int i = MouseCoords.X; i < MouseCoords.X + 3; i++)
                    {
                        for (int j = MouseCoords.Y; j < MouseCoords.Y + 3; j++)
                        {
                            if (map[i, j].cheked)
                                spriteBatch.Draw(pixel, new Rectangle(494 + ((i - Camera.X) * fieldPixelSize), 44 + ((j - Camera.Y) * fieldPixelSize), 32, 32), map[i, j].terrain == TypeOfTerrain.Earth && map[i,j].unit == null ? Color.Green * 0.5f : Color.Red * 0.5f);
                        }
                    }
                }
            }


        }
        public bool CheckTerrainToBuild(int x, int y)
        {
            bool res = true;
            if(x > 0 && x < 97 && y > 0 && y < 97)
            {
                for(int i = x; i < x+3; i++)
                {
                    for (int j = y; j < y + 3; j++)
                    {
                        if(map[i,j].terrain != TypeOfTerrain.Earth || map[i, j].unit != null)
                        {
                            res = false;
                        }
                    }
                }
            }
            else
            {
                res = false;
            }
            return res;
        }
       
        public void Update(GameTime gameTime)
        {
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
