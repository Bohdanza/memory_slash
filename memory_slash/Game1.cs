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
using System.Threading;

namespace memory_slash
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameWorld mainWorld;
        private Texture2D noisePixel;
        private SpriteFont LargeMetal, MediumMetal, SmallMetal;
        private string currentPhrase = "";
        private List<string> phrases = new List<string> {"Nice try", "One more time?", "World of pain", 
                                                        "#$*%!", 
                                                        "Those annoying chasers...", 
                                                        "I promise I'll update this until tomorrow", 
                                                        "I've spent almost 20 hours testing...", 
                                                        "Horrible bullets...", "I CAN SEE SUN",
                                                        "Danger in the centre",
                                                        "Score orbs closer to the centre",
                                                        "SPACE", "THE END"};

        private int timeSinceLastDeath = 0, timeKeyIsPressed = 0, maxScore = 0, currentMode = 0;
        private bool previousSpaceState = false;
        private List<int> HighScores = new List<int>();
        public const int ModesCount = 3;
        private List<string> ModeNames = new List<string> {"Classic mode", "Time mode", "Laser Hell"};
        private List<string> ModeDescriptions = new List<string> { "Collect as many red orbs as you can\nThe more you have the higher is difficulty", "Survive as long as you can\nScore would increase automatically", "Everything is as usuall,\n but bullets replaced with lasers" };

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.ApplyChanges();

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;

            _graphics.ApplyChanges();

            this.IsFixedTimeStep = true;

            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / 60d);
            _graphics.ApplyChanges();

            this.Window.IsBorderless = false;

            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            IsMouseVisible = false;

            this.Window.Title = "SPACE";
        }

        protected void DrawWorld()
        {
            mainWorld.Draw(_spriteBatch);
        }
        
        protected override void Initialize()
        {
            if (File.Exists("score_info"))
            {
                using (StreamReader sr = new StreamReader("score_info"))
                {
                    List<string> tmplist = sr.ReadToEnd().Split('\n').ToList();

                    for (int i = 0; i < ModesCount; i++)
                    {
                        if (i < tmplist.Count)
                        {
                            HighScores.Add(Int32.Parse(tmplist[i]));
                        }
                        
                        HighScores.Add(0);
                    }
                }
            }
            else
            {
                for (int i = 0; i < ModesCount; i++)
                {
                    HighScores.Add(0);
                }
            }

            // TODO: Add your initialization logic here
            mainWorld = new GameWorld(Content, 0);
            mainWorld.KillHero();

            mainWorld.Score = 0;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            noisePixel = Content.Load<Texture2D>("noise_pixel");
            // TODO: use this.Content to load your game content here

            LargeMetal = Content.Load<SpriteFont>("metal_font_large");
            MediumMetal = Content.Load<SpriteFont>("metal_font_medium");
            SmallMetal = Content.Load<SpriteFont>("metal_font_small");
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            mainWorld.Update(Content);

            var rnd = new Random();

            var ks = Keyboard.GetState();

            if(ks.IsKeyDown(Keys.Escape))
            {
                using (StreamWriter sr = new StreamWriter("score_info"))
                {
                    foreach (var currentScore in HighScores)
                    {
                        sr.WriteLine(currentScore);
                    }
                }

                Exit();
            }

            if (!mainWorld.referenceToHero.Alive)
            {
                HighScores[mainWorld.PlayMode] = Math.Max(HighScores[mainWorld.PlayMode], mainWorld.Score);

                if (currentPhrase == "")
                {
                    File.WriteAllText("score_info", maxScore.ToString());

                    int i = rnd.Next(0, phrases.Count);

                    currentPhrase = phrases[i];
                }

                timeSinceLastDeath++;

                if (!ks.IsKeyDown(Keys.Space) && previousSpaceState)
                {
                    if (timeKeyIsPressed <= 40)
                    {
                        currentMode++;

                        if (currentMode >= ModesCount)
                        {
                            currentMode = 0;
                        }
                    }
                    else if (timeKeyIsPressed > 40)
                    {
                        mainWorld = new GameWorld(Content, currentMode);
                    }
                    
                    timeKeyIsPressed = 0;
                }
                else if(ks.IsKeyDown(Keys.Space))
                {
                    timeKeyIsPressed++;
                }
            }

            previousSpaceState = ks.IsKeyDown(Keys.Space);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var rnd = new Random();

            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            mainWorld.Draw(_spriteBatch);
            
            if(!mainWorld.referenceToHero.Alive)
            {
                _spriteBatch.Draw(noisePixel, new Rectangle(0, 0, 1920, 1080), new Color(0, 0, 0, 100));

                _spriteBatch.DrawString(LargeMetal, ModeNames[currentMode], new Vector2(960 - LargeMetal.MeasureString(ModeNames[currentMode]).X / 2, 200), Color.Lime);

                _spriteBatch.DrawString(SmallMetal, "Press space to select mode\nHold space to play\n\nPress esc to exit\n", new Vector2(1900 - SmallMetal.MeasureString("Press space to select mode").X, 450), Color.Lime);

                _spriteBatch.DrawString(SmallMetal, ModeDescriptions[currentMode], new Vector2(100, 450), Color.Lime);

                int tmpscore = mainWorld.Score;

                if (mainWorld.PlayMode != currentMode)
                {
                    tmpscore = 0;
                }

                string highScoreString = "Score: " + tmpscore;

                _spriteBatch.DrawString(MediumMetal, highScoreString, new Vector2(960 - MediumMetal.MeasureString(highScoreString).X / 2, 450), Color.Lime);
                
                highScoreString = "High score: " + HighScores[currentMode].ToString();

                _spriteBatch.DrawString(MediumMetal, highScoreString, new Vector2(960 - MediumMetal.MeasureString(highScoreString).X / 2, 550), Color.Lime);
            }

            for (int i = 0; i < 1080; i++)
            {
                int ns = rnd.Next(0, 100);
                
                if (ns < 2)
                {
                    _spriteBatch.Draw(noisePixel, new Rectangle(0, i, 1920, 1), Color.Black);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
