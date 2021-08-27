using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Warcraft_1.GameClasses
{
    class Map
    {
        Field[,] map { get; set; } 
        public Map()
        {
            map = new Field[100, 100];
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    map[i, j] = new Field(TypeOfTerrain.Earth, true);
                }
            }
        }



        public void DrawMini(SpriteBatch spriteBatch, Texture2D pixel)
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    spriteBatch.Draw(pixel, new Rectangle(45+(i*4), 45+(j*4), 4, 4), map[i, j].GetFieldColor());
                }
            }
        }
    }
}
