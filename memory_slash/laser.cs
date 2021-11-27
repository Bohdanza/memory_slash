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
    public class Laser:Mob
    {
        public override double X { get => base.X; protected set => base.X = value; }
        public override double Y { get => base.Y; protected set => base.Y = value; }
        public override float Direction { get => base.Direction; protected set => base.Direction = value; }
        public int Lifetime { get; protected set; }
        public int TimeSinceBorn { get; protected set; }

        public Laser(ContentManager contentManager, double x, double y, float Direction, int type, int lifetime)
        {
            X = x;
            Y = y;

            base.Type = type;

            this.Direction = Direction;

            base.Action = "id";

            Lifetime = lifetime;
            TimeSinceBorn = 0;

            base.updateTexture(contentManager, true);
        }

        public override void Update(ContentManager contentManager, GameWorld gameWorld)
        {
            TimeSinceBorn++;

            if(TimeSinceBorn>=Lifetime)
            {
                base.Kill();
            }

            float herodir = GameWorld.GetDirection(gameWorld.referenceToHero.X, gameWorld.referenceToHero.Y, X, Y);

            if (Math.Abs(Direction - herodir) <= 0.015)
            {
                gameWorld.referenceToHero.Kill();
            }

            base.Update(contentManager, gameWorld);
        }

        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(Textures[TexturePhase],
                new Vector2(x, y),
                new Rectangle(0, 0, Textures[TexturePhase].Width, Textures[TexturePhase].Height),
                Color.White,
                Direction,
                new Vector2(Textures[TexturePhase].Width / 2, Textures[TexturePhase].Height / 2), new Vector2(Math.Min(TimeSinceBorn*300, 6800f), 1f), SpriteEffects.None,
                0);
        }
    }
}