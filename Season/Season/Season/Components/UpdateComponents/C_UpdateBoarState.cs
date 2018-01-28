using Microsoft.Xna.Framework;
using MyLib.Device;
using Season.Components.ColliderComponents;
using Season.Components.MoveComponents;
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
    class C_UpdateBoarState : UpdateComponent
    {
        private IState<Entity> nomalState;
        private GameDevice gameDevice;

        private ColliderComponent collider;

        public C_UpdateBoarState(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            nomalState = new JumpState_Com_Boar(gameDevice);

            collider = new C_Collider_Circle("Boar", new Vector2(0, -80), 80);
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            UpdateComponent fallComp = new C_JumpWithBoarAI(Parameter.PlayerLimitSpeed, false);
            entity.RegisterComponent(fallComp);
            entity.RegisterComponent(collider);
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }

        public override void Update()
        {
            nomalState = nomalState.Update(entity);

        }

    }
}
