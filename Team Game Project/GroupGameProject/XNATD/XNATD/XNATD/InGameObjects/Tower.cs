using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XNATD.InGameObjects
{
    public class Tower : IInGameObject
    {
        public Rectangle LocationRect { get; set; }
        public Texture2D PlaceholderSprite { get; set; }
        public Texture2D PlaceholderRedSprite { get; set; }
        public Texture2D Sprite { get; set; }
        public int Range { get; set; }
        public int Damage { get; set; }
        public int Level { get; set; }
        public int Cost { get; set; }
        public bool IsBuilt { get; set; }
        public bool IsValidLocation { get; set; }
        public bool HasCharged { get; set; }
        public bool IsAlive { get; set; }
        public DateTime LastFired { get; set; }
        public TimeSpan FireDelay { get; set; }

        public Tower(int range, int damage, ResourceManager resourceManager, int level = 1, bool isBuilt = false)
        {
            Range = range;
            Damage = damage;
            Level = level;
            IsBuilt = isBuilt;
            IsValidLocation = true;
            Sprite = resourceManager.StandardTowerSprite;
            PlaceholderSprite = resourceManager.TowerPlaceholderSprite;
            PlaceholderRedSprite = resourceManager.TowerPlaceholderRedSprite;
            Cost = 30;
            IsAlive = true;
            FireDelay = new TimeSpan(0, 0, 0, 2);
        }

        public void Update(Game game, Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (IsAlive && !IsBuilt && !HasCharged)
            {
                // if the player can afford id
                if (game.Coins >= Cost)
                {
                    game.Coins -= Cost;
                    HasCharged = true;
                }
                else
                {
                    IsAlive = false;
                }
            }

            if (IsAlive && !IsBuilt)
            {
                // draw the placeholder where the cursor is
                LocationRect = new Rectangle(game.Cursor.LocationRect.Right, game.Cursor.LocationRect.Top,
                    PlaceholderSprite.Width, PlaceholderSprite.Height);

                // get the tile that the user is over, and determine whether a tower can be built there
                Level.LevelTile overlappingTile = game.CurrentLevel.GetLevelTileForLocation(new Vector2(LocationRect.Location.X, LocationRect.Location.Y));
                if (overlappingTile == InGameObjects.Level.LevelTile.PathHorizontal ||
                    overlappingTile == InGameObjects.Level.LevelTile.PathVertical ||
                    overlappingTile == InGameObjects.Level.LevelTile.PathCornerDownToLeft ||
                    overlappingTile == InGameObjects.Level.LevelTile.PathCornerUpToLeft ||
                    overlappingTile == InGameObjects.Level.LevelTile.PathCornerUpToRight ||
                    overlappingTile == InGameObjects.Level.LevelTile.PathCornerDownToRight)
                {
                    IsValidLocation = false;
                }
                else
                {
                    IsValidLocation = true;

                    // tower can be built
                    if (game.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                    {
                        IsBuilt = true;
                    }
                }
            }
            else if (IsAlive)
            {
                // assures fixed firing rate
                if (DateTime.Now - LastFired > FireDelay)
                {
                    // get the nearest enemy to the tower
                    Enemy nearestEnemy = null;
                    float nearestEnemyDistance = 0.0f;
                    foreach (Enemy enemy in game.Enemies)
                    {
                        if (nearestEnemy == null)
                        {
                            float towerToEnemyLength = new Vector2(LocationRect.Center.X - enemy.LocationRect.Center.X, LocationRect.Center.Y - enemy.LocationRect.Center.Y).Length();
                            nearestEnemy = enemy;
                            nearestEnemyDistance = towerToEnemyLength;
                        }
                        else
                        {
                            float towerToEnemyLength = new Vector2(LocationRect.Center.X - enemy.LocationRect.Center.X, LocationRect.Center.Y - enemy.LocationRect.Center.Y).Length();
                            if (towerToEnemyLength <= nearestEnemyDistance)
                            {
                                nearestEnemy = enemy;
                                nearestEnemyDistance = towerToEnemyLength;
                            }
                        }
                    }

                    if (nearestEnemy != null && nearestEnemy.IsAlive && nearestEnemy.Sprite1 != nearestEnemy.BloodSplatSprite)
                    {
                        // fire a projectile at the nearest enemy
                        Projectile projectile = new Projectile(LocationRect.Center.X, LocationRect.Center.Y, Damage, 5, nearestEnemy, game.ResourceManager);
                        game.GameObjects.Add(projectile);
                        LastFired = DateTime.Now;
                    }
                }
            }
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                if (!IsBuilt)
                {
                    spriteBatch.Draw(IsValidLocation ? PlaceholderSprite : PlaceholderRedSprite, LocationRect, Color.White);
                }
                else
                {
                    spriteBatch.Draw(Sprite, LocationRect, Color.White);
                }
            }
        }


        public bool ShouldBeRemoved()
        {
            return !IsAlive;
        }
    }
}
