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
                                                        "Horrible bullets...", "I CAN SEE SUN"};
        private int timeSinceLastDeath = 0;

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

            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            this.Window.Title = "Space!";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            mainWorld = new GameWorld(Content);

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            mainWorld.Update(Content);

            var rnd = new Random();

            if (!mainWorld.referenceToHero.Alive)
            {
                if (currentPhrase == "")
                {
                    int i = rnd.Next(0, phrases.Count);

                    currentPhrase = phrases[i];
                }

                var ks = Keyboard.GetState();

                timeSinceLastDeath++;

                if(ks.IsKeyDown(Keys.Space)&&timeSinceLastDeath>=30)
                {
                    mainWorld = new GameWorld(Content);

                    timeSinceLastDeath = 0;
                    
                    currentPhrase = "";
                }
            }

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

                _spriteBatch.DrawString(LargeMetal, currentPhrase, new Vector2(960 - LargeMetal.MeasureString(currentPhrase).X / 2, 230), Color.Lime);

                _spriteBatch.DrawString(MediumMetal, "Press SPACE to restart... And to do everything else!", new Vector2(960 - MediumMetal.MeasureString("Press SPACE to restart... And to do everything else!").X / 2, 350), Color.Lime);

                _spriteBatch.DrawString(SmallMetal, "Collect those blue ring\n And avoid everything else!", new Vector2(960 - MediumMetal.MeasureString("Press SPACE to restart... And to do everything else!").X / 2, 450), Color.Lime);
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
