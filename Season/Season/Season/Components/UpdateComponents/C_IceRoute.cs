using Microsoft.Xna.Framework;
using MyLib.Utility;
using Season.Components.ColliderComponents;
using Season.Components.DrawComponents;
using Season.Def;
using Season.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.UpdateComponents
{
    class C_IceRoute : UpdateComponent
    {
        private Timer aliveTimer;
        private int bezierIndex;
        private Vector2 startPosition;
        private Vector2 endPosition;

        private C_Collider_PointInHintArea childRightStop;
        private C_Collider_PointInHintArea childLeftStop;
        private C_DrawIceRoute draw;

        bool canMoveRight;
        bool canMoveLeft;

        public C_IceRoute(Vector2 startPosition, Vector2 endPosition)
        {
            this.startPosition = startPosition;
            this.endPosition = endPosition;

            draw = new C_DrawIceRoute(startPosition, endPosition);

            childRightStop = new C_Collider_PointInHintArea("childRightStop", startPosition - Vector2.One * 20, new Vector2(60, 40));
            childLeftStop = new C_Collider_PointInHintArea("childLeftStop", endPosition - new Vector2(40, 20), new Vector2(60, 40));

            List<Vector2> route = new List<Vector2>() {
                startPosition,
                (startPosition + endPosition) / 2,
                endPosition,
            };
            bezierIndex = BezierStage.AddRoute(route);

            aliveTimer = Parameter.IceRouteTime;
            aliveTimer.Dt = new Timer.timerDelegate(DeActive);

            canMoveRight = false;
            canMoveLeft = false;
        }

        public bool CanMoveRight {
            get { return canMoveRight; }
            set {
                if (canMoveRight == value) { return; }
                canMoveRight = value;
                if (canMoveRight) {
                    childRightStop.DeActive();
                    childRightStop = null;
                }
                else {
                    childRightStop = new C_Collider_PointInHintArea("childRightStop", startPosition - Vector2.One * 20, new Vector2(60, 40));
                }
            }
        }

        public bool CanMoveLeft {
            get { return canMoveLeft; }
            set {
                if (canMoveLeft == value) { return; }
                canMoveLeft = value;
                if (canMoveLeft) {
                    childLeftStop.DeActive();
                    childLeftStop = null;
                }
                else {
                    childLeftStop = new C_Collider_PointInHintArea("childLeftStop", endPosition - new Vector2(40, 20), new Vector2(60, 40));
                }
            }
        }

        public override void Update() {
            aliveTimer.Update();





        }


        public Vector2 GetStartPositon() { return startPosition; }
        public Vector2 GetEndPositon() { return endPosition; }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            draw.Active();
            TaskManager.AddTask(draw);
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
            BezierStage.DeleteRoute(bezierIndex);
        }


    }
}
