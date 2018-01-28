//作成日：　2017.10.17
//作成者：　柏
//クラス内容：  移動更新用実装（円周運動）
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
    public class MoveCircle : IMoveAble
    {
        private float radius;
        private bool isClockwise;
        public MoveCircle(float radius, bool isClockwise) {
            this.radius = radius;
            this.isClockwise = isClockwise;
        }

        private MoveCircle(MoveCircle other)
            : this(other.radius, other.isClockwise)
        { }

        public IMoveAble Clone()
        {
            return new MoveCircle(this);
        }

        public void Move(ref Vector2 velocity, ref float speed) {
            float radian = (float)Math.Atan2(velocity.Y, velocity.X);
            if (isClockwise) {
                radian -= MathHelper.ToRadians(1);
            } else {
                radian += MathHelper.ToRadians(1);
            }
            velocity = new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian)) * radius;
        }
    }
}