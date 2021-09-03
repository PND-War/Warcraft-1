using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Warcraft_1.GameClasses.Units
{
    class OrcWarrior : AUnit
    {
        public OrcWarrior() : base(1, 50, 250, new Point(0, 0), new Point(MapClasses.Map.fieldPixelSize, MapClasses.Map.fieldPixelSize))
        {
            this.race = Race.ORC;
            this.role = Role.WARRIOR;
        }
        public override void Load(ContentManager Content)
        {

        }
        public override void Update(GameTime gameTime)
        {
            Regeneration();
        }

    }
}
