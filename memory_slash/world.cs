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
    public class GameWorld
    {
        List<MapObject> mapObjects = new List<MapObject>();
        MapObject referenceToHero;
        const int blockSize = 10;

        public GameWorld(ContentManager contentManager)
        {
            referenceToHero = AddObject(new Hero(contentManager, 0, 0));

            var rnd = new Random();

            int q = rnd.Next(35, 100);

            for (int i = 0; i < q; i++)
            {
                double tmpx = (rnd.NextDouble() - 0.5) * 500;
                double tmpy = (rnd.NextDouble() - 0.5) * 500;

                AddObject(new Asteroid(contentManager, tmpx, tmpy, 0.2, (float)((rnd.NextDouble() - 0.5) * 0.004), 1));
            }
        }

        public void Update(ContentManager contentManager)
        {
            int l = 1;

            for (int i = 0; i < mapObjects.Count; i += l)
            {
                l = 1;

                mapObjects[i].Update(contentManager, this);

                if(!mapObjects[i].Alive)
                {
                    mapObjects.RemoveAt(i);

                    l = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int x = -(int)(referenceToHero.X * blockSize);
            int y = -(int)(referenceToHero.Y * blockSize);

            mapObjects.Sort((a, b) => a.Y.CompareTo(b.Y));

            foreach(var currentObject in mapObjects)
            {
                currentObject.Draw(spriteBatch, (int)(x + currentObject.X * blockSize) + 960, (int)(y + currentObject.Y * blockSize) + 540);
            }
        }

        public MapObject AddObject(MapObject mapObject)
        {
            mapObjects.Add(mapObject);

            return mapObject;
        }
    }
}