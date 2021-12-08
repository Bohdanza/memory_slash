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
        private int timeSinceLastMovement = 1000, movementDuration = 20;
        private double px;
        private double py;
        public int ShieldPower = 0;
        private Texture2D shieldTexture;

        public Hero(ContentManager contentManager, double x, double y)
        {
            base.Action = "id";

            Speed = 2.7;

            base.Radius = 1.7;
            base.Type = 0;

            X = x;
            Y = y;

            px = X;
            py = Y;

            shieldTexture = contentManager.Load<Texture2D>("shield");

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

                for (int i = 0; i < 10; i++)
                {
                    double nx = X + (rnd.NextDouble() - 0.5)*3;
                    double ny = Y + (rnd.NextDouble() - 0.5)*3;

                    MapObject part = new Particle(contentManager, nx, ny, rnd.Next(5, 15), 0);

                    gameWorld.AddObject(part);
                }
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

        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            base.Draw(spriteBatch, x, y);

            if(ShieldPower>0)
            {
                spriteBatch.Draw(shieldTexture, new Vector2(x-shieldTexture.Width/2, y-shieldTexture.Height/2), new Color(255,255,255,ShieldPower*2));
            }
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
            if (ShieldPower>0)
            {
                ShieldPower--;
            }
            else
            {
                base.Kill();
            }
        }
    }
}