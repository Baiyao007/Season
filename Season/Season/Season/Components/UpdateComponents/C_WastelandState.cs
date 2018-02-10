using Microsoft.Xna.Framework;
using Season.Components.NormalComponents;
using Season.Entitys;
using Season.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.UpdateComponents
{
    class C_WastelandState : UpdateComponent
    {
        public bool IsPurify { get; set; }
        public List<List<int>> moveAblePoints;
        private float targetY;
        private float startY;
        public Vector2 startPosition;
        public Vector2 endPosition;

        private C_PlayerState playerState;
        private ColliderComponent startCollider;
        private ColliderComponent endCollider;

        public C_WastelandState(List<int> lb) {
            IsPurify = false;
            moveAblePoints = new List<List<int>>() {
                new List<int>() { lb[0], lb[1] + 1 },
                new List<int>() { lb[0], lb[1] + 2 },
                new List<int>() { lb[0], lb[1] + 3 },
            };
        }


        public override void Update() {
            if (startCollider == null) { return; }
            if (startCollider.ThroughStart("PlayerSkill") || endCollider.ThroughStart("PlayerSkill"))
            {
                for (int i = 0; i < moveAblePoints.Count; i++)
                {
                    BezierStage.UpdateBezierPoint(Purify(), moveAblePoints[i]);
                }
            }
        }

        public float Purify() {
            IsPurify = true;
            return targetY;
        }
        public float Pollute() {
            IsPurify = false;
            return startY;
        }

        public void SetStartY(float y) { startY = y; }
        public void SetTargetY(float y) { targetY = y; }

        public bool IsInWasteland(int l, int b) {
            if (IsPurify) { return false; }
            bool isInL = moveAblePoints[0][0] == l;
            bool isInB = b >= moveAblePoints[0][1] && b <= moveAblePoints[2][1] + 1;
            return isInL && isInB;
        }

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            playerState = (C_PlayerState)EntityManager.FindWithName("Player")[0].GetNormalComponent("C_PlayerState");
            startCollider = entity.GetColliderComponent("WastelandStart");
            endCollider = entity.GetColliderComponent("WastelandEnd");
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }
    }
}
