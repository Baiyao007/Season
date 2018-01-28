using Microsoft.Xna.Framework;
using MyLib.Device;
using MyLib.Utility;
using MyLib.Utility.Action.Movements;
using MyLib.Utility.Action.TheChange;
using Season.Components.ColliderComponents;
using Season.Components.DrawComponents;
using Season.Components.NormalComponents;
using Season.Entitys;
using Season.States;
using Season.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.UpdateComponents
{
    class C_UpdateShrubState : UpdateComponent
    {
        private GameDevice gameDevice;
        private C_SeasonState myState;
        private C_SeasonState otherState;
        private C_PlayerState playerState;

        private ColliderComponent collider;
        private C_DrawAnimetion drawComp;

        private List<string> effectNames1;
        private List<string> effectNames2;
        private Timer effectTimer;
        private eSeason nextSeason;
        private bool isPurify;

        private static Random rand = new Random();

        public C_UpdateShrubState(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            nextSeason = eSeason.Spring;

            effectNames1 = new List<string>() { "P_Sakura", "P_Leaf", "P_MapleLeafYellow", "P_SnowFlake" };
            effectNames2 = new List<string>() { "P_Sakura", "P_Rain", "P_MapleLeafRed", "P_Snow" };
            effectTimer = new Timer(1.5f);
            effectTimer.SetIsTime();

            isPurify = false;
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            myState = new C_SeasonState(eSeason.None);
            entity.RegisterComponent(myState);

            collider = new C_Collider_Circle("Shrub", new Vector2(0, -150), 150);
            entity.RegisterComponent(collider);

            drawComp = (C_DrawAnimetion)entity.GetDrawComponent("C_DrawAnimetion");
            playerState = (C_PlayerState)EntityManager.FindWithName("Player")[0].GetNormalComponent("C_PlayerState");
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }

        public override void Update()
        {
            Purify();
            SpringAction();

            if (effectTimer.IsTime) { return; }
            effectTimer.Update();
            CreatParticleEffect();
            if (effectTimer.IsTime) {
                myState.SetNowSeason(nextSeason);
                drawComp.SetNowAnim(myState.GetNowSeason().ToString());
                StartAutumn();
            }
        }

        private void SpringAction() {
            if (myState.GetNowSeason() != eSeason.Spring) { return; }
            HiddenOut();
            DoHidden();
        }

        private void DoHidden() {
            if (!collider.IsThrough("Player")) { return; }
            drawComp.SetNowAnim("Hidden");
            playerState.SetHidden(true);
        }

        private void HiddenOut() {
            if (!collider.ThroughEnd("PlayerHidden")) { return; }
            drawComp.SetNowAnim("Spring");
            playerState.SetHidden(false);
        }

        private void StartAutumn() {
            if (myState.GetNowSeason() != eSeason.Autumn) { return; }
            CreatApple();
        }

        private void CreatApple()
        {
            //実体生成
            Entity apple = Entity.CreateEntity("Apple", "Fruit", new Transform2D());

            //位置設定
            apple.transform.Position = entity.transform.Position - new Vector2(0, 100);

            apple.RegisterComponent(new C_Switch3());
            apple.RegisterComponent(new C_BezierPoint());
            apple.RegisterComponent(new C_Energy("", 10, 90));

            C_DrawAnimetion drawComp = new C_DrawAnimetion(new Vector2(38, 36), 100);
            drawComp.AddAnim("Idle", new AnimData(1, 1, 1, "A_Apple_Idle"));
            apple.RegisterComponent(drawComp);
            drawComp.SetNowAnim("Idle");

            apple.RegisterComponent(new C_UpdateFruitState(gameDevice));
        }


        private void Purify() {
            if (isPurify) { return; }
            if (!collider.ThroughStart("PlayerSkill")) { return; }
            if (myState.GetNowSeason() != eSeason.None) { return; }
            if (otherState == null) {
                otherState = (C_SeasonState)collider.GetOtherEntity("PlayerSkill").GetUpdateComponent("C_SeasonState");
            }

            isPurify = true;
            nextSeason = otherState.GetNowSeason();
            effectTimer.Initialize();
            drawComp.SetShaderOn(effectTimer.GetLimitTime() - 0.01f, "A_Shrub_"+ nextSeason, "ShrubMask");
            if (nextSeason == eSeason.Spring) { drawComp.SetDepth(18); }
        }


        private void CreatParticleEffect()
        {
            Vector2 creatPosition = entity.transform.Position;
            gameDevice.GetParticleGroup.AddParticles(
                effectNames1[(int)nextSeason],       //name
                2, 4,                               //count
                creatPosition + new Vector2(30, -150), 
                creatPosition + new Vector2(30, -150),       //position
                1, 1,               //speed
                0.4f, 0.8f,         //size
                0.6f, 0.8f,               //alpha
                0, 0,          //angle
                0.5f, 1.0f,         //alive
                new MoveEllipse(rand.Next(360),new Vector2(20, 5),false),  //moveType
                new ChangeToLucency(new Timer(0.5f))   //changeType
            );

            gameDevice.GetParticleGroup.AddParticles(
                effectNames2[(int)nextSeason],       //name
                2, 4,                               //count
                creatPosition + new Vector2(30, -150),
                creatPosition + new Vector2(30, -150),       //position
                1, 1,               //speed
                0.4f, 0.8f,         //size
                0.6f, 0.8f,               //alpha
                0, 0,          //angle
                0.5f, 1.0f,         //alive
                new MoveEllipse(rand.Next(360), new Vector2(20, 5), false),  //moveType
                new ChangeToLucency(new Timer(0.5f))   //changeType
            );
        }

    }
}
