using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XNATD.InGameObjects
{
    public class Enemy : IInGameObject
    {
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Bounty { get; set; }
        public Rectangle LocationRect { get; set; }
        public Texture2D Sprite1 { get; set; }
        public Texture2D Sprite2 { get; set; }
        public Texture2D BloodSplatSprite { get; set; }
        public Vector2 Velocity { get; set; }
        public Boolean IsAlive { get; set; }
        public Lifebar Lifebar { get; set; }
        public DateTime TimeAtActualDeath { get; set; }
        public TimeSpan BloodSplatDelay { get; set; }

        // animation
        public DateTime LastSwitchTime { get; set; }
        public TimeSpan SwitchDelay { get; set; }
        public bool IsOnSecondSprite { get; set; }

        public Enemy(int health, Vector2 velocity, int x, int y, ResourceManager resourceManager)
        {
            Health = health;
            MaxHealth = health;
            Velocity = velocity;
            Bounty = 20;
            Sprite1 = resourceManager.EnemySprite;
            Sprite2 = resourceManager.EnemySprite2;
            BloodSplatSprite = resourceManager.BloodSplatSprite;
            LocationRect = new Rectangle(x, y, Sprite1.Width, Sprite1.Height);
            IsAlive = true;
            IsOnSecondSprite = false;
            Lifebar = new Lifebar(Health, MaxHealth, resourceManager);
            BloodSplatDelay = new TimeSpan(0, 0, 2);
            TimeAtActualDeath = new DateTime(3000, 1, 1);
            LastSwitchTime = new DateTime(1, 1, 1);
            SwitchDelay = new TimeSpan(0, 0, 0, 0, 500);
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                //animation
                Texture2D sprite = IsOnSecondSprite ? Sprite2 : Sprite1;
                if (DateTime.Now - LastSwitchTime > SwitchDelay)
                {
                    IsOnSecondSprite = !IsOnSecondSprite;
                    LastSwitchTime = DateTime.Now;
                }

                // draw the lifebar and the enemy
                Lifebar.Draw(spriteBatch);
                spriteBatch.Draw(sprite, LocationRect, Color.White);
            }
        }

        public void Update(Game game, GameTime gameTime)
        {
            if (IsAlive)
            {
                Lifebar.Value = Health;
                Lifebar.MoveTo(LocationRect.Location.X, LocationRect.Location.Y - 6);
                Lifebar.Update(game, gameTime);
                Velocity = GetNewVelocity(game);
                LocationRect = new Rectangle((int)(LocationRect.X + Velocity.X), (int)(LocationRect.Y + Velocity.Y), LocationRect.Width, LocationRect.Height);

                // if the enemy is dead
                if (Health <= 0)
                {
                    // if the blood splat has displayed for long enough, kill fully
                    if (DateTime.Now - TimeAtActualDeath > BloodSplatDelay)
                    {
                        IsAlive = false;
                    }
                    else if (Sprite1 != BloodSplatSprite)
                    {
                        // change the sprite to a blood splat
                        Sprite1 = BloodSplatSprite;
                        Sprite2 = BloodSplatSprite;
                        game.Coins += Bounty;
                        TimeAtActualDeath = DateTime.Now;
                        Velocity = new Vector2(0);
                        Lifebar.MaxValue = 0;
                        Lifebar.Value = 0;
                    }
                }

                // if the enemy has reached the end
                if (game.CurrentLevel.GetLevelTileForLocation(new Vector2(LocationRect.Center.X, LocationRect.Center.Y)) == Level.LevelTile.EndPoint)
                {
                    game.Lives -= 1;
                    IsAlive = false;
                }
            }
        }

        public Vector2 GetNewVelocity(Game game)
        {
            Level level = game.CurrentLevel;
            int offsetX = Velocity.X < 0 ? -1 : 1;
            int offsetY = Velocity.Y < 0 ? -1 : 1;
            Level.LevelTile currentTile = level.GetLevelTileForLocation(new Vector2(LocationRect.Location.X - ((Sprite1.Width /2) * offsetX), LocationRect.Location.Y - ((Sprite1.Height /2) * offsetY)));

            // get the velocity depending on what tile it's on.
            switch (currentTile)
            {
                case Level.LevelTile.PathCornerDownToLeft: return new Vector2(Velocity.Y != 0 ? Velocity.Y * -1 : Velocity.X, 0);
                case Level.LevelTile.PathCornerDownToRight: return new Vector2(Velocity.Y != 0 ? Velocity.Y : Velocity.X, 0);
                case Level.LevelTile.PathCornerUpToRight: return new Vector2(0, Velocity.X != 0 ? Velocity.X * -1 : Velocity.Y);
                case Level.LevelTile.PathCornerUpToLeft: return new Vector2(0, Velocity.X != 0 ? Velocity.X : Velocity.Y);
            }

            return Velocity;
        }

        public void Damage(int Damage)
        {
            Health -= Damage;
        }

        public bool ShouldBeRemoved()
        {
            return !IsAlive;
        }
    }
}
