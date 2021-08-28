using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Text.Json;

using System.IO;
using Newtonsoft.Json;

namespace Warcraft_1.GameClasses
{
    class Map
    {
        public Texture2D maptiles;
        private Point Camera = new Point(0, 0);
        Field[,] map { get; set; }
        public Map()
        {
            map = new Field[100, 100];
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
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
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    spriteBatch.Draw(pixel, new Rectangle(45 + (i * 4), 45 + (j * 4), 4, 4), map[i, j].GetFieldColor());
                }
            }
        }
        public void DrawMain(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 44; i++)
            {
                for (int j = 0; j < 31; j++)
                {
                    spriteBatch.Draw(maptiles, new Rectangle(494 + i * 32, 44 + j * 32, 32, 32), map[i, j].GetFieldTerrain(), Color.White);
                }
            }
        }
    }
}
