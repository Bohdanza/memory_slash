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
    public abstract class Mob:MapObject
    {
        public override double X { get => base.X; protected set => base.X = value; }
        public override double Y { get => base.Y; protected set => base.Y = value; }
        public override float Direction { get; protected set; }
        public virtual double Speed { get; protected set; }
        public virtual string Action { get; protected set; }
        public int Type { get; protected set; }
        private int timeSinceLastTextureUpdate = 0;
        protected string pact="";
        public virtual double Radius { get; protected set; }

        protected override void updateTexture(ContentManager contentManager, bool reload)
        {
            if (reload)
            {
                base.TexturePhase = 0;

                Textures = new List<Texture2D>();

                while (File.Exists("Content/" + Type.ToString() + "mob_" + Action + base.TexturePhase.ToString()+".xnb"))
                {
                    Textures.Add(contentManager.Load<Texture2D>(Type.ToString() + "mob_" + Action + base.TexturePhase.ToString()));

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
            timeSinceLastTextureUpdate++;

            base.Update(contentManager, gameWorld);

            if (timeSinceLastTextureUpdate >= 16)
            {
                timeSinceLastTextureUpdate = 0;

                this.updateTexture(contentManager, false);
            }
            else if(pact != Action)
            {
                timeSinceLastTextureUpdate = 0;

                this.updateTexture(contentManager, true);
            }

            pact = Action;
        }

        public override void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            base.Draw(spriteBatch, x, y);
        }
    }
}