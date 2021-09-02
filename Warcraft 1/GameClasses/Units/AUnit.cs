using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

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
        public Point positionInMoving;
        public SoundEffect action;

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

        protected Point spriteSize;

        public Point positionToMove;

        // Базовый конструктор, все параметры инициализирует нулем
        public AUnit() : base()
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

            this.spriteSize = new Point(0, 0);

            this.positionToMove = new Point(0, 0);
        }

        // Конструктор только с самыми базовыми параметрами (скорость, здоровье, позиция и т.д)
        protected AUnit(int speed, int damage, int maxHealth,
            Point position, Point spriteSize) : this()
        {
            this.speed = speed;
            this.damage = damage;
            this.maxHealth = maxHealth;
            this.currentHealth = maxHealth;

            this.positionToMove = position;
            this.spriteSize = spriteSize;
        }

        // Конструктор с переопределением всех параметров для потомков
        protected AUnit(int speed, int armor, int damage, int maxHealth,
            int regeneration, int gold, int food, int reward, string bio,
            Race race, Role role, Point position, Point spriteSize)
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

            this.spriteSize = spriteSize;

            this.positionToMove = position;
        }

        static public AUnit DeepCopy(AUnit unit)
        {
            AUnit aUnit = new HumWarrior();

            aUnit.speed = unit.speed;
            aUnit.armor = unit.armor;
            aUnit.damage = unit.damage;
            aUnit.maxHealth = unit.maxHealth;
            aUnit.currentHealth = unit.currentHealth;
            aUnit.regeneration = unit.regeneration;
            aUnit.Texture = unit.Texture;
            aUnit.positionToMove = unit.positionToMove;
            aUnit.positionInMoving = unit.positionInMoving;
            aUnit.gold = unit.gold;
            aUnit.food = unit.food;
            aUnit.reward = unit.reward;

            aUnit.bio = unit.bio;
            aUnit.race = unit.race;
            aUnit.role = unit.role;

            aUnit.spriteSize = unit.spriteSize;
            return aUnit;
        }

        public abstract void Load(ContentManager Content);
        public abstract void Update(GameTime gameTime);

        protected void Regeneration()
        {
            if (this.currentHealth < this.maxHealth)
                this.currentHealth += this.regeneration;

            if (this.currentHealth > this.maxHealth)
                this.currentHealth = this.maxHealth;
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