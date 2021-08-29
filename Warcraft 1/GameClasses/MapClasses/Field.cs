using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraft_1.GameClasses
{
    public enum TypeOfTerrain
    {
        Earth,
        Water,
        Road,
        Tree,
        Mine,
        Bridge
    }
    class Field
    {
        public TypeOfTerrain terrain { get; set; }
        public bool cheked { get; set; } = true;
        public Texture2D fieldTexture { get; set; }
        public Units.AUnit unit { get; set; }
        public int spriteId { get; set; }
        //public Field()
        //{
        //    cheked = false;
        //    terrain = TypeOfTerrain.Earth;
        //}
        //public Field(Texture2D texture)
        //{
        //    cheked = false;
        //    terrain = TypeOfTerrain.Earth;
        //    fieldTexture = texture;
        //}
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
                    return new Rectangle(396, 99, 32, 32);
                case TypeOfTerrain.Water:
                    return new Rectangle(264, 66, 32, 32);
                case TypeOfTerrain.Road:
                    return Logic_Classes.GroundSprite.GetRecquiredSprite(TypeOfTerrain.Road, spriteId);
                case TypeOfTerrain.Tree:
                    return new Rectangle(429, 33, 32, 32);
                case TypeOfTerrain.Mine:
                    return new Rectangle(264, 66, 32, 32);
                case TypeOfTerrain.Bridge:
                    return new Rectangle(66, 264, 32, 32);
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
        private bool CheckFriendly(Units.AUnit unitG)
        {
            if (unitG.GetRace() == Units.Race.HUMAN) return true;
            return false;
        }

    }
}
