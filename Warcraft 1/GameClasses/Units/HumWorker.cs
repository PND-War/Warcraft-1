using Microsoft.Xna.Framework;

namespace Warcraft_1.Units
{
    class HumWorker : AUnit
    {
        public HumWorker() : base(1, 10, 100, new Point(0, 0), new Point(GameClasses.Map.fieldPixelSize, GameClasses.Map.fieldPixelSize))
        {
            this.race = Race.HUMAN;
            this.role = Role.WORKER;
        }
        public override void Load()
        {
            if (!IsLoaded)
            {
                Init(Logic_Classes.UnitsTextures.Worker, new Rectangle(0, 0, GameClasses.Map.fieldPixelSize, GameClasses.Map.fieldPixelSize));
                IsLoaded = true;
            }
        }
        public override void Update(GameTime gameTime)
        {
            //this.UpdateAnim(false, Logic_Classes.DIRS.DOWN);
        }
    }
}
