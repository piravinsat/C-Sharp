using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonDefense
{
    public class Lifebar : IGameObject
    {
        public Rectangle locationRect;
        public int maxValue;
        public int value;
        public Texture2D redSprite;
        public Texture2D greenSprite;

        public Lifebar(int value, int maxValue)
        {
            greenSprite = ResourceManager.lifebarGreenSprite;
            redSprite = ResourceManager.lifebarRedSprite;
            this.value = value;
            this.maxValue = maxValue;
            MoveTo(0, 0);
        }

        public void MoveTo(int x, int y)
        {
            locationRect = new Rectangle(x, y, 20, 4);
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            double greenPercentage = (double)value / maxValue;
            double redPercentage = 1.0 - greenPercentage;

            // fill in the bar with the correct amount of green and red according to these percentages
            spriteBatch.Draw(greenSprite, new Rectangle((int)locationRect.X, (int)locationRect.Y,
                (int)(locationRect.Width * greenPercentage), (int)locationRect.Height), Color.White);
            spriteBatch.Draw(redSprite, new Rectangle((int)(locationRect.X + (locationRect.Width * greenPercentage)), (int)(locationRect.Y),
                (int)(locationRect.Width * redPercentage), (int)locationRect.Height), Color.White);
        }

        public void Update(Game1 game)
        {
        }
    }
}
