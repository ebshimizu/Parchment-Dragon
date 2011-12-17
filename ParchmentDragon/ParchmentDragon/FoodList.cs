using System;
using System.Collections.Generic;
using System.Linq;
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
using ParchmentDragon.Stages;
using ParchmentDragon.Enemies;

namespace ParchmentDragon
{
    public class FoodList
    // Similar to EnemyList, this maintains a global list of food for use anywhere.
    {
        private Food[] foods;
        static int NUMBER_OF_FOODS = 9;
        Random random;

        public FoodList(ContentManager content)
        {
            foods = new Food[NUMBER_OF_FOODS];
            foods[0] = new Food(content.Load<Texture2D>("Food\\strawberry"), Vector2.Zero, .2f);
            foods[1] = new Food(content.Load<Texture2D>("Food\\cake"), Vector2.Zero, .2f);
            foods[2] = new Food(content.Load<Texture2D>("Food\\apple"), Vector2.Zero, .2f);
            foods[3] = new Food(content.Load<Texture2D>("Food\\burger"), Vector2.Zero, .2f);
            foods[4] = new Food(content.Load<Texture2D>("Food\\cherry"), Vector2.Zero, .2f);
            foods[5] = new Food(content.Load<Texture2D>("Food\\cupcake"), Vector2.Zero, .2f);
            foods[6] = new Food(content.Load<Texture2D>("Food\\drumstick"), Vector2.Zero, .2f);
            foods[7] = new Food(content.Load<Texture2D>("Food\\pear"), Vector2.Zero, .2f);
            foods[8] = new Food(content.Load<Texture2D>("Food\\steak"), Vector2.Zero, .2f);
            random = new Random();
        }

        public Food getFood(int foodNumber, Vector2 position, Vector2 velocity, float value)
        {
            return foods[foodNumber].copy(position, velocity, value);
        }

        public Food getRandomFood(Vector2 position, Vector2 velocity, float value)
        {
            int foodnum = random.Next(0, NUMBER_OF_FOODS);
            return foods[foodnum].copy(position, velocity, value);

        }
    }
}
