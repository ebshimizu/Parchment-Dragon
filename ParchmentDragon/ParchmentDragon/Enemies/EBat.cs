﻿using System;
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
    class EBat : Enemy
    {
        // This is enemy 5 in the enemy list. 
        // TODO: Override getRect() and assign proper scaling.
        public Vector2 velocity;

        public EBat(AnimatedTexture spriteA, AnimatedTexture spriteD, Texture2D loadedBullet,
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
            health = 5;
            maxHealth = health;
            time = new fireTimer(new TimeSpan(0,0,0,1,500), Fire);
            time.Start();
        }

        public override void Move()
        {
            position += velocity;
        }
        public override void Fire()
        {
            foreach (Bullet bullet in bullets)
            {
                if (!bullet.inPlay)
                {
                    bullet.velocity = new Vector2(0, 4f);
                    bullet.position = new Vector2(getCenter().X, getCenter().Y + (height / 2) - 20);
                    bullet.inPlay = true;
                    break;
                }
            }
        }


        // These copy methods are specific for each type of enemy.
        // They should be called on an enemy of a specific type (not the generic superclass).

        public override Enemy Copy()
        {
            // This returns an emeny with default settings. Good if you have a bunch of enemies
            // that don't really have different settings. Don't call this for the rock.
            Enemy enemyCopy = new EBat(
                new AnimatedTexture(aliveTex, 10, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 5),
                new AnimatedTexture(deadTex, 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 7),
                bulletText, maxBullets, scoreValue, 0.5f, Vector2.Zero, Vector2.Zero, 1, 1, false, false);
            return enemyCopy;
        }
        public override Enemy CopyLeft()
        {
            // Enemy copy method for left movement path
            Enemy enemyCopy = new EBat(
                new AnimatedTexture(aliveTex, 10, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 5),
                new AnimatedTexture(deadTex, 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 7),
                bulletText, maxBullets, scoreValue, 0.5f, new Vector2(-50f, -50f), new Vector2(2f, 2f), 1, 1, false, true);
            return enemyCopy;
        }

        public override Enemy CopyRight()
        {
            // Enemy copy method for right movement path
            Enemy enemyCopy = new EBat(
                new AnimatedTexture(aliveTex, 10, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 5),
                new AnimatedTexture(deadTex, 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 7),
                bulletText, maxBullets, scoreValue, 0.5f, new Vector2(650 - getWidth(), -50f), new Vector2(-2f, 2f), 1, 1, false, false);
            return enemyCopy;
        }

        public override Enemy Copy(Vector2 position, Vector2 velocity, int mpattern, int fpattern, bool foodDrop, bool left)
        {
            Enemy enemyCopy = new EBat(
                new AnimatedTexture(aliveTex, 10, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 5),
                new AnimatedTexture(deadTex, 35, 30, Vector2.Zero, 0.0f, 1.0f, 0.2f, 7),
                bulletText, maxBullets, scoreValue, 0.5f, position, velocity, mpattern, fpattern, foodDrop, left);
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

        public override Rectangle  getRect()
        {
            return new Rectangle(
                (int)(position.X + (width * .3f)),
                (int)(position.Y + (height * .4f)),
                (int)(width * .4f),
                (int)(height * .4f));
        }
    }
}