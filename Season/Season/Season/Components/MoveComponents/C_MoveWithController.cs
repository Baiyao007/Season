//作成日：　2017.11.20
//作成者：　柏
//クラス内容：　Controllerで移動するComponent実装
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using MyLib.Utility;
using Season.Components.DrawComponents;
using Season.Components.NormalComponents;
using System;

namespace Season.Components.MoveComponents
{
    class C_MoveWithController : MoveComponent
    {
        private InputState inputState;
        private float startSpeed;
        private C_Switch3 playerDirection;
        private C_BezierPoint bezierPoint;
        private C_DrawAnimetion animControl;
        private C_CharaState state;
        private float speedSwith;
        private int isWallDirection;
        private bool isWall;

        private ColliderComponent playerCollider;

        public C_MoveWithController(float speed, InputState inputState)
            : base(speed, Vector2.Zero)
        {
            this.inputState = inputState;
            startSpeed = speed;
            speedSwith = startSpeed * 0.95f;
            isWall = false;
            isWallDirection = 0;
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            playerDirection = (C_Switch3)entity.GetNormalComponent("C_Switch3");
            bezierPoint = (C_BezierPoint)entity.GetNormalComponent("C_BezierPoint");
            state = (C_CharaState)entity.GetNormalComponent("C_CharaState");
            animControl = (C_DrawAnimetion)entity.GetDrawComponent("C_DrawAnimetion");
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }

        protected override void UpdateMove()
        {
            if (state.IsJump) { return; }
            base.UpdateMove();

            CheckSpeed();
            CheckAnim();
            Move();
        }

        private void CheckAnim() {
            if (speed < startSpeed * 0.009f) {
                SetStop();
                return;
            }
            if (speed <= speedSwith) { animControl.SetNowAnim("Walk"); }
            else{ animControl.SetNowAnim("Run"); }
        }

        private void SetStop() {
            speed = 0;
            animControl.SetNowAnim("Idle");
        }

        private void CheckSpeed() {
            int direction = 0;
            if (inputState.IsDown(Keys.Right, Buttons.DPadRight)) {
                direction = 1;
            }
            else if (inputState.IsDown(Keys.Left, Buttons.DPadLeft)) {
                direction = -1;
            }

            SetDirection(direction);
            speed += direction == 0 ? -startSpeed * 0.02f : startSpeed * 0.01f;
            Method.Clamp(0, startSpeed, ref speed);
        }

        private void SetDirection(int direction) {
            if (direction > 0) {
                playerDirection.SetRight(false);
                Camera2D.TurnRight();
            }
            else if (direction < 0) {
                playerDirection.SetLeft(false);
                Camera2D.TurnLeft();
            }
        }

        private void Move() {
            UpdateIsWall();

            if (playerDirection.IsRight()) { bezierPoint.ToRight((int)speed); }
            else { bezierPoint.ToLeft((int)speed); }

            if (bezierPoint.IsEnd()) {
                state.IsJump = true;
                return;
            }
            bezierPoint.Rotate();
            entity.transform.Position = bezierPoint.GetNowPosition();
        }


        private void UpdateIsWall() {
            if (!isWall) {
                isWall = IsWall();
                if (isWall) {
                    if (playerDirection.IsRight()) {
                        isWallDirection = 1;
                    }
                    else if (playerDirection.IsLeft()) {
                        isWallDirection = -1;
                    }
                }
            }
            if (isWall) {
                if (isWallDirection == 1 && playerDirection.IsRight()) { speed = 0; }
                else if (isWallDirection == -1 && playerDirection.IsLeft()) { speed = 0; }
                else { isWall = false; }
            }
        }


        private bool IsWall() {
            playerCollider = entity.GetColliderComponent("Player");
            if (playerCollider == null) { return false; }
            if (playerCollider.IsJostle("Wall")) {
                float offset = (playerCollider.centerPosition - playerCollider.GetOtherEntity("Wall").transform.Position).X;
                offset = playerCollider.radius - Math.Abs(offset);
                if (playerDirection.IsRight()) { bezierPoint.ToLeft((int)Math.Round(offset + 1)); }
                else { bezierPoint.ToRight((int)Math.Round(offset + 1)); }
                SetStop();
                return true;
            }
            return false;
        }
    }
}
