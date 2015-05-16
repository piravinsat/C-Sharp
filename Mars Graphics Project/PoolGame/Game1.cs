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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using MarsGame.Classes;
using GraphicsCw;

namespace MarsGame
{

	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		#region ClassMembers
		GraphicsDeviceManager _graphics;
		SpriteBatch _spriteBatch;
		PoolTable _table;
		Matrix _world, _view, _projection;
		SampleGrid _grid;
		SampleArcBallCamera _camera; 
		Texture2D _rhulLogo;
		#endregion
		
		/// <summary>
		/// this is the data structure where we store all our our balls!
		/// </summary>
		List<Asteroids> _balls = new List<Asteroids>();
        Texture2D _background;
        SpriteFont _font;
        Mars _marsModel;
        Moon _deimos;
        Moon _phobos;

		#region Constructor
		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			_graphics.PreferMultiSampling = true;

			this.Window.AllowUserResizing = true;
			this.IsMouseVisible = true;

			Window.ClientSizeChanged +=new EventHandler<EventArgs>(Window_ClientSizeChanged);
		}

		/// <summary>
		/// we change the aspect ratio and projection matrix if the screen gets resized to make
		/// it look right all the time
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Window_ClientSizeChanged(object sender, EventArgs e)
		{
			float aspectRatio = (float)_graphics.GraphicsDevice.Viewport.Width /
					(float)_graphics.GraphicsDevice.Viewport.Height;
			float fov = MathHelper.PiOver4 * aspectRatio * 3 / 4;
		
			_projection = Matrix.CreatePerspectiveFieldOfView(fov,
				aspectRatio, .1f, 10000f);

			_graphics.ApplyChanges();
		} 
		#endregion

        DepthStencilState depthStensilState = null;

		#region Init
		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			base.Initialize();

            _graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            _graphics.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            _graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            depthStensilState = new DepthStencilState() { DepthBufferEnable = true };

            _graphics.ApplyChanges();

		} 
		#endregion

		private void AddSomeBalls(int howmany)
		{
			var balls = Enumerable
				.Range( 0, howmany )
				.Select( i => new Asteroids( _graphics.GraphicsDevice, _table, 100 ) );

			_balls.AddRange(balls);
		}
		#region LoadContent

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
            //Load the background
            _background = Content.Load<Texture2D>(@"Textures/space");
			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

            _table = new PoolTable(_graphics.GraphicsDevice);

            //Load the font
            _font = Content.Load<SpriteFont>(@"Fonts/Title");

            //Load Mars model
            _marsModel = new Mars(_graphics.GraphicsDevice);
            _marsModel.LoadContent(Content);

            //Load Deimos moon
            Model _demiosModel = Content.Load<Model>("Models/deimos");
            Texture2D _demiosTexture = Content.Load<Texture2D>("Textures/deimosbump");
            _deimos = new Moon(_graphics.GraphicsDevice, _demiosModel, _demiosTexture, new Vector3(4000,0,0), 30.35f);

            //Load Phobos moon
            Model _phobosModel = Content.Load<Model>("Models/phobos");
            Texture2D _phobosTexture = Content.Load<Texture2D>("Textures/phobosbump");
            _phobos = new Moon(_graphics.GraphicsDevice, _phobosModel, _phobosTexture, new Vector3(2000,0,0), 7.66f);


			SetupTable();

			///call load content on each of the ball instances
			_balls.ForEach(b => b.LoadContent(Content));

			///set up the grid
			_grid = new SampleGrid();
			_grid.GridColor = Color.Black;
			_grid.GridScale = 50.0f;
			_grid.GridSize = 100;
			_grid.LoadGraphicsContent(_graphics.GraphicsDevice);

			_camera = new SampleArcBallCamera(
							ArcBallCameraMode.RollConstrained);
			_camera.Distance = 8000;
			_camera.OrbitRight(MathHelper.PiOver4);
			///orbit up a bit for perspective
			_camera.OrbitUp(0.5f);

			float aspectRatio = (float)_graphics.GraphicsDevice.Viewport.Width /
				(float)_graphics.GraphicsDevice.Viewport.Height;
			float fov = MathHelper.PiOver4 * aspectRatio * 3 / 4;

			//_projection = Matrix.CreatePerspectiveFieldOfView(fov,
				//aspectRatio, .1f, 10000f);

            _projection = Matrix.CreatePerspectiveFieldOfView(fov,
                aspectRatio, .1f, 50000f);

			///create a default world matrix
			_world = Matrix.Identity;


			//Set the grid to draw on the x/z plane around the origin
			_grid.WorldMatrix = Matrix.Identity;
			
			_rhulLogo = Content.Load<Texture2D>("Textures/rhul_logo");

		}

		#endregion
		
		/// <summary>
		/// this is the method that adds balls to the table
		/// play around in here if you want to change the size, mass, colour etc of the balls
		/// </summary>
		private void SetupTable()
		{

			Random rand = new Random();
		
			/// a loop to 10
			for (int i = 0; i < 10; i++)
			{
				float radius = 100;

				Asteroids ball = new Asteroids(_graphics.GraphicsDevice, _table, radius);
				ball.Colour = Color.Gray;
				ball.Mass = 10;
				
				_balls.Add(ball);
			}

			/// a loop to 15
			for (int i = 0; i < 15; i++)
			{
				float radius = (float) (rand.NextDouble() * 150);
			    Asteroids ball = new Asteroids(_graphics.GraphicsDevice, _table, radius);
                ball.Colour = Color.Gray;
				ball.Mass = 20;
				_balls.Add(ball);
			}

			/// a loop to 3
			for (int i = 0; i < 3; i++)
			{
				//float radius = 300;
				//Ball ball = new Ball(_graphics.GraphicsDevice, _table, radius);
				//ball.Colour = Color.Green;
				//ball.Mass = 30;
				//_balls.Add(ball);
			}
		}

		#region UnloadContent
		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		} 
		#endregion
		#region Update

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				this.Exit();

			GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
			KeyboardState keyboardState = Keyboard.GetState();

            _marsModel.Update(gameTime);
            _phobos.Update(gameTime);
            _deimos.Update(gameTime);


			/// fullscreen toggle
			if( keyboardState.IsKeyDown(Keys.F) ) {
				_graphics.IsFullScreen = !_graphics.IsFullScreen;
				_graphics.ApplyChanges();
			}

			///shake up the balls!
			if (keyboardState.IsKeyDown(Keys.R))
			{
				ShakeUpTheBalls();
			}

			_camera.HandleDefaultGamepadControls(gamePadState, gameTime);
			_camera.HandleDefaultKeyboardControls(keyboardState, gameTime);

			///perform collision detection+response
			//Asteroids.PerformCollisionDetectionResponse(_balls, _table);

			///call Update on each of the ball instances
			_balls.ForEach(b => b.Update(gameTime));

			//handle inputs specific to this sample
			HandleInput(gameTime, gamePadState, keyboardState);

			_view = _camera.ViewMatrix;
			_grid.ViewMatrix = _camera.ViewMatrix;
			_grid.ProjectionMatrix = _projection;

			/// sinsoidal function, used for cool background colour oscillation!
			_sintime = (float) (Math.Sin( gameTime.TotalGameTime.TotalSeconds/2 ) + 1) / 2;

			base.Update(gameTime);
		}

		/// <summary>
		/// shake up all of the balls!!!
		/// </summary>
		private void ShakeUpTheBalls()
		{
			foreach( Asteroids ball in _balls ) {
			
				ball.SetRandomVelocity();
			
			}
		} 
		#endregion

		Color rhul_colour1 = new Color( 11f/255, 102f/255, 172f/255 );
		Color rhul_colour2 = new Color(26f / 255, 70f / 255, 132f / 255);
		float _sintime = 0;
		
		#region Draw
		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear( Color.Lerp( rhul_colour1, rhul_colour2, _sintime ) );

            Rectangle rect;
            rect.X = 0;
            rect.Y = 0;
            rect.Width = _graphics.GraphicsDevice.Viewport.Width;
            rect.Height = _graphics.GraphicsDevice.Viewport.Height;
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, rect, Color.White);
            _spriteBatch.End();

            GraphicsDevice.DepthStencilState = depthStensilState;

            ///call Draw on each of the ball instances
            _balls.ForEach(b => b.Draw(gameTime, _view, _projection));
            //_grid.Draw();

            //Draw Mars
            _marsModel.Draw(gameTime, _view, _projection);
            //Draw the Moons
            _deimos.Draw(gameTime, _view, _projection);
            _phobos.Draw(gameTime, _view, _projection);


            _spriteBatch.Begin();
			///draw the rhul logo
           _spriteBatch.Draw(_rhulLogo, new Rectangle(10, 10, 266, 77), Color.White);
		//	_spriteBatch.End();

          //  _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, "Mars Project", new Vector2(100, 100), Color.White);
            _spriteBatch.End();

			base.Draw(gameTime);
		} 
		#endregion

		#region HandleInput
		/// <summary>
		/// this stuff sends input to the camera -- controlled by thr xbox controller
		/// dont worry about this for now
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="gamePadState"></param>
		/// <param name="keyboardState"></param>
		private void HandleInput(GameTime gameTime, GamePadState gamePadState,
			KeyboardState keyboardState)
		{
			float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

			//handle mesh rotation inputs
			float dx =
				SampleArcBallCamera.ReadKeyboardAxis(keyboardState, Keys.Left,
				Keys.Right) + gamePadState.ThumbSticks.Left.X;
			float dy =
				SampleArcBallCamera.ReadKeyboardAxis(keyboardState, Keys.Down,
				Keys.Up) + gamePadState.ThumbSticks.Left.Y;

			//apply mesh rotation to world matrix
			if (dx != 0)
			{
				_world = _world * Matrix.CreateFromAxisAngle(_camera.Up,
					elapsedTime * dx);
			}
			if (dy != 0)
			{
				_world = _world * Matrix.CreateFromAxisAngle(_camera.Right,
					elapsedTime * -dy);
			}
		} 
		#endregion
	}
}
