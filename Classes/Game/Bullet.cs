using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace monogame_test
{
    public class Bullet : GameObject
    {
        public Bullet(GameObject from, float speed) : base("bullet", new Vector2(64,32))
        {
            Position = from.Position;
            Direction = from.Direction;
            Velocity = Vector2.Multiply(Vector2.Normalize(Direction), scaleFactor: speed);
            Wraps = false;
        }

        public void Load(ContentManager content)
        {
            content.Load<Texture2D>(SpriteName);
        }
    }
}