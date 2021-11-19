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
    public class Hero:Mob
    {
        private float rotationSpeed = 0.07f;
        public override double X { get => base.X; protected set => base.X = value; }
        public override double Y { get => base.Y; protected set => base.Y = value; }
        public override float Direction { get => base.Direction; protected set => base.Direction = value; }
        private int timeSinceLastMovement = 1000, movementDuration = 75;
        
        public Hero(ContentManager contentManager, double x, double y)
        {
            base.Action = "id";

            Speed = 0.7;

            base.Type = 0;

            base.updateTexture(contentManager, true);
        }

        public override void Update(ContentManager contentManager, GameWorld gameWorld)
        {
            timeSinceLastMovement++;

            if (Action == "id")
            {
                Direction += rotationSpeed;

                Direction %= (float)(Math.PI * 2);

                KeyboardState ks = Keyboard.GetState();

                if(ks.IsKeyDown(Keys.Space))
                {
                    Action = "fly";

                    timeSinceLastMovement = 0;
                }
            }
            else if(Action=="fly")
            {
                if(timeSinceLastMovement>=movementDuration)
                {
                    Action = "id";
                }

                base.Move(Direction, Speed);

                var rnd = new Random();

                double nx = X + rnd.NextDouble() - 0.5;
                double ny = Y + rnd.NextDouble() - 0.5;

                MapObject part = new Particle(contentManager, nx, ny, rnd.Next(25, 35), 0);

                gameWorld.AddObject(part);
            }

            base.Update(contentManager, gameWorld);
        }
    }
}