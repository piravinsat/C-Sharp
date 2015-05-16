using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonDefense
{
    class TowerBullet : Bullet
    {
        Mob target;
        int damage;

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

            if (center.X == target.center.X && center.Y == target.center.Y) 
            {
                target.Hurt(damage);
                shouldRemove = true;
            }
        }
    }
}
