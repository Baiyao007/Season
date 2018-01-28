using Microsoft.Xna.Framework;
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
        private Timer attackTimer;
        private float attackTime;

        public AttackState_Com_Player(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            inputState = gameDevice.GetInputState;
            attackTime = 2;
            attackTimer = new Timer(attackTime);
        }

        protected override void Initialize(Entity entity) { }

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState)
        {
            if (playerAnim == null) {
                playerAnim = (C_DrawAnimetion)entity.GetDrawComponent("C_DrawAnimetion");
                playerAnim.SetNowAnim("Attack");

                playerDirection = (C_Switch3)entity.GetNormalComponent("C_Switch3");

                if (playerDirection.IsRight()) {
                    ColliderComponent attackCollider = new C_Collider_Circle("PlayerAttack", new Vector2( 100, -110), 50);
                    entity.RegisterComponent(attackCollider);
                    attackCollider.Destroy(attackTime);
                }
                else if (playerDirection.IsLeft()) {
                    ColliderComponent attackCollider = new C_Collider_Circle("PlayerAttack", new Vector2(-100, -110), 50);
                    entity.RegisterComponent(attackCollider);
                    attackCollider.Destroy(attackTime);
                }

            }
            attackTimer.Update();


            if (attackTimer.IsTime)
            {
                nextState = new MoveState_Com_Player(gameDevice, 0);
                return eStateTrans.ToNext;
            }

            nextState = this;
            return eStateTrans.ToThis;
        }

        protected override void ExitAction(Entity entity) { }
    }
}
