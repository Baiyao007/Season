using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MyLib.Device;
using Season.Def;
using System;
using MyLib.Utility;
using Season.Components.NormalComponents;
using Season.Utility;
using Microsoft.Xna.Framework.Graphics;
using Season.Components.UpdateComponents;

namespace Season.Components.DrawComponents
{
    class C_DrawRouteEffect : DrawComponent
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
        private GraphicsDevice graphicsDevice;
        private bool isSleep;

        private Vector2 size;

        public C_DrawRouteEffect(float alpha = 1, float depth = 16)
        {
            this.alpha = alpha;
            this.depth = depth;
            imageNames = new List<string>();
            imageEffects = new List<ImageEffect>();

            creatTimer = new Timer(0.1f);
            creatTimer.Dt = new Timer.timerDelegate(CreatEffectOne);
            creatNO = 0;
            size = Vector2.One;

            graphicsDevice = Renderer_2D.GetGraphicsDevice();
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

        public void SetSize(Vector2 size) { this.size = size; }

        private void CreatEffectOne() {
            int nowNo = 0;
            if (imageNames.Count > 1) {
                nowNo = creatNO;
                creatNO++;
                if (creatNO >= imageNames.Count) { creatNO -= imageNames.Count; }
            }

            float offset = rand.Next(randArea) + offsetY;

            ImageEffect effect = new ImageEffect(imageNames[nowNo], entity.transform.Position + new Vector2(0, offset), aliveSecond);
            effect.size = size;
            imageEffects.Add(effect);

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

            graphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            imageEffects.ForEach(i => {
                i.Update();

                Vector2 drawPosition = i.position + Camera2D.GetOffsetPosition();
                Vector3 drawP3 = new Vector3(drawPosition, 0);

                i.VertexUpdate(drawP3);
                graphicsDevice.SetVertexBuffer(i.vertexBuffer);
                foreach (EffectPass pass in i.effect.CurrentTechnique.Passes) {
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(
                        PrimitiveType.TriangleStrip,
                        i.vertexPositions, 0, 2
                    );
                }
            });
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