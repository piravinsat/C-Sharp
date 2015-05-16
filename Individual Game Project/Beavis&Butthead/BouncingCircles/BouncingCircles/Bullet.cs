using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Circle
{
    class Bullet
    {
        public Vector2 centre; // position vector of centre of circle
        public float radius = 5.0f; //radius of circle

        public Texture2D sprite = null; //sprite to draw

        Rectangle destination = new Rectangle(0, 0, 0, 0); //where the sprite will appear
        public Color color = Color.White;
        public bool isActive;
        public int damage = 1;
        float bulletMoveSpeed;

        Rectangle viewport;

        public Bullet(Texture2D the_sprite)
        {
            sprite = the_sprite;
        }

        public void Initialize(Rectangle viewport, Texture2D the_sprite, Vector2 the_centre)
        {
            sprite = the_sprite;
            centre = the_centre;
            this.viewport = viewport;

            isActive = true;
            bulletMoveSpeed = 20f;
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

        //private float RotationAngle;
        public void Update()
        {
            //Bullet goes right
            centre.X += bulletMoveSpeed;

            //Bullet disappears when off screen
            if (centre.X + sprite.Width / 2 > viewport.Width)
            {
                isActive = false;
            }
        }

        public void Draw(SpriteBatch sbatch)
        {

            sbatch.Draw(sprite, centre, null, Color.White, 0f, new Vector2(Width/2, Height/2), 1f, SpriteEffects.None, 0f);
        }

    }
    }
