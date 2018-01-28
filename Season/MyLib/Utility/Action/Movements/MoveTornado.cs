using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility.Action.Movements
{
    public class MoveTornado : IMoveAble
    {
        private bool isClockwise;
        private float radian;
        public MoveTornado(bool isClockwise)
        {
            this.isClockwise = isClockwise;
            radian = 0;
        }

        private MoveTornado(MoveTornado other)
            : this(other.isClockwise)
        { }

        public IMoveAble Clone()
        {
            return new MoveTornado(this);
        }

        public void Move(ref Vector2 velocity, ref float speed)
        {
            //半径操作
            speed *= 1.05f;

            //回る
            radian += MathHelper.ToRadians(30);
            if (radian >= Math.PI * 2) { radian -= (float)Math.PI * 2; }

            //方向リセット	楕円上昇
            velocity = new Vector2((float)Math.Cos(radian) * 8, (float)Math.Sin(radian));

            //上がっていく
            velocity -= new Vector2(0, 6.0f / speed);

            //透明度下がる
            //*m_Alpha *= 0.99f;
        }
    }
}