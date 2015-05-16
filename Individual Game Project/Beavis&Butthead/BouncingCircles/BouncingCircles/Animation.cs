using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Circle
{
    class Animation
    {
        /* responsibility is to store a defined animation sequence */

        public Texture2D spriteSheet; // assumed to be a horizontal row of equal-sized pictures

        int nPictures; // number of frames in the spriteSheet

        int currentPicture; // between zero and the number of pictures

        Rectangle[] sourceRectangles; // the positions of each picture in the sprite sheet

        public Animation(Texture2D _spriteSheet, int _nPictures)
        {
            spriteSheet = _spriteSheet;
            nPictures = _nPictures;
            currentPicture = 0;
            //now set up the array of source Rectangles

            int pictureWidth = spriteSheet.Width / nPictures;
            int pictureHeight = spriteSheet.Height;

            sourceRectangles = new Rectangle[nPictures];
            for (int rectx = 0; rectx < nPictures; rectx++)
            {
                int rectXcoordinate = rectx * pictureWidth;
                sourceRectangles[rectx] = new Rectangle(rectXcoordinate, 0, pictureWidth, pictureHeight);
            }

        }

        public Rectangle GetSourceRectangle()
        {
            //   return sourceRectangles[currentPicture]; // This doesn't quite work !
            // We need to increment currentPicture, and 
            int thisPicture = currentPicture;
            currentPicture = currentPicture + 1;
            if (currentPicture >= nPictures)
            {
                currentPicture = 0;
            }
            return sourceRectangles[thisPicture];

        }



    }
}
