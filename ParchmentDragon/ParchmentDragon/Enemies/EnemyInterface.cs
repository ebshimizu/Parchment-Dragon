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
    interface EnemyInterface
    {
        // Basic enemy functions. Collisions handled in a separate class.
        void Move();
//        void Fire(object o, System.Timers.ElapsedEventArgs e);
        void Fire();
        void Hit(PlayerCharacter pc, FoodList foods, List<Food> foodList);
        void Draw(SpriteBatch spriteBatch);
        void Update(GameTime gameTime, Rectangle enemyFieldBound, Rectangle fieldBound);
        Enemy Copy();
        Enemy Copy(Vector2 position, Vector2 velocity, int mpattern, int fpattern, bool foodDrop, bool left);
    }
}
