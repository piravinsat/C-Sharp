using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Circle
{
    class McVicker
    {
        public Vector2 centre; // position vector of centre of circle
        public float radius; //radius of circle
        public Vector2 velocity; //velocity of circle

        public Texture2D sprite = null; //sprite to draw
        bool IsAnimated;
        Animation animation;

        Rectangle destination = new Rectangle(0, 0, 0, 0); //where the sprite will appear
        public Color color = Color.White;
        public bool isColliding = false;
        public int health = 1;
        public int damage = 1;


        public McVicker(Vector2 the_centre, float the_radius,
    Vector2 the_velocity, Texture2D the_sprite)
        {
            centre = the_centre;
            radius = the_radius;
            velocity = the_velocity;
            sprite = the_sprite;

            IsAnimated = false;
        }


            public McVicker(Vector2 the_centre, float the_radius, Vector2 the_velocity, Texture2D the_spriteSheet, int the_nPictures)
            {
                centre = the_centre;
                radius = the_radius;
                velocity = the_velocity;
                sprite = the_spriteSheet;
                IsAnimated = true;
                animation = new Animation(the_spriteSheet, the_nPictures);
            }

            //private float RotationAngle;
        public void Update(int screenWidth, int screenHeight)
        {
            centre += velocity;

            if (centre.X - radius < 0)
            {
                centre.X = radius;
                if (velocity.X < 0) // put this condition in to prevent the 'sticking bug'
                {
                    velocity.X = -velocity.X;
                }
            }
            if (centre.Y - radius < 0) // off top of screen
            {
                centre.Y = radius;
                if (velocity.Y < 0)
                {
                    velocity.Y = -velocity.Y;
                }
            }
            if (centre.X + radius > screenWidth)
            {
                centre.X = screenWidth - radius;
                if (velocity.X > 0)
                {
                    velocity.X = -velocity.X;
                }
            }
            if (centre.Y + radius > screenHeight)
            {
                centre.Y = screenHeight - radius;
                if (velocity.Y > 0)
                {
                    velocity.Y = -velocity.Y;
                }
            }

            destination = new Rectangle((int)(centre.X - radius),
                (int)(centre.Y - radius),
                (int)(2*radius),
                (int)(2*radius) );
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

        public void Draw(SpriteBatch sbatch)
        {
            if (!IsAnimated)
            {
                sbatch.Draw(sprite, destination, color);
            }
            else
            {
                sbatch.Draw(animation.spriteSheet, destination, animation.GetSourceRectangle(), color);
            }
        }
        public static bool InCollision(McVicker circ1, McVicker circ2)
        {
            Vector2 centre2centre = circ2.centre - circ1.centre;
            float squaredDistanceOfCentres = centre2centre.LengthSquared();
            float radiusSum = circ1.radius + circ2.radius;
            return squaredDistanceOfCentres < radiusSum * radiusSum;
        }

        public static void Bounce(McVicker c1, McVicker c2)
        {
            // get vector between centres
            Vector2 c2c = c2.centre - c1.centre;

            // get relative velocity
            Vector2 relv = c2.velocity - c1.velocity;

            // find component of relative velocity along line betwen centres
            Vector2 rvcc = (Vector2.Dot(relv, c2c) / Vector2.Dot(c2c, c2c)) * c2c;

            // add equal and opposite velocities to both circles to reverse the relative velocity
            if(Vector2.Dot(c2c,relv) < 0)
            {
            c2.velocity = c2.velocity - rvcc;
            c1.velocity = c1.velocity + rvcc;
            }
        }


    }
}
