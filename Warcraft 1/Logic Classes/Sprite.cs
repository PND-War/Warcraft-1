using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    class Sprite
    {
        public Texture2D Texture;        // Текстура 

        public Rectangle Rect;           // Границы отрисовки

        public float Frame = 0;           // Начальный кадр       (Если есть анимация)
        private float MaxFrame = 0;       // Количество кадров    (Если есть анимация)

        public Sprite()
        {
            this.Rect = new Rectangle(0, 0, 32, 32);
            Texture = null;
        }
        public Sprite(Texture2D texture, Rectangle rect)
        {
            this.Rect = rect;
            this.Texture = texture;
            //this.Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.MaxFrame = texture.Width / rect.Width;
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
            spriteBatch.Draw(this.Texture, this.Rect, Color.White);      // Отрисовка спрайта
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
        }   //Обновляет анимацию, если она есть


        //Private methods
        #region Private methods

        private void UpdateFrame(bool IsMoving)
        {
            if (Frame >= MaxFrame) Frame = 0;
            if (IsMoving) Frame += 0.2f;
        }               // Обновляет кадр анимации
        private void UpdateRect(DIRS Direction)
        {
            this.Rect.X =  (int)Direction * Map.fieldPixelSize;              // Обновляем кадр анимации исходя из кадра
            this.Rect.Y = (int)this.Frame * Map.fieldPixelSize;         // Обновляем номер анимации исходя из направления
        }         // Обновляет рамки текущего кадра в анимации

        #endregion

        //public void ChangeSprite()
        //{
        //    this.Rect = new Rectangle(this.Rect.Y);
        //}
    }
}
