using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Warcraft_1.GameClasses.Units
{
    class HumWarrior : AUnit
    {
        public HumWarrior() : base(1, 50, 250, new Point(0, 0), new Point(MapClasses.Map.fieldPixelSize, MapClasses.Map.fieldPixelSize))
        {
            this.race = Race.HUMAN;
            this.role = Role.WARRIOR;
        }
        public override void Load(ContentManager Content)
        {
            if (!IsLoaded)
            {
                action = Content.Load<Microsoft.Xna.Framework.Audio.SoundEffect>("Sounds/Game/HumWorker");
                Init(SpriteAndUnits.UnitsTextures.Worker, new Rectangle(0, 0, MapClasses.Map.fieldPixelSize, MapClasses.Map.fieldPixelSize));
                IsLoaded = true;
            }
        }
        public override void Update(GameTime gameTime)
        {
            Regeneration();
        }
        public override void UpdateAnim(bool IsMoving)
        {
            this.UpdateFrame(IsMoving);
            this.UpdateRect(lastDir, false, false);
        }

    }
}
