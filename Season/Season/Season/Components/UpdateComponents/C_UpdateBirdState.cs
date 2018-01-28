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
    class C_UpdateBirdState : UpdateComponent
    {
        private IState<Entity> nomalState;
        private GameDevice gameDevice;

        private ColliderComponent collider;
        private C_ChildState childState;

        public C_UpdateBirdState(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            nomalState = new FlyState_Com_Bird(gameDevice);
        }

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            UpdateComponent moveComp = new C_FlyWithBirdAI(3);
            entity.RegisterComponent(moveComp);

            collider = new C_Collider_Circle("Bird", new Vector2(0, -30), 30);
            entity.RegisterComponent(collider);

            childState = (C_ChildState)entity.GetNormalComponent("C_ChildState");
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }

        public override void Update() {
            nomalState = nomalState.Update(entity);

            if (childState.FollowSwitch) { return; }
            childState.FollowSwitch = collider.ThroughStart("PlayerSkill");
        }

    }
}
