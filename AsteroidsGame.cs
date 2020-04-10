using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace monogame_test
{
    public class AsteroidsGame : Game
    {
        SpriteBatch? spriteBatch;
        GraphicsDeviceManager graphics;
        
        private KeyboardState lastKeyboardState = new KeyboardState();
        private GamePadState lastGamepadState = new GamePadState();

        private List<GameObject> Objects = new List<GameObject>();
        private HashSet<GameObject> ToRemove = new HashSet<GameObject>();
        private HashSet<GameObject> ToAdd = new HashSet<GameObject>();

        public int Score = 0;

        public static int GameWidth = 1024;
        public static int GameHeight = 768;

        public AsteroidsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = GameWidth;
            graphics.PreferredBackBufferHeight = GameHeight;
            graphics.ApplyChanges();

            var Player1Ship = new Ship(this)
            {
                Position = new Vector2(100, 100),
            };

            for (int i = 0; i < 5; i++)
            {
                SpawnAsteroid();
            }

            base.Initialize();
        }

        public void SpawnAsteroid()
        {
            var rand = new Random();
            var asteroid = new Asteroid(this)
            {
                Position = new Vector2(rand.Next(0, graphics.PreferredBackBufferWidth), rand.Next(0, graphics.PreferredBackBufferHeight)),
                Velocity = new Vector2(rand.Next(-10, 10), rand.Next(-10, 10)),
                RotationalVelocity = rand.Next(-10, 10)
            };
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            foreach (var o in Objects)
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
                BackwardAmount = (gamepadState.ThumbSticks.Left.Y < 0 ? gamepadState.ThumbSticks.Left.Y * -1 : 0) + (keyboardState.IsKeyDown(Keys.S) ? 1 : 0),
                LeftAmount = (gamepadState.ThumbSticks.Left.X < 0 ? gamepadState.ThumbSticks.Left.X * -1 : 0) + (keyboardState.IsKeyDown(Keys.A) ? 1 : 0),
                RightAmount = (gamepadState.ThumbSticks.Left.X > 0 ? gamepadState.ThumbSticks.Left.X : 0) + (keyboardState.IsKeyDown(Keys.D) ? 1 : 0),
                Fire = ((gamepadState.Buttons.A > 0 && lastGamepadState.Buttons.A == 0) || keyboardState.IsKeyDown(Keys.Space) && !lastKeyboardState.IsKeyDown(Keys.Space))
            };

            foreach (var o in Objects)
            {
                // Check for collision with other objects
                foreach (var other in Objects)
                {
                    if (other == o)
                        continue;

                    if (new Rectangle(o.Position.ToPoint(), o.Size.ToPoint()).Intersects(new Rectangle(other.Position.ToPoint(), other.Size.ToPoint())))
                    {
                        o.CollidesWith(other);
                    }
                }

                o.Update(gameTime, input);
            }

            // Update list of objects

            Objects.RemoveAll((GameObject obj) => ToRemove.Contains(obj));
            Objects.AddRange(ToAdd);

            ToAdd.Clear();
            ToRemove.Clear();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            lastKeyboardState = keyboardState;
            lastGamepadState = gamepadState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (spriteBatch == null)
            {
                return;
            }
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var o in Objects)
            {
                o.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }


        public void RemoveObject(GameObject obj)
        {
            ToRemove.Add(obj);
        }

        public void AddObject(GameObject obj)
        {
            ToAdd.Add(obj);
            obj.LoadSprite(this.Content);
        }
    }
}
