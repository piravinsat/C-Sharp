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
        public Vector2 center;
        public Vector2 velocity;
        protected int health;
        protected int maxHealth;
        protected Lifebar lifebar;
        public bool isAlive;
        public Texture2D sprite;
        private Random random;

        public Mob(int health, Vector2 center, Vector2 velocity)
        {
            this.health = health;
            this.maxHealth = health;
            this.center = center;
            this.velocity = velocity;
            isAlive = true;
            sprite = ResourceManager.mob;
            lifebar = new Lifebar(health, maxHealth, sprite.Width);
            random = new Random();
        }

        public void Hurt(int damage)
        {
            health -= damage;
        }

        public void Update(Game1 game)
        {
            velocity = new Vector2(random.Next(-2, 5), random.Next(-2, 5));
            center += velocity;

            lifebar.MoveTo((int)center.X - sprite.Width / 2, (int)(center.Y - 10) - sprite.Height / 2);
            lifebar.value = health;

            if (health <= 0)
            {
                isAlive = false;
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if (isAlive)
            {
                lifebar.Draw(sb);
                sb.Draw(sprite, new Rectangle((int)center.X - sprite.Width / 2, (int)center.Y - sprite.Height / 2, (int)sprite.Width, (int)sprite.Height), Color.White);
            }
        }
    }
}
