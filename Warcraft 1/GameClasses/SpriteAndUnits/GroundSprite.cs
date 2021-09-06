using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Warcraft_1.GameClasses.MapClasses;

namespace Warcraft_1.SpriteAndUnits
{
    public static class GroundSprite
    {
        private static int fieldSize = 33;
        public static Dictionary<int, Point> groundId = new Dictionary<int, Point>()
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
             {17, new Point(264, 66)},
             {18, new Point(198, 0)},
             //Trees
             {19, new Point(396, 0)},
             {20, new Point(429, 0)},
             {21, new Point(462, 0)},
             {22, new Point(396, 33)},
             {23, new Point(429, 33)},
             {24, new Point(462, 33)},
             {25, new Point(396, 66)},
             {26, new Point(429, 66)},
             {27, new Point(462, 66)},
             //BridgeVer
             {28, new Point(33, 264)},
             {29, new Point(66, 264)},
             {30, new Point(99, 264)},
             {31, new Point(33, 297)},
             {32, new Point(66, 297)},
             {33, new Point(99, 297)},
             {34, new Point(33, 330)},
             {35, new Point(66, 330)},
             {36, new Point(99, 330)},
             {37, new Point(33, 363)},
             {38, new Point(66, 363)},
             {39, new Point(99, 363)},
             {40, new Point(33, 396)},
             {41, new Point(66, 396)},
             {42, new Point(99, 396)},
             //BridgeHor
             {43, new Point(264, 231)},
             {44, new Point(297, 231)},
             {45, new Point(330, 231)},
             {46, new Point(363, 231)},
             {47, new Point(396, 231)},
             {48, new Point(264, 264)},
             {49, new Point(297, 264)},
             {50, new Point(330, 264)},
             {51, new Point(363, 264)},
             {52, new Point(396, 264)},
             {53, new Point(264, 297)},
             {54, new Point(297, 297)},
             {55, new Point(330, 297)},
             {56, new Point(363, 297)},
             {57, new Point(396, 297)},
             //Roads
             {58, new Point(396, 396)},
             {59, new Point(429, 396)},
             {60, new Point(429, 429)},
             {61, new Point(429, 462)},
             {62, new Point(396, 132)},
             {63, new Point(429, 132)},
             {64, new Point(462, 132)},
             {65, new Point(363, 165)},
             {66, new Point(396, 165)},
             {67, new Point(429, 165) },
             {68, new Point(462, 165)},
             {69, new Point(363, 0)}, //rock1
             {70, new Point(363, 33)}, //rock2
             {71, new Point(363, 66)}, //rock3
             {72, new Point(363, 99)}, //crater1
             {73, new Point(363, 132)}, //crater2
             //Special
             {74, new Point(132, 165)}, //unchecked
             {75, new Point(396, 99)}, //defaultground
             {76, new Point(198, 165)}, //watergroundslevaverhniz 7
             {77, new Point(231, 165)}, //watergroundslevaverhniz
             {78, new Point(264, 165)}, //watergroundslevaverhniz
             {79, new Point(297, 165)}, //watergroundslevaverhniz
             {80, new Point(330, 165)}, //watergroundslevaverhnizchtototam
             //Buildings
             //mine
             {81, new Point(0, 0)},
             {82, new Point(33, 0)},
             {83, new Point(66, 0)},
             {84, new Point(0, 33)},
             {85, new Point(33, 33)},
             {86, new Point(66, 33)},
             {87, new Point(0, 66)},
             {88, new Point(33, 66)},
             {89, new Point(66, 66)},
             //mainBuild
             {90, new Point(fieldSize * 9, 0)},
             {91, new Point(fieldSize*10, 0)},
             {92, new Point(fieldSize*11, 0)},
             {93, new Point(fieldSize * 9, fieldSize)},
             {94, new Point(fieldSize * 10, fieldSize)},
             {95, new Point(fieldSize * 11, fieldSize)},
             {96, new Point(fieldSize * 9, fieldSize*2)},
             {97, new Point(fieldSize * 10, fieldSize*2)},
             {98, new Point(fieldSize * 11, fieldSize*2)},
             //barracks
             {99, new Point(fieldSize * 12, 0)},
             {100, new Point(fieldSize*13, 0)},
             {101, new Point(fieldSize*14, 0)},
             {102, new Point(fieldSize * 12, fieldSize)},
             {103, new Point(fieldSize * 13, fieldSize)},
             {104, new Point(fieldSize * 14, fieldSize)},
             {105, new Point(fieldSize * 12, fieldSize*2)},
             {106, new Point(fieldSize * 13, fieldSize*2)},
             {107, new Point(fieldSize * 14, fieldSize*2)},
             //farm
             {108, new Point(fieldSize * 6, 0)},
             {109, new Point(fieldSize*7, 0)},
             {110, new Point(fieldSize*8, 0)},
             {111, new Point(fieldSize * 6, fieldSize)},
             {112, new Point(fieldSize * 7, fieldSize)},
             {113, new Point(fieldSize * 8, fieldSize)},
             {114, new Point(fieldSize * 6, fieldSize*2)},
             {115, new Point(fieldSize * 7, fieldSize*2)},
             {116, new Point(fieldSize * 8, fieldSize*2)},
        };

        public static Rectangle GetRecquiredSprite(TypeOfTerrain terrain, int id)
        {
            Point Location = new Point();
            groundId.TryGetValue(GetStartId(terrain) + id, out Location);

            return new Rectangle(Location, new Point(32, 32));
        }
        public static Rectangle GetRecquiredSprite(int id)
        {
            Point Location = new Point();
            groundId.TryGetValue(id, out Location);

            return new Rectangle(Location, new Point(32, 32));
        }
        public static int GetStartId(TypeOfTerrain terrain)
        {
            switch (terrain)
            {
                case TypeOfTerrain.Water:
                    return 0;
                case TypeOfTerrain.Tree:
                    return 18;
                case TypeOfTerrain.Bridge:
                    return 27;
                case TypeOfTerrain.Road:
                    return 57;
                case TypeOfTerrain.Mine:
                    return 80;
            }
            return 0;
        }
    }
}