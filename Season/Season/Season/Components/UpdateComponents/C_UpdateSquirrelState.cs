using Microsoft.Xna.Framework;
using MyLib.Device;
using Season.Components.ColliderComponents;
using Season.Components.MoveComponents;
using Season.Components.NormalComponents;
using Season.Def;
using Season.Entitys;
using Season.States;
using Season.States.Normal_Obj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.UpdateComponents
{
    class C_UpdateSquirrelState : UpdateComponent
    {
        private IState<Entity> nomalState;
        private GameDevice gameDevice;

        private ColliderComponent collider;
        private C_ChildState childState;
        private C_Switch3 childDirect;
        private C_Energy childHP;
        private C_PlayerState playerState;

        public C_UpdateSquirrelState(GameDevice gameDevice) {
            this.gameDevice = gameDevice;
            nomalState = new JumpState_Com_Squirrel(gameDevice);
        }

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            UpdateComponent fallComp = new C_JumpWithSquirrelAI(Parameter.PlayerLimitSpeed, false);
            entity.RegisterComponent(fallComp);
            
            collider = new C_Collider_Circle("Squirrel", new Vector2(0, -30), 50);
            entity.RegisterComponent(collider);

            childDirect = (C_Switch3)entity.GetNormalComponent("C_Switch3");
            childState = (C_ChildState)entity.GetNormalComponent("C_ChildState");
            childHP = (C_Energy)entity.GetNormalComponent("C_Energy");

            playerState = (C_PlayerState)TaskManager.GetNormalComponent(EntityManager.FindWithName("Player")[0], "C_PlayerState")[0];
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }

        public override void Update() {
            nomalState = nomalState.Update(entity);

            if (childState.FollowSwitch) { return; }
            if (collider.ThroughStart("PlayerSkill")) {
                childState.FollowSwitch = true;
                playerState.AddChild(entity);
            }
        }

    }
}
