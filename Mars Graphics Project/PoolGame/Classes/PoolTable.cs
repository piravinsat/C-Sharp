using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MarsGame.Classes
{
	/// <summary>
	/// this class describes the pool table
	/// </summary>
	public class PoolTable
	{

		//public float Width = 5000;
		//public float Height = 5000;
        public float Width = 12000;
        public float Height = 10500;
		public float WallBounce = 0.7f;
	
		private Model _model;
		private GraphicsDevice _device;

		public PoolTable(GraphicsDevice device)
		{
			_device = device;

		}

		public void LoadContent(ContentManager contentManager)
		{

            ///would you like to add model?
			/// _model = contentManager.Load<Model>("Models/pooltable");

		}

		public void Update(GameTime gametime)
		{



		}
		
		public struct Wall { 
			public Vector2 Vector;
			public Vector2 Normal;
		}
		
		
	
	}
}
