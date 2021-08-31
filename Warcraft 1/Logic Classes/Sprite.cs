using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraft_1.Logic_Classes
{
    public enum DIRS
    {
        UP = 0,
        RIGHT = 1,
        DOWN = 2,
        LEFT = 3
    }
    public enum AXIS
    {
        X = 1,
        Y = 2
    }
    class Sprite
    {
        //private Vector2 Origin;           // Начальная точка
        public Vector2 Position;         // Позиция
        public Texture2D Texture;        // Текстура 

        private Rectangle Rect;           // Границы отрисовки

        public float Frame = 0;           // Начальный кадр       (Если есть анимация)
        private float MaxFrame = 0;       // Количество кадров    (Если есть анимация)

        public Sprite()
        {
            this.Rect = new Rectangle(0, 0, 32, 32);
            Position = new Vector2(0, 0);
            Texture = null;
        }
        public Sprite(Texture2D texture, Rectangle rect, Vector2 position)
        {
            this.Rect = rect;
            this.Texture = texture;
            this.Position = position;
            //this.Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.MaxFrame = texture.Width / rect.Width;
        }

        public void Init(Texture2D texture, Rectangle rect, Vector2 position)
        {
            this.Rect = rect;
            this.Texture = texture;
            this.Position = position;
            this.MaxFrame = 5;
        }

        //Public methods
        #region Public methods

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, Position, this.Rect, Color.White);      // Отрисовка спрайта
        }
        #endregion

        //Protected methods
        #region Protected methods

        //, float Differ
        protected void UpdatePosition(AXIS Axis)
        {
            if (Axis == AXIS.X) this.Position.X += 0.1f;
            else if (Axis == AXIS.Y) this.Position.Y += 0.1f;
        }    //Обновляет позицию 
        protected void UpdateAnim(bool IsMoving, int Direction)
        {
            this.UpdateFrame(IsMoving);
            this.UpdateRect(Direction);
        }   //Обновляет анимацию, если она есть

        #endregion

        //Private methods
        #region Private methods

        private void UpdateFrame(bool IsMoving)
        {
            if (Frame >= MaxFrame) Frame = 0;
            if (IsMoving) Frame += 0.1f;
        }               // Обновляет кадр анимации
        private void UpdateRect(int Direction)
        {
            this.Rect.X = (int)this.Frame * this.Rect.Width;              // Обновляем кадр анимации исходя из кадра
            this.Rect.Y = Direction * this.Rect.Height;         // Обновляем номер анимации исходя из направления
        }         // Обновляет рамки текущего кадра в анимации

        #endregion

    }
}
