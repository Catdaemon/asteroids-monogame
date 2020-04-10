using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace monogame_test
{
    public class Ship : GameObject
    {
        public Ship() : base("ship", new Vector2(100,100))
        {

        }

        public void Load(ContentManager content)
        {
            content.Load<Texture2D>(SpriteName);
        }

        public override void Update(AsteroidsGame ctx, GameTime gameTime, InputStatus input)
        {
            var delta = (float)(gameTime.ElapsedGameTime.TotalSeconds);

            RotationalVelocity += (-input.LeftAmount + input.RightAmount) * delta;            
            
            this.AddVelocity((input.ForwardAmount - input.BackwardAmount) * delta * 100);

            if (input.Fire) {
                var bullet = new Bullet(this, 500);
                ctx.AddObject(bullet);
            }

            base.Update(ctx, gameTime, input);
        }

        public override void CollidesWith(AsteroidsGame ctx, GameObject other)
        {
            if (other is Asteroid)
            {
                // bounce
                this.Velocity = Vector2.Normalize(this.Position - other.Position) * (this.Velocity.Length() * 0.5f);
            }

            base.CollidesWith(ctx, other);
        }
    }
}