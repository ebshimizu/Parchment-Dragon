#region File Description
//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
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
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class StageEndScreen : MenuScreen
    {
        #region Initialization
        PlayerCharacter pc;
        bool isFinalStage;
        int nextstage;

        /// <summary>
        /// Constructor.
        /// </summary>
        public StageEndScreen(PlayerCharacter pc, bool final, int nextstage)
            : base("Game Clear")
        {
            isFinalStage = final;
            this.nextstage = nextstage;

            // Create our menu entries.
            if(!isFinalStage) {
                MenuEntry nextStage = new MenuEntry("Next Stage");
                nextStage.Selected += NextStageSelected;
                MenuEntries.Add(nextStage);
            }
            else {
                MenuEntry playAgainYes = new MenuEntry("Yes");
                MenuEntry playAgainNo = new MenuEntry("No");
                playAgainYes.Selected += BackToGameSelected;
                playAgainNo.Selected += BackToMenuSelected;
                MenuEntries.Add(playAgainYes);
                MenuEntries.Add(playAgainNo);
            }
            this.pc = pc;
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the do not continue item selected
        /// </summary>
        void BackToMenuSelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen());
        }

        void BackToGameSelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new Stage1Screen());
        }

        void NextStageSelected(object sender, PlayerIndexEventArgs e)
        {
            // What will happen here is that we'll load the next numbered stage screen with a switch case statement or something.
            // LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new Stage1Screen());
        }

        #endregion

        #region Draw


        /// <summary>
        /// Draws the pause menu screen. This darkens down the gameplay screen
        /// that is underneath us, and then chains to the base MenuScreen.Draw.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            Vector2 position = new Vector2(400, 450);
            Vector2 scorePos = new Vector2(400, 200);
            Vector2 contPos = new Vector2(400, 375);
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            if (ScreenState == ScreenState.TransitionOn)
            {
                position.X -= transitionOffset * 256;
                scorePos.X -= transitionOffset * 256;
                contPos.X -= transitionOffset * 256;
            }
            else
            {
                position.X += transitionOffset * 512;
                scorePos.X += transitionOffset * 512;
                contPos.X += transitionOffset * 512;
            }

            spriteBatch.Begin();

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.DrawCenter(this, position, isSelected, gameTime);

                position.Y += menuEntry.GetHeight(this);
            }

            // Draw the menu title.
            Vector2 titlePosition = new Vector2(400, 80);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(0, 0, 0, TransitionAlpha);
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;
            String title;

            if (!isFinalStage)
            {
                title = "Stage " + String.Format("{0:d}", nextstage - 1);
            }
            else
            {
                title = menuTitle;
            }

            spriteBatch.DrawString(font, title, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            String score = "Score: " + String.Format("{0:d}", pc.getScore());
            Vector2 scoreOrigin = font.MeasureString(score) / 2;
            spriteBatch.DrawString(font, score, scorePos, Color.Black, 0, scoreOrigin, 1, SpriteEffects.None, 0);

            if (isFinalStage)
            {
                String cont = "Play Again?";
                Vector2 contOrigin = font.MeasureString(cont) / 2;
                spriteBatch.DrawString(font, cont, contPos, Color.Black, 0, contOrigin, 1, SpriteEffects.None, 0);
                spriteBatch.End();
            }
        }

        #endregion
    }
}
