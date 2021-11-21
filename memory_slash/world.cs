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
        public MapObject referenceToSun { get; protected set; }
        const int blockSize = 1;
        private Texture2D backgroundGrid, noisePixel;
        private SpriteFont interfaceFont;

        public GameWorld(ContentManager contentManager)
        {
            var rnd = new Random();

            referenceToSun = AddObject(new Asteroid(contentManager, 0, 0, 0, (float)(Math.PI*2), 2, 40.0, 500));

            //AddObject(new SpaceStation(contentManager, 0, -135, 0.1, 0.000833333f, 3, 5.3, 10));

            PlaceDysonSphere(contentManager, 150, 0.08, 5, 84, 0.075);

            //PI*2*R == PI*2*speed/rotationSpeed =>
            // =>  R = speed/rotationSpeed
            referenceToHero = AddObject(new Hero(contentManager, 0, -1550));

            AddObject(new SpaceStation(contentManager, 0, -1600, 0.1, 0.002f, 10, 12.5, 40));

            PlaceDysonSphere(contentManager, 1400, 0.04, 2, 1250, 0.005);

            backgroundGrid = contentManager.Load<Texture2D>("backgroung_grid");
            
            noisePixel = contentManager.Load<Texture2D>("noise_pixel");

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

            var rnd = new Random();

            for(int i=0; i<1080; i++)
            {
                int ns = rnd.Next(0, 100);

                if(ns<2)
                {
                    spriteBatch.Draw(noisePixel, new Rectangle(0, i, 1920, 1), Color.Black);
                }
            }
        }

        public MapObject AddObject(MapObject mapObject)
        {
            mapObjects.Add(mapObject);

            return mapObject;
        }

        protected void PlaceDysonSphere(ContentManager contentManager, int radius, double speed, int divide, int count, double multiplier)
        {
            for (int i = 0; i < count; i++)
            {
                if (i % divide != 0)
                {
                    //MAGIC
                    //9==4.5*2

                    //PI*2*R == PI*2*speed/rotationSpeed =>
                    // =>  R = speed/rotationSpeed
                    double rot = i * multiplier;

                    double y = -Math.Cos((float)rot) * radius;
                    double x = Math.Sin((float)rot) * radius;
                    
                    var reference = AddObject(new Asteroid(contentManager, x, y, speed, (float)(speed / radius), 8, 4.5, 7));

                    ((Mob)reference).ChangeRotation((float)rot);
                }
            }
        }

        public static double GetDist(double x1, double y1, double x2, double y2)
        {
            double nx = x1 - x2;
            double ny = y1 - y2;

            return Math.Sqrt(nx * nx + ny * ny);
        }

        public static float GetDirection(double x1, double y1, double x2, double y2)
        {
            return (float)Math.Atan2(y1 - y2, x1 - x2);
        }
    }
}