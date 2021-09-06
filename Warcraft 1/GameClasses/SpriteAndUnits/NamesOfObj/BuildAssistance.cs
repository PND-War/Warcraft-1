using System.Collections.Generic;

namespace Warcraft_1.SpriteAndUnits
{
    public static class BuildAssistance
    {
        public static Dictionary<int, string> BuildName = new Dictionary<int, string>()
        {
            { 0, "TOWNHALL" },
            { 1, "BARRACKS" },
            { 2, "FARM" }
        };
        public static int GetCurrency(bool wood, GameClasses.MapClasses.BuildingType name)
        {
            int cur = 0;
            if (wood)
            {
                switch (name)
                {
                    case GameClasses.MapClasses.BuildingType.MainBuild:
                        cur = 500;
                        break;
                    case GameClasses.MapClasses.BuildingType.Barracks:
                        cur = 325;
                        break;
                    case GameClasses.MapClasses.BuildingType.Farm:
                        cur = 175;
                        break;
                    case GameClasses.MapClasses.BuildingType.None:
                        break;
                }
            }
            else
            {
                switch (name)
                {
                    case GameClasses.MapClasses.BuildingType.MainBuild:
                        cur = 450;
                        break;
                    case GameClasses.MapClasses.BuildingType.Barracks:
                        cur = 275;
                        break;
                    case GameClasses.MapClasses.BuildingType.Farm:
                        cur = 125;
                        break;
                    case GameClasses.MapClasses.BuildingType.None:
                        break;
                }
            }
            return cur;
        }
    }
}
