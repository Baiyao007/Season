using Microsoft.Xna.Framework;
using MyLib.Utility;
using Season.Components.NormalComponents;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.MoveComponents
{
    class C_MoveWithBoarAI : MoveComponent
    {
        private float startSpeed;
        private C_Switch3 childDirection;
        private C_BezierPoint bezierPoint;
        private Entity player;
        private bool isFall;
        private bool isEscape;
        private bool isPatrol;
        private Timer patrolTimer;
        private bool isRight;

        public C_MoveWithBoarAI(float speed)
            : base(speed, Vector2.Zero)
        {
            startSpeed = speed;
            isFall = false;
            isEscape = false;
            isPatrol = true;
            isRight = true;
            patrolTimer = new Timer(3);
            patrolTimer.Dt = new Timer.timerDelegate(Patrol);
        }

        public void SetEscape() { isEscape = true; }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            childDirection = (C_Switch3)entity.GetNormalComponent("C_Switch3");
            bezierPoint = (C_BezierPoint)entity.GetNormalComponent("C_BezierPoint");

            childDirection.SetRight(false);

            if (childDirection.IsRight()) { entity.transform.Angle = 360; }
            else if (childDirection.IsLeft()) { entity.transform.Angle = 180; }
            else if (childDirection.IsNone()) { entity.transform.Angle = 360; }
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }

        protected override void UpdateMove()
        {
            if (isFall) { return; }
            base.UpdateMove();

            player = EntityManager.FindWithTag("Player")[0];
            CheckDirection();
            patrolTimer.Update();
            Move();
        }

        private void CheckDirection()
        {
            if (isEscape) { return; }
            ActionCheck();
        }

        private void ActionCheck() {
            if (player.transform.Position.X > entity.transform.Position.X + 50 &&
                Vector2.DistanceSquared(player.transform.Position, entity.transform.Position) < 800 * 800)
            {
                childDirection.SetRight(false);
                speed = startSpeed;
                isPatrol = false;
            }
            else if (player.transform.Position.X < entity.transform.Position.X - 50 &&
                Vector2.DistanceSquared(player.transform.Position, entity.transform.Position) < 800 * 800)
            {
                childDirection.SetLeft(false);
                speed = startSpeed;
                isPatrol = false;
            }
            else {
                //childDirection.SetNone();
                isPatrol = true;
            }
        }

        private void Patrol() {
            if (!isPatrol) { return; }
            isRight = !isRight;
            if (isRight) { childDirection.SetRight(false); }
            else { childDirection.SetLeft(false); }
        }


        private void Move() {
            if (childDirection.IsNone()) { }
            else if (childDirection.IsRight()) {
                if (CheckFall(true)) { return; }
            }
            else if (childDirection.IsLeft()) {
                if (CheckFall(false)) { return; }
            }
            entity.transform.Position = bezierPoint.GetNowPosition();
        }

        private bool CheckFall(bool isRight) {
            if (isRight) { bezierPoint.ToRight((int)speed); }
            else { bezierPoint.ToLeft((int)speed); }

            if (bezierPoint.IsEnd()) {
                isFall = true;
                return true;
            }
            bezierPoint.Rotate();
            return false;
        }


        public bool GetIsFall() { return isFall; }

    }
}
