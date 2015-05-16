using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNATD.InGameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNATD.UI
{
    class Button : IInGameObject
    {
        public Rectangle LocationRectangle { get; set; }
        public Texture2D Sprite { get; set; }
        public Texture2D IconSprite { get; set; }
        public Texture2D OverlaySprite { get; set; }
        public Boolean IsPressed { get; set; }
        public Boolean IsHovered { get; set; }

        public event ClickEventHandler Click;
        public delegate void ClickEventHandler(object sender, EventArgs e);

        public Button(int x, int y, ResourceManager resourceManager)
        {
            Sprite = resourceManager.ButtonSprite;
            IconSprite = resourceManager.StandardTowerSprite;
            OverlaySprite = resourceManager.SpannerSprite;
            LocationRectangle = new Rectangle(x, y, Sprite.Width, Sprite.Height);
            IsPressed = false;
            IsHovered = false;
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            // draw the sprites, giving a grey tint if the button is hovered over
            spriteBatch.Draw(Sprite, LocationRectangle, IsHovered ? Color.LightGray : Color.White);
            spriteBatch.Draw(IconSprite, new Rectangle(LocationRectangle.X + 10, LocationRectangle.Y + 5, IconSprite.Width, IconSprite.Height),
                IsHovered ? Color.LightGray : Color.White);
            spriteBatch.Draw(OverlaySprite, new Rectangle(LocationRectangle.X + 5, LocationRectangle.Y + 8, IconSprite.Width, IconSprite.Height),
                IsHovered ? Color.LightGray : Color.White);
        }

        protected void OnClick(EventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }
        }

        public void Update(Game game, GameTime gameTime)
        {
            bool mouseIsPressed = (game.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed);

            // if the mouse is over the button
            if (LocationRectangle.Contains(new Point(game.Cursor.LocationRect.Right, game.Cursor.LocationRect.Top)))
            {
                if (!IsHovered)
                {
                    // tell the cursor so it can change icon
                    game.Cursor.RegisterHover();
                    IsHovered = true;
                }

                if (mouseIsPressed)
                {
                    // trigger the onclick
                    OnClick(EventArgs.Empty);
                }
            }
            else
            {
                game.Cursor.DeregisterHover();
                IsHovered = false;
            }
        }


        public bool ShouldBeRemoved()
        {
            return false;
        }
    }
}
