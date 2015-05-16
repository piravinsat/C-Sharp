using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNATD.InGameObjects
{
    public class Lifebar : IInGameObject
    {
        public Rectangle LocationRect { get; set; }
        public int MaxValue { get; set; }
        public int Value { get; set; }
        private Texture2D RedSprite { get; set; }
        private Texture2D GreenSprite { get; set; }

        public Lifebar(int value, int maxValue, ResourceManager resourceManager)
        {
            GreenSprite = resourceManager.LifebarGreenSprite;
            RedSprite = resourceManager.LifebarRedSprite;
            Value = value;
            MaxValue = maxValue;
            MoveTo(0, 0);
        }

        public void MoveTo(int x, int y)
        {
            LocationRect = new Rectangle(x, y, 20, 4);
        }

        public void Update(Game game, GameTime gameTime)
        {
            // do nothing
        }
        
        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            double greenPercentage = (double)Value / MaxValue;
            double redPercentage = 1.0 - greenPercentage;

            // fill in the bar with the correct amount of green and red according to these percentages
            spriteBatch.Draw(GreenSprite, new Rectangle((int)LocationRect.X, (int)LocationRect.Y, 
                (int)(LocationRect.Width * greenPercentage), (int)LocationRect.Height), Color.White);
            spriteBatch.Draw(RedSprite, new Rectangle((int)(LocationRect.X + (LocationRect.Width * greenPercentage)), (int)(LocationRect.Y),
                (int)(LocationRect.Width * redPercentage), (int)LocationRect.Height), Color.White);
        }


        public bool ShouldBeRemoved()
        {
            return false;
        }
    }
}
