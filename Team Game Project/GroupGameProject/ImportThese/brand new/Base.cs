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
        int maxHealth, health, Width, Height;
        Rectangle destinationRectangle;
        public Lifebar life;
        public BaseItem[] items;

        public Base()
        {
            Width = ResourceManager.baseTexture.Width;
            Height = ResourceManager.baseTexture.Height;
            destinationRectangle = new Rectangle(192, 512, Width, Height);
            health = 1000;
            maxHealth = health;
            life = new Lifebar(50, maxHealth, 100);
            life.MoveTo(320, 505);
            items = new BaseItem[7];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new BaseItem(i, 1, 1);
            }
        }
        public int Health { get; set; }

        public void Update(Game1 game)
        {
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.Draw(ResourceManager.baseTexture, destinationRectangle, Color.White);
            foreach (BaseItem i in items) i.Draw(sb);
            life.Draw(sb);
        }
    }
}
