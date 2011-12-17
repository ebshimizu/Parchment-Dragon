using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using AnimatedSprite;

namespace ParchmentDragon
{
    public class Food
    {
        AnimatedTexture foodImage;
        public bool inPlay;
        Vector2 position;
        Vector2 velocity;
        float gaugeValue;       // Indicates what percent the food increases the gauge.

        public Food(Texture2D foodImage, Vector2 position, float value)
        // Construct new food with static position.
        {
            this.foodImage = new AnimatedTexture(foodImage, 2, 2, Vector2.Zero, 0.0f, 1.0f, .8f);
            this.position = position;
            velocity = Vector2.Zero;
            inPlay = true;
            gaugeValue = value;
        }

        public Food(Texture2D foodImage, Vector2 position, Vector2 velocity, float value)
        // Construct new food with velocity.
        {
            this.foodImage = new AnimatedTexture(foodImage, 2, 2, Vector2.Zero, 0.0f, 1.0f, .8f);
            this.position = position;
            this.velocity = velocity;
            inPlay = true;
            gaugeValue = value;
        }

        public void Draw(SpriteBatch spriteBatch)
        // Draws a food object if it's in play.
        {
            if (inPlay)
            {
                foodImage.DrawFrame(spriteBatch, position);
            }
        }

        public virtual void Update(GameTime gameTime, Rectangle stagingArea, Rectangle fieldBound)
        {
            // Update the animation.
            foodImage.UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
            // Make the food move.
            Move();
            // If the food moves out of the staging area make it not in play.
            if (!stagingArea.Intersects(getRect()))
            {
                inPlay = false;
            }
        }

        private void Move()
        {
            position.X += velocity.X;
            position.Y += velocity.Y;
        }

        public float getValue()
        {
            return gaugeValue;
        }

        public void eaten()
        {
            // If you eat it it's no longer in play. :)
            inPlay = false;
        }

        public Rectangle getRect()
        {
            return new Rectangle(
                (int)(position.X + (foodImage.myTexture.Width / 2)*.2f),
                (int)(position.Y + (foodImage.myTexture.Height*.2f)),
                (int)((foodImage.myTexture.Width / 2)*.6f),
                (int)(foodImage.myTexture.Height*.6f));
        }

        public Food copy(Vector2 position, Vector2 velocity, float value)
        {
            // Used to add new food objects to the stage food list.
            return new Food(foodImage.myTexture, position, velocity, value);
        }
    }
}
