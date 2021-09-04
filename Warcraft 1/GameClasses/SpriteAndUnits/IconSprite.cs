using Microsoft.Xna.Framework;
using Warcraft_1.GameClasses.MapClasses;
using Warcraft_1.GameClasses.Units;

namespace Warcraft_1.SpriteAndUnits
{
    static class IconSprite
    {
        public static int XScale = 146;
        public static int YScale = 103;
        public static Rectangle obtainIcon = new Rectangle(XScale * 6, YScale*2, XScale, YScale-1);
        public static Rectangle buildIcon = new Rectangle(XScale * 4, YScale*2, XScale, YScale-1);
        static public Rectangle GetTextureBounds(Race race, Role role)
        {
            
            Rectangle pt = new Rectangle();
            switch (race)
            {
                case Race.HUMAN:
                    switch (role)
                    {
                        case Role.WORKER:
                            pt = new Rectangle(XScale*4-1, 0, XScale, YScale);
                            break;
                        case Role.WARRIOR:
                            pt = new Rectangle(0, 0, XScale, YScale);
                            break;
                        case Role.NONE:
                            pt = new Rectangle(XScale*7, YScale*2, XScale, YScale-5);
                            break;
                    }
                    break;
                case Race.ORC:
                    switch (role)
                    {
                        case Role.WORKER:
                            pt = new Rectangle(XScale*5-1, 0, XScale, YScale);
                            break;
                        case Role.WARRIOR:
                            pt = new Rectangle(XScale-1, 0, XScale, YScale);
                            break;
                        case Role.NONE:
                            pt = new Rectangle(XScale*7, YScale * 2, XScale, YScale-5);
                            break;
                    }
                    break;
            }
            return pt;
        }
        static public Rectangle GetTextureBounds(Race race, BuildingType type)
        {

            Rectangle pt = new Rectangle();
            switch (race)
            {
                case Race.HUMAN:
                    switch (type)
                    {
                        case BuildingType.MainBuild:
                            pt = new Rectangle(XScale * 6, YScale, XScale, YScale);
                            break;
                        case BuildingType.Barracks:
                            pt = new Rectangle(XScale * 2, YScale, XScale, YScale);
                            break;
                        case BuildingType.Farm:
                            pt = new Rectangle(0, YScale, XScale, YScale);
                            break;
                    }
                    break;
                case Race.ORC:
                    switch (type)
                    {
                        case BuildingType.MainBuild:
                            pt = new Rectangle(XScale*7, YScale, XScale, YScale);
                            break;
                        case BuildingType.Barracks:
                            pt = new Rectangle(XScale * 3, YScale, XScale, YScale);
                            break;
                        case BuildingType.Farm:
                            pt = new Rectangle(XScale, YScale, XScale, YScale);
                            break;
                    }
                    break;
            }
            return pt;
        }

    }
}
