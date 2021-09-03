using Microsoft.Xna.Framework;
using Warcraft_1.GameClasses.Units;

namespace Warcraft_1.SpriteAndUnits
{
    static class IconSprite
    {
        public static int XScale = 146;
        public static int YScale = 103;
        static public Rectangle GetTextureBounds(Race race, Role role)
        {
            
            Rectangle pt = new Rectangle();
            switch (race)
            {
                case Race.HUMAN:
                    switch (role)
                    {
                        case Role.WORKER:
                            pt = new Rectangle(583, 0, XScale, YScale);
                            break;
                        case Role.WARRIOR:
                            pt = new Rectangle(0, 0, XScale, YScale);
                            break;
                        case Role.NONE:
                            pt = new Rectangle(1022, 206, XScale, YScale);
                            break;
                    }
                    break;
                case Race.ORC:
                    switch (role)
                    {
                        case Role.WORKER:
                            pt = new Rectangle(729, 0, XScale, YScale);
                            break;
                        case Role.WARRIOR:
                            pt = new Rectangle(146, 0, XScale, YScale);
                            break;
                        case Role.NONE:
                            pt = new Rectangle(1022, 206, XScale, YScale);
                            break;
                    }
                    break;
            }
            return pt;
        }
    }
}
