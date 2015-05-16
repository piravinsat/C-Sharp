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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public List<Bullet> bullets = new List<Bullet>();
        public List<Tower> towers = new List<Tower>();
        public List<Mob> mobs = new List<Mob>();
        public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

        Player player;
        public Level level;
        ResourceManager resources;

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

            player = new Player();
            level = new Level();
            spawnPoints.Add(new SpawnPoint(100, 100, 0.5));
            
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

            keyBoardState = Keyboard.GetState();

            time = gameTime.TotalGameTime.TotalMilliseconds;

            player.Update(this);

            foreach (Tower t in towers) t.Update(null);
            for (int i = 0; i < bullets.Count; i++)
            {
                Bullet b = bullets[i];
                if (b.shouldRemove) bullets.RemoveAt(i);
                b.Update(this);
            }

            foreach (SpawnPoint spawnPoint in spawnPoints) spawnPoint.Update(this);
            foreach (Mob m in mobs) m.Update(this);

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            level.Draw(spriteBatch);
            foreach (Tower t in towers) t.Draw(spriteBatch);
            player.Draw(spriteBatch);
            foreach (Bullet b in bullets) b.Draw(spriteBatch);
            foreach (Mob m in mobs) m.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
