using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Warcraft_1.GameClasses.Units
{
    class HumWorker : AUnit
    {
        public bool IsCarryingWood = false;
        public bool IsCarryingGold = false;
        public HumWorker() : base(1, 10, 100, new Point(0, 0), new Point(MapClasses.Map.fieldPixelSize, MapClasses.Map.fieldPixelSize))
        {
            this.race = Race.HUMAN;
            this.role = Role.WORKER;
        }
        public override void Load(ContentManager Content)
        {
            if (!IsLoaded)
            {
                action = Content.Load<SoundEffect>("Sounds/Game/HumWorker");
                Init(SpriteAndUnits.UnitsTextures.Worker, new Rectangle(0, 0, MapClasses.Map.fieldPixelSize, MapClasses.Map.fieldPixelSize));
                IsLoaded = true;
            }
        }
        public override void Update(GameTime gameTime)
        {
            this.UpdateAnim(false);
        }
        public override void UpdateAnim(bool IsMoving)
        {
            this.UpdateFrame(IsMoving);
            this.UpdateRect(lastDir, IsCarryingWood, IsCarryingGold);
        }
    }
}
