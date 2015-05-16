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
using System.IO;
using XNATD.InGameObjects;
using XNATD.UI;

namespace XNATD
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        private const string LEVELS_PATH = "Content/Levels";
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Level> levels;
        Random random = new Random();
        DateTime lastWaveAt = DateTime.Now;
        TimeSpan waveDelay = new TimeSpan(0, 0, 0, 1);
        int waveCount;

        public ResourceManager ResourceManager { get; set; }
        public Level CurrentLevel { get; set; }
        public Cursor Cursor { get; private set; }
        public List<IInGameObject> GameObjects { get; set; }
        public List<Tower> Towers { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<IInGameObject> UIElements { get; set; }
        public MouseState MouseState { get { return Mouse.GetState(); } }
        public int Lives { get; set; }
        public int Coins { get; set; }

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;

            Enemies = new List<Enemy>();
            GameObjects = new List<IInGameObject>();
            Towers = new List<Tower>();

            // set default values
            Lives = 10;
            Coins = 30;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // initialise the resource manaer that we'll pass around
            ResourceManager = new ResourceManager(Content);
            // read in the levels from the directory
            levels = GetLevelsList();
            CurrentLevel = levels[0];
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // create the cursor and toolbar
            Cursor = new Cursor(ResourceManager);
            Toolbar toolbar = new Toolbar(GraphicsDevice.Viewport.Width - 100, 30, ResourceManager);

            // and add them, including the current level, to the list of game objects
            GameObjects.Add(CurrentLevel);
            GameObjects.Add(toolbar);
            GameObjects.Add(Cursor);
        }


        /// <summary>
        /// Retrieve a list of level for the game
        /// </summary>
        private List<Level> GetLevelsList()
        {
            const char PATH_TOKEN = 'p';
            const char GROUND_TOKEN = '-';
            const char START_TOKEN = 's';
            const char END_TOKEN = 'e';

            // get a reference to an enumerable for the levels directory
            IEnumerable<String> directoryEnumerable = Directory.EnumerateFiles(LEVELS_PATH);

            // initialise a levels list
            List<Level> levels = new List<Level>();

            // enumerate through the filenames in the level directory
            foreach (string fileName in directoryEnumerable)
            {
                // get a reference to an enumerable for the file's lines
                List<string> linesInFile = File.ReadLines(fileName).ToList();

                // list of lists of tiles -- the grid for the level
                List<List<Level.LevelTile>> levelTileGrid = new List<List<Level.LevelTile>>();

                // enumerate through the file's lines
                int lineNumber = 0;
                foreach (string line in linesInFile)
                {
                    // the list of tiles for the current line
                    List<Level.LevelTile> lineTiles = new List<Level.LevelTile>();

                    // enumerate through the characters in the line
                    int characterNumber = 0;
                    foreach (char character in line)
                    {
                        // initalising tile to a default
                        Level.LevelTile tile = Level.LevelTile.Nothing;

                        // select a tile depending on the character
                        switch (character)
                        {
                            case GROUND_TOKEN: tile = Level.LevelTile.Ground; break;
                            case PATH_TOKEN: tile = Level.LevelTile.PathVertical; break;
                            case START_TOKEN: tile = Level.LevelTile.StartPoint; break;
                            case END_TOKEN: tile = Level.LevelTile.EndPoint; break;
                        }

                        // change to corner if the tile is at a junction between two paths
                        if (tile == Level.LevelTile.PathVertical)
                        {
                            if (lineNumber - 1 > 0 && linesInFile[lineNumber - 1][characterNumber] == PATH_TOKEN)
                            {
                                if (characterNumber + 1 < line.Length && line[characterNumber + 1] == PATH_TOKEN)
                                {
                                    tile = Level.LevelTile.PathCornerDownToRight;
                                }
                                else if (characterNumber - 1 > 0 && line[characterNumber - 1] == PATH_TOKEN)
                                {
                                    tile = Level.LevelTile.PathCornerDownToLeft;
                                }
                            }
                            else if (lineNumber + 1 < linesInFile.Count && linesInFile[lineNumber + 1][characterNumber] == PATH_TOKEN)
                            {
                                if (characterNumber + 1 < line.Length && line[characterNumber + 1] == PATH_TOKEN)
                                {
                                    tile = Level.LevelTile.PathCornerUpToRight;
                                }
                                else if (characterNumber - 1 > 0 && line[characterNumber - 1] == PATH_TOKEN)
                                {
                                    tile = Level.LevelTile.PathCornerUpToLeft;
                                }
                            }

                            // set the path tile to horizontal if it has a path left and right
                            if (tile == Level.LevelTile.PathVertical)
                            {
                                if ((characterNumber + 1 < line.Length && line[characterNumber + 1] == PATH_TOKEN) &&
                                    (characterNumber - 1 > 0 && line[characterNumber - 1] == PATH_TOKEN))
                                {
                                    tile = Level.LevelTile.PathHorizontal;
                                }
                            }
                        }
                        // add the tile to the list
                        lineTiles.Add(tile);

                        characterNumber++;
                    }

                    // add the line of tiles to the grid
                    levelTileGrid.Add(lineTiles);

                    lineNumber++;
                }

                // flatten each line of the grid to an array
                List<Level.LevelTile[]> levelTilesArrayList = levelTileGrid.Select(line => line.ToArray()).ToList();
                // flatten the list of the lines to an array
                Level.LevelTile[][] levelTilesArrayArray = levelTilesArrayList.ToArray();
                // add the level
                levels.Add(new Level(levelTilesArrayArray, ResourceManager));
            }

            return levels;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>s
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // remove all the game objects that are no longer needed
            for (int i = 0; i > GameObjects.Count; i++)
            {
                if (GameObjects[i].ShouldBeRemoved())
                {
                    GameObjects.Remove(GameObjects[i]);
                }
            }

            // if the user presses, escape, exit the game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (DateTime.Now - lastWaveAt >= new TimeSpan(0, 0, 0, 0, random.Next(500, 5000 + (int)Math.Ceiling(0.1f*waveCount/4))))
            {
                Enemy enemy = new Enemy(40 + (int)Math.Ceiling(2.0f*(waveCount^10)), new Vector2(0, 1), (int)CurrentLevel.StartCoord.X + 10, (int)CurrentLevel.StartCoord.Y + 10, ResourceManager);
                Enemies.Add(enemy);
                GameObjects.Add(enemy);
                lastWaveAt = DateTime.Now;
                waveCount++;
            }

            for (int i = 0; i < GameObjects.Count(); i++)
            {
                GameObjects[i].Update(this, gameTime);
            }

            base.Update(gameTime);

            Window.AllowUserResizing = true;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            if (Lives > 0)
            {
                // draw each of the game objects
                foreach (IInGameObject gameObject in GameObjects)
                {
                    gameObject.Draw(spriteBatch);
                }
            }
            else
            {
                // display "game over" message
                spriteBatch.DrawString(ResourceManager.Font, "GAME OVER \n <press ESC to exit>", new Vector2 (500, 500), Color.White);
                Lives = 0;
            }

            spriteBatch.DrawString(ResourceManager.Font, "Lives " + Lives, new Vector2(2), Color.Black);
            spriteBatch.DrawString(ResourceManager.Font, "Lives " + Lives, new Vector2(1), Color.White);

            spriteBatch.DrawString(ResourceManager.Font, "Coins " + Coins, new Vector2(2, 20), Color.Black);
            spriteBatch.DrawString(ResourceManager.Font, "Coins " + Coins, new Vector2(1, 20), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void AddTower()
        {
            Tower tower = new Tower(5, 50, ResourceManager);
            Towers.Add(tower);
            GameObjects.Add(tower);
        }
    }
}
