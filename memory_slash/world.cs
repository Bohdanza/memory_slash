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
        public MapObject referenceToHero { get; protected set; }
        const int blockSize = 10;
        private Texture2D backgroundGrid;
        private SpriteFont interfaceFont;

        public GameWorld(ContentManager contentManager)
        {
            referenceToHero = AddObject(new Hero(contentManager, 0, 0));

            var rnd = new Random();

            int q = rnd.Next(35, 100);

            for (int i = 0; i < q; i++)
            {
                double tmpx = (rnd.NextDouble() - 0.5) * 500;
                double tmpy = (rnd.NextDouble() - 0.5) * 500;

                AddObject(new Asteroid(contentManager, tmpx, tmpy, 0.2, (float)((rnd.NextDouble() - 0.5) * 0.004), 1, 7d));
            }

            backgroundGrid = contentManager.Load<Texture2D>("backgroung_grid");

            interfaceFont = contentManager.Load<SpriteFont>("interface_font");
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

            int qbx = -((-x) % backgroundGrid.Width);
            int qby = -((-y) % backgroundGrid.Height);

            for (int i = qbx - backgroundGrid.Width; i <= 1920; i += backgroundGrid.Width)
            {
                for (int j = qby - backgroundGrid.Height; j <= 1080; j += backgroundGrid.Height)
                {
                    spriteBatch.Draw(backgroundGrid, new Vector2(i, j), Color.White);
                }
            }

            mapObjects.Sort((a, b) => a.Y.CompareTo(b.Y));

            foreach(var currentObject in mapObjects)
            {
                currentObject.Draw(spriteBatch, (int)(x + currentObject.X * blockSize) + 960, (int)(y + currentObject.Y * blockSize) + 540);
            }

            ((Hero)referenceToHero).DrawInterface(spriteBatch, interfaceFont, new Color(10, 200, 6, 184));
        }

        public MapObject AddObject(MapObject mapObject)
        {
            mapObjects.Add(mapObject);

            return mapObject;
        }

        public double GetDist(double x1, double y1, double x2, double y2)
        {
            double nx = x1 - x2;
            double ny = y1 - y2;

            return Math.Sqrt(nx * nx + ny * ny);
        }
    }
}