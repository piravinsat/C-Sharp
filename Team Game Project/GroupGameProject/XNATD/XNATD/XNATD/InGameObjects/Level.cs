using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNATD.InGameObjects
{
    public class Level : IInGameObject
    {
        public enum LevelTile
        {
            Nothing,
            Ground,
            StartPoint,
            EndPoint,
            PathVertical,
            PathHorizontal,
            PathCornerDownToRight,
            PathCornerDownToLeft,
            PathCornerUpToRight,
            PathCornerUpToLeft,
            LeftCorner,
            RightCorner
        }

        private const int TILE_DIMENSION = 40;

        public LevelTile[][] LevelTiles { get; set; }
        public Rectangle LevelRect { get; private set; }
        public Vector2 StartCoord { get { return GetLocationForFirstTile(LevelTile.StartPoint); } }
        public Vector2 EndCoord { get { return GetLocationForFirstTile(LevelTile.EndPoint); } }
        public ResourceManager ResourceManager { get; set; }

        public Level(LevelTile[][] levelTiles, ResourceManager resourcesManager)
        {
            LevelTiles = levelTiles;
            ResourceManager = resourcesManager;
            LevelRect = new Rectangle(0, 0, LevelTiles.Length * TILE_DIMENSION, LevelTiles[0].Length * TILE_DIMENSION);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int rowNumber = 0;
            foreach (LevelTile[] tileRow in LevelTiles)
            {
                int columnNumber = 0;
                foreach (LevelTile tile in tileRow)
                {
                    spriteBatch.Draw(GetTexture2DFromLevelTile(tile), 
                        new Rectangle(columnNumber * TILE_DIMENSION, rowNumber * TILE_DIMENSION, TILE_DIMENSION, TILE_DIMENSION), Color.White);
                    columnNumber++;
                }
                rowNumber++;
            }
        }

        public void Update(Game game, GameTime gameTime)
        {
           
        }

        private LevelTile GetTile(int x, int y) 
        {
            return LevelTiles[x][y];
        }

        private Vector2 GetLocationForFirstTile(LevelTile tile) 
        {
            for (int x = 0; x < LevelTiles[0].Length; x++)
            {
                for (int y = 0; y < LevelTiles.Length; y++)
                {
                    if (LevelTiles[y][x] == tile)
                    {
                        return new Vector2(x * TILE_DIMENSION, y * TILE_DIMENSION);
                    }
                }
            }

            return new Vector2(-1, -1);
        }

        public LevelTile GetLevelTileForLocation(Vector2 location)
        {
            int xCoord = (int)Math.Floor(location.X / TILE_DIMENSION);
            int yCoord = (int)Math.Floor(location.Y / TILE_DIMENSION);

            if (xCoord < LevelTiles[0].Length && yCoord < LevelTiles.Length && xCoord > 0 && yCoord > 0)
            {
                return LevelTiles[yCoord][xCoord];
            }

            return LevelTile.Nothing;
        }

        private Texture2D GetTexture2DFromLevelTile(Level.LevelTile levelTile)
        {
            switch (levelTile)
            {
                case Level.LevelTile.Ground: return ResourceManager.GroundSprite;
                case Level.LevelTile.PathHorizontal: return ResourceManager.PathHorizontalSprite;
                case Level.LevelTile.PathVertical: return ResourceManager.PathVerticalSprite;
                case Level.LevelTile.PathCornerDownToLeft: return ResourceManager.PathCornerDownToLeftSprite;
                case Level.LevelTile.PathCornerDownToRight: return ResourceManager.PathCornerDownToRightSprite;
                case Level.LevelTile.PathCornerUpToLeft: return ResourceManager.PathCornerUpToLeftSprite;
                case Level.LevelTile.PathCornerUpToRight: return ResourceManager.PathCornerUpToRightSprite;
                case Level.LevelTile.StartPoint: return ResourceManager.PathVerticalSprite;
                case Level.LevelTile.EndPoint: return ResourceManager.PathVerticalSprite;
                default: return ResourceManager.NothingSprite;
            }
        }


        public bool ShouldBeRemoved()
        {
            return false;
        }
    }
}
