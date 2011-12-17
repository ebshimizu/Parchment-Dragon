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

namespace ScrollingBackground
{
    class scrollingBackground
    {
        // Declare a bunch of vectores for position and related stuff.
        private Vector2 screenpos, origin, textureSize, sidebarOffset;
        // Background texture.
        private Texture2D bg;
        private Texture2D bgNext;
        private Texture2D plain;
        private Texture2D mountain;
        private Texture2D volcano;
        // Saves height of screen.
        private int screenHeight;
        //"switch" to make smooth transitions between background changes.
        private Texture2D changeSwitch;
        public void Load(GraphicsDevice device, Texture2D bg_plain, Texture2D bg_mountain, Texture2D bg_volcano)
        {
            // Stores each of the needed textures into a new object.  Sadly there doesn't seem to be any sort of better way 
            // to do this besides making new objects for each. 
            bg = bg_plain;
            bgNext = bg_plain;
            plain = bg_plain;
            mountain = bg_mountain;
            volcano = bg_volcano;
            changeSwitch = bg_plain;


            screenHeight = device.Viewport.Height;
            int screenWidth = device.Viewport.Width;
            // Set the origin so that we're drawing from the 
            // center of the top edge.
            origin = new Vector2(bg.Width / 2, 0);
            // Set the screen position to the center of the screen.
            screenpos = new Vector2(screenWidth / 2, screenHeight / 2);
            sidebarOffset = new Vector2((bg.Width * .75f)+origin.X, screenHeight / 2);
            // Offset to draw the second texture, when necessary.
            textureSize = new Vector2(0, bg.Height);
        }
        public void Update(float deltaY)
        {
            // Updates position of background.
            // First part is pretty self-explanatory.
            screenpos.Y += deltaY;
            sidebarOffset.Y += deltaY;
            // This part does some sneaky maths to figure out when the background should repeat.
            if (screenpos.Y >= bg.Height)
            {
                Texture2D temp = bg;

                if (changeSwitch != bg)
                {
                    bg = changeSwitch;
                }
                bgNext = temp;
                
            }
            screenpos.Y = screenpos.Y % bg.Height;
            sidebarOffset.Y = sidebarOffset.Y % bg.Height;
        }
        public void Draw(SpriteBatch batch)
        {
            Rectangle sourceRect = new Rectangle((int)(bg.Width * .75f), 0, (int)(bg.Width * .25f), bg.Height);

            if (screenpos.Y < screenHeight)
            {

                    batch.Draw(bgNext, screenpos, null, Color.White, 0, origin, 1, SpriteEffects.None, 1f);
                    batch.Draw(bgNext, sidebarOffset, sourceRect, Color.White, 0, origin, 1, SpriteEffects.None, 0.1f);

            }

                batch.Draw(bg, screenpos - textureSize, null, Color.White, 0, origin, 1, SpriteEffects.None, 1f);
                batch.Draw(bg, sidebarOffset - textureSize, sourceRect, Color.White, 0, origin, 1, SpriteEffects.None, 0.1f);
            
        }
        public void changeBackground(int transition)
        {
            if (transition == 1)
                changeSwitch = plain;
            if (transition == 2)
                changeSwitch = mountain;
            if (transition == 3)
                changeSwitch = volcano;
        }
    }
}