using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraft_1.GameClasses
{
    enum TypeOfTerrain
    {
        Earth,
        Wather,
        Road,
        Tree
    }
    class Field
    {
        TypeOfTerrain terrain;
        Units.AUnit unit;
        public Color GetFieldColor()
        {
            Color returnableColor = new Color();
            switch (terrain)
            {
                case TypeOfTerrain.Earth:
                    returnableColor = Color.DarkGreen;
                    break;
                case TypeOfTerrain.Wather:
                    returnableColor = Color.LightBlue;
                    break;
                case TypeOfTerrain.Road:
                    returnableColor = Color.Brown;
                    break;
                case TypeOfTerrain.Tree:
                    returnableColor = Color.Green;
                    break;
            }
            if (unit != null)
            {
                returnableColor = GetUnitColor(unit);
            }
            return returnableColor;
        }
        private Color GetUnitColor(Units.AUnit unit)
        {
            return CheckFriendly(unit) ? Color.LightGreen : Color.OrangeRed;
        }
        private bool CheckFriendly(Units.AUnit unit)
        {
            if (unit.GetRace() == Units.Race.HUMAN) return true;
            return false;
        }
    }
}
