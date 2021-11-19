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
    public abstract class MapObject
    {
        public virtual double X { get; protected set; }
        public virtual double Y { get; protected set; }
        public virtual bool Alive { get; protected set; } = true;
        public List<Texture2D> Textures { get; protected set; }
        public int TexturePhase { get; protected set; }
        public virtual float Direction { get; protected set; }
        public virtual double Radius { get; protected set; }

        protected virtual void updateTexture(ContentManager contentManager, bool reload)
        {
            
        }

        public virtual void Update(ContentManager contentManager, GameWorld gameWorld)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(Textures[TexturePhase],
                new Vector2(x, y),
                new Rectangle(0, 0, Textures[TexturePhase].Width, Textures[TexturePhase].Height),
                Color.White,
                Direction, 
                new Vector2(Textures[TexturePhase].Width/2, Textures[TexturePhase].Height/2), 1f, SpriteEffects.None,
                0);
        }

        public virtual void Move(float direction, double speed)
        {
            X += Math.Cos(direction) * speed;
            Y += Math.Sin(direction) * speed;
        }

        public virtual void Kill()
        {
            Alive = false;
        }
    }
}