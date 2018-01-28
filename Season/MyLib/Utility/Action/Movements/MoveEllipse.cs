using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility.Action.Movements
{
    public class MoveEllipse : IMoveAble
    {
        private Vector2 radius;
        private bool isClockwise;
        private float startAngle;
        private float nowAngle;
        private Vector2 velo;
        private static Random rand = new Random();
        public MoveEllipse(float angle, Vector2 radius, bool isClockwise)
        {
            nowAngle = rand.Next(360);
            startAngle = angle;
            this.radius = radius;
            this.isClockwise = isClockwise;
            velo = Vector2.Zero;
        }

        private MoveEllipse(MoveEllipse other)
            : this(other.startAngle, other.radius, other.isClockwise)
        { }

        public IMoveAble Clone()
        {
            return new MoveEllipse(this);
        }

        public void Move(ref Vector2 velocity, ref float speed)
        {
            if (isClockwise) { nowAngle -= 10; }
            else { nowAngle += 10; }
            Methord.Warp(0, 360 - 1, ref nowAngle);
            float radian = MathHelper.ToRadians(nowAngle);

            velocity = new Vector2((float)Math.Cos(radian) * radius.X, (float)Math.Sin(radian) * radius.Y);
            velocity = Methord.RotateVector2(velocity, startAngle);
        }
    }
}