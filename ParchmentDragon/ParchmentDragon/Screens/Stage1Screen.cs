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
    class Stage1Screen : GameplayScreen
    {
        #region Fields

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public Stage1Screen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public Stage1Screen(PlayerCharacter pc, Stage stage, EnemyList eList, FoodList fList)
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
            base.LoadContent();
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
            Stage1 s = (Stage1) stage;
            if (s.getBoss() != null)
            {
                Collisions.checkSpecialCollisions(player1, s.getBoss(), viewportRect);
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (stage.end && IsActive)
            {
                // Temporary Test
                ScreenManager.AddScreen(new BackgroundScreen(), null);
                ScreenManager.AddScreen(new StageEndScreen(player1, true, 0), ControllingPlayer);
                stage.stopTimers();
            }
        }

        #endregion
    }
}
