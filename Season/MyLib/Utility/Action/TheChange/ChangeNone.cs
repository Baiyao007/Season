using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility.Action.TheChange
{
    public class ChangeNone : IChangeAble
    {
        public ChangeNone() { }

        private ChangeNone(ChangeNone other)
            : this()
        { }

        public IChangeAble Clone() {
            return new ChangeNone(this);
        }

        public void Change(ref Vector2 size, ref float alpha) { }

    }
}
