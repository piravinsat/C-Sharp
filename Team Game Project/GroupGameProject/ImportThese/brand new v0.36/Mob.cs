using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonDefense
{
    public class Mob : IGameObject
    {

        public enum MobType
        {
            spacehopper,
            discoStu,
            theEye,
        }
        public MobType type;
        public Vector2 center;
        public Vector2 velocity;
        protected int health;
        protected int maxHealth;
        protected Lifebar lifebar;
        public bool isAlive;
        private Random random;
        public bool shouldRemove = false;
        public int Width, Height, killPoints;

        public Mob(int health, Vector2 center, Vector2 velocity)
        {
            this.health = health;
            this.maxHealth = health;
            this.center = center;
            this.velocity = velocity;
            isAlive = true;
            Height = Width = ResourceManager.mob.Width;
            lifebar = new Lifebar(health, maxHealth, Width);
            random = new Random();
            int r = random.Next() % 3;
            if (r == 0) { type = MobType.spacehopper; killPoints = 10; }
            else if (r == 1) { type = MobType.discoStu; killPoints = 50; }
            else if (r == 2) { type = MobType.theEye; killPoints = 100; }
        }

        public void Hurt(int damage)
        {
            health -= damage;
        }

        public void Update(Game1 game)
        {
            velocity = new Vector2(random.Next(-2, 5), random.Next(-2, 5));
            center += velocity;

            lifebar.MoveTo((int)center.X - Width / 2, (int)(center.Y - 10) - Height / 2);
            lifebar.value = health;

            if ((center.X + Width / 2 < 0 || center.X - Width / 2 > game.screenWidth) || (center.Y + Height / 2 < 0 || center.Y - Height / 2 > game.screenHeight)) shouldRemove = true;

            if (health <= 0 && isAlive)
            {
                isAlive = false;
                shouldRemove = true;
                game.level.moonBase.Points += killPoints;
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if (isAlive)
            {
                lifebar.Draw(sb);
                if(type == MobType.spacehopper) sb.Draw(ResourceManager.mob, new Rectangle((int)center.X - Width / 2, (int)center.Y - Height / 2, (int) Width, (int) Height), Color.White);
                else if (type == MobType.discoStu) sb.Draw(ResourceManager.discoStuMob, new Rectangle((int)center.X - Width / 2, (int)center.Y - Height / 2, (int)Width, (int)Height), Color.White);
                else if (type == MobType.theEye) sb.Draw(ResourceManager.theEyeMob, new Rectangle((int)center.X - Width / 2, (int)center.Y - Height / 2, (int)Width, (int)Height), Color.White);
            }
        }
    }
}
