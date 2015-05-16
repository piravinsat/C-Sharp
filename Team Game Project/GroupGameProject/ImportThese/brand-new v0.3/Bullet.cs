using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonDefense
{
    //Bullets shot by the player and by the tower
    public class Bullet : IGameObject
    {
        public Vector2 center, velocity;
        int Width, Height;
        public bool shouldRemove = false;
        Rectangle destinationRectangle;

        public Bullet(Vector2 c, Vector2 v)
        {
            center = c;
            velocity = v;
            Width = ResourceManager.bulletTexture.Width;
            Height = ResourceManager.bulletTexture.Height;
        }

        public virtual void Update(Game1 game)
        {
            //If the bullet goes outside of the screen, set shouldRemove to true, so that on the next update it will be removed.
            if ((center.X + Width / 2 < 0 || center.X - Width / 2 > game.screenWidth) || (center.Y + Height / 2 < 0 || center.Y - Height / 2 > game.screenHeight)) shouldRemove = true;
            //Move the bullet
            center += velocity;

            foreach (Mob m in game.mobs)
            {
                if (Math.Abs(center.X - m.center.X) <= m.sprite.Width / 2 && Math.Abs(center.Y - m.center.Y) <= m.sprite.Height / 2)
                {
                    m.Hurt(5);
                    shouldRemove = true;
                }
            }

            destinationRectangle = new Rectangle((int)center.X - Width / 2, (int)center.Y - Height / 2, Width, Height);
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.Draw(ResourceManager.bulletTexture, destinationRectangle, Color.White);
        }
    }
}
