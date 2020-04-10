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
        SpriteBatch? spriteBatch;
        private KeyboardState lastKeyboardState = new KeyboardState();
        private GamePadState lastGamepadState = new GamePadState();


        private SortedSet<GameObject> Objects = new SortedSet<GameObject>(comparer: new DepthComp());
        private HashSet<GameObject> ToRemove = new HashSet<GameObject>();
        private HashSet<GameObject> ToAdd = new HashSet<GameObject>();

        public AsteroidsGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            var Player1Ship = new Ship()
            {
                Position = new Vector2(100,100),
                Depth = -1,
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
                BackwardAmount = (gamepadState.ThumbSticks.Left.Y < 0 ? gamepadState.ThumbSticks.Left.Y * -1 : 0) + (keyboardState.IsKeyDown(Keys.S) ? 1 : 0),
                LeftAmount = (gamepadState.ThumbSticks.Left.X < 0 ? gamepadState.ThumbSticks.Left.X * -1 : 0) + (keyboardState.IsKeyDown(Keys.A) ? 1 : 0),
                RightAmount = (gamepadState.ThumbSticks.Left.X > 0 ? gamepadState.ThumbSticks.Left.X : 0) + (keyboardState.IsKeyDown(Keys.D) ? 1 : 0),
                Fire = ((gamepadState.Buttons.A > 0 && lastGamepadState.Buttons.A == 0 ) || keyboardState.IsKeyDown(Keys.Space) && !lastKeyboardState.IsKeyDown(Keys.Space))
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
                        o.CollidesWith(this, other);
                    }
                }

                o.Update(this, gameTime, input);

                if (o.Wraps)
                {
                    if (o.Position.X > graphics.PreferredBackBufferWidth)
                        o.Position = new Vector2(0f, o.Position.Y);
                    if (o.Position.X < 0)
                        o.Position = new Vector2(graphics.PreferredBackBufferWidth, o.Position.Y);
                    if (o.Position.Y > graphics.PreferredBackBufferHeight)
                        o.Position = new Vector2(o.Position.X, 0);
                    if (o.Position.Y < 0)
                        o.Position = new Vector2(o.Position.X, graphics.PreferredBackBufferHeight);
                }
                else
                {
                    if (o.Position.X > graphics.PreferredBackBufferWidth || o.Position. X < 0 || o.Position.Y > graphics.PreferredBackBufferHeight || o.Position.Y < 0)
                    {
                        RemoveObject(o);
                    }
                }
            }

            // Update list of objects

            Objects.UnionWith(ToAdd);
            
            Objects.ExceptWith(ToRemove);

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
            if (spriteBatch == null) {
                return;
            }
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

        public void RemoveObject(GameObject obj) {
            ToRemove.Add(obj);
        }

        public void AddObject(GameObject obj) {
            ToAdd.Add(obj);
            obj.LoadSprite(this.Content);
        }
    }

    public class DepthComp: IComparer<GameObject> {
        public int Compare(GameObject a, GameObject b) {
            // If the references are the same, returning 0 will dedupe them in the set.
            if (a == b)
            {
                return 0;
            }

            // Then try depth

            if (a.Depth != b.Depth) {
                return a.Depth > b.Depth ? 1 : -1;
            }

            // Then sprite name

            var nameComprare = a.SpriteName.CompareTo(b.SpriteName);

            if (nameComprare != 0) {
                return nameComprare;
            }

            // Finally, ID.
            // We cannot simply return '1' because that breaks the SortedSet.
            // We also cannot use a proprty which changes, e.g. position.
            return a.ID.CompareTo(b.ID);            
        }
    }
}
