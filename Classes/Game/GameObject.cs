using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace monogame_test
{
    public class GameObject
    {
        public string SpriteName { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; } = new Vector2(0,0);
        public Vector2 Size { get; set; } = new Vector2(0,0);
        public Vector2 Velocity { get; set; } = new Vector2(0,0);
        public Vector2 Direction { get; set; } = new Vector2(0, -1);
        public float RotationalVelocity = 0;

        public GameObject(string spriteName, Vector2 size)
        {
            this.SpriteName = spriteName;
            this.Size = size;
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

            
        }

        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, position: Position, origin: new Vector2(Size.X * 0.5f, Size.Y * 0.5f), rotation: (float)Math.Atan2(Direction.Y, Direction.X));
            //batch.Draw(Texture, Position);
        }

        public void AddVelocity(float thrust)
        {
            Velocity = Velocity + (Direction * thrust);
        }

        public virtual void CollidesWith(GameObject other)
        {
            
        }
    }
}