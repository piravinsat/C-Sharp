using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNATD.InGameObjects
{
    public class Projectile : IInGameObject
    {
        public Texture2D Sprite { get; set; }
        public Vector2 Velocity { get; set; }
        public int Speed { get; set; }
        public int Damage { get; set; }
        public Enemy Target { get; set; }
        public Rectangle LocationRect { get; set; }
        public bool IsAlive { get; set; }

        public Projectile(int x, int y, int damage, int speed, Enemy target, ResourceManager resourceManager)
        {
            Sprite = resourceManager.BasicProjectileSprite;
            Speed = speed;
            LocationRect = new Rectangle(x, y, Sprite.Width, Sprite.Height);
            Target = target;
            IsAlive = true;
            Damage = damage;
        }

        public void Update(Game game, Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (IsAlive)
            {
                // calculate the direction to point the projectile in
                Vector2 bulletToTarget = new Vector2(Target.LocationRect.Center.X - LocationRect.Center.X, Target.LocationRect.Center.Y - LocationRect.Center.Y);
                bulletToTarget.Normalize();
                Velocity = bulletToTarget * Speed; // set velocity according to direction and speed

                // update the position
                LocationRect = new Rectangle((int)(LocationRect.X + Velocity.X), (int)(LocationRect.Y + Velocity.Y), LocationRect.Width, LocationRect.Height);
                
                // if the projectile has hit the target
                if (LocationRect.Intersects(Target.LocationRect))
                {
                    // damage the target
                    Target.Damage(Damage);
                    // kill the bullet -- it's no longer needed
                    IsAlive = false;
                    Damage = 0;
                }
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                spriteBatch.Draw(Sprite, LocationRect, Color.White);
            }
        }


        public bool ShouldBeRemoved()
        {
            return !IsAlive;
        }
    }
}
