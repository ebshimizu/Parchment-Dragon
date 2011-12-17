#region File Description
//-----------------------------------------------------------------------------
// AnimatedTexture.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AnimatedSprite
{
    public class AnimatedTexture
    {
        public int framecount;
        public Texture2D myTexture;
        private float TimePerFrame;
        public int Frame;
        private float TotalElapsed;
        private bool Paused;
        private int framesPerRow;
        public int rows;

        public float Rotation, Scale, Depth;
        public Vector2 Origin;
        public AnimatedTexture(Vector2 origin, float rotation, 
            float scale, float depth)
        {
            this.Origin = origin;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
        }

        public AnimatedTexture(Texture2D loadedTexture, int frameCount, int framesPerSecond,
            Vector2 origin, float rotation,
            float scale, float depth)
        {
            this.Origin = origin;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
            framecount = frameCount;
            framesPerRow = frameCount;
            rows = 1;       
            myTexture = loadedTexture;
            TimePerFrame = (float)1 / framesPerSecond;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
        }

        public AnimatedTexture(Texture2D loadedTexture, int frameCount, int framesPerSecond,
            Vector2 origin, float rotation,
            float scale, float depth, int framesRow)
        {
            this.Origin = origin;
            this.Rotation = rotation;
            this.Scale = scale;
            this.Depth = depth;
            framecount = frameCount;
            framesPerRow = framesRow;
            rows = framecount / framesPerRow;
            myTexture = loadedTexture;
            TimePerFrame = (float)1 / framesPerSecond;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
        }

        public void Load(ContentManager content, string asset, 
            int frameCount, int framesPerSec)
        {
            framecount = frameCount;
            myTexture = content.Load<Texture2D>(asset);
            TimePerFrame = (float)1 / framesPerSec;
            Frame = 0;
            TotalElapsed = 0;
            Paused = false;
        }

        // class AnimatedTexture
        public void UpdateFrame(float elapsed)
        {
            if (Paused)
                return;
            TotalElapsed += elapsed;
            if (TotalElapsed > TimePerFrame)
            {
                Frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                Frame = Frame % framecount;
                TotalElapsed -= TimePerFrame;
            }
        }

        // A bit cryptically titled, but this stops updating the frame when it's done playing once.
        // When this is done, it returns false, which indicates that the image is done animating.
        public bool UpdateFrameOnce(float elapsed)
        {
            if (Paused)
                return true;
            TotalElapsed += elapsed;
            if (TotalElapsed > TimePerFrame && Frame < framecount)
            {
                Frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                TotalElapsed -= TimePerFrame;
            }
            return Frame < framecount;
        }
        // class AnimatedTexture
        public void DrawFrame(SpriteBatch batch, Vector2 screenPos)
        {
            DrawFrame(batch, Frame, screenPos);
        }
        public void DrawFrame(SpriteBatch batch, int frame, Vector2 screenPos)
        {
            int FrameWidth = myTexture.Width / framesPerRow;
            int FrameHeight = myTexture.Height / rows;
            Rectangle sourcerect = new Rectangle(FrameWidth * (frame % framesPerRow), FrameHeight * (frame / framesPerRow),
                FrameWidth, FrameHeight);
            batch.Draw(myTexture, screenPos, sourcerect, Color.White,
                Rotation, Origin, Scale, SpriteEffects.None, Depth);
        }

        public bool IsPaused
        {
            get { return Paused; }
        }
        public void Reset()
        {
            Frame = 0;
            TotalElapsed = 0f;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
        public void Play()
        {
            Paused = false;
        }
        public void Pause()
        {
            Paused = true;
        }

    }
}
