using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Xna.Framework.Input;

namespace Warcraft_1.GameClasses
{
    enum CameraMaxVal
    {
        X = 44,
        Y = 31
    }
    class Map
    {
        public Texture2D maptiles;
        public Texture2D buildingtiles;
        public Logic_Classes.Group group;

        public Point Camera = new Point(0, mapSize - (int)CameraMaxVal.Y);
        Field[,] map { get; set; }
        private const int fieldPixelSize = 32;
        public const int mapSize = 100;
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
        }

        public void Read(string path)
        {
            map = JsonConvert.DeserializeObject<Field[,]>(File.ReadAllText(path));
            map[7, 86].unit = new Units.HumWorker();
        }
        public void Save(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(map));
            }
            else
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(map));
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
                    spriteBatch.Draw(map[i, j].terrain == TypeOfTerrain.Mine ? buildingtiles : maptiles, new Rectangle(494 + (i - +Camera.X) * fieldPixelSize, 44 + (j - +Camera.Y) * fieldPixelSize, fieldPixelSize, fieldPixelSize), map[i, j].GetFieldTerrain(), Color.White);
                    if (map[i, j].unit != null)
                    {
                        map[i, j].unit.Load();
                        map[i, j].unit.Position = new Vector2(494 + (i - +Camera.X) * fieldPixelSize, 44 + (j - +Camera.Y) * fieldPixelSize);
                        map[i, j].unit.Draw(spriteBatch);
                    }
                }
            }

            if(group.FocusedUnit != new Point(-1,-1))
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
                MouseCoords = new Point((Mouse.GetState().Position.X - 494) / fieldPixelSize + Camera.X, (Mouse.GetState().Position.Y - 44) / fieldPixelSize + Camera.Y);
                if ((MouseCoords.X > 0 && MouseCoords.X < 100) && (MouseCoords.Y > 0 && MouseCoords.Y < 100) && map[MouseCoords.X, MouseCoords.Y].unit != null) group.FocusedUnit = MouseCoords;
                else if ((MouseCoords.X > 0 && MouseCoords.X < 100) && (MouseCoords.Y > 0 && MouseCoords.Y < 100)) group.FocusedUnit = new Point(-1, -1);
            }

            for (int i = 0 + Camera.X; i < (int)CameraMaxVal.X + Camera.X; i++)
            {
                for (int j = 0 + Camera.Y; j < (int)CameraMaxVal.Y + Camera.Y; j++)
                {
                    if (map[i, j].unit != null)
                    {
                        map[i, j].unit.Update(gameTime, i, j);
                    }
                }
            }
        }
    }
}
