using System.Collections.Generic;

namespace Warcraft_1.SpriteAndUnits
{
    public static class UnitAssistance
    {
        public static Dictionary<int, string> UnitName = new Dictionary<int, string>()
        {
            { 0, "WORKER" },
            { 1, "WARRIOR" },
            { 2, "RIDER" }
        };
        public static int GetCurrency(bool wood, GameClasses.Units.Role role)
        {
            int cur = 0;
            if(wood)
            {
                switch (role)
                {
                    case GameClasses.Units.Role.WORKER:
                        cur = 25;
                        break;
                    case GameClasses.Units.Role.WARRIOR:
                        cur = 50;
                        break;
                    case GameClasses.Units.Role.RIDER:
                        cur = 50;
                        break;
                }
            }
            else
            {
                switch (role)
                {
                    case GameClasses.Units.Role.WORKER:
                        cur = 75;
                        break;
                    case GameClasses.Units.Role.WARRIOR:
                        cur = 125;
                        break;
                    case GameClasses.Units.Role.RIDER:
                        cur = 175;
                        break;
                }
            }
            return cur;
        }
        
    }
}
