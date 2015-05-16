using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonDefense
{
    //Is the player
    class Player : IGameObject
    {
        public Vector2 center, velocity;
        int health, Width, Height;
        bool canFire = true;
        Vector2 bulletVelocity, facing;
        int bulletSpeed = 15;
        Rectangle destinationRectangle, sourceRectangle;
        double oldTime, timeBetweenShots = 1000;
        bool isCarryingTower = false, ZIsBeingPressed = false;
        public Tower towerBeingCarried;

        public Player()
        {
            Width = ResourceManager.playerTexture.Width;
            Height = ResourceManager.playerTexture.Height;
            center = new Vector2(352, 544);
        }

        public void Update(Game1 game)
        {
            if (!canFire && (game.time - oldTime > timeBetweenShots))
            {
                canFire = true;
                oldTime = game.time;
            }
            //Fire bullets according to key presses
            if (canFire && !isCarryingTower)
            {
                //This has a strange bug, where if you press left and right (or up and down) at the same time
                //the player can't move.
                if(game.keyBoardState.IsKeyDown(Keys.Up)) bulletVelocity.Y -= bulletSpeed;
                if (game.keyBoardState.IsKeyDown(Keys.Down)) bulletVelocity.Y += bulletSpeed;
                if (game.keyBoardState.IsKeyDown(Keys.Left)) bulletVelocity.X -= bulletSpeed;
                if (game.keyBoardState.IsKeyDown(Keys.Right)) bulletVelocity.X += bulletSpeed;
                //Allow only moving bullets to be added
                if (bulletVelocity != Vector2.Zero) game.bullets.Add(new Bullet(center, bulletVelocity));
                velocity -= bulletVelocity / bulletSpeed;
                bulletVelocity = Vector2.Zero;
                canFire = false;
            }

            //Change player velocity
            if (game.keyBoardState.IsKeyDown(Keys.W)) velocity.Y--;
            if (game.keyBoardState.IsKeyDown(Keys.S)) velocity.Y++;
            if (game.keyBoardState.IsKeyDown(Keys.A)) velocity.X--;
            if (game.keyBoardState.IsKeyDown(Keys.D)) velocity.X++;

            if (game.keyBoardState.IsKeyDown(Keys.Z))
            {
                if (!isCarryingTower && game.level.moonBase.Points > 0 && (game.level.selectedTile.type == Tile.TileType.baseTile))
                {
                    if (game.level.selectedTile.baseItemType != BaseItem.BaseItemTypes.upgrade1 && game.level.selectedTile.baseItemType != BaseItem.BaseItemTypes.upgrade2)
                    {
                        isCarryingTower = true;
                        towerBeingCarried = new Tower(game.level.selectedTile.baseItemType, center);
                        game.level.moonBase.Points -= towerBeingCarried.towerCost;
                    }
                }
                else if (isCarryingTower && !ZIsBeingPressed)
                {
                    if ((game.level.selectedTile.type != Tile.TileType.baseTile && game.level.selectedTile.type != Tile.TileType.spawnerTile) && game.level.selectedTile.isEmpty)
                    {
                        game.level.selectedTile.isEmpty = false;
                        isCarryingTower = false;
                        towerBeingCarried.MoveTo((game.level.selectedTile.x * 32) + 16, (game.level.selectedTile.y * 32) + 16);
                        game.towers.Add(towerBeingCarried);
                        towerBeingCarried = null;
                    }
                }
                ZIsBeingPressed = true;
            }
            else if(game.keyBoardState.IsKeyUp(Keys.Z)) ZIsBeingPressed = false;

            facing = Vector2.Zero;
            if (Math.Abs(velocity.X) > 0.05) facing.X = (velocity.X / Math.Abs(velocity.X)) + (velocity.X / Math.Abs(velocity.X));
            if (Math.Abs(velocity.Y) > 0.05) facing.Y = (velocity.Y / Math.Abs(velocity.Y)) + (velocity.Y / Math.Abs(velocity.Y));

            //Don't let the player outside of the screen
            if (center.X - Width / 2 < 0) center.X = Width / 2;
            if (center.X + Width / 2 > game.screenWidth) center.X = game.screenWidth - Width / 2;
            if (center.Y - Height / 2 < 0) center.Y = Height / 2;
            if (center.Y + Height / 2 > game.screenHeight) center.Y = game.screenHeight - Height / 2;

            //Move the player
            velocity *= 0.8f;
            center += velocity;

            //Create the destinationRectangle here, so that the drawing loop doesn't have to calculate it.
            destinationRectangle = new Rectangle((int)center.X - Width / 2, (int)center.Y - Height / 2, Width, Height);

            if (towerBeingCarried != null)
            {
                towerBeingCarried.MoveTo((int)center.X, (int)(center.Y - Height * 0.75f));
                towerBeingCarried.Update(game);
            }
            game.level.UpdateTile((int) ((center.X / 32) % 20 + facing.X), (int) ((center.Y / 32) % 20 + facing.Y));
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            if(towerBeingCarried != null) towerBeingCarried.Draw(sb);
            if (isCarryingTower) sb.Draw(ResourceManager.playerCarryingTexture, destinationRectangle, Color.White);
            else sb.Draw(ResourceManager.playerTexture, destinationRectangle, Color.White);
        }
    }
}
