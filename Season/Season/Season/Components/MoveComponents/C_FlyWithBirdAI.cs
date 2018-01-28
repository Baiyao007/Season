using Microsoft.Xna.Framework;
using Season.Components.NormalComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.MoveComponents
{
    class C_FlyWithBirdAI : MoveComponent
    {
        private float startSpeed;
        private C_Switch3 childDirection;

        private float moveAreaLeft;
        private float moveAreaRight;

        public C_FlyWithBirdAI(float speed)
            : base(speed, Vector2.Zero)
        {
            startSpeed = speed;
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            childDirection = (C_Switch3)entity.GetNormalComponent("C_Switch3");

            if (childDirection.IsRight()) { entity.transform.Angle = 360; }
            else if (childDirection.IsLeft()) { entity.transform.Angle = 180; }
            else if (childDirection.IsNone()) { entity.transform.Angle = 360; }

            moveAreaLeft = entity.transform.Position.X;
            moveAreaRight = moveAreaLeft + 500;
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }

        protected override void UpdateMove()
        {
            base.UpdateMove();

            CheckDirection();
            CheckAngle();
            Move();
        }

        private void CheckAngle()
        {
            if (childDirection.IsRight()) { entity.transform.Angle = 360; }
            else if (childDirection.IsLeft()) { entity.transform.Angle = 180; }
        }

        private void CheckDirection()
        {
            if (entity.transform.Position.X <= moveAreaLeft) {
                childDirection.SetRight(false);
            }
            else if (entity.transform.Position.X >= moveAreaRight) {
                childDirection.SetLeft(false);
            }
        }

        private void Move()
        {
            if (childDirection.IsRight())
            {
                entity.transform.SetPositionX += startSpeed;
            }
            else if (childDirection.IsLeft())
            {
                entity.transform.SetPositionX -= startSpeed;
            }
        }

    }
}
