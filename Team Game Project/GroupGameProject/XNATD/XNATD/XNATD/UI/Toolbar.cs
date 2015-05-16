using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNATD.UI
{
    class Toolbar : InGameObjects.IInGameObject
    {
        public Rectangle LocationRectangle { get; set; }
        public List<Button> Buttons { get; set; }
        public bool IsTowerBeingCreated { get; set; }

        public Toolbar(int x, int y, ResourceManager resourceManager)
        {
            IsTowerBeingCreated = false;
            Buttons = new List<Button>();
            LocationRectangle = new Rectangle(x, y, 50, 100);
            Button towerButton = new Button(LocationRectangle.X + 5, LocationRectangle.Y + 5, resourceManager);
            Buttons.Add(towerButton);

            towerButton.Click += new Button.ClickEventHandler(towerButton_Click);
        }

        void towerButton_Click(object sender, EventArgs e)
        {
            IsTowerBeingCreated = !IsTowerBeingCreated;
        }

        public void Update(Game game, Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (Button button in Buttons)
            {
                button.Update(game, gameTime);
            }

            if (IsTowerBeingCreated)
            {
                game.AddTower();
                IsTowerBeingCreated = false;
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            foreach (Button button in Buttons)
            {
                button.Draw(spriteBatch);
            }
        }


        public bool ShouldBeRemoved()
        {
            return false;
        }
    }
}
