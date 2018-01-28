using Microsoft.Xna.Framework;
using MyLib.Utility;
using Season.Components.ColliderComponents;
using Season.Components.DrawComponents;
using Season.Components.NormalComponents;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.MoveComponents
{
    class C_FlyWithEagleAI : MoveComponent
    {
        private float startSpeed;
        private C_Switch3 eagleDirection;

        private float moveAreaLeft;
        private float moveAreaRight;


        //監視エリア設定
        private Entity overseeEntity;
        private Vector2 overseeCentre;
        private Vector2 overseeOffset;      //Eagleからのoffset

        //監視円設定
        private C_Collider_Circle overseeCollider;
        private int overseeCircleRadius;

        //監視エリア中心からのoffset
        private Vector2 moveOffset;
        private float moveAngle;
        private float moveRadius;
        private bool toBig;
        private bool creatment;

        private C_DrawSpriteAutoSize drawOverseeArea;
        

        private Timer moveTimer;
        private static Random rand = new Random();

        public C_FlyWithEagleAI(float speed)
            : base(speed, Vector2.Zero)
        {
            startSpeed = speed;

            overseeCircleRadius = 100;
            overseeOffset = Vector2.One * 200;
            moveTimer = new Timer(rand.Next(3) + 2);
            moveTimer.Dt = new Timer.timerDelegate(TurnRound);

            moveAngle = 0;
            moveRadius = 0;
            toBig = true;
            creatment = true;


            overseeEntity = Entity.CreateEntity("OverseeCircle", "OverseeCircle", new Transform2D());


            //drawOverseeArea = new C_DrawSpriteAutoSize("CollisionArea", Vector2.Zero, 300);
            //drawOverseeArea.SetColor(Color.Red);
            //overseeEntity.RegisterComponent(drawOverseeArea);

            overseeCollider = new C_Collider_Circle("OverseeCircle", Vector2.Zero, overseeCircleRadius, eCollitionType.Through, false);
            overseeEntity.RegisterComponent(overseeCollider);
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            overseeEntity.SetParent(entity);

            eagleDirection = (C_Switch3)entity.GetNormalComponent("C_Switch3");

            moveAreaLeft = entity.transform.Position.X;
            moveAreaRight = moveAreaLeft + 500;

        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
            overseeEntity.DeActive();
        }

        protected override void UpdateMove()
        {   
            base.UpdateMove();

            overseeCentre = entity.transform.Position + overseeOffset;
            overseeEntity.transform.Position = overseeCentre;
            moveTimer.Update();
            CheckInOverseeArea();
            CheckAngle();
            Move();


            moveAngle++;
            if (moveAngle >= 360) { moveAngle -= 360; }


            if (moveRadius >= 200) {
                toBig = false;
            }
            else if(moveRadius <= 0) {
                toBig = true;
            }
            if (toBig) { moveRadius++; }
            else { moveRadius--; }


            if (overseeCircleRadius >= 150) {
                creatment = false;
            }
            else if (overseeCircleRadius <= 80) {
                creatment = true;
            }
            if (creatment) { overseeCircleRadius++; }
            else { overseeCircleRadius--; }

            moveOffset = new Vector2(
                (float)Math.Cos(MathHelper.ToRadians(moveAngle)), 
                (float)Math.Sin(MathHelper.ToRadians(moveAngle))) 
                * moveRadius;
            overseeCollider.centerPosition = overseeEntity.transform.Position + moveOffset;
            overseeCollider.radius = overseeCircleRadius;
        }

        private void CheckAngle() {
            if (eagleDirection.IsRight()) { entity.transform.Angle = 360; }
            else if (eagleDirection.IsLeft()) { entity.transform.Angle = 180; }
        }

        private void CheckInOverseeArea()
        {
            if (entity.transform.Position.X <= moveAreaLeft) {
                eagleDirection.SetRight(false);
                overseeOffset.X = Math.Abs(overseeOffset.X);
                moveTimer.Initialize();
            }
            else if (entity.transform.Position.X >= moveAreaRight) {
                eagleDirection.SetLeft(false);
                overseeOffset.X = -Math.Abs(overseeOffset.X);
                moveTimer.Initialize();
            }
        }

        private void TurnRound() {
            if (eagleDirection.IsRight()) {
                overseeOffset.X *= -1;
                eagleDirection.SetLeft(false);
            }
            else if (eagleDirection.IsLeft()) {
                overseeOffset.X *= -1;
                eagleDirection.SetRight(false);
            }
        }
        private void Move()
        {
            if (eagleDirection.IsRight()) {
                entity.transform.SetPositionX =entity.transform.Position.X + 3;
            }
            else if (eagleDirection.IsLeft()) {
                entity.transform.SetPositionX = entity.transform.Position.X - 3;
            }
        }

    }
}
