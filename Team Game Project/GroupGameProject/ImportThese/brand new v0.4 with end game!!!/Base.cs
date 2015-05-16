using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonDefense
{
    //The base the player is defending. Has health.
    public class Base: IGameObject
    {
        int maxHealth;
        public int Width, Height;
        Rectangle destinationRectangle;
        public Lifebar life;
        public BaseItem[] items;
        public Vector2 center;

        public Base()
        {
            Width = ResourceManager.baseTexture.Width;
            Height = ResourceManager.baseTexture.Height;
            destinationRectangle = new Rectangle(192, 512, Width, Height);
            center = new Vector2(destinationRectangle.X + Width / 2, destinationRectangle.Y + Height / 2);
            Health = 1000;
            maxHealth = Health;
            life = new Lifebar(Health, maxHealth, 100);
            life.MoveTo(320, 505);
            items = new BaseItem[7];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new BaseItem(i, 1, 1);
            }
            Points = 1000;
        }
        public int Health { get; set; }
        public int Points { get; set; }

        public void Update(Game1 game)
        {
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            life = new Lifebar(Health, maxHealth, 100);
            life.MoveTo(320, 505);
            sb.Draw(ResourceManager.baseTexture, destinationRectangle, Color.White);
            foreach (BaseItem i in items) i.Draw(sb);
            sb.DrawString(ResourceManager.spriteFont, Points.ToString(), new Vector2(191, 511), Color.Black, 0f, Vector2.Zero, 1.1f, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
            sb.DrawString(ResourceManager.spriteFont, Points.ToString(), new Vector2(192, 512), Color.Yellow);
            life.Draw(sb);
        }
    }
}
