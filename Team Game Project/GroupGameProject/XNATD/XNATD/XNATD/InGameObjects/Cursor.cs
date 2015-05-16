using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace XNATD.InGameObjects
{
    public class Cursor : IInGameObject
    {
        public Texture2D PointerSprite { get; set; }
        public Texture2D HandSprite { get; set; }
        private int HoverCount { get; set; }
        public Rectangle LocationRect { get; set; }

        public Cursor(ResourceManager resourceManager)
        {
            PointerSprite = resourceManager.CursorPointerSprite;
            HandSprite = resourceManager.CursorHandSprite;
            LocationRect = Rectangle.Empty;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (HoverCount > 0)
            {
                spriteBatch.Draw(HandSprite, LocationRect, Color.White);
            }
            else
            {
                spriteBatch.Draw(PointerSprite, LocationRect, Color.White);
            }
        }

        public void DeregisterHover()
        {
            if (HoverCount > 0)
            {
                HoverCount--;
            }
        }

        public void RegisterHover()
        {
            HoverCount++;
        }

        public void Update(Game game, GameTime gameTime)
        {
            LocationRect = new Rectangle(game.MouseState.X, game.MouseState.Y, PointerSprite.Width, PointerSprite.Height);
        }


        public bool ShouldBeRemoved()
        {
            return false;
        }
    }
}
