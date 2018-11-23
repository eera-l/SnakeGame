using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SnakeGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Snake snake;  
        private Rectangle rect;
        private Texture2D texture;
        private int screenWidth;
        private int screenHeight;
        private Vector2 direction;
        private KeyboardState ks;
        private KeyboardState oldKey;
        private bool foodAvailable = false;
        private Random rnd = new Random();
        private Rectangle rect1;
        private Texture2D texture1;
        private byte nsew;
        private float timer = 2f;
        private List<Vector2> listOPositions = new List<Vector2>();
        private SpriteFont spriteFont;
        private SpriteFont recordSprite;
        private Vector2 fontPos;
        private Vector2 recordPos;
        private int score = 0;
        private int record = 0;
        private bool dead = false;
        private Vector2 dir;
        private Color[] backgroundColors = new Color[] {Color.Aqua, Color.Aquamarine, Color.Black, Color.BlueViolet,
        Color.CornflowerBlue, Color.DarkOrchid, Color.DeepSkyBlue, Color.DodgerBlue, Color.Green, Color.ForestGreen,
        Color.MidnightBlue, Color.Navy, Color.SlateBlue, Color.MediumSlateBlue, Color.Orchid, Color.DarkGreen,
        Color.Plum, Color.SpringGreen, Color.Gold, Color.Black, Color.Black, Color.Black, Color.Navy, Color.Navy, 
        Color.MidnightBlue, Color.MidnightBlue, Color.MidnightBlue, Color.ForestGreen, Color.ForestGreen, Color.ForestGreen,
        Color.Gold, Color.Gold, Color.Gold};
        private Color backgroundColor;
        private bool once = true;
        private bool paused = false; 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            score = 0;
            try
            {
                ReadRecord();
            }
            catch
            {
                record = 0;
            }
            dead = false;
            graphics.ApplyChanges();
            direction.X = 30f;
            direction.Y = 300f;
            listOPositions.Add(new Vector2(30f, 300f));
            texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new Color[] {Color.White});
            snake = new Snake(new Rectangle((int)(direction.X + 0.5), (int)(direction.Y + 0.5), 15, 15));
            screenWidth = Window.ClientBounds.Right - Window.ClientBounds.Left;
            screenHeight = Window.ClientBounds.Bottom - Window.ClientBounds.Top;
            // automate snake
            //dir = new Vector2((rect1.X - direction.X), (rect1.Y - direction.Y));
            //dir.Normalize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            spriteFont = Content.Load<SpriteFont>("HUD");
            recordSprite = Content.Load<SpriteFont>("HUD");

            fontPos = new Vector2(12f, 12f);
            recordPos = new Vector2(470f, 12f);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
	        ks = Keyboard.GetState();

            if (score % 70 == 0)
            {
                ChangeBackgroundColor();
            }

            if (score % 70 != 0)
            {
                once = true;
            }

            if (!dead)
            {
                SaveDirectionKeys();

                MoveSnake();
            }

            MoveSnakePastBoundaries();


           if (!foodAvailable)
           {
                CreateFood();
           }

            // automate the snake to eat food 
           /*dir = new Vector2((rect1.X - direction.X), (rect1.Y - direction.Y));
           dir.Normalize();
           direction.X += dir.X * 10;
           direction.Y += dir.Y * 10;
           MoveAllBody();*/

            if (snake.Body[0].Intersects(rect1))
            {

                EatFood();
            }
            if (dead && timer < 0.5)
            {
                Initialize();
                timer = 2f;
            }

            for (int i = 2; i < snake.Body.Count; i++) {
                
                if (snake.Body[0].Intersects(snake.Body[i])) //if it intersects with its tail
                {
                    dead = true;
                    if (record == score)
                    {
                        WriteRecord();
                    }
                }
            }

           for (int i = 0; i < snake.Body.Count; i++)
                {
                    if (snake.Body[i].Width > 0)
                    {
                        if (nsew == 0)
                        {
                            snake.Body[i] = new Rectangle((int)listOPositions[i].X, (int)listOPositions[i].Y, 15, 15);
                        }
                        if (nsew == 1)
                        {
                            snake.Body[i] = new Rectangle((int)listOPositions[i].X, (int)listOPositions[i].Y, 15, 15);
                        }
                        if (nsew == 2)
                        {
                            snake.Body[i] = new Rectangle((int)listOPositions[i].X, (int)listOPositions[i].Y, 15, 15);
                        }
                        if (nsew == 3)
                        {
                            snake.Body[i] = new Rectangle((int)listOPositions[i].X, (int)listOPositions[i].Y, 15, 15);
                        }
                    }
                }
                base.Update(gameTime);
        }

        private void CreateFood()
        {
            int rows = 0;
            int cols = 0;
            rows = rnd.Next(1, screenWidth - 15);
            cols = rnd.Next(1, screenHeight - 15);

            while (rows % 15 != 0)
            {
                rows = rnd.Next(1, screenWidth - 15);
                for (int i = 0; i < listOPositions.Count; i++)
                {
                    if (rows == listOPositions[i].X && cols == listOPositions[i].Y) // do not create food where the snake is
                    {
                        rows = 38;
                    }
                }
            }

            while (cols % 15 != 0)
            {
                cols = rnd.Next(1, screenHeight - 15);
                for (int i = 0; i < listOPositions.Count; i++)
                {
                    if (rows == listOPositions[i].X && cols == listOPositions[i].Y) // do not create food where the snake is
                    {
                        cols = 38;
                    }
                }
            }
            
            rect1 = new Rectangle(rows, cols, 15, 15);
            texture1 = new Texture2D(GraphicsDevice, 1, 1);
            texture1.SetData(new Color[] {Color.Fuchsia});
            foodAvailable = true;
        }

        private void SaveDirectionKeys()
        {
            if (!paused)
            {
                if (ks.IsKeyDown(Keys.Up))
                {
                    oldKey = ks;
                }
                else if (ks.IsKeyDown(Keys.Down))
                {
                    oldKey = ks;
                }
                else if (ks.IsKeyDown(Keys.Right))
                {
                    oldKey = ks;
                }
                else if (ks.IsKeyDown(Keys.Left))
                {
                    oldKey = ks;
                }
            }
            if (ks.IsKeyDown(Keys.Space))
            {
                oldKey = ks;
            }
        }

        private void MoveSnake()
        {
            if (!paused)
            {
                if ((ks.IsKeyUp(Keys.Up) && oldKey.IsKeyDown(Keys.Up)) || ks.IsKeyDown(Keys.Up))
                {
                    if (timer < 1.93)
                    {
                        direction.Y -= 15f;
                        nsew = 0;
                        timer = 2f;
                        MoveAllBody();
                    }
                }
                else if ((ks.IsKeyUp(Keys.Down) && oldKey.IsKeyDown(Keys.Down)) || ks.IsKeyDown(Keys.Down))
                {
                    if (timer < 1.93)
                    {
                        direction.Y += 15f;
                        nsew = 1;
                        timer = 2f;
                        MoveAllBody();
                    }
                }
                else if ((ks.IsKeyUp(Keys.Right) && oldKey.IsKeyDown(Keys.Right)) || ks.IsKeyDown(Keys.Right))
                {
                    if (timer < 1.93)
                    {
                        direction.X += 15f;
                        nsew = 2;
                        timer = 2f;
                        MoveAllBody();
                    }
                }
                else if ((ks.IsKeyUp(Keys.Left) && oldKey.IsKeyDown(Keys.Left)) || ks.IsKeyDown(Keys.Left))
                {
                    if (timer < 1.93)
                    {
                        direction.X -= 15f;
                        nsew = 3;
                        timer = 2f;
                        MoveAllBody();
                    }
                }
            }
            if (ks.IsKeyDown(Keys.Space))
            {
                if (!paused)
                {
                    paused = true;
                }
                else
                {
                    paused = false;
                }

            }
        }

        private void MoveSnakePastBoundaries()
        {
            if (direction.X > screenWidth)
            {
                direction.X = 0f;
            }
            if (direction.X < 0)
            {
                direction.X = screenWidth;
            }
            if (direction.Y > screenHeight)
            {
                direction.Y = 0f;
            }
            if (direction.Y < 0)
            {
                direction.Y = screenHeight;
            }
        }
        private void MoveAllBody()
        {
            for (int i = listOPositions.Count - 1; i > 0; i--)
            {
                if (listOPositions[i] != null)
                {
                    listOPositions[i] = new Vector2(listOPositions[i - 1].X, listOPositions[i - 1].Y); // switch positions for the the body's rectangles
                }
            }
            listOPositions[0] = new Vector2(direction.X, direction.Y);
        }

        private void EatFood()
        {
            rect1 = new Rectangle(0, 0, 0, 0);
            texture1 = null;
            foodAvailable = false;
            snake.Body.Add(new Rectangle(snake.Body[snake.Body.Count - 1].X, snake.Body[snake.Body.Count - 1].Y, 15, 15));
            listOPositions.Add(new Vector2(snake.Body[snake.Body.Count - 1].X, snake.Body[snake.Body.Count - 1].Y));
            score += 10;
            if (score > record)
            {
                record = score;
            }
        }

        private void ChangeBackgroundColor()
        {
            if (once)
            {
                int colorInt = rnd.Next(0, backgroundColors.Length - 1);
                backgroundColor = backgroundColors[colorInt];
                once = false;
            }
        }

        private void ReadRecord()
        {
            StreamReader streamReader = new StreamReader("record.txt");
            record = Int32.Parse(streamReader.ReadLine());
            streamReader.Close();
        }

        private void WriteRecord()
        {
            StreamWriter streamWriter = new StreamWriter("record.txt");
            streamWriter.Write(record);
            streamWriter.Close();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            //spriteBatch.Draw(texture, rect, Color.White);

           for (int i = 0; i < snake.Body.Count; i++)
                {
                    if (snake.Body[i].Width > 0)
                    {
                        if (i == 0)
                            spriteBatch.Draw(texture, snake.Body[i], Color.White);
                        else
                            spriteBatch.Draw(texture, snake.Body[i], Color.White);
                    }
                }
            string output = "Score: " + score.ToString();
            Vector2 fontOrigin = new Vector2(0, 0);
            string recordOutput = "Record: " + record.ToString();
            Vector2 recordOrigin = new Vector2(0, 0);

            if (backgroundColor == Color.Gold || backgroundColor == Color.SpringGreen || backgroundColor == Color.Plum
                || backgroundColor == Color.Aqua || backgroundColor == Color.Aquamarine)
            {
                spriteBatch.DrawString(spriteFont, output, fontPos, Color.Black,
                0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(recordSprite, recordOutput, recordPos, Color.Black,
                0, recordOrigin, 1.0f, SpriteEffects.None, 0.5f);
            }
            else
            {
                spriteBatch.DrawString(spriteFont, output, fontPos, Color.LightGreen,
                0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                spriteBatch.DrawString(recordSprite, recordOutput, recordPos, Color.LightGreen,
                0, recordOrigin, 1.0f, SpriteEffects.None, 0.5f);
            }

            if (rect1.X > 0)
            {
                spriteBatch.Draw(texture1, rect1, Color.Fuchsia);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
