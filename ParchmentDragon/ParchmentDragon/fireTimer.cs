using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
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
    public delegate void FireBullet();

    class fireTimer
    {
        public TimeSpan interval;
        private TimeSpan elapsed;
        bool stopped;
        FireBullet fire;

        public fireTimer()
        {
            interval = new TimeSpan();
            stopped = true;
        }

        public fireTimer(TimeSpan interval, FireBullet fireFunc)
        {
            fire = fireFunc;
            this.interval = interval;
            stopped = true;
        }

        public void update(GameTime time)
        {
            if (!stopped)
            {
                elapsed += time.ElapsedGameTime;
                if (elapsed.CompareTo(interval) >= 0)
                {
                    fire();
                    elapsed = new TimeSpan();
                }
            }
        }

        public void Stop()
        {
            stopped = true;
        }

        public void Start()
        {
            stopped = false;
        }
    }
}
