using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Warcraft_1.GameClasses.MapClasses;

namespace Warcraft_1.GameClasses.Units
{
    public class Sprite
    {
        [JsonIgnore]
        public Texture2D Texture { get; set; }
        public Rectangle Rect;
        protected DIRS lastDir = DIRS.DOWN;
        public float Frame = 0;
        private float MaxFrame = 0;


        public Sprite()
        {
            this.Rect = new Rectangle(0, 0, 32, 32);
            Texture = null;
        }

        public void Init(Texture2D texture, Rectangle rect)
        {
            this.Rect = rect;
            this.Texture = texture;
            this.MaxFrame = 8;
        }

        //Public methods
        #region Public methods

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Rect, Color.White);
        }
        #endregion


        //, float Differ
        //protected void UpdatePosition(AXIS Axis)
        //{
        //    if (Axis == AXIS.X) this.Position.X += 0.1f;
        //    else if (Axis == AXIS.Y) this.Position.Y += 0.1f;
        //}    //Обновляет позицию 
        public void UpdateAnim(bool IsMoving, DIRS Direction)
        {
            lastDir = Direction;
            this.UpdateFrame(IsMoving);
            this.UpdateRect(Direction, false, false);
        }
        public virtual void UpdateAnim(bool IsMoving)
        {
            this.UpdateFrame(IsMoving);
            this.UpdateRect(lastDir, false, false);
        }

        public void UpdateAnim(bool IsMoving, DIRS Direction, bool IsCarryingWood, bool IsCarryingGold)
        {
            lastDir = Direction;
            this.UpdateFrame(IsMoving);
            this.UpdateRect(Direction, IsCarryingWood, IsCarryingGold);
        }
        //Private methods
        #region Private methods

        protected void UpdateFrame(bool IsMoving)
        {
            if (Frame >= MaxFrame) Frame = 0;
            if (IsMoving) Frame += 0.2f;
        }
        protected void UpdateRect(DIRS Direction, bool IsCarryingWood, bool IsCarryingGold)
        {
            if(IsCarryingGold)
            {
                Rect.X = 16 * Map.fieldPixelSize + (int)Direction * Map.fieldPixelSize;
            }
            else
            {
                Rect.X = (IsCarryingWood ? 8 : 0) * Map.fieldPixelSize + (int)Direction * Map.fieldPixelSize;
            }
            Rect.Y = (int)this.Frame * Map.fieldPixelSize;
        }
        #endregion

        //public void ChangeSprite()
        //{
        //    this.Rect = new Rectangle(this.Rect.Y);
        //}
    }
}
