using Microsoft.Xna.Framework;

namespace Warcraft_1.Logic_Classes
{
    public class Group
    {
        public Point FocusedUnit { get; private set; }
        public bool WoodOntain { get; private set; } = false;
        public bool GoldOntain { get; private set; } = false;
        public Group()
        {
            FocusedUnit = new Point(-1, -1);
        }
        public void ChangePoint(int x, int y)
        {
            FocusedUnit = new Point(x, y);
            OntainChange(false);
        }
        public void ChangePoint(Point pt)
        {
            FocusedUnit = new Point(pt.X, pt.Y);
            OntainChange(false);
        }
        public void OntainChange(bool can, GameClasses.MapClasses.TypeOfTerrain typeOfTerrain)
        {
            switch (typeOfTerrain)
            {
                case GameClasses.MapClasses.TypeOfTerrain.Tree:
                    WoodOntain = can;
                    break;
                case GameClasses.MapClasses.TypeOfTerrain.Mine:
                    GoldOntain = can;
                    break;
            }
        }
        public void OntainChange(bool can)
        {
             WoodOntain = can;
             GoldOntain = can;
        }
    }
}
