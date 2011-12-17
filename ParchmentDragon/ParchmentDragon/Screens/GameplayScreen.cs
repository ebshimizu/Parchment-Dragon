#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
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
using ScrollingBackground;
using ParchmentDragon;
using ParchmentDragon.Stages;
using ParchmentDragon.Enemies;

#endregion

namespace ParchmentDragon
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        protected ContentManager content;
        protected SpriteFont gameFont;
        protected Rectangle viewportRect;
        protected EnemyList enemies;
        public FoodList foods;
        public PlayerCharacter player1;
        protected Stage stage;
        // Rectangle to allow enemies to spawn off screen. This is a badly named rectangle. I'll rename it later.
        protected Rectangle enemyFieldBound;
        protected SoundEffect backgroundMusic;
        protected SoundEffectInstance instance;
        protected Texture2D healthBarBG;
        protected Texture2D healthBarFill;
        protected SpriteFont UIFont;
        protected String score;
        protected String mult;
        protected String lives;
        protected bool barAnimating = true;
        float percentHealth = 0.0f;
        protected Vector2 scoreLabelPos = new Vector2(615, 20);
        protected Vector2 livesLabelPos = new Vector2(615, 70);
        protected Vector2 scorePos = new Vector2(615, 35);
        protected Vector2 multPos = new Vector2(675, 550);
        protected Vector2 livesPos = new Vector2(615, 85);

        protected Random random = new Random();

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public GameplayScreen(PlayerCharacter pc, Stage stage, EnemyList eList, FoodList fList)
        {
            player1 = pc;
            this.stage = stage;
            enemies = eList;
            foods = fList;
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("gamefont");
            // Create a new SpriteBatch, which can be used to draw textures.
            viewportRect = new Rectangle(0, 0,
              (int)(this.ScreenManager.GraphicsDevice.Viewport.Width * .75f),
              this.ScreenManager.GraphicsDevice.Viewport.Height);
            enemyFieldBound = new Rectangle(
                -(int)(this.ScreenManager.GraphicsDevice.Viewport.Width * .25f),
                -(int)(this.ScreenManager.GraphicsDevice.Viewport.Height * .25f),
                (int)(this.ScreenManager.GraphicsDevice.Viewport.Width * 1.5f),
                (int)(this.ScreenManager.GraphicsDevice.Viewport.Height * 1.5f));

            if (enemies == null) enemies = new EnemyList(content);
            if (foods == null) foods = new FoodList(content);
            if (stage == null) stage = new Stage1(content, this.ScreenManager.GraphicsDevice, enemies);
            if (player1 == null)
            {
                player1 = new PlayerCharacter(
                content.Load<Texture2D>("PC\\player_flying"),
                content.Load<Texture2D>("PC\\player_death"),
                content.Load<Texture2D>("Bullets\\fireball0001"),
                content.Load<Texture2D>("Food\\foodgauge"),
                content.Load<Texture2D>("Food\\foodbar"),
                viewportRect);
                player1.LoadSounds(content);
            }
            healthBarBG = content.Load<Texture2D>("Enemies\\healthBar");
            healthBarFill = content.Load<Texture2D>("Enemies\\healthBarFill");
            UIFont = content.Load<SpriteFont>("UI");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);
            backgroundMusic = content.Load<SoundEffect>("Sounds\\backgroundmusic");

            instance = backgroundMusic.CreateInstance();
            instance.IsLooped = true;
            instance.Play();

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (IsActive)
            {
                if (stage.paused)
                {
                    stage.startTimers();
                }
                stage.addStageTime(gameTime.ElapsedGameTime);
                // Add timer restart code after pause.
                stage.updateBackground(2.3f);
                Collisions.updateCollisions(player1, stage.getEnemyList(), stage.getFoodList(), foods, viewportRect);
                player1.Update(gameTime);
                stage.updateList(gameTime);
                foreach (Enemy e in stage.getEnemyList())
                {
                    e.getPC(player1);
                    e.Update(gameTime, enemyFieldBound, viewportRect);
                }
                foreach (Food f in stage.getFoodList())
                {
                    f.Update(gameTime, enemyFieldBound, viewportRect);
                }
                if (player1.getLives() == 0 && player1.doneAnimating)
                {
                    ScreenManager.AddScreen(new GameOverScreen(player1), ControllingPlayer);
                    stage.stopTimers();
                }
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
                stage.stopTimers();
            }
            else
            {
                //Move player etc
            }
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.BackToFront, SaveStateMode.None);
            stage.drawBackground(spriteBatch);
            player1.Draw(spriteBatch);
            foreach (Enemy e in stage.getEnemyList())
            {
                e.Draw(spriteBatch);
                if (e.hasHealthBar) DrawHealthBar(spriteBatch, e);
            }
            foreach (Food f in stage.getFoodList())
            {
                f.Draw(spriteBatch);
            }
            score = String.Format("{0:d}", player1.getScore());
            lives = String.Format("{0:d}", player1.getLives());
            mult = "x" + String.Format("{0:d}", player1.getMult());
            Color multColor = Color.Black;
            if (player1.getMult() == 4) multColor = Color.Green;
            spriteBatch.DrawString(UIFont, "Score", scoreLabelPos, Color.Black, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.05f);
            spriteBatch.DrawString(UIFont, "Lives", livesLabelPos, Color.Black, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.05f);
            spriteBatch.DrawString(UIFont, score, scorePos, Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.05f);
            spriteBatch.DrawString(UIFont, lives, livesPos, Color.Black, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.05f);
            spriteBatch.DrawString(UIFont, mult, multPos, multColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.05f);
            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0)
                ScreenManager.FadeBackBufferToBlack(255 - TransitionAlpha);
        }

        public void DrawHealthBar(SpriteBatch spriteBatch, Enemy e)
        {
            spriteBatch.Draw(healthBarBG, new Vector2(-10, -10), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            if (barAnimating && e.alive)
            {
                if (percentHealth >= (float) e.getHealth() / (float) e.getMaxHealth())
                {
                    barAnimating = false;
//                    percentHealth = e.getHealth() / e.getMaxHealth();
                }
                else if(e.alive)
                {
                    percentHealth += 0.02f;
                }
            }
            else if (!barAnimating && e.alive)
            {
                percentHealth = (float) e.getHealth() / (float) e.getMaxHealth();               
            }
            else if (!e.alive)
            {
                barAnimating = true;
                percentHealth = 0.0f;
            }
            Rectangle bar = new Rectangle(75, 0, (int) (450*percentHealth), 150);
            spriteBatch.Draw(healthBarFill, new Vector2(65, -10), bar, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.05f);
        }

        #endregion
    }
}
