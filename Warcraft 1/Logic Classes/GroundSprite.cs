using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraft_1.Logic_Classes
{

    static class GroundSprite
    {
        static Dictionary<int, Point> groundId = new Dictionary<int, Point>()
        {
             //Water
             {1, new Point(231, 0)},
             {2, new Point(264, 0)},
             {3, new Point(297, 0)},
             {4, new Point(198, 33)},
             {5, new Point(231, 33)},
             {6, new Point(297, 33)},
             {7, new Point(330, 33)},
             {8, new Point(198, 66)},
             {9, new Point(330, 66)},
             {10, new Point(198, 99)},
             {11, new Point(231, 99)},
             {12, new Point(297, 99)},
             {13, new Point(330, 99)},
             {14, new Point(231, 132)},
             {15, new Point(264, 132)},
             {16, new Point(297, 132)},
             //Trees
             {17, new Point(396, 0)},
             {18, new Point(429, 0)},
             {19, new Point(462, 0)},
             {20, new Point(396, 33)},
             {21, new Point(429, 33)},
             {22, new Point(462, 33)},
             {23, new Point(396, 66)},
             {24, new Point(429, 66)},
             {25, new Point(462, 66)},
             //BridgeVer
             {26, new Point(33, 231 )},
             {27, new Point(66, 231)},
             {28, new Point(99, 231)},
             {29, new Point(33, 264)},
             {30, new Point(66, 264)},
             {31, new Point(99, 264)},
             {32, new Point(33, 297)},
             {33, new Point(66, 297)},
             {34, new Point(99, 297)},
             {35, new Point(33, 330)},
             {36, new Point(66, 330)},
             {37, new Point(99, 330)},
             {38, new Point(33, 363)},
             {39, new Point(66, 363)},
             {40, new Point(99, 363)},
             {41, new Point(33, 396)},
             {42, new Point(66, 396)},
             {43, new Point(99, 396)},
             {44, new Point(33, 429)},
             {45, new Point(66, 429)},
             {46, new Point(99, 429)},
             //BridgeHor
             {47, new Point(231, 231)},
             {48, new Point(264, 231)},
             {49, new Point(297, 231)},
             {50, new Point(330, 231)},
             {51, new Point(363, 231)},
             {52, new Point(396, 231)},
             {53, new Point(429, 231)},
             {54, new Point(231, 264)},
             {55, new Point(264, 264)},
             {56, new Point(297, 264)},
             {57, new Point(330, 264)},
             {58, new Point(363, 264)},
             {59, new Point(396, 264)},
             {60, new Point(429, 264)},
             {61, new Point(231, 297)},
             {62, new Point(264, 297)},
             {63, new Point(297, 297)},
             {64, new Point(330, 297)},
             {65, new Point(363, 297)},
             {66, new Point(396, 297)},
             {67, new Point(429, 297)},
             //Roads
             {68, new Point(396, 396)},
             {69, new Point(429, 396)},
             {70, new Point(429, 429)},
             {71, new Point(429, 462)},
             {72, new Point(396, 132)},
             {73, new Point(429, 132)},
             {74, new Point(462, 132)},
        };

        static Rectangle GetRecquiredSprite(GameClasses.TypeOfTerrain terrain, int id)
        {
            Point Location = new Point();
            groundId.TryGetValue(GetStartId(terrain), out Location);

            return new Rectangle(Location, new Point(32,32));
        }
        private static int GetStartId(GameClasses.TypeOfTerrain terrain)
        {
            switch (terrain)
            {
                case GameClasses.TypeOfTerrain.Water:
                    return 0;
                case GameClasses.TypeOfTerrain.Tree:
                    return 16;
                case GameClasses.TypeOfTerrain.Bridge:
                    return 25;
                case GameClasses.TypeOfTerrain.Road:
                    return 67;
            }
            return 0;
        }
    }
}