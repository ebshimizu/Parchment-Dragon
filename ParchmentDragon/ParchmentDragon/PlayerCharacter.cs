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
    // Class for the player character (or characters, we can create more than one instance but not right now.)
    // Contains PC sprite control, score, food guage and the like.
    class PlayerCharacter
    {
        AnimatedTexture flySprite;             // PC Flying animation.
        AnimatedTexture deathSprite;             // PC death animation. :(
        protected Bullet[] fireballs;          // Fireball objects. To be fired of course.
        public Vector2 position;    // Position of PC.
        public float rotation;      // Rotation of PC.
        public Vector2 center;      // Center of PC
        public bool alive;          // Is the PC alive?
        public bool respawning;     // Is PC respawning?
        int lives;                  // Number of Lives. Can be initialized with overloaded constructor. Default is 3.
        int maxBullets;             // Maximum number of bullets PC is allowed to fire at a time. Default is 40 (10 sets of 4 on screen at most).
        public int width, height;
        KeyboardState keyboardState;        // Keyboard state monitor.
        KeyboardState prevKeyboardState;    // Previous Keyboard state monitor. Used for doing single press events.
        GamePadState prevGamePadState;
        GamePadState gamePadState;          // Controler state monitor.
        Rectangle viewportRect;             // Size of viewport to prevent you from going off screen.
        int shootCounter = 5;           // Limits how fast shooting with the spacebar held down is. Hard coded. 12 fireballs/second. (60/5)
        float gaugeLevel = 0f;         // Controls how many fireballs are shot. 4 stages: 0-24% 1 fireball, 25-59% 2 fireballs, 60-84% 3 fireballs, 85-100% 4 fireballs.
        int respawnTime = 90;          // 1.5 second wait time. Also 1.5 second invincibility after respawn.
        int respawnCounter;
        int multiplier;
        int score;
        public bool doneAnimating;
        SoundEffect crunch;
        SoundEffect enemyDie;
        SoundEffect enemyHit;
        SoundEffect playerDie;
        SoundEffect fireballShoot;
        Texture2D foodGauge, foodBar;

        // Standard constructor
        public PlayerCharacter(Texture2D flyingSprite, Texture2D deadSprite, Texture2D loadedBullet,
            Texture2D foodGauge, Texture2D foodBar, Rectangle sentViewportRect)
        {
            // Assign loaded sprite to local PC object.
            flySprite = new AnimatedTexture(flyingSprite, 25, 30, Vector2.Zero, 0.0f, 1.0f, .9f);
            deathSprite = new AnimatedTexture(deadSprite, 35, 20, Vector2.Zero, 0.0f, 1.0f, .9f, 7);

            width = flyingSprite.Width / 25;
            height = flyingSprite.Height;
            // Make a new PC alive at beginning.
            alive = true;

            // Used for screen bounds.
            viewportRect = sentViewportRect;    

            // Initial player position. Currently a little off center due to the way XNA handles positions.
            position = new Vector2(viewportRect.Width / 2 - (width / 2), viewportRect.Height * .9f);

            // No initial rotation.
            rotation = 0.0f;

            // Vector representing center of sprite.
            center = new Vector2(width / 2, height / 2);

            // Default constructor values. 3 Lives. 40 bullets at a time.
            lives = 3;
            maxBullets = 75;
            respawning = false;

            // Declare fireball array.
            fireballs = new Bullet[maxBullets];
            // Initialize fireball objects, which are bullet objects.
            for (int i = 0; i < maxBullets; i++)
            {
                // Create new fireball at index.
                fireballs[i] = new Bullet(loadedBullet, .32f);
            }
            prevKeyboardState = Keyboard.GetState(); // Initialize keyboard state to something. Doesn't matter what.
            prevGamePadState = GamePad.GetState(PlayerIndex.One); // Initialize gamepad to something. IDC what.
            this.foodGauge = foodGauge;
            this.foodBar = foodBar;
        }
        
        // Overloaded constructor allows a custom number of lives and bullets.
        public PlayerCharacter(Texture2D flyingSprite, Texture2D deadSprite, Texture2D loadedBullet,
            Texture2D foodGauge, Texture2D foodBar, Rectangle sentViewportRect, int numLives, int numBullets)
        {
            // See annotations for default constructor. This is practically the same.
            flySprite = new AnimatedTexture(flyingSprite, 25, 30, Vector2.Zero, 0.0f, 1.0f, .9f);
            deathSprite = new AnimatedTexture(deadSprite, 35, 20, Vector2.Zero, 0.0f, 1.0f, .9f, 7);
            width = flyingSprite.Width / 25;
            height = flyingSprite.Height;
            alive = true;
            viewportRect = sentViewportRect;
            position = new Vector2(viewportRect.Width / 2, viewportRect.Height * .9f);
            rotation = 0.0f;
            center = new Vector2(width / 2, height / 2);
            lives = numLives;
            maxBullets = numBullets;
            fireballs = new Bullet[maxBullets];
            // Initialize fireball objects, which are bullet objects.
            for (int i = 0; i < maxBullets; i++)
            {
                fireballs[i] = new Bullet(loadedBullet, .5f);
            }
            prevKeyboardState = Keyboard.GetState(); // Initialize keyboard state to something. Doesn't matter what.
            this.foodGauge = foodGauge;
            this.foodBar = foodBar;
        }

        public void LoadSounds(ContentManager content)
        {
            this.crunch = content.Load<SoundEffect>("Sounds\\crunch");
            this.enemyDie = content.Load<SoundEffect>("Sounds\\enemydie");
            this.enemyHit = content.Load<SoundEffect>("Sounds\\enemyhitshootlouder");
            this.playerDie = content.Load<SoundEffect>("Sounds\\playerdeathwings");
            this.fireballShoot = content.Load<SoundEffect>("Sounds\\shootbig");
        }

        public void PlayEnemyDeath()
        {
            this.enemyDie.Play(.5f, 0, 0);
        }

        public void PlayEnemyHit()
        {
            this.enemyHit.Play();
        }

        // Update method for player character. Note that it does not replace the 
        // update method in Game1.cs
        public void Update(GameTime gameTime)
        {
            if (gaugeLevel <= .25f) multiplier = 1;
            else if (.25 < gaugeLevel & gaugeLevel <= .6f) multiplier = 2;
            else if (.6f < gaugeLevel & gaugeLevel <= .85f) multiplier = 3;
            else if (gaugeLevel > .85f) multiplier = 4;
            if (alive || respawning)
            {
                // Decrease the food gauge by ~2.5% per second.
                gaugeLevel -= .00035f;

                // Get new keyboard state and assign it to PC keyboard state.
                keyboardState = Keyboard.GetState();
                gamePadState = GamePad.GetState(PlayerIndex.One);

                // Call to PC move method.
                Move();

                // Call to PC fire method.
                Fire();

                // Update previous keyboard state.
                prevKeyboardState = keyboardState;
                flySprite.UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
                // Make sure gauge can't fall below 0 or above 1.
                gaugeLevel = MathHelper.Clamp(gaugeLevel, 0, 1.0f);
            }
            else
            {
                // If we're not respawning we're dying. This call evaluates to !(true) while the death animation is still going.
                // Once we're done animating the death sprite, we're now in respawn mode.
                respawning = !(deathSprite.UpdateFrameOnce((float)gameTime.ElapsedGameTime.TotalSeconds));
            }
            // Respawn function.
            if (respawning)
            {
                // If you're out of lives, this runs once and nothing happens except that you're completely dead at this point.
                if (lives <= 0)
                {
                    respawning = false;
                    doneAnimating = true;
                }
                // Increment the counter to know when to come back in.
                respawnCounter++;
                // Keep updating the flying animation for continuity.
                flySprite.UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);

                // If we have enough lives, put the player back in the default position after 1.5 seconds.
                if (lives > 0 && respawnCounter == respawnTime)
                {
                    position = new Vector2(viewportRect.Width / 2 - (width / 2), viewportRect.Height * .9f);
                }
                // After another 1.5 seconds, if the palyer has enough lives, take away the invincibility.
                // Collsisions are checked in Collisions.cs which uses the respawning boolean to figure out whether to count the hits.
                if (lives > 0 && respawnCounter == respawnTime * 2)
                {
                    respawning = false;
                    alive = true;
                    deathSprite.Frame = 0;
                }
            }

            // Update fireballs by calling each one.
            foreach (Bullet fireball in fireballs)
            {
                fireball.Update(viewportRect);
            }
        }

        // Moves player character based on object instance of keyboardState.
        public void Move()
        {
            if (gamePadState.IsConnected)
            {
                if (alive || respawning)
                {
                    position.X += gamePadState.ThumbSticks.Left.X * 6f;
                    position.Y -= gamePadState.ThumbSticks.Left.Y * 6f;
                }
            }

            // Move Left.
            if ((alive || respawning) && keyboardState.IsKeyDown(Keys.Left))
            {
                position.X -= 6f;
            }
            // Move right.
            if ((alive || respawning) && keyboardState.IsKeyDown(Keys.Right))
            {
                position.X += 6f;
            }
            // Move Up.
            if ((alive || respawning) && keyboardState.IsKeyDown(Keys.Up))
            {
                position.Y -= 6f;
            }
            // Move Down.
            if ((alive || respawning) && keyboardState.IsKeyDown(Keys.Down))
            {
                position.Y += 6f;
            }
            // Those last annotations seemed pretty obvious but I wrote them anyway.
            // Clamp posision to stay in screen. Funny geometry goes on here. Read the XNA sprite positioning documentation.
            position.X = MathHelper.Clamp(position.X, -width * .43f, (viewportRect.Width - width * .60f));
            position.Y = MathHelper.Clamp(position.Y, 0, viewportRect.Height - height * .5f);
        }

        // Fires fireballs at a set rate while space is held down.
        public void Fire()
        {
            if ((alive || (respawning && respawnCounter >= respawnTime)) && (keyboardState.IsKeyDown(Keys.Space)
                || gamePadState.Buttons.A == ButtonState.Pressed || gamePadState.Triggers.Right == 1))
            {
                // Shoot only every 5 internal counts, which should equate to ~9ish times per second.
                if (shootCounter == 0)
                {
                    // Go through and find a dead fireball. And shoot it.
                    // This index controls how many fireballs are shot out and at what angle they're shot at.
                    int index = 0;
                    foreach (Bullet fireball in fireballs)
                    {
                        if (index == 0 && !fireball.inPlay)
                        {
                            // Negative velocity moves fireball up. Yup. Origin is top left, highest Y value is at bottom.
                            fireball.velocity = new Vector2(0, -10f);
                            // Put fireball initially at head. Put it slightly off center if guage level is in stages 2 or 4.
                            if ((0.25f <= gaugeLevel & gaugeLevel <= 0.6f) | (0.85f <= gaugeLevel))
                            {
                                fireball.position = new Vector2(position.X + (0.85f * center.X) - (fireball.center.X), position.Y);
                            }
                            // If gauge level is 1 or 3 stick fireball at head.
                            else
                            {
                                fireball.position = new Vector2(position.X + center.X - (fireball.center.X), position.Y);
                            }
                            fireball.inPlay = true;
                            // If gauge level is in stage 2 or above fire another fireball.
                            if (gaugeLevel >= 0.25f) { index++; }
                            else { break; }
                        }
                        if (index == 1 && !fireball.inPlay)
                        {
                            // If in stage 3 or greater fire fireball at angle to the right.
                            if (0.6f <= gaugeLevel)
                            {
                                fireball.velocity = new Vector2((float)(Math.Cos((MathHelper.Pi / 6.0f)) * 3f), -10f);
                                fireball.position = new Vector2(position.X + center.X - (fireball.center.X), position.Y);
                            }
                            // Otherwise put it slightly to the right of the first fireball shot.
                            else
                            {
                                fireball.velocity = new Vector2(0, -10f);
                                fireball.position = new Vector2(position.X + (center.X * 1.15f) - (fireball.center.X), position.Y);
                            }
                            fireball.inPlay = true;
                            // If gauge level is in stage 3 or greater fire another one.
                            if (gaugeLevel >= 0.6f) { index++; }
                            else { break; }
                        }
                        if (index == 2 && !fireball.inPlay)
                        {
                            // A somewhat redundant check is used here but it's consistent with the format above.
                            // If gauge level is 3 or greater fire a fireball at and angle to the left.
                            if (0.6f <= gaugeLevel)
                            {
                                fireball.velocity = new Vector2((float)(Math.Cos((MathHelper.Pi / 6.0f)) * -3f), -10f);
                                fireball.position = new Vector2(position.X + center.X - (fireball.center.X), position.Y);
                            }
                            // If gauge level is 4 fire one more.
                            fireball.inPlay = true;
                            if (gaugeLevel >= 0.85f) { index++; }
                            else { break; }
                        }
                        if (index == 3 && !fireball.inPlay)
                        {
                            // There's only one possibility here. Fire the fourth fireball off center of the first one shot.
                            fireball.velocity = new Vector2(0, -10f);
                            fireball.position = new Vector2(position.X + (center.X * 1.15f) - (fireball.center.X), position.Y);
                            fireball.inPlay = true;
                            break;
                        }
                    }
                // Play sound effect.
                this.fireballShoot.Play(.3f, 0, 0);
                }
            }
            // Sets the counter to a value from 0 to 4.
            shootCounter = (shootCounter + 1) % 5;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // If player is alive, draw the bullets and the flying animation.
            if (alive)
            {
                foreach (Bullet fireball in fireballs)
                {
                    fireball.Draw(spriteBatch);
                }
                flySprite.DrawFrame(spriteBatch, position);
            }
            // If player recently died make the sprite blink and draw fireballs.
            else if (respawning)
            {
                if (respawnCounter % 4 == 0 && respawnCounter >= respawnTime)
                {
                    flySprite.DrawFrame(spriteBatch, position);
                }
                foreach (Bullet fireball in fireballs)
                {
                    fireball.Draw(spriteBatch);
                }
            }
            // Otherwise player is dying, so we draw the death animation
            else
            {
                deathSprite.DrawFrame(spriteBatch, position);
            }
            // Draws the outline for the player's food gauge on the right.
            spriteBatch.Draw(foodGauge,
                new Vector2(700, 250),
                null,
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0f);
            // Draws the actual food gauge. The geometry here is confusing. Because XNA likes things to go top down,
            // what happens here is that we're kind of moving the starting point for the bar up and simaultaneously making
            // the bar bigger to make it look like it's filling up.
            Rectangle foodRect = new Rectangle(
                0, 0,
                (int)foodBar.Width,
                (int)(foodBar.Height * gaugeLevel));
            spriteBatch.Draw(foodBar,
                new Vector2(745, 315+256),
                foodRect,
                Color.White,
                0.0f,
                new Vector2(0, foodBar.Height*gaugeLevel),
                1.0f,
                SpriteEffects.None,
                0.05f);
        }

        public void Hit()
        {
            alive = false;
            gaugeLevel = 0;
            lives -= 1;
            respawnCounter = 0;
            this.playerDie.Play();
            if (lives <= 0)
            {
                // Game Over. This is checked in a different method, but it is here for possible future use.
            }
        }

        public void eatFood(Food lunch)
        {
            // If player hits a food item, we eat it and the food gauge fills up.
            gaugeLevel += lunch.getValue();
            lunch.eaten();
            // 10 points times multiplier awarded for eating food.
            score += 10 * multiplier;
            this.crunch.Play();
        }

        // Returns a list of fireballs contained by the player character.
        public Bullet[] getBullets()
        {
            return fireballs;
        }

        // Gets the hit box for the player.
        public Rectangle getRect()
        {
            return new Rectangle(
                (int)(position.X + (5f/12f) * width),
                (int)(position.Y + (.2f * height)),
                (int)(width * (1f/6f)),
                (int)(height * .25f));
        }

        // Adds to the player's score according to the passed in enemy.
        public void addToScore(Enemy e)
        {
            score += e.getScore()*multiplier;
        }

        // The following few methods could probably be handled by some spiffy C# code but I haven't had time to look at it.
        public int getScore()
        {
            return score;
        }

        public int getLives()
        {
            return lives;
        }

        public int getMult()
        {
            return multiplier;
        }

        public Vector2 getPos()
        {
            return position;
        }

        // If player uses up all their lives, they can continue. The player character is reset in this method.
        public void reset()
        {
            lives = 3;
            score = 0;
            gaugeLevel = 0;
            respawning = true;
            alive = false;
            doneAnimating = false;
        }
    }
}
