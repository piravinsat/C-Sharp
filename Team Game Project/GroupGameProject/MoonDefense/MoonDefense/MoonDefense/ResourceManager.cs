using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MoonDefense
{
    class ResourceManager
    {
        private ContentManager contentManager;

        public static Texture2D playerTexture;
        public static Texture2D bulletTexture;
        public static Texture2D lifebarRedSprite;
        public static Texture2D lifebarGreenSprite;

        public ResourceManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            LoadSprites();
        }

        private void LoadSprites()
        {
            playerTexture = contentManager.Load<Texture2D>("man");
            bulletTexture = contentManager.Load<Texture2D>("bullet");
            lifebarGreenSprite = contentManager.Load<Texture2D>("lifebar_green");
            lifebarRedSprite = contentManager.Load<Texture2D>("lifebar_red");
        }
    }
}
