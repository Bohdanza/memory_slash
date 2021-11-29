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
        const int blockSize = 2;
        private Texture2D backgroundGrid;
        private SpriteFont interfaceFont;
        public int Score = 0, PlayMode=0;
        public int Time { get; private set; } = 1;

        public GameWorld(ContentManager contentManager, int playMode)
        {
            PlayMode = playMode;

            var rnd = new Random();

            PlaceSystem(contentManager, 0, 0, rnd.Next(3, 7));

            //PI*2*R == PI*2*speed/rotationSpeed =>
            // =>  R = speed/rotationSpeed
            referenceToHero = AddObject(new Hero(contentManager, 0, -1600));

            PlaceDysonSphere(contentManager, 1700, 0.1, 10000, 800, 0.025);

            backgroundGrid = contentManager.Load<Texture2D>("backgroung_grid");
            
            interfaceFont = contentManager.Load<SpriteFont>("interface_font");
        }

        public void Update(ContentManager contentManager)
        {
            Time++;

            Time %= Int32.MaxValue;

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

            if (referenceToHero.Alive)
            {
                Random rnd = new Random();

                //Here we summon:

                //simple bullets
                if (rnd.Next(0, 100) <= Score)
                {
                    double j = rnd.NextDouble() * Math.PI * 2;

                    double rot = j;

                    double y = -Math.Cos((float)rot) * 1699;
                    double x = Math.Sin((float)rot) * 1699;

                    var reference = AddObject(new Bullet(contentManager, x, y, 3.5, 0, 10, 4.5, 1));

                    ((Mob)reference).ChangeRotation((float)(rot + Math.PI * 0.5));
                }

                //comet bullets
                if (Score >= 3 && rnd.Next(0, 100) <= Score / 3)
                {
                    double j = rnd.NextDouble() * Math.PI * 2;

                    double rot = j;

                    double y = -Math.Cos((float)rot) * 1699;
                    double x = Math.Sin((float)rot) * 1699;

                    var reference = AddObject(new Bullet(contentManager, x, y, 3d + rnd.NextDouble(), (float)(rnd.NextDouble() - 0.5) * 0.00025f, 11, 7, 20));

                    ((Mob)reference).ChangeRotation((float)(rot + Math.PI * 0.5));
                }

                //chasers
                if (Score >= 6 && rnd.Next(0, 100) <= Score / 6)
                {
                    int rad = rnd.Next(0, 1650);

                    double j = rnd.NextDouble() * Math.PI * 2;

                    double rot = j;

                    double y = -Math.Cos((float)rot) * rad;
                    double x = Math.Sin((float)rot) * rad;

                    var reference = AddObject(new Enemy(contentManager, x, y, 9, 2, 9d, 0, 400, 0, 500));

                    ((Mob)reference).ChangeRotation((float)(rot + Math.PI * 0.5));
                }

                //bullet chasers
                if (Score >= 7 && rnd.Next(0, 100) <= Score / 7)
                {
                    int rad = rnd.Next(0, 1650);

                    double j = rnd.NextDouble() * Math.PI * 2;

                    double rot = j;

                    double y = -Math.Cos((float)rot) * rad;
                    double x = Math.Sin((float)rot) * rad;

                    var reference = AddObject(new Enemy(contentManager, x, y, 12, 2.3, 9d, 2, 500, 150, 500));

                    ((Mob)reference).ChangeRotation((float)(rot + Math.PI * 0.5));
                }

                //double bullet chasers
                if (Score >= 9 && rnd.Next(0, 100) <= Score / 9)
                {
                    int rad = rnd.Next(0, 1650);

                    double j = rnd.NextDouble() * Math.PI * 2;

                    double rot = j;

                    double y = -Math.Cos((float)rot) * rad;
                    double x = Math.Sin((float)rot) * rad;

                    var reference = AddObject(new Enemy(contentManager, x, y, 14, 2.6, 9d, 4, 500, 200, 550));

                    ((Mob)reference).ChangeRotation((float)(rot + Math.PI * 0.5));
                }

                //rotating chasers
                if (Score >= 10 && rnd.Next(0, 100) <= Score / 10)
                {
                    int rad = rnd.Next(0, 1650);

                    double j = rnd.NextDouble() * Math.PI * 2;

                    double rot = j;

                    double y = -Math.Cos((float)rot) * rad;
                    double x = Math.Sin((float)rot) * rad;

                    var reference = AddObject(new Enemy(contentManager, x, y, 15, 2.1, 9d, 0, 400, 0, 450));

                    ((Mob)reference).ChangeRotation((float)(rot + Math.PI * 0.5));
                }

                //Lasers
                if (Score >= 5 && rnd.Next(0, 75) <= Score / 5)
                {
                    double j = rnd.NextDouble() * Math.PI * 2;

                    double rot = j;

                    double y = -Math.Cos((float)rot) * 1699;
                    double x = Math.Sin((float)rot) * 1699;

                    var reference = AddObject(new Laser(contentManager, x, y, (float)(rot + Math.PI * 0.5), 13, 40));
                }

                //score
                if (PlayMode == 0)
                {
                    if (rnd.Next(0, 10000) <= 25 + 100 - Score)
                    {
                        int rad = rnd.Next(120, 1650);

                        double j = rnd.NextDouble() * Math.PI * 2;

                        double rot = j;

                        double y = -Math.Cos((float)rot) * rad;
                        double x = Math.Sin((float)rot) * rad;

                        var reference = AddObject(new ScoreParticle(contentManager, x, y, 0, 0, 1, 4.5, 2));

                        //((Mob)reference).ChangeRotation((float)(rot + Math.PI * 0.5));
                    }
                }
                else if (PlayMode == 1)
                {
                    if (Time % 500 == 0)
                    {
                        Score++;
                    }
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

           // mapObjects.Sort((a, b) => a.Y.CompareTo(b.Y));
            
            for (int i = 0; i < mapObjects.Count; i++)
            {
                var currentObject = mapObjects[i];

                if (GetDist(referenceToHero.X, referenceToHero.Y, currentObject.X, currentObject.Y) <= 1920 / blockSize || mapObjects[i] is Laser)
                {
                    currentObject.Draw(spriteBatch, (int)(x + currentObject.X * blockSize) + 960, (int)(y + currentObject.Y * blockSize) + 540);
                }
            }

            ((Hero)referenceToHero).DrawInterface(spriteBatch, interfaceFont, new Color(10, 200, 6, 184));

            string drawingScoreString = "Score: " + Score.ToString();

            spriteBatch.DrawString(interfaceFont, drawingScoreString, new Vector2(1920 - interfaceFont.MeasureString(drawingScoreString).X - 10, 10), Color.Lime);
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

        protected void PlaceSystem(ContentManager contentManager, int x, int y, int planetsCount)
        {
            AddObject(new Asteroid(contentManager, x, y, 0, 0f, 2, 124, 3000));

            Random rnd = new Random();

            int currentRadius = rnd.Next(200, 250);

            for (int i = 0; i < planetsCount; i++)
            {
                double speed = rnd.NextDouble() * 5;
                double rotationSpeed = speed / currentRadius;

                //PI*2*R == PI*2*speed/rotationSpeed =>
                // =>  R = speed/rotationSpeed

                //48 32 16

                int size = rnd.Next(4, 7);
                int mass = 0, radius = 0;

                if(size==4)
                {
                    radius = 48;
                    mass = 900;
                }
                else if (size == 5)
                {
                    radius = 32;
                    mass = 600;
                }
                else if (size == 6)
                {
                    radius = 16;
                    mass = 300;
                }

                AddObject(new Asteroid(contentManager, x, y - currentRadius, speed, (float)rotationSpeed, size, radius, mass));

                currentRadius += rnd.Next(150, 250);
            }

            AddObject(new Asteroid(contentManager, 0, -2300, 0, 0, 3, -1, -3));
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