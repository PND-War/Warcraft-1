using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraft_1.Units
{
    class HumWorker : AUnit
    {
        public HumWorker() : base(1, 10, 100, new Point(0, 0), new Point(32, 32))
        {
            this.race = Race.HUMAN;
            this.role = Role.WORKER;
        }
        public override void Load()
        {
            if (!IsLoaded)
            {
                Init(Logic_Classes.UnitsTextures.Worker, new Rectangle(0, 0, 32, 32));
                IsLoaded = true;
            }
        }
        public override void Update(GameTime gameTime)
        {
            this.UpdateAnim(false, Logic_Classes.DIRS.DOWN);
        }
    }
}
