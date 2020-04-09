using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace monogame_test
{
    public class AsteroidsGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private List<GameObject> Objects = new List<GameObject>();
        private Ship Player1Ship;

        public AsteroidsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Player1Ship = new Ship()
            {
                Position = new Vector2(100,100)
            };

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            Objects.Add(Player1Ship);

            var rand = new Random();

            for(int i=0; i< 5; i++)
            {
                var asteroid = new Asteroid()
                {
                    Position = new Vector2(rand.Next(0, graphics.PreferredBackBufferWidth), rand.Next(0, graphics.PreferredBackBufferHeight)),
                    Velocity = new Vector2(rand.Next(-10, 10), rand.Next(-10, 10)),
                    RotationalVelocity = rand.Next(-10, 10)
                };
                Objects.Add(asteroid);
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            foreach(var o in Objects)
            {
                o.LoadSprite(this.Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            var gamepadState = GamePad.GetState(PlayerIndex.One);
            var keyboardState = Keyboard.GetState();

            var input = new InputStatus()
            {
                ForwardAmount = (gamepadState.ThumbSticks.Left.Y > 0 ? gamepadState.ThumbSticks.Left.Y : 0) + (keyboardState.IsKeyDown(Keys.W) ? 1 : 0),
                BackwardAmount = (gamepadState.ThumbSticks.Left.Y < 0 ? gamepadState.ThumbSticks.Left.Y : 0) + (keyboardState.IsKeyDown(Keys.S) ? 1 : 0),
                LeftAmount = (gamepadState.ThumbSticks.Left.X < 0 ? gamepadState.ThumbSticks.Left.X : 0) + (keyboardState.IsKeyDown(Keys.A) ? 1 : 0),
                RightAmount = (gamepadState.ThumbSticks.Left.X > 0 ? gamepadState.ThumbSticks.Left.X : 0) + (keyboardState.IsKeyDown(Keys.D) ? 1 : 0),
            };

            foreach(var o in Objects)
            {
                // Check for collision with other objects
                foreach(var other in Objects)
                {
                    if (other == o)
                        continue;

                    if (new Rectangle(o.Position.ToPoint(), o.Size.ToPoint()).Intersects(new Rectangle(other.Position.ToPoint(), other.Size.ToPoint())))
                    {
                        o.CollidesWith(other);
                    }
                }

                o.Update(gameTime, input);

                if (o.Position.X > graphics.PreferredBackBufferWidth)
                    o.Position = new Vector2(0f, o.Position.Y);
                if (o.Position.X < 0)
                    o.Position = new Vector2(graphics.PreferredBackBufferWidth, o.Position.Y);
                if (o.Position.Y > graphics.PreferredBackBufferHeight)
                    o.Position = new Vector2(o.Position.X, 0);
                if (o.Position.Y < 0)
                    o.Position = new Vector2(o.Position.X, graphics.PreferredBackBufferHeight);
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach(var o in Objects)
            {
                o.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
