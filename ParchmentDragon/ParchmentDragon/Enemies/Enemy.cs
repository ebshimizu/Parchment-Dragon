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
    // *NOTE: Collisions will be handled in a completely separate class.

    // Abstract class implements enemy interface to ensure that each enemy does the basics.
    // Subclasses will use the basic superclass functions for initialization and
    // calls to each enemy will not have to be explicitly typed.
    abstract class Enemy : EnemyInterface
    {
        protected int health;                   // Amount of health for each enemy type.
        protected int maxHealth;
        protected AnimatedTexture spriteAlive;  // Enemy sprite
        protected AnimatedTexture spriteDead;   // Animated texture for enemy.
        protected Vector2 position;             // Position of Enemy
        protected int scoreValue;               // How many points you get for killing the enemy.
        protected Vector2 center;               // Center of sprite image.
        protected float rotation;               // Values for Sprite rotation;
        public bool alive, animating;           // Boolean value for enemy state (alive or dead).
        protected int maxBullets;               // Maximum number of Bullets that enemy can fire
        protected Bullet[] bullets;             // Think of this as the ammo for each enemy.
        protected int width, height;            // Width and height for the sprite
        protected int movePattern, firePattern;
        protected bool dropsFood;
//        protected Timer time;
        protected fireTimer time;
        protected Timer enemyTime;
        protected bool isLeft;
        protected PlayerCharacter player;       // contains all data for the player character.
        public bool hasHealthBar = false;
        protected int[] healthBarDivisions;     // Contains a list of HP values where the health bar should indicate a change for the enemy.ss
        Random rand;
        protected bool isBoss = false;

        // Textures saved for copying the enemy.
        protected Texture2D aliveTex;
        protected Texture2D deadTex;
        protected Texture2D bulletText;

        public Enemy(AnimatedTexture spriteA, AnimatedTexture spriteD, Texture2D loadedBullet,
            int numBullets, float scale, Vector2 initialPosition,
            int mpattern, int fpattern, bool foodDrop, bool left)
        {
            // Assign essential textures.
            spriteAlive = spriteA;
            spriteDead = spriteD;
            width = (int)((spriteA.myTexture.Width / (spriteA.framecount / spriteA.rows)) * spriteA.Scale);
            height = (int)((spriteA.myTexture.Height / spriteA.rows) * spriteA.Scale);
            rotation = 0.0f;            // No initial rotation.
            animating = true;
            alive = true;         // Enemies start alive when they are added to the enemy list.
            maxBullets = numBullets;
            position = initialPosition;
            center = new Vector2(width / 2, height / 2);      // Center value assigned.
            movePattern = mpattern;
            firePattern = fpattern;
            dropsFood = foodDrop;
//            time = new fireTimer();
            enemyTime = new Timer();
            isLeft = left;
            
            // Save textures for copying enemies.
            aliveTex = spriteA.myTexture;
            deadTex = spriteD.myTexture;
            bulletText = loadedBullet;

            // Initializae new array of bullets.
            bullets = new Bullet[maxBullets];
            for (int i = 0; i < maxBullets; i++)
            {
                bullets[i] = new Bullet(loadedBullet, scale);
            }
            rand = new Random();
        }

        public abstract void Move();
//        public abstract void Fire(object o, System.Timers.ElapsedEventArgs e);
        public abstract void Fire();
        public abstract Enemy Copy();
        public abstract Enemy CopyLeft();
        public abstract Enemy CopyRight();

        // Need another copy method for prioviding starting location and position, firing pattern, movement pattern and if it drops food.
        // Makes code more compact.
        public abstract Enemy Copy(Vector2 position, Vector2 velocity, int mpattern, int fpattern, bool foodDrop, bool left);

        // If enemy is hit by a bullet as determined in the Collisions class do this.
        public virtual void Hit(PlayerCharacter pc, FoodList foods, List<Food> foodList)
        {
            pc.PlayEnemyHit();
            health--;
            if (health <= 0)
            {
                alive = false;
                pc.PlayEnemyDeath();
                //stop timer
                time.Stop();
                //time.Close();
                enemyTime.Stop();
                enemyTime.Close();
                if (isBoss)
                {
                    foodList.Add(foods.getRandomFood(
                        new Vector2(position.X + center.X - 16, position.Y + center.Y),
                        new Vector2(0, 1),
                        .2f));
                    foodList.Add(foods.getRandomFood(
                        new Vector2(position.X + center.X - 48, position.Y + center.Y),
                        new Vector2(0, 1),
                        .2f));
                }
                else if(rand.Next(0, 100) < 20)
                {
                    foodList.Add(foods.getRandomFood(
                        new Vector2(position.X + center.X - 32, position.Y + center.Y),
                        new Vector2(0, 1),
                        .2f));
                }
                pc.addToScore(this);
                pc.PlayEnemyDeath();
            }
        }

        public virtual void Die()
        {
            if (!isBoss)
            {
                alive = false;
                //stop timer
                time.Stop();
                //time.Close();
                enemyTime.Stop();
                enemyTime.Close();
                // Do not add to score. This happens when PC hits enemy.
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Basic Drawing functions. Depending on the animation, some subclasses will have to override this.
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
            if (alive)
            {
                spriteAlive.DrawFrame(spriteBatch, position);
                // spriteBatch.Draw(sprite, position, Color.White);
            }
            if (!alive && spriteDead.Frame < spriteDead.framecount)
            {
                spriteDead.DrawFrame(spriteBatch, position);
            }
        }

        public virtual void Update(GameTime gameTime, Rectangle enemyFieldBound, Rectangle fieldBound)
        {
            // Toggles between alive animation states and dead animation states.
            if (alive)
            {
                spriteAlive.UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
                time.update(gameTime);
            }
            if (!alive)
            {
                animating = spriteDead.UpdateFrameOnce((float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            // Handles when enemy moves off screen. Otherwise subclasses must implement other update tasks.
            if (!enemyFieldBound.Intersects(getRect()))
            {
                alive = false;
            }
            foreach (Bullet bullet in bullets)
            {
                bullet.Update(fieldBound);
            }
        }

        public Bullet[] getBullets()
        {
            return bullets;
        }

        public virtual Rectangle getRect()
        {   
            return new Rectangle(
               (int)(position.X + (width * .1f)),
               (int)(position.Y + (height * .1f)),
               (int)(width * .8f),
               (int)(height * .8f));
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public void stopTimer()
        {
            time.Stop();
            enemyTime.Stop();
        }

        public void startTimer()
        {
            time.Start();
            enemyTime.Start();
        }

        public int getScore()
        {
            return scoreValue;
        }

        public void getPC(PlayerCharacter pc)
        {
            player = pc;
        }

        public int[] getHealthDiv()
        {
            return healthBarDivisions;
        }

        public int getMaxHealth()
        {
            return maxHealth;
        }

        public int getHealth()
        {
            return health;
        }

        public Vector2 getCenter()
        {
            return new Vector2(position.X + (width / 2), position.Y + (height / 2));
        }
    }
}
