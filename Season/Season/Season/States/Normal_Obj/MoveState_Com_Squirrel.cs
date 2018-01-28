using MyLib.Device;
using Season.Components;
using Season.Components.MoveComponents;
using Season.Components.NormalComponents;
using Season.Components.UpdateComponents;
using Season.Def;
using Season.Entitys;
using Season.States.Normal_Obj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.States
{
    class MoveState_Com_Squirrel : IState<Entity>
    {
        private GameDevice gameDevice;
        private C_MoveWithSquirrelAI moveComp;
        private ColliderComponent collider;
        private C_PlayerState playerState;
        private C_EntityDeadCheck deadCheck;

        public MoveState_Com_Squirrel(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
        }

        protected override void Initialize(Entity entity)
        {
            deadCheck = (C_EntityDeadCheck)entity.GetUpdateComponent("C_EntityDeadCheck");
            moveComp = (C_MoveWithSquirrelAI)entity.GetUpdateComponent("C_MoveWithSquirrelAI");
            playerState = (C_PlayerState)EntityManager.FindWithName("Player")[0].GetNormalComponent("C_PlayerState");
        }

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState)
        {
            if (moveComp.GetIsFall())
            {
                Console.WriteLine("Child Fall");
                entity.RemoveComponent(moveComp);
                UpdateComponent fallComp = new C_JumpWithSquirrelAI(Parameter.PlayerLimitSpeed);
                entity.RegisterComponent(fallComp);
                nextState = new JumpState_Com_Squirrel(gameDevice);
                return eStateTrans.ToNext;
            }


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
            if (deadCheck.IsDead()) { return true; }
            if (playerState.IsHidden) { return false; }
            collider = entity.GetColliderComponent("Squirrel");
            if (collider == null) { return false; }
            if (collider.IsThrough("Boar")) { 
                entity.RemoveComponent(moveComp);
                collider.DeActive();
                return true;

            }
            return false;
        }

        protected override void ExitAction(Entity entity) { }
    }
}
