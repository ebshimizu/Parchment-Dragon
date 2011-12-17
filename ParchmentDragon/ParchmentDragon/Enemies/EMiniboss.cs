using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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
    class EMiniboss : Enemy
    {
        // This is enemy 4 in the enemy list. 
        // TODO: Override getRect() and assign proper scaling.
        public Vector2 velocity;
        private bool firstwavefinished = false;

        public EMiniboss(AnimatedTexture spriteA, AnimatedTexture spriteD, Texture2D loadedBullet,
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
            health = 150;
            maxHealth = health;
            hasHealthBar = true;
            time = new fireTimer(new TimeSpan(0,0,0,1,500), Fire);
            time.Start();
            enemyTime.Interval = 55000;
            enemyTime.Elapsed += new System.Timers.ElapsedEventHandler(Retreat);
            enemyTime.Start();
            isBoss = true;
        }

        public override void Move()
        {
            if (getCenter().Y > 200)
                velocity.Y = 0f;

            position += velocity;
        }

        public void Retreat(object o, System.Timers.ElapsedEventArgs e)
        {
            velocity.Y = -2f;
            position += velocity;
        }

        public override void Fire()
        {
            if (health > 75) /* first bullet pattern */
            {
                for (int i = 0; i < maxBullets - 4; i += 5)
                {
                    if (!bullets[i].inPlay && !bullets[i + 1].inPlay && !bullets[i + 2].inPlay && !bullets[i + 3].inPlay &&
                            !bullets[i + 4].inPlay)
                    {
                        bullets[i].velocity = new Vector2(0, 4f);
                        bullets[i].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i].inPlay = true;

                        bullets[i + 1].velocity = new Vector2(-4f, 0);
                        bullets[i + 1].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i + 1].inPlay = true;

                        bullets[i + 2].velocity = new Vector2(4f, 0);
                        bullets[i + 2].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i + 2].inPlay = true;

                        bullets[i + 3].velocity = new Vector2(-2.5f, 2.5f);
                        bullets[i + 3].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i + 3].inPlay = true;

                        bullets[i + 4].velocity = new Vector2(2.5f, 2.5f);
                        bullets[i + 4].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i + 4].inPlay = true;

                        break;
                    }
                }
            }
            else /* second bullet pattern */
            {
                //if (!firstwavefinished)
                //{
                //    foodList.Add(foods.getRandomFood(
                //        new Vector2(position.X + center.X - 48, position.Y + center.Y),
                //        new Vector2(0, 1),
                //        .2f));
                //    foodList.Add(foods.getRandomFood(
                //        new Vector2(position.X + center.X - 16, position.Y + center.Y),
                //        new Vector2(0, 1),
                //        .2f));
                //    firstwavefinished = true;
                //}
                for (int i = 0; i < maxBullets - 8; i += 9)
                {
                    if (!bullets[i].inPlay && !bullets[i + 1].inPlay && !bullets[i + 2].inPlay && !bullets[i + 3].inPlay &&
                        !bullets[i + 4].inPlay && !bullets[i + 5].inPlay && !bullets[i + 6].inPlay && !bullets[i + 7].inPlay)
                    {
                        bullets[i].velocity = new Vector2(0, 4f);
                        bullets[i].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i].inPlay = true;

                        bullets[i + 1].velocity = new Vector2(0, -4f);
                        bullets[i + 1].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i + 1].inPlay = true;

                        bullets[i + 2].velocity = new Vector2(-4f, 0);
                        bullets[i + 2].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i + 2].inPlay = true;

                        bullets[i + 3].velocity = new Vector2(4f, 0);
                        bullets[i + 3].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i + 3].inPlay = true;

                        bullets[i + 4].velocity = new Vector2(-2.5f, 2.5f);
                        bullets[i + 4].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i + 4].inPlay = true;

                        bullets[i + 5].velocity = new Vector2(2.5f, 2.5f);
                        bullets[i + 5].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i + 5].inPlay = true;

                        bullets[i + 6].velocity = new Vector2(-2.5f, -2.5f);
                        bullets[i + 6].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i + 6].inPlay = true;

                        bullets[i + 7].velocity = new Vector2(2.5f, -2.5f);
                        bullets[i + 7].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i + 7].inPlay = true;

                        // calculate vector from boss to player
                        double x_diff = (player.position.X + player.width / 2) - (position.X + width / 2);
                        double y_diff = (player.position.Y + player.height / 2) - (position.Y + height - 20);
                        double theta = Math.Atan2(y_diff, x_diff);
                        x_diff = 3 * Math.Cos(theta);
                        y_diff = 3 * Math.Sin(theta);
                        bullets[i + 8].velocity = new Vector2((float)x_diff, (float)y_diff);
                        bullets[i + 8].position = new Vector2(getCenter().X - 5, getCenter().Y + (height / 2) - 20);
                        bullets[i + 8].inPlay = true;
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
            Enemy enemyCopy = new EMiniboss(
                new AnimatedTexture(aliveTex, 25, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 5),
                new AnimatedTexture(deadTex, 40, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 8),
                bulletText, maxBullets, scoreValue, 0.25f, new Vector2(300 - getWidth() / 2, -50f), new Vector2(0, 2f), 1, 1, false, false);
            return enemyCopy;
        }
        public override Enemy CopyLeft()
        {
            // Enemy copy method for left movement path
            Enemy enemyCopy = new EMiniboss(
                new AnimatedTexture(aliveTex, 25, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 5),
                new AnimatedTexture(deadTex, 40, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 8),
                bulletText, maxBullets, scoreValue, 0.25f, new Vector2(250 - getWidth() / 2, -50f), new Vector2(0, 2.5f), 1, 1, false, true);
            return enemyCopy;
        }

        public override Enemy CopyRight()
        {
            // Enemy copy method for right movement path
            Enemy enemyCopy = new EMiniboss(
                new AnimatedTexture(aliveTex, 25, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 5),
                new AnimatedTexture(deadTex, 40, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 8),
                bulletText, maxBullets, scoreValue, 0.25f, new Vector2(350 - getWidth() / 2, -50f), new Vector2(0, 2.5f), 1, 1, false, false);
            return enemyCopy;
        }

        public override Enemy Copy(Vector2 position, Vector2 velocity, int mpattern, int fpattern, bool foodDrop, bool left)
        {
            Enemy enemyCopy = new EMiniboss(
                new AnimatedTexture(aliveTex, 25, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 5),
                new AnimatedTexture(deadTex, 40, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 8),
                bulletText, maxBullets, scoreValue, 0.25f, position, velocity, mpattern, fpattern, foodDrop, left);
            return enemyCopy;
        }

        public override void Update(GameTime gameTime, Rectangle enemyFieldBound, Rectangle fieldBound)
        {
            if (alive)
            {
                Move();
            }
            base.Update(gameTime, enemyFieldBound, fieldBound);
        }

        public override Rectangle getRect()
        {
            return new Rectangle(
                (int)(position.X + (width * .33f)),
                (int)(position.Y + (height * .2f)),
                (int)(width * .3f),
                (int)(height * .7f));
        }
    }
}
