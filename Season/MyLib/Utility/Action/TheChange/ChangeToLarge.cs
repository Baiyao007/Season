using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility.Action.TheChange
{
    public class ChangeToLarge : IChangeAble
    {
        private Timer timer;
        private Vector2 startSize;

        public ChangeToLarge(Timer timer)
        {
            this.timer = timer;
            startSize = -Vector2.One;
        }

        private ChangeToLarge(ChangeToLarge other)
            : this(other.timer)
        { }

        public IChangeAble Clone()
        {
            return new ChangeToLarge(this);
        }


        public void Change(ref Vector2 size, ref float alpha)
        {
            if (startSize == -Vector2.One) { startSize = size; }

            timer.Update();
            size = startSize * (1 - timer.Rate());  //拡大する
        }

    }
}
