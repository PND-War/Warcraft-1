using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Newtonsoft.Json;

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
        public Point Camera = new Point(0, 0);
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
        }

        public void Read()
        {
            map = JsonConvert.DeserializeObject<Field[,]>(File.ReadAllText("map.wc"));
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
                    if(i == (int)CameraMaxVal.X -1 || j == (int)CameraMaxVal.Y -1 || i == 0 || j == 0)
                    {
                        spriteBatch.Draw(pixel, new Rectangle(45 + (i * 4)+Camera.X*4, 45 + (j * 4)+Camera.Y*4, 4, 4), Color.White);
                    }
                }
            }
        }
        public void DrawMain(SpriteBatch spriteBatch)
        {
            for (int i = 0+Camera.X; i < (int)CameraMaxVal.X + Camera.X; i++)
            {
                for (int j = 0 + Camera.Y; j < (int)CameraMaxVal.Y + Camera.Y; j++)
                {
                    spriteBatch.Draw(maptiles, new Rectangle(494 + (i- +Camera.X) * fieldPixelSize, 44 + (j- +Camera.Y) * fieldPixelSize, fieldPixelSize, fieldPixelSize), map[i, j].GetFieldTerrain(), Color.White);
                }
            }
        }
    }
}
