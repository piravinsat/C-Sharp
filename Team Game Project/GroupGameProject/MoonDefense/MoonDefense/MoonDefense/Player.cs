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
        Vector2 center, velocity;
        int health, Width, Height;
        bool canFire = true;
        Vector2 bulletVelocity;
        int bulletSpeed = 15;
        Rectangle destinationRectangle, sourceRectangle;
        double oldTime, timeBetweenShots = 1000;

        public Player()
        {
            Width = ResourceManager.playerTexture.Width;
            Height = ResourceManager.playerTexture.Height;
        }

        public void Update(Game1 game)
        {
            if (game.time - oldTime > timeBetweenShots)
            {
                canFire = true;
                oldTime = game.time;
            }
            //Fire bullets according to key presses
            if (game.keyBoardState.IsKeyDown(Keys.Space) && canFire)
            {
                //This has a strange bug, where if you press left and right (or up and down) at the same time
                //the player can't move.
                if(game.keyBoardState.IsKeyDown(Keys.Up)) bulletVelocity.Y -= bulletSpeed;
                if (game.keyBoardState.IsKeyDown(Keys.Down)) bulletVelocity.Y += bulletSpeed;
                if (game.keyBoardState.IsKeyDown(Keys.Left)) bulletVelocity.X -= bulletSpeed;
                if (game.keyBoardState.IsKeyDown(Keys.Right)) bulletVelocity.X += bulletSpeed;
                //Allow only moving bullets to be added
                if (bulletVelocity != Vector2.Zero) game.bullets.Add(new Bullet(center, bulletVelocity));
                bulletVelocity = Vector2.Zero;
                canFire = false;
            }

            //Change player velocity
            if (game.keyBoardState.IsKeyDown(Keys.W)) velocity.Y--;
            if (game.keyBoardState.IsKeyDown(Keys.S)) velocity.Y++;
            if (game.keyBoardState.IsKeyDown(Keys.A)) velocity.X--;
            if (game.keyBoardState.IsKeyDown(Keys.D)) velocity.X++;

            //Don't let the player outside of the screen
            if (center.X - Width / 2 < 0) center.X = Width / 2;
            if (center.X + Width / 2 > game.screenWidth) center.X = game.screenWidth - Width / 2;
            if (center.Y - Height / 2 < 0) center.Y = Height / 2;
            if (center.Y + Height / 2 > game.screenHeight) center.Y = game.screenHeight - Height / 2;

            //Move the player
            velocity.X *= 0.89f;
            velocity.Y *= 0.89f;
            center += velocity;

            //Create the destinationRectangle here, so that the drawing loop doesn't have to calculate it.
            destinationRectangle = new Rectangle((int)center.X - Width / 2, (int)center.Y - Height / 2, Width, Height);
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.Draw(ResourceManager.playerTexture, destinationRectangle, Color.White);
        }
    }
}
