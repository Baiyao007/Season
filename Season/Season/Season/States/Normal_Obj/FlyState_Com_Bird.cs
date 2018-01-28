using MyLib.Device;
using Season.Components;
using Season.Components.MoveComponents;
using Season.Components.NormalComponents;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.States.Normal_Obj
{
    class FlyState_Com_Bird : IState<Entity>
    {
        private GameDevice gameDevice;
        private C_FlyWithBirdAI moveComp;
        private ColliderComponent collider;
        private C_PlayerState playerState;

        public FlyState_Com_Bird(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
        }

        protected override void Initialize(Entity entity)
        {
            moveComp = (C_FlyWithBirdAI)entity.GetUpdateComponent("C_FlyWithBirdAI");
            playerState = (C_PlayerState)EntityManager.FindWithName("Player")[0].GetNormalComponent("C_PlayerState");
        }

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState)
        {
            //Damage判定
            if (CollitionCheck(entity))
            {
                nextState = new DeathState_Com("Bom", gameDevice);
                return eStateTrans.ToNext;
            }

            nextState = this;
            return eStateTrans.ToThis;
        }

        private bool CollitionCheck(Entity entity)
        {
            if (playerState.IsHidden) { return false; }

            collider = entity.GetColliderComponent("Bird");
            if (collider == null) { return false; }

            if (collider.IsThrough("Boar")) 
            {
                entity.RemoveComponent(moveComp);
                collider.DeActive();
                return true;
            }
            return false;
        }


        protected override void ExitAction(Entity entity) { }
    }
}
