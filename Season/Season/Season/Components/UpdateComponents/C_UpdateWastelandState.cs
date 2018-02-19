using Microsoft.Xna.Framework;
using MyLib.Utility;
using Season.Components.ColliderComponents;
using Season.Components.NormalComponents;
using Season.Entitys;
using Season.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.UpdateComponents
{
    class C_UpdateWastelandState : UpdateComponent
    {

        private List<C_IceRoute> routes;

        private C_Collider_PointInHintArea damageArea;
        private C_Collider_Square createRouteArea;

        private C_Collider_PointInHintArea stopRight;
        private C_Collider_PointInHintArea stopLeft;

        private int[] startPoint_LB;
        private int[] endPoint_LB;
        private int[] centrePoint_Lb;

        bool canMoveRight;
        bool canMoveLeft;

        private Timer damageTimer;

        public Vector2 startPosition;
        public Vector2 endPosition;


        public C_UpdateWastelandState(List<int> lb) {
            routes = new List<C_IceRoute>();
            startPoint_LB = new int[2] { lb[0], lb[1] };
            endPoint_LB = new int[2] { lb[0], lb[1] + 8 };
            centrePoint_Lb = new int[2] { lb[0], lb[1] + 4 };

            canMoveRight = false;
            canMoveLeft = false;

            damageTimer = new Timer(2);
        }

        private void CreatRoute(Vector2 startPosition, Vector2 endPosition) {
            C_IceRoute newRoute = new C_IceRoute(startPosition, endPosition);
            newRoute.Active();
            TaskManager.AddTask(newRoute);
            routes.Add(newRoute);
        }

        public override void Update() {
            routes.RemoveAll(r => !r.IsAtive);
            CheckCanMove();
            CheckDamage();
            CreatRoute();
        }

        private void CreatRoute() {
            if (createRouteArea.ThroughEnd("PlayerSkill")) {
                C_SeasonState season = (C_SeasonState)createRouteArea.GetOtherEntity("PlayerSkill").GetUpdateComponent("C_SeasonState");
                if (season.GetNowSeason() != eSeason.Winter) { return; }
                C_Collider_Circle playerSkillCollider = (C_Collider_Circle)createRouteArea.GetOtherCollider("PlayerSkill");
                Vector2 colliderCentre = playerSkillCollider.centerPosition;
                float radius = playerSkillCollider.radius;

                Vector2 lt = createRouteArea.points[0];
                Vector2 rb = createRouteArea.points[1];
                Vector2 rayNormal = Vector2.Normalize(rb - lt);

                Vector2 crossPoint1 = lt;
                Vector2 crossPoint2 = lt;

                Method.Circle_Ray(ref colliderCentre, lt, rayNormal, radius, ref crossPoint1, ref crossPoint2);

                crossPoint1 = crossPoint1.X > rb.X ? rb : crossPoint1;
                crossPoint2 = crossPoint2.X > rb.X ? rb : crossPoint2;

                //射線あたり判定からルート生成の始点と終点をゲット
                if (crossPoint1 == crossPoint2) { return; }
                CreatRoute(crossPoint1, crossPoint2);
            }
        }

        private void CheckCanMove() {
            canMoveRight = false;
            canMoveLeft = false;

            if (routes.Count > 0) {
                routes.ForEach(r => {
                    if (r.GetStartX() - startPosition.X < 50) {
                        canMoveRight = true;
                    }
                    if (endPosition.X - r.GetEndX() < 50) {
                        canMoveLeft = true;
                    }
                });
            }

            if (canMoveRight) {
                stopRight.SetSleep();
            }
            else {
                stopRight.Awake();
            }

            if (canMoveLeft) {
                stopLeft.SetSleep();
            }
            else {
                stopLeft.Awake();
            }

        }

        private void CheckDamage() {
            if (damageArea.IsThrough("Player")) {
                damageTimer.Update();
                if (damageTimer.IsTime)
                {
                    Entity player = damageArea.GetOtherEntity("Player");
                    ((C_Energy)player.GetNormalComponent("C_Energy")).Damage(1);
                    damageTimer.Initialize();
                }
            }
            if (damageArea.ThroughEnd("Player")) {
                damageTimer.Initialize();
            }
        }

        public bool IsInWasteland(int l, int b) {
            return true;
        }

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            //DamageAreaの設置
            startPosition = BezierStage.GetControllPointPosition(startPoint_LB[0], startPoint_LB[1] + 1);
            endPosition = BezierStage.GetControllPointPosition(startPoint_LB[0], startPoint_LB[1] + 7);
            Vector2 damageAreaRY = BezierStage.GetControllPointPosition(startPoint_LB[0], startPoint_LB[1] + 4);

            Vector2 damageAreaSize = new Vector2(
                endPosition.X - startPosition.X,
                damageAreaRY.Y - startPosition.Y);

            damageArea = new C_Collider_PointInHintArea("WastelandDamage", startPosition + damageAreaSize / 2, damageAreaSize);
            damageArea.Active();
            TaskManager.AddTask(damageArea);

            //Route生成用エリアの設置
            Vector2 createRouteAreaLT = BezierStage.GetControllPointPosition(startPoint_LB[0], startPoint_LB[1] + 1);
            Vector2 createRouteAreaRB = BezierStage.GetControllPointPosition(startPoint_LB[0], startPoint_LB[1] + 7);
            createRouteAreaLT.Y -= 50;
            Vector2 createRouteAreaSize = createRouteAreaRB - createRouteAreaLT;

            createRouteArea = new C_Collider_Square("CanCreateRoute", createRouteAreaLT + createRouteAreaSize / 2, createRouteAreaSize);
            createRouteArea.Active();
            TaskManager.AddTask(createRouteArea);


            //ChildControll用エリア設置
            C_Collider_PointInHintArea childJump1 = new C_Collider_PointInHintArea("ChildJump", startPosition,Vector2.One * 60);
            C_Collider_PointInHintArea childJump2 = new C_Collider_PointInHintArea("ChildJump", endPosition, Vector2.One * 60);
            childJump1.Active();
            childJump2.Active();
            TaskManager.AddTask(childJump1);
            TaskManager.AddTask(childJump2);

            stopRight = new C_Collider_PointInHintArea("ChildStopR", startPosition, Vector2.One * 100);
            stopLeft = new C_Collider_PointInHintArea("ChildStopL", endPosition, Vector2.One * 100);
            stopRight.Active();
            stopLeft.Active();
            TaskManager.AddTask(stopRight);
            TaskManager.AddTask(stopLeft);
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
            damageArea.DeActive();
            createRouteArea.DeActive();
        }
    }
}
