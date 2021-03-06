﻿using Microsoft.Xna.Framework;
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

            List<Vector2> route = new List<Vector2>() {
                startPosition,
                (startPosition + endPosition) / 2,
                endPosition,
            };
            bezierIndex = BezierStage.AddRoute(route);

            aliveTimer = new Timer(Parameter.IceRouteTime);
            aliveTimer.Dt = new Timer.timerDelegate(DeActive);

            canMoveRight = false;
            canMoveLeft = false;
        }

        public float GetStartX() { return startPosition.X; }
        public float GetEndX() { return endPosition.X; }

        public bool CanMoveRight {
            get { return canMoveRight; }
            set {
                if (canMoveRight == value) { return; }
                canMoveRight = value;
                if (canMoveRight) {
                    childRightStop.SetSleep();
                }
                else {
                    childRightStop.Awake();
                }
            }
        }

        public bool CanMoveLeft {
            get { return canMoveLeft; }
            set {
                if (canMoveLeft == value) { return; }
                canMoveLeft = value;
                if (canMoveLeft) {
                    childLeftStop.SetSleep();
                }
                else {
                    childLeftStop.Awake();
                }
            }
        }

        public override void Update() {
            aliveTimer.Update();
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            draw = new C_DrawIceRoute(startPosition, endPosition);
            childRightStop = new C_Collider_PointInHintArea("childRightStop", endPosition - Vector2.One * 20, Vector2.One * 100);
            childLeftStop = new C_Collider_PointInHintArea("childLeftStop", startPosition - new Vector2(-20, 20), Vector2.One * 100);

            draw.Active();
            childRightStop.Active();
            childLeftStop.Active();

            TaskManager.AddTask(draw);
            TaskManager.AddTask(childRightStop);
            TaskManager.AddTask(childLeftStop);
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
            BezierStage.DeleteRoute(bezierIndex);
            childRightStop.DeActive();
            childLeftStop.DeActive();
        }


    }
}
