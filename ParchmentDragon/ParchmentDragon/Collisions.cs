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

namespace ParchmentDragon
{
    static class Collisions
    {
        // Currently implementing basic rectangular collisions. May implement more complex collisions later. Basics first.

        public static void updateCollisions(PlayerCharacter pc, List<Enemy> enemyList, List<Food> foodList, FoodList foods, Rectangle fieldBound)
        {
            // Checks for fireballs hitting enemies.
            foreach (Enemy enemy in enemyList)
            {
                foreach (Bullet bullet in pc.getBullets())
                {
                    Rectangle bulletRect = bullet.getRect();
                    // If the fireball rectangle intersects the enemy rectangle hit the enemy.
                    if(bulletRect.Intersects(enemy.getRect()) &&
                        bullet.inPlay && enemy.alive)
                    {
                        enemy.Hit(pc, foods, foodList);
                        bullet.inPlay = false;
                    }
                }

                foreach (Bullet bullet in enemy.getBullets())
                {
                    Rectangle bulletRect = bullet.getRect();

                    // If an enemy hits the player character while the player is not respawning then you're dead. :(
                    if (bulletRect.Intersects(pc.getRect()) && bullet.inPlay && pc.alive && !pc.respawning)
                    {
                        pc.Hit();
                        bullet.inPlay = false;
                    }
                }
                
                // Don't run into enemies. You'll die.
                if(enemy.getRect().Intersects(pc.getRect()) && enemy.alive && pc.alive)
                {
                    enemy.Die();
                    pc.Hit();
                    pc.PlayEnemyDeath();
                }
            }

            // Check food collisions.
            foreach (Food food in foodList)
            {
                if (food.getRect().Intersects(pc.getRect()) & (pc.alive | pc.respawning))
                {
                    pc.eatFood(food);
                }
            }
        }

        public static void checkSpecialCollisions(PlayerCharacter pc, EBossS1 boss, Rectangle fieldBounds)
        {
            // This is a pretty hackey way to do this and I'll re-write this system in a bit but I'm in a hurry now because it's 2AM on Monday.
            foreach (ExplodingBullet b in boss.getExplodingBullets())
            {
                Rectangle bulletRect = b.getRect();

                // If the bullet is exploded, check the shards.
                if (b.isExploded())
                {
                    foreach (Bullet bullet in b.getShards())
                    {
                        Rectangle shardRect = bullet.getRect();
                        if (shardRect.Intersects(pc.getRect()) && bullet.inPlay && pc.alive && !pc.respawning)
                        {
                            pc.Hit();
                            bullet.inPlay = false;
                        }
                    }
                }
                // Otherwise check the main bullet and not the shards.
                else
                {
                    if (bulletRect.Intersects(pc.getRect()) && b.inPlay && pc.alive && !pc.respawning)
                    {
                        pc.Hit();
                        b.inPlay = false;
                    }
                }
            }
        }
    }
}
