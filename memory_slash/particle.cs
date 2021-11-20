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
    public class Particle:MapObject
    {
        public override double X { get => base.X; protected set => base.X = value; }
        public override double Y { get => base.Y; protected set => base.Y = value; }
        public override bool Alive { get => base.Alive; protected set => base.Alive = value; }
        public int Lifetime { get; protected set; }
        public int TimeSinceCreation { get; protected set; }
        public int Type { get; protected set; }
        public Vector2 DrawMovement = new Vector2(0, 0);
        public Vector2 DrawPlus { get; protected set; } = new Vector2(0, 0);

        public Particle(ContentManager contentManager, double x, double y, int lifetime, int type)
        {
            TimeSinceCreation = 0;

            Lifetime = lifetime;

            X = x;
            Y = y;

            Type = type;

            this.updateTexture(contentManager, true);
        }

        protected override void updateTexture(ContentManager contentManager, bool reload)
        {
            if (reload)
            {
                base.TexturePhase = 0;

                Textures = new List<Texture2D>();

                while (File.Exists("Content/" + Type.ToString() + "particle" +  base.TexturePhase.ToString() + ".xnb"))
                {
                    Textures.Add(contentManager.Load<Texture2D>(Type.ToString() + "particle" + base.TexturePhase.ToString()));

                    TexturePhase++;
                }

                base.TexturePhase = 0;
            }
            else
            {
                TexturePhase++;

                TexturePhase %= Textures.Count;
            }
        }

        public override void Update(ContentManager contentManager, GameWorld gameWorld)
        {
            TimeSinceCreation++;

            if(TimeSinceCreation>=Lifetime)
            {
                Alive = false;
            }

            DrawPlus = new Vector2(DrawPlus.X + DrawMovement.X, DrawPlus.Y + DrawMovement.Y);

            base.Update(contentManager, gameWorld);
        }

        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(Textures[TexturePhase],
               new Vector2(x + DrawPlus.X, y + DrawPlus.Y),
               new Rectangle(0, 0, Textures[TexturePhase].Width, Textures[TexturePhase].Height),
               Color.White,
               Direction,
               new Vector2(Textures[TexturePhase].Width / 2, Textures[TexturePhase].Height / 2), 1f, SpriteEffects.None,
               0);
        }
    }
}