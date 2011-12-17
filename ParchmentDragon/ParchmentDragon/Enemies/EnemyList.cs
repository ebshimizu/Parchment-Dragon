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
using ScrollingBackground;
using AnimatedSprite;

namespace ParchmentDragon.Enemies
{
    // This class contructs an array containing dummy enemies of every type in the game.
    // To use this, you will copy enemies from the array into the list of enemies for the stage
    // using the overriden copy methods provided in the Enemy classes.
    // See the enemies.txt file (which will be written soon) for a list of what number corresponds to what enemy.
    class EnemyList
    {
        private Enemy[] enemies;
        static int NUMBER_OF_ENEMIES = 10;

        public EnemyList(ContentManager content)
        {
            enemies = new Enemy[NUMBER_OF_ENEMIES];

            AnimatedTexture rockAlive = new AnimatedTexture(content.Load<Texture2D>("Enemies\\rock_alive"), 1, 1, Vector2.Zero, 0.0f, 1.0f, 0.2f);
            AnimatedTexture rockDead = new AnimatedTexture(content.Load<Texture2D>("Enemies\\rock_anim1"), 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f);
            Texture2D rockBullet = content.Load<Texture2D>("Bullets\\rock_bullet");
            ERock rock = new ERock(rockAlive, rockDead, rockBullet, 5, 10,
                .3f, Vector2.Zero, Vector2.Zero, 0, 0, false, false);

            AnimatedTexture lionAlive = new AnimatedTexture(content.Load<Texture2D>("Enemies\\lion_alive"), 1, 1, Vector2.Zero, 0.0f, .75f, 0.2f);
            AnimatedTexture lionDead = new AnimatedTexture(content.Load<Texture2D>("Enemies\\lion_death"), 35, 30, Vector2.Zero, 0.0f, .75f, 0.2f, 7);
            Texture2D lionBullet = content.Load<Texture2D>("Bullets\\lion_bullet");
 
            ELion lion = new ELion(lionAlive, lionDead, lionBullet, 10, 20,
                .2f, Vector2.Zero, Vector2.Zero, 0, 0, false, false);

            AnimatedTexture bflyAlive = new AnimatedTexture(content.Load<Texture2D>("Enemies\\butterflyfly"), 10, 30, Vector2.Zero, 0.0f, 1.5f, 0.2f, 5);
            AnimatedTexture bflyDead = new AnimatedTexture(content.Load<Texture2D>("Enemies\\butterflydeath"), 35, 20, Vector2.Zero, 0.0f, 1.5f, 0.2f, 7);
            Texture2D bflyBullet = content.Load<Texture2D>("Bullets\\bfly_bullet");

            EBfly bfly = new EBfly(bflyAlive, bflyDead, bflyBullet, 20, 20,
                .9f, Vector2.Zero, Vector2.Zero, 0, 0, false, false);

            AnimatedTexture mBossAlive = new AnimatedTexture(content.Load<Texture2D>("Enemies\\minibossfly"), 25, 30, Vector2.Zero, 0.0f, 1.0f, 0.75f, 5);
            AnimatedTexture mBossDead = new AnimatedTexture(content.Load<Texture2D>("Enemies\\minibossdeath"), 40, 30, Vector2.Zero, 0.0f, 1.0f, 0.75f, 8);
            Texture2D mBossBullet = content.Load<Texture2D>("Bullets\\fireball0001");

            EMiniboss miniboss1 = new EMiniboss(mBossAlive, mBossDead, mBossBullet, 100, 500,
                .6f, Vector2.Zero, Vector2.Zero, 0, 0, false, false);

            AnimatedTexture batAlive = new AnimatedTexture(content.Load<Texture2D>("Enemies\\batfly"), 10, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 5);
            AnimatedTexture batDead = new AnimatedTexture(content.Load<Texture2D>("Enemies\\batdeath"), 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 7);
            Texture2D batBullet = content.Load<Texture2D>("Bullets\\bat_bullet");

            EBat bat = new EBat(batAlive, batDead, batBullet, 15, 100,
                .6f, Vector2.Zero, Vector2.Zero, 0, 0, false, false);

            AnimatedTexture eyeAlive = new AnimatedTexture(content.Load<Texture2D>("Enemies\\iSpikefly"), 75, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 15);
            AnimatedTexture eyeDead = new AnimatedTexture(content.Load<Texture2D>("Enemies\\ispikedeath"), 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 7);
            Texture2D eyeBullet = content.Load<Texture2D>("Bullets\\eye_bullet");

            EEye eye = new EEye(eyeAlive, eyeDead, eyeBullet, 20, 15,
                1.0f, Vector2.Zero, Vector2.Zero, 0 , 0, false, false);

            AnimatedTexture boss1Alive = new AnimatedTexture(content.Load<Texture2D>("Enemies\\boss1fly"), 50, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 10);
            AnimatedTexture boss1Dead = new AnimatedTexture(content.Load<Texture2D>("Enemies\\boss1death"), 55, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 11);
            Texture2D bossBullet = content.Load<Texture2D>("Bullets\\fireball0001");

            EBossS1 stage1Boss = new EBossS1(boss1Alive, boss1Dead, bossBullet, 100, 2000,
                1.0f, Vector2.Zero, Vector2.Zero, 0, 0, false, false);

            enemies[0] = rock;
            enemies[1] = lion;
            enemies[2] = bfly;
            enemies[3] = eye;
            enemies[4] = miniboss1;
            enemies[5] = bat;
            enemies[6] = stage1Boss;
        }

        public Enemy getEnemy(int enemyNumber)
        {
            return enemies[enemyNumber];
        }
    }
}
