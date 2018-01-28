using Microsoft.Xna.Framework;
using MyLib.Device;
using MyLib.Utility;
using Season.Components;
using Season.Components.DrawComponents;
using Season.Components.MoveComponents;
using Season.Components.NormalComponents;
using Season.Components.UpdateComponents;
using Season.Def;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.States.Normal_Obj
{
    class AttackState_Com_Eagle : IState<Entity>
    {
        private GameDevice gameDevice;
        private C_DrawAnimetion eagleAnim;
        private C_Switch3 eagleDirection;
        private C_EnemyState eagleState;
        private C_EntityDeadCheck deadCheck;
        private Vector2 startPosition;
        private Vector2 targetPosition;
        private Vector2 unitVelocity;
        private float speed;
        private bool toTarget;

        public AttackState_Com_Eagle(GameDevice gameDevice, Vector2 startPosition, Vector2 targetPosition)
        {
            this.gameDevice = gameDevice;
            this.startPosition = startPosition;
            this.targetPosition = targetPosition;
            toTarget = true;
            unitVelocity = targetPosition - startPosition;
            speed = unitVelocity.Length() / 30;
            unitVelocity.Normalize();
        }

        protected override void Initialize(Entity entity)
        {
            deadCheck = (C_EntityDeadCheck)entity.GetUpdateComponent("C_EntityDeadCheck");
            eagleAnim = (C_DrawAnimetion)entity.GetDrawComponent("C_DrawAnimetion");
            eagleState = (C_EnemyState)entity.GetNormalComponent("C_EnemyState");
            eagleAnim.SetNowAnim("Attack");
            eagleDirection = (C_Switch3)entity.GetNormalComponent("C_Switch3");
        }

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState)
        {
            UpdateAttack(entity);
            if (CheckAttackEnd(entity)) {
                nextState = new FlyState_Com_Eagle(gameDevice);
                return eStateTrans.ToNext;
            }

            if (deadCheck.IsDead()) { entity.DeActive(); }

            nextState = this;
            return eStateTrans.ToThis;
        }

        private void UpdateAttack(Entity entity) {
            if (toTarget)
            {
                entity.transform.Position += unitVelocity * speed;
            }
            else {
                entity.transform.Position -= unitVelocity * speed / 3;
            }
            if (toTarget && entity.transform.Position.Y > targetPosition.Y)
            {
                toTarget = !toTarget;
                eagleAnim.SetNowAnim("Fly");
            }
        }
        private bool CheckAttackEnd(Entity entity) {
            return !eagleState.IsCaughtChild() && !toTarget && entity.transform.Position.Y < startPosition.Y;
        }

        protected override void ExitAction(Entity entity) {
            entity.RegisterComponent(new C_FlyWithEagleAI(Parameter.PlayerLimitSpeed));    
        }
    }
}
