using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility.Action.TheChange
{
    public class ChangeLuceBig : IChangeAble
    {
        private Timer timer;
        private Vector2 startSize;

        public ChangeLuceBig(Timer timer)
        {
            this.timer = timer;
            startSize = -Vector2.One;
        }

        private ChangeLuceBig(ChangeLuceBig other)
            : this(other.timer.Clone())
        { }

        public IChangeAble Clone()
        {
            return new ChangeLuceBig(this);
        }


        public void Change(ref Vector2 size, ref float alpha)
        {
            if (startSize == -Vector2.One) { startSize = size / 2; }

            timer.Update();
            size = startSize * (1 - timer.InterpoRate());  //拡大する
            if (timer.Rate() < 0.5f) {
                size *= 2.0f;
                alpha *= 1.05f;
            }
            alpha =1 - timer.InterpoRate();   //透明になる
        }

    }
}
