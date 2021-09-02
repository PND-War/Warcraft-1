using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Warcraft_1.Units
{
    class OrcWorker : AUnit
    {
        public OrcWorker() : base(1, 10, 100, new Point(0, 0), new Point(GameClasses.Map.fieldPixelSize, GameClasses.Map.fieldPixelSize))
        {
            this.race = Race.ORC;
            this.role = Role.WORKER;
        }
        public override void Load(ContentManager Content)
        {
            if (!IsLoaded)
            {
                //action = Content.Load<SoundEffect>("Sounds/Game/HumWorker");
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
