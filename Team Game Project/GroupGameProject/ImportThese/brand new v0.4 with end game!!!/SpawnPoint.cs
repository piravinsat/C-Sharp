using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonDefense
{
    public class SpawnPoint : IGameObject
    {
        private static Random random;
        private Vector2 center;
        private double spawnChancePerSecond;
        private DateTime lastSpawnTime;

        public SpawnPoint(int x, int y, double spawnChancePerSecond)
        {
            random = new Random(Environment.TickCount);
            center = new Vector2(x, y);
            this.spawnChancePerSecond = spawnChancePerSecond;
        }

        public void Update(Game1 game)
        {
            if (DateTime.Now - lastSpawnTime >= new TimeSpan(0, 0, random.Next(1, 4)))
            {
                lastSpawnTime = DateTime.Now;
                if (random.NextDouble() <= spawnChancePerSecond)
                {
                    // spawn mob
                    game.mobs.Add(new Mob((int)game.time/1000, center, new Vector2(1, 1)));
                }
            }
        }


        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
          
        }
    }
}
 