using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using MyLib.Utility;
using Season.Components.DrawComponents;
using Season.Components.NormalComponents;
using Season.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.MoveComponents
{
    enum eJumpType {
        Up = -20,
        Fall = 0,
        Down = 20,
    }

    class C_JumpWithController : MoveComponent
    {
        private InputState inputState;
        private float startSpeed;
        private float currentJumpPower;
        private C_Switch3 playerDirection;
        private C_BezierPoint bezierPoint;

        private C_DrawAnimetion animControl;
        private bool isLand;
        private int isWallDirection;
        private bool isWall;

        private ColliderComponent playerCollider;

        public C_JumpWithController(float speed, InputState inputState, eJumpType jumpType)
            : base(speed, Vector2.Zero)
        {
            this.inputState = inputState;
            startSpeed = speed;
            currentJumpPower += (int)jumpType;
            isLand = false;
            isWall = false;
            isWallDirection = 0;
        }

        protected override void UpdateMove() {
            base.UpdateMove();
            if (isLand) { return; }
            Jump();
        }


        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            playerDirection = (C_Switch3)entity.GetNormalComponent("C_Switch3");
            bezierPoint = (C_BezierPoint)entity.GetNormalComponent("C_BezierPoint");

            animControl = (C_DrawAnimetion)entity.GetDrawComponent("C_DrawAnimetion");
            animControl.SetNowAnim("Jump");

            if (playerDirection.IsRight()) { entity.transform.Angle = 330; }
            else if (playerDirection.IsLeft()) { entity.transform.Angle = 210; }
            else if (playerDirection.IsNone()) { entity.transform.Angle = 360; }

            entity.transform.SetPositionY = entity.transform.Position.Y + currentJumpPower;
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }

        private void Jump() {
            CheckSpeed();
            JumpingMove();
            CheckLand();

            //速度更新
            currentJumpPower += 1.2f;
            speed *= 0.98f;
            if (Math.Abs(speed) <= 0.05f) { SetStop(); }

            //角度補間(空中姿勢調整)
            if (playerDirection.IsRight()) {
                entity.transform.Angle = entity.transform.Angle >= 360 ? 360 : entity.transform.Angle * 1.003f;
            }
            else if (playerDirection.IsLeft()) {
                entity.transform.Angle = entity.transform.Angle <= 180 ? 180 : entity.transform.Angle * 0.995f;
            }
        }

        //ジャンプ中移行チェック
        private void JumpingMove() {
            UpdateIsWall();

            //横移動後
            entity.transform.Position += new Vector2((int)speed, 0);

            //MovePointCheck
            bezierPoint.CheckJumpMove();
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
            } 
        }

        private void CheckSpeed() {
            int direction = 0;
            if (inputState.IsDown(Keys.Right, Buttons.DPadRight)) {
                direction = 1;
            }
            else if (inputState.IsDown(Keys.Left, Buttons.DPadLeft)) {
                direction = -1;
            }
            speed = direction == 0 ? speed : startSpeed * direction;
            SetDirection(direction);
        }

        private void SetDirection(int direction) {
            if (direction > 0) {
                playerDirection.SetRight(true);
                Camera2D.TurnRight();
            }
            else if (direction < 0) {
                playerDirection.SetLeft(true);
                Camera2D.TurnLeft();
            }
            else {
                playerDirection.SetNone();
            }
        }

        private void SetStop() {
            speed = 0;
            playerDirection.SetNone();
        }

        private bool IsWall() {
            playerCollider = entity.GetColliderComponent("Player");
            if (playerCollider == null) { return false; }
            if (playerCollider.IsJostle("Wall")) {
                Vector2 wallPosition = playerCollider.GetOtherEntity("Wall").transform.Position;

                while (entity.transform.Angle < 0) { entity.transform.Angle += 360; }
                int angle = (int)(entity.transform.Angle / 90);

                float radian = MathHelper.ToRadians(entity.transform.Angle);
                Vector2 direction = new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian));

                if (angle % 4 == 0 || angle % 4 == 3) { //Right
                    Vector2 offset = Method.RightAngleMove(direction, playerCollider.offsetPosition.Length());
                    entity.transform.Position = new Vector2(wallPosition.X - (playerCollider.radius + offset.X) - 10, entity.transform.Position.Y);
                }
                else {  //Left
                    Vector2 offset = Method.RightAngleMove(direction, playerCollider.offsetPosition.Length());
                    entity.transform.Position = new Vector2(wallPosition.X + (playerCollider.radius - offset.X) + 10, entity.transform.Position.Y);
                }
                SetStop();
                return true;
            }
            return false;
        }


        //チェックしながら落下処理
        private void CheckLand() {
            float powercut = 0;
            float cutPower = 3f;    //チェック間隔設定

            Vector2 testPosition = bezierPoint.GetNowPosition();

            //落下チェック
            while (currentJumpPower > 0) {
                if (currentJumpPower < cutPower) { cutPower = currentJumpPower; }
                entity.transform.Position += new Vector2(0, cutPower);
                currentJumpPower -= cutPower;
                powercut += cutPower;

                if (bezierPoint.IsEnd()) { continue; }
                if (testPosition == Vector2.Zero) { continue; }

                if (entity.transform.Position.Y >= testPosition.Y - 10 &&
                    entity.transform.Position.Y <= testPosition.Y + 5)
                {
                    isLand = true;
                    animControl.SetNowAnim("Run");
                    break;
                }
            }
            entity.transform.Position += new Vector2(0, currentJumpPower);
            currentJumpPower += powercut;
        }

        public bool GetIsLand() { return isLand; }

    }

}
