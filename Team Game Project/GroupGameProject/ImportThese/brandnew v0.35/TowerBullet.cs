using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonDefense
{
    class TowerBullet : Bullet
    {
        public Mob target;
        public int damage;

        public TowerBullet(Vector2 c, Vector2 v, Mob target, int damage) : base(c, v)
        {
            this.target = target;
            this.damage = damage;
        }

        public override void Update(Game1 game) 
        {
            base.Update(game);
            Vector2 bulletToTarget = target.center - center;
            bulletToTarget.Normalize();
            velocity = bulletToTarget * velocity.Length();

            if (center.X - target.center.X <= 0.01f && center.Y - target.center.Y <= 0.01f)
            {
                target.Hurt(damage);
                shouldRemove = true;
            }
        }
    }
}
