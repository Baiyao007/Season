using MyLib.Device;
using Season.Components;
using Season.Components.MoveComponents;
using Season.Components.UpdateComponents;
using Season.Def;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.States.Normal_Obj
{
    class MoveState_Com_Boar : IState<Entity>
    {
        private GameDevice gameDevice;
        private C_MoveWithBoarAI moveComp;
        private ColliderComponent collider;
        
        public MoveState_Com_Boar(GameDevice gameDevice) {
            this.gameDevice = gameDevice;
        }

        protected override void Initialize(Entity entity) {
            moveComp = (C_MoveWithBoarAI)entity.GetUpdateComponent("C_MoveWithBoarAI");
            collider = entity.GetColliderComponent("Boar");
        }

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState) {
            if (moveComp.GetIsFall()) {
                entity.RemoveComponent(moveComp);
                UpdateComponent fallComp = new C_JumpWithBoarAI(Parameter.PlayerLimitSpeed);
                entity.RegisterComponent(fallComp);
                nextState = new JumpState_Com_Boar(gameDevice);
                return eStateTrans.ToNext;
            }

            //Damage判定
            if (CollitionCheck(entity)) {
                nextState = new EscapeState_Com_Boar(gameDevice);
                return eStateTrans.ToNext;
            }

            nextState = this;
            return eStateTrans.ToThis;
        }

        private bool CollitionCheck(Entity entity) {
            if (collider == null) { return false; }
            if (collider.IsThrough("PlayerAttack")) {
                collider.DeActive();
                return true;
            }
            return false;
        }

        protected override void ExitAction(Entity entity) { }
    }
}
