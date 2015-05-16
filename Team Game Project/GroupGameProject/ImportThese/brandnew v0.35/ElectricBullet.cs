using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonDefense
{
    class ElectricBullet : TowerBullet
    {
        public ElectricBullet(Vector2 c, Vector2 v, Mob target, int damage)
            : base(c, v, target, damage)
        {
            
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.Draw(ResourceManager.electricBullet, center, Color.White);
        }
    }
}
