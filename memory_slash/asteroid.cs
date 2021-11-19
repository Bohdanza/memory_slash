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
    public class Asteroid:Mob
    {
        public override double X { get => base.X; protected set => base.X = value; }
        public override double Y { get => base.Y; protected set => base.Y = value; }
        public override float Direction { get => base.Direction; protected set => base.Direction = value; }
        public float RotationSpeed { get; protected set; }

        public Asteroid(ContentManager contentManager, double x, double y, double speed, float rotationSpeed, int type)
        {
            base.Action = "id";
            base.Type = type;

            X = x;
            Y = y;

            RotationSpeed = rotationSpeed;
            Speed = speed;

            updateTexture(contentManager, true);
        }

        public override void Update(ContentManager contentManager, GameWorld gameWorld)
        {
            Direction += RotationSpeed;

         //   Direction %= (float)(Math.PI * 2);

            base.Move(Direction, Speed);

            base.Update(contentManager, gameWorld);
        }
    }
}