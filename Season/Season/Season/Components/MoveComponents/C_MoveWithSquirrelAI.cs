using Microsoft.Xna.Framework;
using Season.Components.DrawComponents;
using Season.Components.NormalComponents;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.MoveComponents
{
    class C_MoveWithSquirrelAI : MoveComponent
    {
        private float startSpeed;
        private C_Switch3 childDirection;
        private C_BezierPoint bezierPoint;
        private C_ChildState childState;
        private ColliderComponent collider;
        private Entity player;
        private C_Energy energy;
        private C_DrawAnimetion draw;
        private bool isJump;
        private float restExpend;
        private float moveExpend;
        private bool isCaught;

        public C_MoveWithSquirrelAI(float speed)
            : base(speed, Vector2.Zero)
        {
            startSpeed = speed;
            isJump = false;
            isCaught = false;
        }

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            player = EntityManager.FindWithTag("Player")[0];
            childDirection = (C_Switch3)entity.GetNormalComponent("C_Switch3");
            bezierPoint = (C_BezierPoint)entity.GetNormalComponent("C_BezierPoint");
            childState = (C_ChildState)entity.GetNormalComponent("C_ChildState");
            energy = (C_Energy)entity.GetNormalComponent("C_Energy");
            collider = entity.GetColliderComponent("Squirrel");
            draw = (C_DrawAnimetion)entity.GetDrawComponent("C_DrawAnimetion");
            restExpend = energy.GetLimitEnery() / 60 / 50;  //50秒で体力尽き
            moveExpend = energy.GetLimitEnery() / 60 / 20 + restExpend;


            if (childDirection.IsRight()) { entity.transform.Angle = 360; }
            else if (childDirection.IsLeft()) { entity.transform.Angle = 180; }
            else if (childDirection.IsNone()) { entity.transform.Angle = 360; }
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }

        protected override void UpdateMove() {
            if (collider.IsThrough("ChildJump")) { isJump = true; }
            if (isJump) { return; }
            isCaught = childState.IsBeCaught();
            if (isCaught) {
                Entity enemy = childState.GetEnemyCaughtMe();
                if (!enemy.GetIsActive()) {
                    entity.DeActive();
                    return;
                }
                C_Switch3 enemyDirection = (C_Switch3)enemy.GetNormalComponent("C_Switch3");
                Vector2 offset = enemyDirection.IsRight() ? new Vector2(-20, 0) : new Vector2(20, 0);
                entity.transform.Position = enemy.transform.Position + offset;
                return;
            }

            base.UpdateMove();

            if (childState == null) { return; }
            if (!childState.FollowSwitch) { return; }

            speed = startSpeed;     // - startSpeed *(1 - energy.GetRate())* 0.5f

            CheckDirection();
            Move();
        }

        private void CheckDirection()
        {
            if (player.transform.Position.X > entity.transform.Position.X + 200) {
                childDirection.SetNowRight(false);
            }
            else if (player.transform.Position.X < entity.transform.Position.X - 200) {
                childDirection.SetNowLeft(false);
            }
            else {
                childDirection.SetNone();
            }
            if (IsChildForward()) { childDirection.SetNone(); }
            CheckFruit();
        }

        private bool IsChildForward() {
            bool isCollide = collider.IsThrough("Squirrel");
            if (!isCollide) { return false; }
            Vector2 otherP = collider.GetOtherEntity("Squirrel").transform.Position;
            Vector2 thisP = entity.transform.Position;
            bool isForward = thisP.X.CompareTo(otherP.X) == -1 && childDirection.IsRight() ||
                             thisP.X.CompareTo(otherP.X) ==  1 && childDirection.IsLeft();
            return isCollide && isForward;
        }

        private void CheckFruit() {
            if (collider.ThroughEnd("Fruit")) {
                draw.SetNowAnim("Run");
                return;
            }
            if (collider.IsThrough("Fruit")) {
                childDirection.SetNone();
                draw.SetNowAnim("Eat");
            }
        }

        private void Move() {
            if (childDirection.IsNone()) { energy.Damage(restExpend); }   
            else if (childDirection.IsRight()) {
                if (collider.IsThrough("ChildStopR")) { return; }
                if (CheckFall(true)) { return; }
            }
            else if (childDirection.IsLeft()) {
                if (collider.IsThrough("ChildStopL")) { return; }
                if (CheckFall(false)) { return; }
            }
            energy.Damage(moveExpend);
            entity.transform.Position = bezierPoint.GetNowPosition();
        }

        private bool CheckFall(bool isRight) {
            if (isRight) { bezierPoint.ToRight((int)speed); }
            else { bezierPoint.ToLeft((int)speed); }

            if (bezierPoint.IsEnd()) {
                isJump = true;
                return true;
            }
            bezierPoint.Rotate();
            return false;
        }

        public bool GetIsJump() { return isJump; }

    }
}
