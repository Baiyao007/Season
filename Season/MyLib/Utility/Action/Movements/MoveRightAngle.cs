//作成日：　2017.10.17
//作成者：　柏
//クラス内容：  移動更新用実装（直角曲がる）
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Utility.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility.Action.Movements
{
    public class MoveRightAngle : IMoveAble
    {
        private bool isRight;

        public MoveRightAngle(bool isRight)
        {
            this.isRight = isRight;
        }


        private MoveRightAngle(MoveRightAngle other)
            : this(other.isRight)
        { }

        public IMoveAble Clone()
        {
            return new MoveRightAngle(this);
        }

        public void Move(ref Vector2 velocity, ref float speed)
        {
            if (isRight)
            {
                velocity = Methord.RotateVector2(velocity, -90);
            }
            else {
                velocity = Methord.RotateVector2(velocity, 90);
            }
        }
    }
}
