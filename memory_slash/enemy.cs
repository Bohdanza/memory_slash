using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace memory_slash
{
    public class Enemy:Mob
    {
        public override double X { get => base.X; protected set => base.X = value; }
        public override double Y { get => base.Y; protected set => base.Y = value; }
        public virtual double ViewRadius { get; protected set; }
        public virtual double AttackRadius { get; protected set; }
        public override float Direction { get => base.Direction; protected set => base.Direction = value; }
        public int TimeSinceCreation { get; protected set; }
        public int Lifetime { get; protected set; }

        public Enemy(ContentManager contentManager, double x, double y, int type, double speed, double radius, int mass, double viewRadius, double attackRadius, int lifetime)
        {
            X = x;
            Y = y;

            ViewRadius = viewRadius;
            AttackRadius = attackRadius;

            base.Type = type;

            Speed = speed;

            Mass = mass;

            Action = "id";

            Lifetime = lifetime;

            TimeSinceCreation = 0;

            base.updateTexture(contentManager, true);
        }

        public override void Update(ContentManager contentManager, GameWorld gameWorld)
        {
            TimeSinceCreation++;

            if(TimeSinceCreation>=Lifetime&&Alive)
            {
                var rnd = new Random();

                int count = rnd.Next(100, 200);

                for (int i = 0; i < count; i++)
                {
                    var refer = gameWorld.AddObject(new Particle(contentManager, X, Y, rnd.Next(50, 120), 0));

                    ((Particle)refer).DrawMovement = new Vector2((float)(rnd.NextDouble() - 0.5) * 6, (float)(rnd.NextDouble() - 0.5) * 6);
                }

                Kill();
            }

            double distToHero = GameWorld.GetDist(X, Y, gameWorld.referenceToHero.X, gameWorld.referenceToHero.Y);

            if (distToHero <= ViewRadius)
            {
                if (distToHero <= AttackRadius)
                {
                    gameWorld.referenceToHero.Kill();
                }
                else
                {
                    Action = "fly";

                    Direction = GameWorld.GetDirection(gameWorld.referenceToHero.X, gameWorld.referenceToHero.Y, X, Y);

                    base.Move(Direction, Speed);
                }
            }

            base.Update(contentManager, gameWorld);
        }

        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            base.Draw(spriteBatch, x, y);
        }
    }
}