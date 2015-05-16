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

        public TowerBullet(Vector2 c, Vector2 v, Mob target) : base(c, v)
        {
            this.target = target;
        }

        public void Update(Game1 game) 
        {
            base.Update(game);

            Vector2 bulletToTarget = center - target.center;
            bulletToTarget.Normalize();
            velocity = bulletToTarget * velocity.Length();

            if (target.center == center)
            {
                target.Hurt(damage);
            }
        }
    }
}
