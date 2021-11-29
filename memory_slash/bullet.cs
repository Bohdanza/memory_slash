using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Audio;

namespace memory_slash
{
    public class Bullet : Mob
    {
        public override double X { get => base.X; protected set => base.X = value; }
        public override double Y { get => base.Y; protected set => base.Y = value; }
        public override float Direction { get => base.Direction; protected set => base.Direction = value; }
        public float RotationSpeed { get; protected set; }
        private int timeSinceParticleSummon = 0;
        private SoundEffect FlyEffect;
        private SoundEffectInstance effectInstance;

        public Bullet(ContentManager contentManager, double x, double y, double speed, float rotationSpeed, int type, double radius, int mass)
        {
            Mass = mass;

            base.Action = "id";
            base.Type = type;
            base.Radius = radius;

            X = x;
            Y = y;

            RotationSpeed = rotationSpeed;
            Speed = speed;

            //FlyEffect = contentManager.Load<SoundEffect>("bullet_flying");
          /*  effectInstance = FlyEffect.CreateInstance();

            effectInstance.IsLooped = true;

            effectInstance.Play();
          */
            updateTexture(contentManager, true);
        }

        public override void Update(ContentManager contentManager, GameWorld gameWorld)
        {
            //double dsti = GameWorld.GetDist(X, Y, gameWorld.referenceToHero.X, gameWorld.referenceToHero.Y);

           /* if (dsti <= 150)
            {
                effectInstance.Volume = (float)(0.1 / Math.Max(dsti, 0.01));
            }*/

            Direction += RotationSpeed;

            //   Direction %= (float)(Math.PI * 2);

            base.Move(Direction, Speed);

            timeSinceParticleSummon++;

            double qr = 2 * Math.PI / RotationSpeed;

            if (timeSinceParticleSummon >= 1 && (Type == 4 || Type == 5 || Type == 6))
            {
                timeSinceParticleSummon = 0;

                MapObject part = new Particle(contentManager, X, Y, (int)qr, 0);

                gameWorld.AddObject(part);
            }

            if (GameWorld.GetDist(X, Y, gameWorld.referenceToHero.X + gameWorld.referenceToHero.Radius, gameWorld.referenceToHero.Y + gameWorld.referenceToHero.Radius) <= Radius + gameWorld.referenceToHero.Radius)
            {
                gameWorld.referenceToHero.Kill();
            }

            if (GameWorld.GetDist(0, 0, X, Y) >= 1760)
            {
                base.Kill();

               /* var rnd = new Random();

                int count = rnd.Next(30, 60);

                for (int i = 0; i < count; i++)
                {
                    var refer = gameWorld.AddObject(new Particle(contentManager, X, Y, rnd.Next(50, 120), 0));

                    ((Particle)refer).DrawMovement = new Vector2((float)(rnd.NextDouble() - 0.5) * 3, (float)(rnd.NextDouble() - 0.5) * 3);
                }*/
            }

            if(Type==11)
            {
                var rnd = new Random();

                for (int i = 0; i < 5; i++)
                {
                    double nx = X + (rnd.NextDouble() - 0.5) * 4.5;
                    double ny = Y + (rnd.NextDouble() - 0.5) * 4.5;

                    MapObject part = new Particle(contentManager, nx, ny, rnd.Next(5, 15), 0);

                    gameWorld.AddObject(part);
                }
            }

            base.Update(contentManager, gameWorld);
        }

        public override void Kill()
        {
            //effectInstance.IsLooped = false;

            //effectInstance.Stop();

            base.Kill();
        }
    }
}