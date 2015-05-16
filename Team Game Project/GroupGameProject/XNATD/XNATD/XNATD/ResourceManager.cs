using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNATD
{
    public class ResourceManager
    {
        private ContentManager ContentManager { get; set; }

        public Texture2D CursorPointerSprite { get; private set; }
        public Texture2D CursorHandSprite { get; private set; }
        public Texture2D EnemySprite { get; private set; }
        public Texture2D EnemySprite2 { get; private set; }
        public Texture2D NothingSprite { get; private set; }
        public Texture2D PathHorizontalSprite { get; private set; }
        public Texture2D PathVerticalSprite { get; private set; }
        public Texture2D PathCornerDownToLeftSprite { get; private set; }
        public Texture2D PathCornerDownToRightSprite { get; private set; }
        public Texture2D PathCornerUpToLeftSprite { get; private set; }
        public Texture2D PathCornerUpToRightSprite { get; private set; }
        public Texture2D GroundSprite { get; private set; }
        public Texture2D LifebarGreenSprite { get; private set; }
        public Texture2D LifebarRedSprite { get; private set; }
        public Texture2D ButtonSprite { get; private set; }
        public Texture2D StandardTowerSprite { get; private set; }
        public Texture2D TowerPlaceholderSprite { get; private set; }
        public Texture2D TowerPlaceholderRedSprite { get; private set; }
        public Texture2D SpannerSprite { get; private set; }
        public Texture2D BasicProjectileSprite { get; private set; }
        public Texture2D BloodSplatSprite { get; private set; }

        public SpriteFont Font { get; private set; }

        public ResourceManager(ContentManager contentManager)
        {
            ContentManager = contentManager;
            LoadSprites();
        }

        private void LoadSprites()
        {
            CursorPointerSprite = ContentManager.Load<Texture2D>("cursor_pointer");
            CursorHandSprite = ContentManager.Load<Texture2D>("cursor_hand");
            EnemySprite = ContentManager.Load<Texture2D>("beast_1");
            EnemySprite2 = ContentManager.Load<Texture2D>("beast_2");
            NothingSprite = ContentManager.Load<Texture2D>("black");
            PathHorizontalSprite = ContentManager.Load<Texture2D>("path_horizontal");
            PathVerticalSprite = ContentManager.Load<Texture2D>("path_vertical");
            PathCornerDownToLeftSprite = ContentManager.Load<Texture2D>("path_corner_down_to_left");
            PathCornerDownToRightSprite = ContentManager.Load<Texture2D>("path_corner_down_to_right");
            PathCornerUpToLeftSprite = ContentManager.Load<Texture2D>("path_corner_up_to_left");
            PathCornerUpToRightSprite = ContentManager.Load<Texture2D>("path_corner_up_to_right");
            GroundSprite = ContentManager.Load<Texture2D>("ground");
            LifebarRedSprite = ContentManager.Load<Texture2D>("lifebar_red");
            LifebarGreenSprite = ContentManager.Load<Texture2D>("lifebar_green");
            ButtonSprite = ContentManager.Load<Texture2D>("default_button");
            StandardTowerSprite = ContentManager.Load<Texture2D>("standard_tower");
            TowerPlaceholderSprite = ContentManager.Load<Texture2D>("tower_placeholder");
            TowerPlaceholderRedSprite = ContentManager.Load<Texture2D>("tower_placeholder_red");
            SpannerSprite = ContentManager.Load<Texture2D>("spanner");
            BasicProjectileSprite = ContentManager.Load<Texture2D>("basic_projectile");
            BloodSplatSprite = ContentManager.Load<Texture2D>("blood_splat");

            Font = ContentManager.Load<SpriteFont>("font");
        }
    }
}
