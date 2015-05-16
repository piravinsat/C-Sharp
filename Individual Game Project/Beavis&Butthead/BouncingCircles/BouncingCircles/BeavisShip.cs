using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Circle
{
    class BeavisShip
    {
        public Vector2 centre; // position vector of centre of circle
        public float radius; //radius of circle
        public Vector2 velocity; //velocity of circle
        public Texture2D sprite = null; //sprite to draw
        public bool isActive;
        public int health;

        public BeavisShip(Vector2 the_centre, float the_radius,
    Vector2 the_velocity, Texture2D the_sprite)
        {
            centre = the_centre;
            radius = the_radius;
            velocity = the_velocity;
            sprite = the_sprite;
            isActive = true;
            health = 3;
        }

        public void Update(KeyboardState kbs)
        {

            // TODO: Add your update logic here
            //System.Console.WriteLine("This is the Draw method");

            if (kbs.IsKeyDown(Keys.D)) //Sprite goes right
            {
                velocity.X = velocity.X + 0.1f;
            }

            if (kbs.IsKeyDown(Keys.A)) //Sprite goes left
            {
                velocity.X = velocity.X - 0.1f;
            }
            if (kbs.IsKeyDown(Keys.S)) //Sprite goes down
            {
                velocity.Y = velocity.Y + 0.1f;
            }
            if (kbs.IsKeyDown(Keys.W)) //Sprite goes up
            {
                velocity.Y = velocity.Y - 0.1f;
            }

            if (!kbs.IsKeyDown(Keys.D) | !kbs.IsKeyDown(Keys.A) | !kbs.IsKeyDown(Keys.S) | !kbs.IsKeyDown(Keys.W))
            {
                velocity.X = velocity.X * 0.99f;
                velocity.Y = velocity.Y * 0.99f;
            }

            if (centre.Y + velocity.Y > Game1.viewportRect.Height - Height)
            {
                velocity.Y = velocity.Y * -1;
            }

            if (centre.X + velocity.X > Game1.viewportRect.Width - Width)
            {
                velocity.X = velocity.X * -1;
            }

            if (centre.Y + velocity.Y < 0)
            {
                velocity.Y = velocity.Y * -1;
            }

            if (centre.X + velocity.X < 0)
            {
                velocity.X = velocity.X * -1;
            }

            centre.Y = centre.Y + velocity.Y;
            centre.X = centre.X + velocity.X;


            centre += velocity;
        }

        public int Width
        {
            get { return sprite.Width; }
        }

        public int Height
        {
            get
            {
                return sprite.Height;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 centre)
        {
            spriteBatch.Draw(sprite, centre, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
        } 
    }
}
