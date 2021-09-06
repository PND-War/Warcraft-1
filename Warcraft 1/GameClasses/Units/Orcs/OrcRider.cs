using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
namespace Warcraft_1.GameClasses.Units.Orcs
{
    class OrcRider : AUnit
    {
        public OrcRider() : base(1, 58, 270, new Point(0, 0), new Point(MapClasses.Map.fieldPixelSize, MapClasses.Map.fieldPixelSize))
        {
            this.race = Race.ORC;
            this.role = Role.RIDER;
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
