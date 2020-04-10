using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;


namespace monogame_test
{
    public class GameObject
    {
        public string SpriteName { get; set; }
        public Texture2D? Texture { get; set; }
        public Vector2 Position { get; set; } = new Vector2(0,0);
        public Vector2 Size { get; set; } = new Vector2(0,0);
        public Vector2 Velocity { get; set; } = new Vector2(0,0);
        public Vector2 Direction { get; set; } = new Vector2(0, -1);
        public bool Wraps = true;
        public float RotationalVelocity = 0;

        protected AsteroidsGame GameContext { get; set; }

        public GameObject(AsteroidsGame context, string spriteName, Vector2 size)
        {
            this.SpriteName = spriteName;
            this.Size = size;
            this.GameContext = context;

            GameContext.AddObject(this);
        }       

        public void LoadSprite(ContentManager content)
        {
            Texture = content.Load<Texture2D>(SpriteName);
        }

        public virtual void Update(GameTime gameTime, InputStatus input)
        {
            var delta = (float)(gameTime.ElapsedGameTime.TotalSeconds);
            Direction = Vector2.Transform(Direction, Matrix.CreateRotationZ(RotationalVelocity * delta));
            Position = Position + (Velocity * delta);

            if (Wraps)
            {
                if (Position.X > AsteroidsGame.GameWidth)
                    Position = new Vector2(0f, Position.Y);
                if (Position.X < 0)
                    Position = new Vector2(AsteroidsGame.GameWidth, Position.Y);
                if (Position.Y > AsteroidsGame.GameHeight)
                    Position = new Vector2(Position.X, 0);
                if (Position.Y < 0)
                    Position = new Vector2(Position.X, AsteroidsGame.GameHeight);
            }
            else
            {
                if (Position.X > AsteroidsGame.GameWidth || Position.X < 0 || Position.Y > AsteroidsGame.GameHeight || Position.Y < 0)
                {
                    this.Remove();
                }
            }
        }

        public virtual void Draw(SpriteBatch batch)
        {
            var origin = new Vector2(Size.X * 0.5f, Size.Y * 0.5f);
            var rotation = (float)Math.Atan2(Direction.Y, Direction.X);
            
            if (Texture != null) {
                // TODO: replace with a not-obsolete call
                batch.Draw(Texture, position: Position, color: Color.White, origin: origin, rotation: rotation);
            }           
        }

        public void AddVelocity(float thrust)
        {
            Velocity = Velocity + (Direction * thrust);
        }

        public virtual void CollidesWith(GameObject other)
        {
            
        }

        public virtual void Remove()
        {
            GameContext.RemoveObject(this);
        }
    }
}