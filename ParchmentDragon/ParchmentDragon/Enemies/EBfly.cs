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
    class EBfly : Enemy
    {
        // This is enemy 2 in the enemy list.   
        public Vector2 velocity;

        public EBfly(AnimatedTexture spriteA, AnimatedTexture spriteD, Texture2D loadedBullet,
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
            health = 30;
            maxHealth = health;
            time = new fireTimer(new TimeSpan(0, 0, 0, 1, 500), Fire);
            time.Start();
            enemyTime.Interval = 10000;
            enemyTime.Elapsed += new System.Timers.ElapsedEventHandler(Retreat);
            enemyTime.Start();
        }

        public override void Move()
        {
            position += velocity;
        }

        public void Retreat(object o, System.Timers.ElapsedEventArgs e)
        {
            velocity.Y = -1.5f;
            position += velocity;
        }

        public override void Fire()
        {
            for (int i = 0; i < maxBullets - 2; i+=3)
            {
                if (!bullets[i].inPlay && !bullets[i+1].inPlay && !bullets[i+2].inPlay)
                {
                        bullets[i].velocity = new Vector2(0, 4f);
                        bullets[i].position = new Vector2(getCenter().X, getCenter().Y + (height / 2) - 20);
                        bullets[i].inPlay = true;

                        bullets[i+1].velocity = new Vector2(-1f, 3f);
                        bullets[i + 1].position = new Vector2(getCenter().X - 20, getCenter().Y + (height / 2) - 20);
                        bullets[i+1].inPlay = true;
                    
                        bullets[i+2].velocity = new Vector2(1f, 3f);
                        bullets[i + 2].position = new Vector2(getCenter().X + 20, getCenter().Y + (height / 2) - 20);
                        bullets[i+2].inPlay = true;
                        break;
                }
            }
        }


        // These copy methods are specific for each type of enemy.
        // They should be called on an enemy of a specific type (not the generic superclass).

        public override Enemy Copy()
        {
            // This returns an emeny with default settings. Good if you have a bunch of enemies
            // that don't really have different settings. Don't call this for the rock.
            Enemy enemyCopy = new EBfly(
                new AnimatedTexture(aliveTex, 10, 20, Vector2.Zero, 0.0f, 1.0f, 0.2f, 5),
                new AnimatedTexture(deadTex, 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 7),
                bulletText, maxBullets, scoreValue, 0.5f, Vector2.Zero, Vector2.Zero, 1, 1, false, false);
            return enemyCopy;
        }
        public override Enemy CopyLeft()
        {
            // Enemy copy method for left movement path
            Enemy enemyCopy = new EBfly(
                new AnimatedTexture(aliveTex, 10, 20, Vector2.Zero, 0.0f, 1.0f, 0.2f, 5),
                new AnimatedTexture(deadTex, 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 7),
                bulletText, maxBullets, scoreValue, 0.5f, new Vector2(300 - getWidth() / 2, -50f), new Vector2(0, 2.5f), 1, 1, false, true);
            return enemyCopy;
        }

        public override Enemy CopyRight()
        {
            // Enemy copy method for right movement path
            Enemy enemyCopy = new EBfly(
                new AnimatedTexture(aliveTex, 10, 20, Vector2.Zero, 0.0f, 1.0f, 0.2f, 5),
                new AnimatedTexture(deadTex, 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 7),
                bulletText, maxBullets, scoreValue, 0.5f, new Vector2(300 - getWidth() / 2, -50f), new Vector2(0, 2.5f), 1, 1, false, false);
            return enemyCopy;
        }

        public override Enemy Copy(Vector2 position, Vector2 velocity, int mpattern, int fpattern, bool foodDrop, bool left)
        {
            Enemy enemyCopy = new EBfly(
                new AnimatedTexture(aliveTex, 10, 20, Vector2.Zero, 0.0f, 1.0f, 0.2f, 5),
                new AnimatedTexture(deadTex, 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 7),
                bulletText, maxBullets, scoreValue, 0.5f, position, velocity, mpattern, fpattern, foodDrop, left);
            return enemyCopy;
        }

        public override void Update(GameTime gameTime, Rectangle enemyFieldBound, Rectangle fieldBound)
        {
            if (alive && getCenter().Y < 200)
            {
                Move();
            }
            base.Update(gameTime, enemyFieldBound, fieldBound);
        }

        public override Rectangle getRect()
        {
            return new Rectangle(
                (int)(position.X + (width * .2f)),
                (int)(position.Y + (height * .2f)),
                (int)(width * .6f),
                (int)(height * .6f));
        }
    }
}
