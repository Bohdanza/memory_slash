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
        private double px;
        private double py;

        public Hero(ContentManager contentManager, double x, double y)
        {
            base.Action = "id";

            Speed = 0.7;

            base.Radius = 1.7;
            base.Type = 0;

            X = x;
            Y = y;

            px = X;
            py = Y;

            base.updateTexture(contentManager, true);
        }

        public override void Update(ContentManager contentManager, GameWorld gameWorld)
        {
            px = X;
            py = Y;

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

            if(!Alive)
            {
                var rnd = new Random();

                int count = rnd.Next(100, 200);

                for (int i = 0; i < count; i++)
                {
                    var refer = gameWorld.AddObject(new Particle(contentManager, X, Y, rnd.Next(50, 120), 0));

                    ((Particle)refer).DrawMovement = new Vector2((float)(rnd.NextDouble()-0.5)*6, (float)(rnd.NextDouble() - 0.5) * 6);
                }
            }

            base.Update(contentManager, gameWorld);
        }

        public void DrawInterface(SpriteBatch spriteBatch, SpriteFont font, Color color)
        {
            double tmpx = Math.Truncate(X * 10000) / 10000;
            double tmpy = Math.Truncate(Y * 10000) / 10000;

            double realspeed = GameWorld.GetDist(X, Y, px, py);

            realspeed = Math.Truncate(realspeed * 10000) / 10000;

            spriteBatch.DrawString(font, "X: " + tmpx.ToString() + "  Y: " + tmpy.ToString() + "\nSpeed: " + realspeed.ToString() + "\nRotation speed: " + rotationSpeed.ToString(), new Vector2(0, 0), color);
        }

        public override void Kill()
        {
            base.Kill();
        }
    }
}