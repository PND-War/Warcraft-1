﻿using Microsoft.Xna.Framework;

namespace Warcraft_1.Units
{
    class HumWarrior : AUnit
    {
        public HumWarrior() : base(1, 50, 250, new Point(0, 0), new Point(GameClasses.Map.fieldPixelSize, GameClasses.Map.fieldPixelSize))
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
