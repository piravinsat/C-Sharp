using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MoonDefense
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum GameStates
        {
            inPlay,
            gameOver,
        }
        public GameStates state = GameStates.inPlay;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public List<Bullet> bullets = new List<Bullet>();
        public List<Tower> towers = new List<Tower>();
        public List<Mob> mobs = new List<Mob>();
        public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

        Player player;
        public Level level;
        ResourceManager resources;
        public bool a = false;
        Texture2D b;
        int c;

        public KeyboardState keyBoardState;
        public int screenHeight = 640, screenWidth = 640;
        public double time;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            resources = new ResourceManager(Content);
            b = Content.Load<Texture2D>("KING");

            player = new Player();
            level = new Level(this);
            
            //Change the screen size
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();
        }
        protected override void UnloadContent()
        {
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (state == GameStates.inPlay)
            {
                keyBoardState = Keyboard.GetState();

                time = gameTime.TotalGameTime.TotalMilliseconds;

                player.Update(this);

                foreach (Tower t in towers) t.Update(this);
                for (int i = 0; i < bullets.Count; i++)
                {
                    Bullet b = bullets[i];
                    if (b.shouldRemove) bullets.RemoveAt(i);
                    b.Update(this);
                }

                foreach (SpawnPoint spawnPoint in spawnPoints) spawnPoint.Update(this);
                for (int i = 0; i < mobs.Count; i++)
                {
                    Mob m = mobs[i];
                    if (m.shouldRemove) mobs.RemoveAt(i);
                    m.Update(this);
                }

                if (level.moonBase.Health <= 0) state = GameStates.gameOver;
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            level.Draw(spriteBatch);
            foreach (Tower t in towers) t.Draw(spriteBatch);
            player.Draw(spriteBatch);
            foreach (Mob m in mobs) m.Draw(spriteBatch);
            foreach (Bullet b in bullets) b.Draw(spriteBatch);
            if (a)
            {
                spriteBatch.Draw(b, new Rectangle(0, 0, screenWidth, screenHeight), new Rectangle(c++ * 133, 0, 133, 158), Color.White);
                if (c > 1) c = 0;
            }
            if(state == GameStates.gameOver) spriteBatch.DrawString(ResourceManager.spriteFont, level.moonBase.Points.ToString(), new Vector2((screenWidth / 2) - 50, (screenHeight  / 2)- 50), Color.Yellow);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
