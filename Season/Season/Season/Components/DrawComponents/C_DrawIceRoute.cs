﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyLib.Device;
using MyLib.Utility;
using Season.Components.UpdateComponents;
using Season.Def;
using Season.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawIceRoute : DrawComponent
    {
        private List<string> imageNames;
        private List<ImageEffect> imageEffects;

        private static Random rand = new Random();
        private Timer creatTimer;
        private int creatNO;
        private float offsetY;
        private int randArea;
        private float aliveSecond;
        private GraphicsDevice graphicsDevice;

        private Vector2 startPosition;
        private Vector2 endPosition;

        public C_DrawIceRoute(Vector2 startPosition, Vector2 endPosition, float alpha = 1, float depth = 16)
        {
            this.alpha = alpha;
            this.depth = depth;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            imageNames = new List<string>();
            imageEffects = new List<ImageEffect>();

            creatTimer = new Timer(0.1f);
            creatTimer.Dt = new Timer.timerDelegate(CreatEffectOne);
            creatNO = 0;
            aliveSecond = Parameter.IceRouteTime;

            graphicsDevice = Renderer_2D.GetGraphicsDevice();

            RegisterImage("P_Ice_", 5);
            SetState(0.08f, 0, -20);
        }

        public void RegisterImage(string imageName, int count) {
            imageNames.Clear();
            if (count == 1) {
                imageNames.Add(imageName);
            } else {
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

        private void CreatEffectOne() {
            if (startPosition.X > endPosition.X) { return; }
            if (imageNames.Count > 1) {
                creatNO++;
                creatNO = (int)Method.Warp(0, imageNames.Count, creatNO);
            }

            float offset = rand.Next(randArea) + offsetY;

            ImageEffect effect = new ImageEffect(imageNames[creatNO], startPosition + new Vector2(0, offset), aliveSecond);
            imageEffects.Add(effect);

            aliveSecond -= creatTimer.GetLimitTime();
            startPosition.X += ResouceManager.GetTextureWidth(imageNames[creatNO]) / 2;

            imageEffects.RemoveAll(i => i.isDisappear);
        }

        private void CreatEffect() {
            creatTimer.Update();
        }

        public override void Draw() {
            CreatEffect();

            graphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            imageEffects.ForEach(i => {
                i.Update();

                Vector2 drawPosition = i.position + Camera2D.GetOffsetPosition();
                Vector3 drawP3 = new Vector3(drawPosition, 0);

                i.VertexUpdate(drawP3);
                graphicsDevice.SetVertexBuffer(i.vertexBuffer);
                foreach (EffectPass pass in i.effect.CurrentTechnique.Passes)
                {
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