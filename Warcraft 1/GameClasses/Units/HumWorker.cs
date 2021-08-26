using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraft_1.Units
{
    class HumWorker : AUnit
    {
        public HumWorker() : base(1, 10, 100, new Point(0, 0), new Point(32, 32), null)
        {
            this.race = Race.HUMAN;
            this.role = Role.WARRIOR;
        }
        public override void Load(GraphicsDeviceManager graphics, ContentManager Content)
        {

        }
        public override void Update(GameTime gameTime)
        {
        }
    }
}
