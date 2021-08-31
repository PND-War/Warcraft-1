using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraft_1.Units
{
    class HumWarrior : AUnit
    {
        public HumWarrior() : base(1, 50, 250, new Point(0, 0), new Point(32, 32))
        {
            this.race = Race.HUMAN;
            this.role = Role.WARRIOR;
        }
        public override void Load()
        {
        }
        public override void Update(GameTime gameTime)
        {
            Regeneration();
        }

    }
}
