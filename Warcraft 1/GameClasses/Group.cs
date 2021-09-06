using Microsoft.Xna.Framework;

namespace Warcraft_1.Logic_Classes
{
    public class Group
    {
        public Point FocusedObj { get; private set; }
        public bool WoodObtain { get; private set; } = false;
        public bool GoldOntain { get; private set; } = false;
        public GameClasses.MapClasses.BuildingType buildingType = GameClasses.MapClasses.BuildingType.None;
        public Group()
        {
            FocusedObj = new Point(-1, -1);
        }
        public void ChangePoint(int x, int y)
        {
            FocusedObj = new Point(x, y);
            buildingType = GameClasses.MapClasses.BuildingType.None;
            OntainChange(false);
        }
        public void ChangePoint(Point pt)
        {
            FocusedObj = new Point(pt.X, pt.Y);
            buildingType = GameClasses.MapClasses.BuildingType.None;
            OntainChange(false);
        }
        public void OntainChange(bool can, GameClasses.MapClasses.TypeOfTerrain typeOfTerrain)
        {
            switch (typeOfTerrain)
            {
                case GameClasses.MapClasses.TypeOfTerrain.Tree:
                    WoodObtain = can;
                    break;
                case GameClasses.MapClasses.TypeOfTerrain.Mine:
                    GoldOntain = can;
                    break;
            }
        }
        public void OntainChange(bool can)
        {
             WoodObtain = can;
             GoldOntain = can;
        }
    }
}
