using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MyLib.Device;
using MyLib.Utility;
using MyLib.Utility.Action.Movements;
using MyLib.Utility.Action.TheChange;
using Season.Components.ColliderComponents;
using Season.Components.MoveComponents;
using Season.Components.NormalComponents;
using Season.States.Normal_Obj;
using Season.Def;
using Season.Entitys;
using Season.States;
using Season.Components.DrawComponents;

namespace Season.Components.UpdateComponents
{
    class C_UpdatePlayerState : UpdateComponent
    {
        private IState<Entity> normalState;
        private GameDevice gameDevice;
        private InputState inputState;
        
        private C_SeasonState seasonState;
        private C_Energy energy;
        private ColliderComponent collider;

        private Timer effectTimer;
        private List<string> nameList1;
        private List<string> nameList2;


        public C_UpdatePlayerState(GameDevice gameDevice) {
            this.gameDevice = gameDevice;
            inputState = gameDevice.GetInputState;
            normalState = new JumpState_Com_Player(gameDevice, eJumpType.Fall, 0);

            effectTimer = new Timer(0.15f); //effectの生成間隔を設定
            effectTimer.Dt = new Timer.timerDelegate(CreatSeasonEffect);

            nameList1 = new List<string>() {
                "P_Sakura",
                "P_Leaf",
                "P_MapleLeafRed",
                "P_SnowFlake",
            };
            nameList2 = new List<string>() {
                "P_Sakura",
                "P_Rain",
                "P_MapleLeafYellow",
                "P_Snow",
            };
        }

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            seasonState = new C_SeasonState();
            entity.RegisterComponent(seasonState);
            entity.RegisterComponent(new C_DrawSeasonUI(seasonState));

            collider = new C_Collider_Circle("Player", new Vector2(0, -120), 80, eCollitionType.Jostle);
            entity.RegisterComponent(collider);

            energy = (C_Energy)entity.GetNormalComponent("C_Energy");
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }

        public override void Update() {
            normalState = normalState.Update(entity);
            effectTimer.Update();
        }


        private void CreatSeasonEffect() {
            if (entity == null) { return; }
            Vector2 creatPosition = entity.transform.Position - new Vector2(0, 180);
            int nowSeasonIndex = (int)seasonState.GetNowSeason();

            gameDevice.GetParticleGroup.AddParticles(
                nameList1[nowSeasonIndex],           //name
                1, 1,                               //count
                creatPosition - new Vector2(130, 200),
                creatPosition + new Vector2(270, 200),       //position
                0.6f, 1.5f,         //speed
                1.0f, 1.6f,         //size
                0.5f, 1.0f,         //alpha
                230, 250,           //angle
                2.0f, 2.0f,         //alive
                new MoveLine(),     //moveType
                new ChangeToLucency(new Timer(2))   //changeType
            );

            float speedMin = 1.5f;
            float speedMax = 2.0f;
            if (nowSeasonIndex == 1)
            {
                speedMin = 5.0f;
                speedMax = 7.0f;
            }
            gameDevice.GetParticleGroup.AddParticles(
                nameList2[nowSeasonIndex],           //name
                1, 1,                               //count
                creatPosition - new Vector2(130, 200),
                creatPosition + new Vector2(270, 200),       //position
                speedMin, speedMax,         //speed
                0.3f, 0.8f,         //size
                1, 1,               //alpha
                230, 250,           //angle
                2.0f, 2.0f,         //alive
                new MoveLine(),     //moveType
                new ChangeToLucency(new Timer(2))   //changeType
            );
        }

    }
}
