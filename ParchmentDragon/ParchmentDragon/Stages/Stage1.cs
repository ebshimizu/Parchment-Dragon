using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using ScrollingBackground;
using AnimatedSprite;
using ParchmentDragon.Enemies;

namespace ParchmentDragon.Stages
{
    class Stage1 : Stage
    {
        // Use the copy method to put enemies into the list. It's easier than manually loading them. Trust me.
        EnemyList enemies;
        Enemy miniboss;
        Enemy boss;

        public Stage1(ContentManager content, GraphicsDevice graphics, EnemyList enemies)
        // In general, the initial data needed for the stage to function will be loaded in the constructor.

        {
            activeEnemyList = new List<Enemy>();
            activeFoodList = new List<Food>();
            this.enemies = enemies;
            scrollingBackground testBG = new scrollingBackground();
            testBG.Load(graphics, content.Load<Texture2D>("BG\\bg_plain"),content.Load<Texture2D>("BG\\bg_mountain"),content.Load<Texture2D>("BG\\bg_volcano_1"));
            stageBG = testBG;
            stageTime = new TimeSpan();
            paused = false;
        }
        
        public override void updateList(GameTime gameTime)
        {
            int msecs = stageTime.Milliseconds;
            int mins = stageTime.Minutes;
            int secs = stageTime.Seconds;
            if (mins == 0 && msecs == 0)
            {
                // normal wave 1
                if (3 <= secs && secs < 13)
                {
                    addEnemy(enemies.getEnemy(0).CopyLeft());
                    addEnemy(enemies.getEnemy(0).CopyRight());
                }

                // normal wave 2
                if (18 <= secs && secs < 28)
                {
                    addEnemy(enemies.getEnemy(1).CopyLeft());
                    addEnemy(enemies.getEnemy(1).CopyRight());
                    
                }

                // normal wave 3
                if (secs == 33) addEnemy(enemies.getEnemy(2).CopyLeft());
                if (33 <= secs && secs < 43)
                {
                    addEnemy(enemies.getEnemy(1).CopyLeft());
                    addEnemy(enemies.getEnemy(1).CopyRight());
                }

                // normal wave 4
                if (48 <= secs && secs < 58)
                {
                    addEnemy(enemies.getEnemy(3).CopyLeft());
                    addEnemy(enemies.getEnemy(3).CopyRight());
                }
            }
            if (mins == 1 && msecs == 0)
            {
                // mini boss
                if (secs == 3)
                {
                    miniboss = enemies.getEnemy(4).Copy();
                    addEnemy(miniboss);
                    stageBG.changeBackground(2);
                }
            }
            if (miniboss != null && !miniboss.alive && !miniboss.animating)
            {
                TimeSpan t = new TimeSpan(0, 0, 123 - (60*mins + secs));
                stageTime += t;
                miniboss = null;
            }
            if (mins == 2 && msecs == 0)
            {
                // normal wave 5
                if (secs == 8) addEnemy(enemies.getEnemy(2).CopyLeft());
                if (8 <= secs && secs < 18)
                {
                    addEnemy(enemies.getEnemy(3).CopyLeft());
                    addEnemy(enemies.getEnemy(3).CopyRight());
                }
                // normal wave 6
                if (secs == 23) addEnemy(enemies.getEnemy(2).CopyLeft());
                if (23 <= secs && secs < 33)
                {
                    addEnemy(enemies.getEnemy(5).CopyLeft());
                    addEnemy(enemies.getEnemy(5).CopyRight());
                }
                if (secs == 38)
                {
                    boss = enemies.getEnemy(6).Copy();
                    addEnemy(boss);
                    stageBG.changeBackground(3);
                }
            }
            if (boss != null && !boss.alive && !boss.animating)
            {
                end = true;
            }
            base.updateList(gameTime);
        }

        public EBossS1 getBoss() { return (EBossS1)boss; }
    }
}
