using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MarsGame.Classes
{
	/// <summary>
	/// this is the ball class. It's a kind of general blueprint for all the balls that 
	/// are in the game. You may have heard of "object oriented programming" well now you are 
	/// doing it! The concept here is that we describe everything about a ball and what it can 
	/// do in the general. Then you can create as many "instances" of the ball as you like without
	/// having to write the same-ish code twice. So we are reducing redundant code!
	/// </summary>
	public class Asteroids
	{
		///Up here we are creating all the "properties" of the ball class. 
		///These properties describe its state like position, colour etc. 
		///We add all the properties that we might like to change from instance to instance.
		///The "public" means that these properties can be modified from outside the class.
	
		public Vector3 Position;
		public Vector3 Velocity;
		public Color Colour;
		public float Radius;
		public static float BallBounce = 0.2f;
		public static float BallFriction = 0.9f;
		public static float InitialSpeed = 10000.5f;
		public float Mass= 1.0f;
        public Texture2D _moonTexture;

		
		///Down here we are creating some private class variables, we made these private
		///because we dont need to change them from the outside of the class. So we can only
		///access these variables from inside this ball class. Observe the naming convention, 
		///we use camelCase and prefix with an underscore. This means everywhere you can see
		///its a class-level member variable. Any variable you create is accessible from the current
		///scope and below, scopes are encapsulated with the curly braces i.e. { and }
		///Variables created in "methods" which is the next level of scope down don't start with an underscore by
		///convention
		
		private Model _model;
		private GraphicsDevice _device;
		private PoolTable _table;
		
		
		/// <summary>
		/// random variable shared by all instances of balls, the "static" means its not 
		/// just for one instance of a ball, but rather all balls
		/// </summary>
		private static Random _random = new Random();

		/// <summary>
		/// this is the constructor for the Ball class. When you initialize a ball from the outside,
		/// the code in the class will be executed. The main purpose here is we set the _device to reference 
		/// something from the outside, so we grab the reference from the parameter
		/// we are also setting some defaults on the properties -- these can be modified form the outside
		/// </summary>
		/// <param name="device"></param>
		public Asteroids( GraphicsDevice device, PoolTable table, float radius) 
		{
			_device = device;
			_table = table;
			///set the radius
			Radius = radius;
			///generate a random position on the table
			//Position = new Vector3((float)(_random.NextDouble()) * table.Width, (float)(_random.NextDouble()) * table.Height, radius);

            //Generates at a random position around Mars and its moons.
            Position = new Vector3((float)(_random.NextDouble() - 0.5) * table.Width, (float)(_random.NextDouble() - 0.5) * table.Height, radius);


			Colour = new Color((float)_random.NextDouble(), (float)_random.NextDouble(), (float)_random.NextDouble());

			/// this code is implemented as a method so that it can be called again from outside the ball class
			/// at any time for example the game class will shake up the balls when the user presses the R key
			SetRandomVelocity();
			
		}

		public void SetRandomVelocity()
		{
			///generate a random displacement on the table
			Velocity = new Vector3((float)_random.NextDouble() * InitialSpeed, (float)_random.NextDouble() * InitialSpeed, (float)_random.NextDouble() * InitialSpeed);
		}
		
		/// <summary>
		/// this it the method where we load in resources relevant to this class
		/// from the "content pipeline" such as textures, models etc
		/// </summary>
		/// <param name="contentManager"></param>
		public void LoadContent( ContentManager contentManager ) 
		{
            //Load the ball model
			_model = contentManager.Load<Model>("Models/ball");
            //Load up one of the Mars moon's texture as those moons were probably once Asteroids.
            _moonTexture = contentManager.Load<Texture2D>("Textures/deimosbump");
		}
		
		/// <summary>
		/// this is the update method -- this critical method is where the properties (state)
		/// of the ball are changed over time (note the gametime parameter). So in effect in this 
		/// method you can create your own custom dependency between the ball state i.e. position, velocity
		/// and the elapsed game time. This will allow you to create interesting animations or anything you want.
		/// We are using newtonian mechanics to model the system i.e. force, acceleration, velocity, mass et all
		/// check out newtons second law on the web
		/// </summary>
		/// <param name="gametime"></param>
		public void Update(GameTime gametime)
		{
            //Randomising the speed of the Asteroids going towards Mars
            double speed = ((_random.NextDouble() * 0.0099) + 0.99);
            Position = new Vector3(Position.X * (float)speed, Position.Y * (float)speed, Position.Z * (float)speed);
            //Position = new Vector3(Position.X * (float)0.9999, Position.Y * (float)0.9999, Position.Z * (float)0.9999);

			/// apply friction and update the position of the ball
			
			//float speed = Velocity.Length();

			//Vector3 direction = -Vector3.Normalize( Velocity );
			
			///gravity constant
			//float g = 1000f;
			
			//Vector3 forces = 
				//(speed * direction) * 2  /// the friction force slowing the balls down
				//+ (Mass * g * -Vector3.UnitZ); /// the force of gravity
			
			//Vector3 acceleration = (forces / Mass);
			//float dt = (float) gametime.ElapsedGameTime.TotalSeconds;
			
			///Update the position of the ball
			//Position = Position + dt * Velocity;
			//Velocity = Velocity + dt * acceleration;
		}
		
		/// <summary>
		/// this is the draw method. XNA is an "immediate mode" graphics system,
		/// this means that the entire scene is destroyed and constructed each frame
		/// and there are about thirty frames+ a second! so this code is getting called
		/// AGAIN and AGAIN ... but clearly if we have some kind of animation going on
		/// then the positions of the objects are changing over time so that they appear to move!
		/// </summary>
		/// <param name="gametime"></param>
		public void Draw( GameTime gametime, Matrix view, Matrix projection ) 
		{
			Matrix[] transforms = new Matrix[_model.Bones.Count];
			_model.CopyAbsoluteBoneTransformsTo(transforms);

            // controls how triangles are turned into pixels
			_device.RasterizerState = RasterizerState.CullCounterClockwise;
           	foreach (ModelMesh mesh in _model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.EnableDefaultLighting();

					///the diffuse colour is where we set what colour the ball is!
					effect.DiffuseColor = Colour.ToVector3();

					///this is the critical line in this method -- it describes WHERE the balls are drawn
					///with Matrix.CreateTranslation, and also any "transformations" that are applied to the object
					///being drawn like skew, scale, rotations etc. In this case today we are only interested in saying
					///where the ball is and how big it is, so we use Matrix.CreateTranslation (where) and 
					///Matrix.CreateScale (how big it is)
					effect.World
					= transforms[mesh.ParentBone.Index]
						* Matrix.CreateScale( Radius  )
						* Matrix.CreateTranslation(Position.X, Position.Z, Position.Y);
						//* Matrix.CreateTranslation(-2500, 0, -2500);

					///these params get passed in from the main game class
					///remember we are in a 3d world, so these 2 parameters describe where the 
					///camera is, its orientation, where it's looking at and the field of view etc. 
					effect.View = view;
					effect.Projection = projection;
                    effect.Texture = _moonTexture;
				}

				mesh.Draw();
			}
		}
		
		/// <summary>
		/// this method is different from the rest. This is a "static" method. That basically
		/// means that this method has nothing to do with an instance of a ball, but is rather
		/// more relevant for ALL instances of balls. We use this method to look at all the balls on the 
		/// table, check if they are intersecting (overlapping) each other and if they are, bounce them away!
		/// </summary>
		/// <param name="balls">this is the master list of balls, passed in from the game class</param>
		/*public static void PerformCollisionDetectionResponse( List<Asteroids> balls, PoolTable table ) 
		{
			for(int i = 0; i < balls.Count; i++ ) {
				Asteroids ball1 = balls[i];
				for (int j = i+1; j < balls.Count; j++)
				{
					Asteroids ball2 = balls[j];
					
					Vector3 between = ball2.Position - ball1.Position;
					Vector3 normal = Vector3.Normalize( between);
					float length = between.Length();
					float intersectAmount = (ball1.Radius + ball2.Radius) - length;
					
					///are they intersecting?
					if (intersectAmount > -1)
					{
						Vector3 relativeVelocity = ball1.Velocity - ball2.Velocity;
						float perpAmount = Vector3.Dot(relativeVelocity, normal);
						if (perpAmount > 0){
							Vector3 perpVelocity = perpAmount * normal * BallBounce;
							///bounce ball1
							ball1.Velocity -=  perpVelocity;
							ball2.Velocity +=  perpVelocity;
						}
						
					}
				}
			}
		
			///bounce the balls off the walls
			///we are essentially trying to bound the X and Y components of the balls position to fit within the table
			///when they overshoot, we reverse the component applying some decay (table.WallBounce) i.e. 0.99 so that the bounce isnt completley elastic
			balls.Where( b => b.Position.X < 0 ).ToList().ForEach( b => { b.Position.X=0; b.Velocity.X*=-table.WallBounce; } );
			balls.Where(b => b.Position.X > table.Width).ToList().ForEach(b => { b.Position.X = table.Width; b.Velocity.X *= -table.WallBounce; });
			balls.Where(b => b.Position.Y < 0).ToList().ForEach(b => { b.Position.Y = 0; b.Velocity.Y *= -table.WallBounce; });
			balls.Where(b => b.Position.Y > table.Height).ToList().ForEach(b => { b.Position.Y = table.Height; b.Velocity.Y *= -table.WallBounce; });
            balls.Where(b => b.Position.Z < 0).ToList().ForEach(b => { b.Position.Z = 0; b.Velocity.Z *= -table.WallBounce; });
            balls.Where(b => b.Position.Z > table.Height).ToList().ForEach(b => { b.Position.Z = table.Height; b.Velocity.Z *= -table.WallBounce; });

			///bounce them off the ground in case the students figure out how to get 3 dimensions working!
			//balls.Where(b => b.Position.Z < b.Radius).ToList().ForEach(b => { b.Position.Z = b.Radius; b.Velocity.Z *= -table.WallBounce; });
		
		}*/

		
	}
}
