using Microsoft.Xna.Framework;
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
    class FlyState_Com_Eagle : IState<Entity>
    {
        private GameDevice gameDevice;
        private C_FlyWithEagleAI flyComp;
        private ColliderComponent collider;
        private C_PlayerState playerState;

        public FlyState_Com_Eagle(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
        }

        protected override void Initialize(Entity entity)
        {
            flyComp = (C_FlyWithEagleAI)entity.GetUpdateComponent("C_FlyWithEagleAI");
            playerState = (C_PlayerState)EntityManager.FindWithName("Player")[0].GetNormalComponent("C_PlayerState");

            List<Entity> circleEntity = EntityManager.FindWithName("OverseeCircle");

            for (int i = 0; i < circleEntity.Count; i++) {
                if (circleEntity[i].GetParent() == entity) {
                    collider = circleEntity[i].GetColliderComponent("OverseeCircle");
                }
            }

        }

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState)
        {
            if (collider.IsThrough("Player") && !playerState.IsHidden)
            {
                Entity target = collider.GetOtherEntity("Player");
                Vector2 start = entity.transform.Position;
                Vector2 end = collider.GetOtherEntity("Player").transform.Position;

                collider.GetEntity().DeActive();
                nextState = new AttackState_Com_Eagle(gameDevice, start, end);
                return eStateTrans.ToNext;
            }

            nextState = this;
            return eStateTrans.ToThis;
        }

        protected override void ExitAction(Entity entity) {
            flyComp.DeActive();
        }
    }
}
