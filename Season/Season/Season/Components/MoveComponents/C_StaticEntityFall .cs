using Microsoft.Xna.Framework;
using Season.Components.NormalComponents;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.MoveComponents
{
    class C_StaticEntityFall : MoveComponent
    {
        private float currentJumpPower;
        private C_BezierPoint bezierPoint;
        private bool isLand;

        public C_StaticEntityFall()
            : base(0, Vector2.Zero)
        {
            currentJumpPower = 0;
            isLand = false;
        }

        protected override void UpdateMove() {
            base.UpdateMove();

            if (isLand) { return; }
            Fall();
        }


        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            bezierPoint = (C_BezierPoint)entity.GetNormalComponent("C_BezierPoint");
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }



        private void Fall() {
            bezierPoint.CheckJumpMove();
            CheckLand();

            //速度更新
            currentJumpPower += 1.2f;
        }

        //チェックしながら落下処理
        private void CheckLand() {
            float powercut = 0;
            float cutPower = 5f;    //チェック間隔設定

            Vector2 testPosition = bezierPoint.GetNowPosition();

            //落下チェック
            while (currentJumpPower > 0) {
                if (currentJumpPower < cutPower) { cutPower = currentJumpPower; }
                entity.transform.Position += new Vector2(0, cutPower);
                currentJumpPower -= cutPower;
                powercut += cutPower;

                if (bezierPoint.IsEnd()) { continue; }
                if (testPosition == Vector2.Zero) { continue; }

                if (entity.transform.Position.Y >= testPosition.Y &&
                    entity.transform.Position.Y <= testPosition.Y + 10)
                {
                    isLand = true;
                    break;
                }
            }
            entity.transform.Position += new Vector2(0, currentJumpPower);
            currentJumpPower += powercut;
        }

        public bool GetIsLand() { return isLand; }

    }

}
