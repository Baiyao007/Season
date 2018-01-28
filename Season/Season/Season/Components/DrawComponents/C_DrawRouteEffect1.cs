using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MyLib.Device;
using Season.Def;
using System;
using MyLib.Utility;
using Season.Components.NormalComponents;
using Season.Utility;
using Season.Components.UpdateComponents;

namespace Season.Components.DrawComponents
{
    class C_DrawRouteEffect1 : DrawComponent
    {
        private List<string> imageNames;
        private List<ImageEffect> imageEffects;

        private C_SeasonState seasonState;
        
        private static Random rand = new Random();
        private Timer creatTimer;
        private int creatNO;
        private float offsetY;
        private int randArea;
        private float aliveSecond;

        private bool isSleep;

        public C_DrawRouteEffect1(float alpha = 1, float depth = 16)
        {
            this.alpha = alpha;
            this.depth = depth;
            imageNames = new List<string>();
            imageEffects = new List<ImageEffect>();

            creatTimer = new Timer(0.1f);
            creatTimer.Dt = new Timer.timerDelegate(CreatEffectOne);
            creatNO = 0;
        }

        public void RegisterImage(string imageName, int count) {
            imageNames.Clear();
            if (count == 1) {
                imageNames.Add(imageName);
            }
            else {
                for (int i = 0; i < count; i++) {
                    imageNames.Add(imageName + i);
                }
            }
        }

        public void SetState(float second, int randArea, float offsetY) {
            creatTimer.SetTimer(second);
            this.randArea = randArea;
            this.offsetY = offsetY;
        }

        public void SetAliveSecond(float aliveSecond) {
            this.aliveSecond = aliveSecond;
        }

        private void CreatEffectOne() {
            int nowNo = 0;
            if (imageNames.Count > 1) {
                nowNo = creatNO;
                creatNO++;
                if (creatNO >= imageNames.Count) { creatNO -= imageNames.Count; }
            }

            float offset = rand.Next(randArea) + offsetY;
            imageEffects.Add(new ImageEffect(imageNames[nowNo], entity.transform.Position + new Vector2(0, offset), aliveSecond));
            imageEffects.RemoveAll(i => i.isDisappear);
        }

        private void CreatEffect() {
            if (isSleep) { return; }
            if (imageNames.Count == 0) { return; }
            if (seasonState == null) {
                seasonState = (C_SeasonState)entity.GetUpdateComponent("C_SeasonState");
            }
            if (seasonState.GetNowSeason() == eSeason.Spring) { return; }
            if (seasonState.GetNowSeason() == eSeason.Autumn) { return; }

            creatTimer.Update();
        }

        public void Sleep() { isSleep = true; }
        public void Awake() { isSleep = false; }

        public override void Draw()
        {
            CreatEffect();
            Renderer_2D.Begin(Camera2D.GetTransform());
            imageEffects.ForEach(i => {
                i.Update();
                Renderer_2D.DrawTexture(i.name, i.position, i.alpha, i.rect, Vector2.One, 0, i.imageSize / 2);
            });
            Renderer_2D.End();
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

            imageNames.Clear();
            imageEffects.Clear();
        }



    }
}