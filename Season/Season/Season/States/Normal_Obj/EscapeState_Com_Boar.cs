using MyLib.Device;
using MyLib.Utility;
using Season.Components.MoveComponents;
using Season.Components.NormalComponents;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.States.Normal_Obj
{
    class EscapeState_Com_Boar : IState<Entity>
    {
        private GameDevice gameDevice;
        private Timer timer;
        private C_MoveWithBoarAI moveComp;
        private C_Switch3 direction;

        public EscapeState_Com_Boar(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            timer = new Timer(2);
        }

        protected override void Initialize(Entity entity) {
            moveComp = (C_MoveWithBoarAI)entity.GetUpdateComponent("C_MoveWithBoarAI");
            moveComp.SetEscape();

            direction = (C_Switch3)entity.GetNormalComponent("C_Switch3");
            direction.UTurn(false);
        }

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState) {
            timer.Update();

            entity.GetDrawComponent("C_DrawAnimetion").alpha = timer.Rate();
            if (timer.IsTime) { entity.DeActive(); }

            nextState = this;
            return eStateTrans.ToThis;
        }

        protected override void ExitAction(Entity entity) { }
    }
}
