using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility.Action.TheChange
{
    public class ChangeToLucency : IChangeAble
    {
        private Timer timer;

        public ChangeToLucency(Timer timer) {
            this.timer = timer;
        }

        private ChangeToLucency(ChangeToLucency other)
            : this(other.timer.Clone())
        { }

        public IChangeAble Clone() {
            return new ChangeToLucency(this);
        }


        public void Change(ref Vector2 size, ref float alpha) {
            timer.Update();
            alpha = timer.Rate();   //透明になる
        }

    }
}
