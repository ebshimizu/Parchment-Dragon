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
    class ExplodingBullet : Bullet
    {
        private int numShards; //How many shards it explodes into
        private Bullet[] shards; //array of shards
        private double[] angles; //array of angles where each shard will go
        private bool exploded; //whether the bullet exploded or not
        private int explodeX; //what X coordinate to explode at
        private int explodeY; //what Y coordinate to explode at
        private int explodeVelocity; //how fast the shards should move

        public ExplodingBullet(int n, double sAngle, double eAngle, int x, int y, int v, Texture2D loadSprite, float scale)
                               : base(loadSprite, scale)
        {
            numShards = n;
            exploded = false;
            explodeX = x;
            explodeY = y;
            explodeVelocity = v;
            angles = new double[numShards];
            shards = new Bullet[numShards];
            double angleIncrement = (eAngle - sAngle) / (numShards - 1);
            for (int i = 0; i < numShards; i++)
            {
                angles[i] = sAngle + (angleIncrement * i);
                shards[i] = new Bullet(sprite, scaleFactor);
            }
        }

        private void Explode()
        {
            for (int i = 0; i < numShards; i++)
            {
                double xVeloc = explodeVelocity * Math.Cos(angles[i]);
                double yVeloc = explodeVelocity * Math.Sin(angles[i]);
                shards[i].velocity = new Vector2((float)xVeloc, (float)yVeloc);
                shards[i].position = position;
                shards[i].inPlay = true;
            }
            exploded = true;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (inPlay && !exploded)
            {
                spriteBatch.Draw(sprite, position, null, Color.White, rotation, Vector2.Zero, scaleFactor, SpriteEffects.None, .1f);
            }

            foreach (Bullet bullet in shards)
            {
                bullet.Draw(spriteBatch);
            }
        }

        public override void Update(Rectangle viewportRect)
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
                exploded = false;
            }

            if (inPlay && explodeX == -1)
            {
                if (!exploded && position.Y < (explodeY + 5) && position.Y > (explodeY - 5))
                    Explode();
            }
            else if (inPlay && explodeY == -1)
            {
                if (!exploded && position.X < (explodeX + 5) && position.X > (explodeX - 5))
                    Explode();
            }
            else if (inPlay && !exploded && position.X < (explodeX + 5) && position.X > (explodeX - 5) &&
                position.Y < (explodeY + 5) && position.Y > (explodeY - 5))
                Explode();

            position.X += velocity.X;
            position.Y += velocity.Y;

            foreach (Bullet shard in shards)
            {
                shard.Update(viewportRect);
            }
        }

        public Bullet[] getShards() { return shards; }
        public bool isExploded() { return exploded; }
    }
}