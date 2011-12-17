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

namespace ParchmentDragon
{
    // Class handles bullet drawing and stuff.
    class Bullet
    {
        public Texture2D sprite;           // Bullet sprite.
        public Vector2 position;    // Position of bullet.
        public float rotation;      // Rotation of bullet.
        public Vector2 center;      // Center of Bullet
        public Vector2 velocity;    // How fast bullet is moving.
        public bool inPlay;         // Is the bullet on the field?
        protected float scaleFactor;    // Percentace of original image to scale to.

        public Bullet(Texture2D loadSprite, float scale)
        {
            sprite = loadSprite;
            inPlay = false;
            scaleFactor = scale;
            center = new Vector2(sprite.Width*scaleFactor / 2, sprite.Height*scaleFactor / 2);
            rotation = 0.0f;
            position = Vector2.Zero;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (inPlay)
            {
                spriteBatch.Draw(sprite, position, null, Color.White, rotation, Vector2.Zero, scaleFactor, SpriteEffects.None, .1f);
            }
        }

        public void Update(Rectangle viewportRect)
        {
            // Bullet collisions handled in separate class.
            Rectangle bulletRect = new Rectangle(
                (int)position.X,
                (int)position.Y,
                (int)(sprite.Width * scaleFactor),
                (int)(sprite.Height * scaleFactor));
            if (!viewportRect.Intersects(bulletRect))
            {
                inPlay = false;
            }
            position.X += velocity.X;
            position.Y += velocity.Y;
        }

        public Rectangle getRect()
        {
            return new Rectangle(
                        (int)position.X,
                        (int)position.Y,
                        (int)(sprite.Width * scaleFactor),
                        (int)(sprite.Height * scaleFactor));
        }
    }
}
