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
    public class ScoreParticle : Mob
    {
        public override double X { get => base.X; protected set => base.X = value; }
        public override double Y { get => base.Y; protected set => base.Y = value; }
        public override float Direction { get => base.Direction; protected set => base.Direction = value; }
        public float RotationSpeed { get; protected set; }
        private int timeSinceParticleSummon = 0;

        public ScoreParticle(ContentManager contentManager, double x, double y, double speed, float rotationSpeed, int type, double radius, int mass)
        {
            Mass = mass;

            base.Action = "id";
            base.Type = type;
            base.Radius = radius;

            X = x;
            Y = y;

            RotationSpeed = rotationSpeed;
            Speed = speed;

            updateTexture(contentManager, true);
        }

        public override void Update(ContentManager contentManager, GameWorld gameWorld)
        {
            if (GameWorld.GetDist(X, Y, gameWorld.referenceToHero.X + gameWorld.referenceToHero.Radius, gameWorld.referenceToHero.Y + gameWorld.referenceToHero.Radius) <= Radius + gameWorld.referenceToHero.Radius)
            {
                gameWorld.Score++;

                base.Kill();
            }
            
            base.Update(contentManager, gameWorld);
        }
    }
}