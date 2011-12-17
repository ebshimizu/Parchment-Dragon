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

namespace ParchmentDragon.Stages
{
    abstract class Stage
    {
        protected List<Enemy> activeEnemyList;
        protected List<Food> activeFoodList;
        // Will need to figure out system for multiple backgrounds later.
        protected scrollingBackground stageBG;
        protected TimeSpan stageTime;
        public bool paused;
        public bool end;

        protected void addEnemy(Enemy e)
        {
            activeEnemyList.Add(e);
        }

        protected void addFood(Food f)
        {
            activeFoodList.Add(f);
        }

        public virtual void updateList(GameTime gameTime)
        {
            // This only cleans out enemies which are dead. Each stage subclass will add its own enemies to the list.
            activeEnemyList = activeEnemyList.FindAll(removeEnemy);
            activeFoodList = activeFoodList.FindAll(removeFood);
        }

        public List<Enemy> getEnemyList()
        {
            return activeEnemyList;
        }

        public List<Food> getFoodList()
        {
            return activeFoodList;
        }

        public void drawBackground(SpriteBatch spriteBatch)
        {
            stageBG.Draw(spriteBatch);
        }

        public void updateBackground(float rate)
        {
            stageBG.Update(rate);
        }

        private bool removeEnemy(Enemy e)
        {
            bool bulletsInPlay = false;
            foreach (Bullet bullet in e.getBullets())
            {
                if (bullet.inPlay) bulletsInPlay = true;
            }
            // If the enemy is alive or animating keep it in the list.
            return e.alive || e.animating || bulletsInPlay;
        }

        private bool removeFood(Food f)
        {
            return f.inPlay;
        }

        public void addStageTime(TimeSpan elapsedTime)
        {
            stageTime += elapsedTime;
        }

        public void stopTimers()
        {
            foreach (Enemy e in activeEnemyList)
            {
                e.stopTimer();
            }
            paused = true;
        }

        public void startTimers()
        {
            foreach (Enemy e in activeEnemyList)
            {
                if (e.alive) {
                    e.startTimer();
                }
            }
            paused = false;
        }
    }
}
