using Microsoft.Xna.Framework;
using Season.Components.NormalComponents;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.MoveComponents
{
    class C_JumpWithSquirrelAI : MoveComponent
    {
        private float startSpeed;
        private float currentJumpPower;
        private C_Switch3 childDirection;
        private C_BezierPoint bezierPoint;
        private Entity player;
        private bool isLand;

        public C_JumpWithSquirrelAI(float speed, bool isJump = true)
            : base(speed, Vector2.Zero)
        {
            startSpeed = speed;
            if (isJump)
            {
                currentJumpPower = -25;
            }
            else {
                currentJumpPower = 0;
            }
            isLand = false;
        }

        protected override void UpdateMove()
        {
            base.UpdateMove();

            if (isLand) { return; }
            Jump();
        }


        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            player = EntityManager.FindWithTag("Player")[0];
            childDirection = (C_Switch3)entity.GetNormalComponent("C_Switch3");
            bezierPoint = (C_BezierPoint)entity.GetNormalComponent("C_BezierPoint");


            if (childDirection.IsRight()) { entity.transform.Angle = 330; }
            else if (childDirection.IsLeft()) { entity.transform.Angle = 210; }
            else if (childDirection.IsNone()) { entity.transform.Angle = 360; }

            JumpingMove();

        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }



        private void Jump()
        {
            JumpingMove();
            CheckLand();

            //速度更新
            currentJumpPower += 1.2f;
            speed *= 0.98f;
            if (speed <= 0.01f) { speed = 0; }

            //角度補間(空中姿勢調整)
            if (childDirection.IsRight())
            {
                entity.transform.Angle = entity.transform.Angle >= 360 ? 360 : entity.transform.Angle * 1.002f;
            }
            else if (childDirection.IsLeft())
            {
                entity.transform.Angle = entity.transform.Angle <= 180 ? 180 : entity.transform.Angle * 0.998f;
            }
        }

        //ジャンプ中移行チェック
        private void JumpingMove()
        {
            int direction = 0;

            if (childDirection.IsRight())
            {
                entity.transform.Angle = 360;
                childDirection.SetRight(true);
                direction = 1;
                speed = startSpeed;
            }


            else if (childDirection.IsLeft())
            {
                entity.transform.Angle = 180;
                childDirection.SetLeft(true);
                direction = -1;
                speed = startSpeed;
            }
            else {
                childDirection.SetNone();
            }

            //横移動後チェック
            entity.transform.Position += new Vector2((int)speed * direction, 0);

            //MovePointCheck
            bezierPoint.CheckJumpMove();
        }


        //チェックしながら落下処理
        private void CheckLand()
        {
            float powercut = 0;
            float cutPower = 3f;    //チェック間隔設定

            Vector2 testPosition = bezierPoint.GetNowPosition();

            //落下チェック
            while (currentJumpPower > 0)
            {
                if (currentJumpPower < cutPower) { cutPower = currentJumpPower; }
                entity.transform.Position += new Vector2(0, cutPower);
                currentJumpPower -= cutPower;
                powercut += cutPower;

                if (bezierPoint.IsEnd()) { continue; }
                if (testPosition == Vector2.Zero) { continue; }

                if (entity.transform.Position.Y >= testPosition.Y - 10 &&
                    entity.transform.Position.Y <= testPosition.Y + 5
                    )
                {
                    isLand = true;
                    break;
                }
            }
            entity.transform.Position += new Vector2(0, currentJumpPower);
            currentJumpPower += powercut;
        }

        public bool GetIsLand()
        {
            return isLand;
        }

    }

}
