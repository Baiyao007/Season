using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using MyLib.Utility;
using Season.Components;
using Season.Components.ColliderComponents;
using Season.Components.DrawComponents;
using Season.Components.MoveComponents;
using Season.Components.NormalComponents;
using Season.Def;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.States.Normal_Obj
{
    class AttackState_Com_Player : IState<Entity>
    {
        private GameDevice gameDevice;
        private InputState inputState;
        private C_DrawAnimetion playerAnim;
        private C_Switch3 playerDirection;
        private ColliderComponent attackCollider;
        private Timer attackTimer;
        private float attackTime;

        public AttackState_Com_Player(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            inputState = gameDevice.GetInputState;
            attackTime = 2;
            attackTimer = new Timer(attackTime);
        }

        protected override void Initialize(Entity entity) {
            playerAnim = (C_DrawAnimetion)entity.GetDrawComponent("C_DrawAnimetion");
            playerDirection = (C_Switch3)entity.GetNormalComponent("C_Switch3");

            Vector2 offset = Vector2.Zero;

            if (playerDirection.IsRight()) {
                offset = new Vector2(100, -110);
            }
            else if (playerDirection.IsLeft()) {
                offset = new Vector2(-100, -110);
            }
            attackCollider = new C_Collider_Circle("PlayerAttack", offset, 50);
            entity.RegisterComponent(attackCollider);
        }

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState)
        {
            if (inputState.IsDown(Keys.F)) {
                attackTimer.Update();
                playerAnim.SetNowAnim("Attack");

                if (attackTimer.IsTime) {
                    attackCollider.Destroy(0);
                    playerAnim.SetNowAnim("Idle");
                    nextState = new MoveState_Com_Player(gameDevice, 0);
                    return eStateTrans.ToNext;
                }
                nextState = this;
                return eStateTrans.ToThis;
            }
            else {
                attackCollider.Destroy(0);
                nextState = new MoveState_Com_Player(gameDevice, 0);
                return eStateTrans.ToNext;
            }
        }

        protected override void ExitAction(Entity entity) { }
    }
}
