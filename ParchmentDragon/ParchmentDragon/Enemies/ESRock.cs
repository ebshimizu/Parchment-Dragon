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
    class ESRock : Enemy
    {
        // This is enemy 0 in the enemy list.   
        public Vector2 velocity;

        public ESRock(AnimatedTexture spriteA, AnimatedTexture spriteD, Texture2D loadedBullet,
            int maxBullets, int scoreVal, float scale, Vector2 initialPosition, Vector2 initialVelocity,
            int mpattern, int fpattern, bool dropsFood, bool left)
            : base(spriteA, spriteD, loadedBullet, maxBullets, scale, initialPosition, mpattern, fpattern, dropsFood, left)
        {
            aliveTex = spriteA.myTexture;
            deadTex = spriteD.myTexture;
            bulletText = loadedBullet;
            scoreValue = scoreVal;
            velocity = initialVelocity;
            isLeft = left;
            health = 20;
            maxHealth = health;
            time = new fireTimer(new TimeSpan(0,0,0,1,500), Fire);
            time.Start();
        }

        public override void Move()
        {
            position += velocity;
        }

        public override void Fire()
        {
            for (int i = 0; i < maxBullets; i++)
            {
                if (!bullets[i].inPlay)
                {
                    if (i % 3 == 0)
                    {
                        bullets[i].velocity = new Vector2(0, 4f);
                        bullets[i].position = new Vector2(position.X + width / 2, position.Y + spriteAlive.myTexture.Height - 20);
                        bullets[i].inPlay = true;
                    }
                    if (i % 3 == 1)
                    {
                        bullets[i].velocity = new Vector2(-1f, 3f);
                        bullets[i].position = new Vector2(position.X + width / 2 - 20, position.Y + spriteAlive.myTexture.Height - 20);
                        bullets[i].inPlay = true;
                    }
                    if (i % 3 == 2)
                    {
                        bullets[i].velocity = new Vector2(1f, 3f);
                        bullets[i].position = new Vector2(position.X + width / 2 + 20, position.Y + spriteAlive.myTexture.Height - 20);
                        bullets[i].inPlay = true;
                        break;
                    }
                }
            }
        }


        // These copy methods are specific for each type of enemy.
        // They should be called on an enemy of a specific type (not the generic superclass).

        public override Enemy Copy()
        {
            // This returns an emeny with default settings. Good if you have a bunch of enemies
            // that don't really have different settings. Don't call this for the rock.
            Enemy enemyCopy = new ESRock(
                new AnimatedTexture(aliveTex, 1, 1, Vector2.Zero, 0.0f, 1.0f, 0.2f),
                new AnimatedTexture(deadTex, 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f),
                bulletText, maxBullets, scoreValue, 0.5f, Vector2.Zero, Vector2.Zero, 1, 1, false, false);
            return enemyCopy;
        }
        public override Enemy CopyLeft()
        {
            // Enemy copy method for left movement path
            Enemy enemyCopy = new ESRock(
                new AnimatedTexture(aliveTex, 1, 1, Vector2.Zero, 0.0f, 1.0f, 0.2f),
                new AnimatedTexture(deadTex, 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f),
                bulletText, maxBullets, scoreValue, 0.5f, new Vector2(300 - getWidth() / 2, -50f), new Vector2(0, 2.5f), 1, 1, false, true);
            return enemyCopy;
        }

        public override Enemy CopyRight()
        {
            // Enemy copy method for right movement path
            Enemy enemyCopy = new ESRock(
                new AnimatedTexture(aliveTex, 1, 1, Vector2.Zero, 0.0f, 1.0f, 0.2f),
                new AnimatedTexture(deadTex, 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f),
                bulletText, maxBullets, scoreValue, 0.5f, new Vector2(300 - getWidth() / 2, -50f), new Vector2(0, 2.5f), 1, 1, false, false);
            return enemyCopy;
        }

        public override Enemy Copy(Vector2 position, Vector2 velocity, int mpattern, int fpattern, bool foodDrop, bool left)
        {
            Enemy enemyCopy = new ESRock(
                new AnimatedTexture(aliveTex, 1, 1, Vector2.Zero, 0.0f, 1.0f, 0.2f),
                new AnimatedTexture(deadTex, 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f),
                bulletText, maxBullets, scoreValue, 0.5f, position, velocity, mpattern, fpattern, foodDrop, left);
            return enemyCopy;
        }

        public override void Update(GameTime gameTime, Rectangle enemyFieldBound, Rectangle fieldBound)
        {
            if (alive && position.Y < (200 - getHeight() / 2))
            {
                Move();
            }
            base.Update(gameTime, enemyFieldBound, fieldBound);
        }
    }
}
