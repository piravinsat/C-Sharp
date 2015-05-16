using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace Circle
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D enemyTexture;
        Texture2D playerTexture;
        Texture2D background;

        Texture2D bulletTexture;
        List<Bullet> bullets = new List<Bullet>();
        TimeSpan fireTime;
        TimeSpan previousFireTime;
        
        

        int animationLength; // number of pictures in animation
        Random rg = new Random();

        bool isHold = false;

        int nEnemies = 6;
        McVicker[] circles;
        BeavisShip player;


       public static Rectangle viewportRect;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //Set the ship to fire bullet every 1/4 of a second
            fireTime = TimeSpan.FromSeconds(.15f);

            //System.Console.WriteLine("This is the Initalise method");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            viewportRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);

            // TODO: use this.Content to load your game content here
            //System.Console.WriteLine("This is the LoadContent method");
            enemyTexture = Content.Load<Texture2D>("faces");
            background = Content.Load<Texture2D>("basket_ball_court");
            bulletTexture = Content.Load<Texture2D>("Bullet");
            playerTexture = Content.Load<Texture2D>("ShipTest");

            animationLength = 9;

            circles = new McVicker[nEnemies]; // sets up an array to hold circles in.

            Vector2 randomPosition;
            Vector2 randomVelocity;

            float circleRadius = 25.0f;

            for (int i = 0; i < nEnemies; i++)
            {
                randomPosition = new Vector2((float)(rg.NextDouble() * 100), (float)(rg.NextDouble() * 100));
                randomVelocity = new Vector2((float)(rg.NextDouble() * 3), (float)(rg.NextDouble() * 3));
                circles[i] = new McVicker(randomPosition, circleRadius, randomVelocity, enemyTexture, animationLength);
            }

            player = new BeavisShip(new Vector2(100,100), 20.0f, Vector2.Zero, playerTexture);
        }

        private void AddBullets(Vector2 position)
        {
            Bullet bullet = new Bullet(bulletTexture); 
            
            bullets.Add(bullet);
            bullet.Initialize(viewportRect, bulletTexture, position);
        }

        private void UpdateBullets() //Bullets already in the air
        {
            //Update the Bullets
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update();
                if (bullets[i].isActive == false)
                {
                    bullets.RemoveAt(i);
                }
            }
        }

        private void updateCollision()
        {
            Rectangle rectangle1;
            Rectangle rectangle2;


        // Only create the rectangle once for the player
        rectangle1 = new Rectangle((int)player.centre.X,
        (int)player.centre.Y,
        player.Width,
        player.Height);

        // Ship vs Enemy
        for (int i = 0; i <nEnemies; i++)
        {
        rectangle2 = new Rectangle((int)circles[i].centre.X,
        (int)circles[i].centre.Y,
        circles[i].Width,
        circles[i].Height);

        // Determine if the two objects collided with each
        // other
        if(rectangle1.Intersects(rectangle2))
        {
        // Subtract the health from the player based on
        // the enemy damage
        player.health -= circles[i].damage;

        // Since the enemy collided with the player
        // destroy it
        circles[i].health = 0;

        // If the player health is less than zero we died
        if (player.health <= 0)
        player.isActive = false;
        }

        }
        
            //Enemy vs Enemy Collision
            foreach (McVicker c in circles)
            {
                c.isColliding = false;
            }

            for (int i = 0; i < nEnemies; i++)
                for (int j = i + 1; j < nEnemies; j++)
                {
                    if (McVicker.InCollision(circles[i], circles[j]))
                    {
                        circles[i].isColliding = true;
                        circles[j].isColliding = true;

                        McVicker.Bounce(circles[i], circles[j]);
                    }

                }
            // now we have marked each circle according to whether it is colliding or not

            foreach (McVicker c in circles)
            {
                if (c.isColliding)
                {
                    c.color = Color.Orange;
                }
                else
                {
                    c.color = Color.Green;
                }
            }

            // Bullet vs Enemy Collision
            for (int i = 0; i < bullets.Count; i++)
            {
                for (int j = 0; j < nEnemies; j++)
                {
                    // Create the rectangles we need to determine if we collided with each other
                    rectangle1 = new Rectangle((int)bullets[i].centre.X -
                    bullets[i].Width / 2, (int)bullets[i].centre.Y -
                    bullets[i].Height / 2, bullets[i].Width, bullets[i].Height);

                    rectangle2 = new Rectangle((int)circles[j].centre.X - circles[j].Width / 2,
                    (int)circles[j].centre.Y - circles[j].Height / 2,
                    circles[j].Width, circles[j].Height);

                    // Determine if the two objects collided with each other
                    if (rectangle1.Intersects(rectangle2))
                    {
                        circles[j].health -= bullets[i].damage;
                        //bullets[i].isActive = false;
                    }
                }
            } 
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            //System.Console.WriteLine("This is the UnloadContent method");
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>     
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        //private float RotationAngle;
        protected override void Update(GameTime gameTime)
        {       
             KeyboardState keyboardState;
             keyboardState = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            player.Update(keyboardState);

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                if (isHold == false)
                {
                    AddBullets(player.centre);
                    isHold = true;
                }
            }

            if (keyboardState.IsKeyUp(Keys.Space))
            {
                isHold = false;
            }


            for (int i = 0; i < nEnemies-1; i++)
            {
                circles[i].Update(viewportRect.Width, viewportRect.Height); // this moves the circles and makes them 
                //bounce off thes sides of the screen
            }

            // TODO: Add your game logic here.
          
            updateCollision();
            UpdateBullets();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0.0f, 0.0f), Color.White);          

            for (int i = 0; i < nEnemies; i++)
            {
                if (circles[i].health >= 0)
                {
                    circles[i].Draw(spriteBatch);
                }
            }

            player.Draw(spriteBatch, player.centre);

            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].isActive)
                {
                    bullets[i].Draw(spriteBatch);
                }
            }
            spriteBatch.End();
            //System.Console.WriteLine("This is the Draw method");
            base.Draw(gameTime);
        }
    }
}
