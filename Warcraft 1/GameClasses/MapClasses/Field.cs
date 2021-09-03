using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Warcraft_1.GameClasses
{
    public enum TypeOfTerrain
    {
        Earth,
        Water,
        Road,
        Tree,
        Mine,
        Bridge,
        Building
    }
    public enum BuildingType
    {
        None
    }
    class Field
    {
        public TypeOfTerrain terrain { get; set; }
        public bool cheked { get; set; } = true;
        [JsonIgnore]
        public Units.AUnit unit { get; set; }
        public int spriteId { get; set; }

        public Units.Race buildOf = Units.Race.NONE;
        public BuildingType buildingType = BuildingType.None;
        public bool CheckTerrain()
        {
            bool res = false;
            if (!cheked)
            {
                cheked = true;
                res = true;
            }
            return res;
        }
        public Color GetFieldColor()
        {
            Color returnableColor = new Color();
            switch (terrain)
            {
                case TypeOfTerrain.Earth:
                    returnableColor = Color.ForestGreen;
                    break;
                case TypeOfTerrain.Water:
                    returnableColor = Color.Blue;
                    break;
                case TypeOfTerrain.Road:
                    returnableColor = Color.SandyBrown;
                    break;
                case TypeOfTerrain.Tree:
                    returnableColor = Color.Green;
                    break;
                case TypeOfTerrain.Mine:
                    returnableColor = Color.Yellow;
                    break;
                case TypeOfTerrain.Bridge:
                    returnableColor = Color.DarkOrange;
                    break;
                case TypeOfTerrain.Building:
                    returnableColor = GetBuildColor(buildOf);
                    break;
            }
            if (unit != null)
            {
                returnableColor = GetUnitColor(unit);
            }
            if (!cheked)
            {
                returnableColor = Color.Black;
            }
            return returnableColor;
        }
        public Rectangle GetFieldTerrain()
        {
            switch (terrain)
            {
                case TypeOfTerrain.Earth:
                    return Logic_Classes.GroundSprite.GetRecquiredSprite(TypeOfTerrain.Water, spriteId);
                case TypeOfTerrain.Water:
                    return Logic_Classes.GroundSprite.GetRecquiredSprite(TypeOfTerrain.Water, spriteId);
                case TypeOfTerrain.Road:
                    return Logic_Classes.GroundSprite.GetRecquiredSprite(TypeOfTerrain.Road, spriteId);
                case TypeOfTerrain.Tree:
                    return Logic_Classes.GroundSprite.GetRecquiredSprite(TypeOfTerrain.Tree, spriteId);
                case TypeOfTerrain.Mine:
                    return Logic_Classes.GroundSprite.GetRecquiredSprite(TypeOfTerrain.Mine, spriteId);
                case TypeOfTerrain.Bridge:
                    return Logic_Classes.GroundSprite.GetRecquiredSprite(TypeOfTerrain.Bridge, spriteId);
            }
            return new Rectangle(264, 66, 32, 32);
        }

        public bool PlaceAUnit(Units.AUnit unitG)
        {
            bool res = false;
            if (unit == null && (terrain == TypeOfTerrain.Earth || terrain == TypeOfTerrain.Road || terrain == TypeOfTerrain.Bridge))
            {
                unit = unitG;
                res = true;
            }
            return res;
        }
        public bool ClearUnitPlace()
        {
            bool res = false;
            if (unit != null)
            {
                unit = null;
                res = true;
            }
            return res;
        }
        private Color GetUnitColor(Units.AUnit unitG)
        {
            return CheckFriendly(unitG) ? Color.GreenYellow : Color.OrangeRed;
        }
        private Color GetBuildColor(Units.Race race)
        {
            return CheckFriendly(race) ? Color.GreenYellow : Color.OrangeRed;
        }
        private bool CheckFriendly(Units.AUnit unitG)
        {
            if (unitG.GetRace() == Units.Race.HUMAN) return true;
            return false;
        }
        private bool CheckFriendly(Units.Race race)
        {
            if (race == Units.Race.HUMAN) return true;
            return false;
        }
    }
}
