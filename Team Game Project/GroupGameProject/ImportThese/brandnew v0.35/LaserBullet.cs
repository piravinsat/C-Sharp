using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MoonDefense
{
    class LaserBullet : TowerBullet
    {

        public LaserBullet(Microsoft.Xna.Framework.Vector2 center, Mob m, int p)
            : base(center, Vector2.Zero, m, p)
        {

            this.center.Y -= 2;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if (target.isAlive)
            {
                Texture2D sprite = ResourceManager.laserBeam;
                Rectangle dest = new Rectangle((int)center.X, (int)center.Y, (int)(target.center - center).Length(), 3);
                double angle = Math.Asin((target.center.Y - center.Y) / dest.Width);
                sb.Draw(sprite, dest, null, Color.White, (float)angle, Vector2.Zero, SpriteEffects.None, 1.0f);
            }
        }

        public override void Update(Game1 game)
        {
            if (target.isAlive)
            {
                target.Hurt(damage);
            }
        }
    }
}
