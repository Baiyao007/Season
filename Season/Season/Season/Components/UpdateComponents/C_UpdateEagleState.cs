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
    class C_UpdateEagleState : UpdateComponent
    {
        private IState<Entity> nomalState;
        private GameDevice gameDevice;

        public C_UpdateEagleState(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            nomalState = new FlyState_Com_Eagle(gameDevice);
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            entity.RegisterComponent(new C_FlyWithEagleAI(Parameter.PlayerLimitSpeed));
            entity.RegisterComponent(new C_Collider_Circle("Eagle", new Vector2(0, -80), 80));
            entity.RegisterComponent(new C_EnemyState());
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
