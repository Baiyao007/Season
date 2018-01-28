using Microsoft.Xna.Framework.Input;
using MyLib;
using MyLib.Device;
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
    class SkillSelect_Com : IState<Entity>
    {
        private GameDevice gameDevice;
        private InputState inputState;

        private C_SeasonState seasonState;
        private C_DrawRouteEffect routeEffect;

        public SkillSelect_Com(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            inputState = gameDevice.GetInputState;
        }

        protected override void Initialize(Entity entity) {
            seasonState = (C_SeasonState)entity.GetUpdateComponent("C_SeasonState");
            routeEffect = (C_DrawRouteEffect)entity.GetDrawComponent("C_DrawRouteEffect");
        }

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState)
        {
            if (inputState.WasDown(Keys.Z, Buttons.LeftShoulder))
            {
                Console.WriteLine("Do Move");
                SetEffect();
                nextState = new MoveState_Com_Player(gameDevice, 0);
                return eStateTrans.ToNext;
            }

            if (inputState.WasDown(InputParameter.LeftKey, InputParameter.LeftButton)) {
                seasonState.ToBeforeSeason();
            }

            if (inputState.WasDown(InputParameter.RightKey, InputParameter.RightButton)) {
                seasonState.ToNextSeason();
            }

            nextState = this;
            return eStateTrans.ToThis;
        }

        private void SetEffect() {
            switch (seasonState.GetNowSeason()) {
                case eSeason.Summer:
                    routeEffect.RegisterImage("P_Flower_", 4);
                    routeEffect.SetState(0.1f, 10, -65);
                    routeEffect.SetAliveSecond(2);
                    break;
                case eSeason.Winter:
                    routeEffect.RegisterImage("P_Ice_", 5);
                    routeEffect.SetState(0.08f, 0, -20);
                    routeEffect.SetAliveSecond(3);
                    break;
            }
        }

        protected override void ExitAction(Entity entity) { }
    }
}
