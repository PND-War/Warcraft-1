using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraft_1.GameClasses
{
    enum TypeOfTerrain
    {
        Earth,
        Wather
    }
    class Field
    {
        TypeOfTerrain terrain;
        Units.AUnit unit;
    }
}
