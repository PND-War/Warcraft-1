using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Warcraft_1.GameClasses;

namespace Warcraft_1.Logic_Classes
{
    public enum DIRS
    {
        UP = 0,
        UPRIGHT = 1,
        RIGHT = 2,
        DOWNRIGHT = 3,
        DOWN = 4,
        DOWNLEFT = 5,
        LEFT = 6,
        UPLEFT = 7,
        NONE
    }
    public enum AXIS
    {
        X = 1,
        Y = 2
    }
    public class Sprite
    {
        [JsonIgnore]
        public Texture2D Texture { get; set; }
        public Rectangle Rect;       

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
            this.MaxFrame = 5;
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
            this.UpdateFrame(IsMoving);
            this.UpdateRect(Direction);
        }


        //Private methods
        #region Private methods

        private void UpdateFrame(bool IsMoving)
        {
            if (Frame >= MaxFrame) Frame = 0;
            if (IsMoving) Frame += 0.2f;
        }
        private void UpdateRect(DIRS Direction)
            {
                Rect.X = (int)Direction * Map.fieldPixelSize;
                Rect.Y = (int)this.Frame * Map.fieldPixelSize;
            }
        #endregion

        //public void ChangeSprite()
        //{
        //    this.Rect = new Rectangle(this.Rect.Y);
        //}
    }
}
