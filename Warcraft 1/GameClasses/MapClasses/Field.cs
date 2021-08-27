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
        Tree,
        Mine
    }
    class Field
    {
        bool cheked = false;
        TypeOfTerrain terrain;
        Units.AUnit unit;
        public bool CheckTerrain()
        {
            bool res = false;
            if(!cheked)
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
                case TypeOfTerrain.Mine:
                    returnableColor = Color.Yellow;
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
        public bool PlaceAUnit(Units.AUnit unitG)
        {
            bool res = false;
            if (unit == null)
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
            return CheckFriendly(unitG) ? Color.LightGreen : Color.OrangeRed;
        }
        private bool CheckFriendly(Units.AUnit unitG)
        {
            if (unitG.GetRace() == Units.Race.HUMAN) return true;
            return false;
        }
       
    }
}
