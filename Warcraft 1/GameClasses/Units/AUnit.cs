using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraft_1.Units
{
    enum Race
    {
        HUMAN,
        ORC,
        NONE
    }

    enum Role
    {
        WORKER,
        WARRIOR,
        NONE
    }

    abstract class AUnit : Logic_Classes.Sprite
    {
        protected bool IsLoaded;
        public bool IsMoving;

        protected int speed;
        protected int armor;
        protected int damage;
        protected int maxHealth;
        protected int currentHealth;
        protected int regeneration;

        protected int gold;
        protected int food;
        protected int reward;

        protected string bio;
        protected Race race;
        protected Role role;

        protected Point position;
        protected Point spriteSize;
        public Texture2D texture;

        protected Point positionToMove;

        // Базовый конструктор, все параметры инициализирует нулем
        protected AUnit() : base()
        {
            this.speed = 0;
            this.armor = 0;
            this.damage = 0;
            this.maxHealth = 0;
            this.currentHealth = 0;

            this.gold = 0;
            this.food = 0;
            this.reward = 0;

            this.bio = "Unknown";
            this.race = Race.NONE;
            this.role = Role.NONE;

            this.position = new Point(0, 0);
            this.spriteSize = new Point(0, 0);
            this.texture = null;

            this.positionToMove = new Point(0, 0);
        }

        // Конструктор только с самыми базовыми параметрами (скорость, здоровье, позиция и т.д)
        protected AUnit(int speed, int damage, int maxHealth,
            Point position, Point spriteSize, Texture2D texture) : this()
        {
            this.speed = speed;
            this.damage = damage;
            this.maxHealth = maxHealth;
            this.currentHealth = maxHealth;

            this.position = position;
            this.spriteSize = spriteSize;
            this.texture = texture;
        }

        // Конструктор с переопределением всех параметров для потомков
        protected AUnit(int speed, int armor, int damage, int maxHealth,
            int regeneration, int gold, int food, int reward, string bio,
            Race race, Role role, Point position, Point spriteSize,
            Texture2D texture)
        {
            this.speed = speed;
            this.armor = armor;
            this.damage = damage;
            this.maxHealth = maxHealth;
            this.currentHealth = maxHealth;
            this.regeneration = regeneration;

            this.gold = gold;
            this.food = food;
            this.reward = reward;

            this.bio = bio;
            this.race = race;
            this.role = role;

            this.position = position;
            this.spriteSize = spriteSize;
            this.texture = texture;

            this.positionToMove = new Point(0, 0);
        }

        public abstract void Load();
        public abstract void Update(GameTime gameTime, int i, int j);

        protected void Regeneration()
        {
            if (this.currentHealth < this.maxHealth)
                this.currentHealth += this.regeneration;

            if (this.currentHealth > this.maxHealth)
                this.currentHealth = this.maxHealth;
        }
        protected void UpdatePosition()
        {
            if (this.position.X < this.positionToMove.X) this.position.X += speed;
            else this.position.X -= speed;

            if (this.position.Y < this.positionToMove.Y) this.position.Y += speed;
            else this.position.Y -= speed;
        }

        public Race GetRace()
        {
            return this.race;
        }
        public Role GetRole()
        {
            return this.role;
        }
        public int GetCurHP()
        {
            return currentHealth;
        }
        public int GetMaxHP()
        {
            return maxHealth;
        }
    }
}