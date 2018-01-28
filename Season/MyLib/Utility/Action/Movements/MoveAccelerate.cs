//作成日：　2017.10.17
//作成者：　柏
//クラス内容：  移動更新用実装（加速度・減速度運動）
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using MyLib.Utility.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MyLib.Utility.Action.Movements
{
    public class MoveAccelerate : IMoveAble
    {
        private bool isAccelerate;

        public MoveAccelerate(bool isAccelerate)
        {
            this.isAccelerate = isAccelerate;
        }

        private MoveAccelerate(MoveAccelerate other)
                : this(other.isAccelerate)
        { }

        public IMoveAble Clone()
        {
            return new MoveAccelerate(this);
        }

        public void Move(ref Vector2 velocity, ref float speed)
        {
            if (isAccelerate)
            {
                speed *= 1.02f;
            }
            else {
                speed *= 0.98f;
            }
        }
    }
}
