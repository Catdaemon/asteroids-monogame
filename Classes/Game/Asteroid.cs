using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace monogame_test
{
    public class Asteroid : GameObject
    {
        public Asteroid() : base("asteroid", new Vector2(100,100))
        {

        }

        public void Load(ContentManager content)
        {
            content.Load<Texture2D>(SpriteName);
        }

        public override void CollidesWith(GameObject other)
        {
            if (other is Ship)
            {
                // get pushed
                this.Velocity = Vector2.Normalize(this.Position - other.Position) * (other.Velocity.Length() * 0.5f);
            }

            base.CollidesWith(other);
        }
    }
}