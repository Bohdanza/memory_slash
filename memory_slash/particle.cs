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

            base.Update(contentManager, gameWorld);
        }
    }
}