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
    class JumpState_Com_Squirrel : IState<Entity>
    {
        private GameDevice gameDevice;
        private C_JumpWithSquirrelAI jumpComp;
        private C_EntityDeadCheck deadCheck;

        public JumpState_Com_Squirrel(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
        }

        protected override void Initialize(Entity entity)
        {
            jumpComp = (C_JumpWithSquirrelAI)entity.GetUpdateComponent("C_JumpWithSquirrelAI");
            deadCheck = (C_EntityDeadCheck)entity.GetUpdateComponent("C_EntityDeadCheck");
        }

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState)
        {
            if (jumpComp.GetIsLand())
            {
                UpdateComponent moveComp = new C_MoveWithSquirrelAI(Parameter.PlayerLimitSpeed);
                entity.RemoveComponent(jumpComp);
                entity.RegisterComponent(moveComp);

                nextState = new MoveState_Com_Squirrel(gameDevice);
                return eStateTrans.ToNext;
            }
            if (deadCheck.IsDead())
            {
                entity.RemoveComponent(jumpComp);
                nextState = new DeathState_Com("Bom", gameDevice);
                return eStateTrans.ToNext;
            }
            nextState = this;
            return eStateTrans.ToThis;
        }

        protected override void ExitAction(Entity entity) { }
    }
}
