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
        public static Texture2D playerCarryingTexture;
        public static Texture2D bulletTexture;
        public static Texture2D lifebarRedSprite;
        public static Texture2D lifebarGreenSprite;
        //Some of these need renaming :/
        public static Texture2D tile1;
        public static Texture2D tile2;
        public static Texture2D tile3;
        public static Texture2D spawnerTile;
        public static Texture2D baseTexture;
        public static Texture2D exampleTower;
        public static Texture2D electricTower;
        public static Texture2D turretTower;
        public static Texture2D sniperTower;
        public static Texture2D mob;
        public static Texture2D upgradeTexture;
        public static Texture2D levelTexture;
        public static SpriteFont spriteFont;

        public ResourceManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            LoadSprites();
        }

        private void LoadSprites()
        {
            playerTexture = contentManager.Load<Texture2D>("man");
            playerCarryingTexture = contentManager.Load<Texture2D>("mancarryingsomething");
            bulletTexture = contentManager.Load<Texture2D>("bullet");
            lifebarGreenSprite = contentManager.Load<Texture2D>("lifebar_green");
            lifebarRedSprite = contentManager.Load<Texture2D>("lifebar_red");
            tile1 = contentManager.Load<Texture2D>("tile1");
            tile2 = contentManager.Load<Texture2D>("tile2");
            tile3 = contentManager.Load<Texture2D>("MoonCrater");
            spawnerTile = contentManager.Load<Texture2D>("spawnerTile");
            baseTexture = contentManager.Load<Texture2D>("base");
            exampleTower = contentManager.Load<Texture2D>("tower");
            electricTower = contentManager.Load<Texture2D>("electricTower");
            turretTower = contentManager.Load<Texture2D>("Turret");
            sniperTower = contentManager.Load<Texture2D>("Tower3");
            mob = contentManager.Load<Texture2D>("SpaceHooper");
            upgradeTexture = contentManager.Load<Texture2D>("upgrade");
            levelTexture = contentManager.Load<Texture2D>("level");
            spriteFont = contentManager.Load<SpriteFont>("SpriteFont1");
        }
    }
}
