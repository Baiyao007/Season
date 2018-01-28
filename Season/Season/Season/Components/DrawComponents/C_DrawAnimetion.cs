using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyLib.Device;
using MyLib.Utility;
using Season.Components.NormalComponents;
using Season.Entitys;
using System;
using System.Collections.Generic;

namespace Season.Components.DrawComponents
{
    class C_DrawAnimetion : DrawComponent
    {
        private Effect effect;
        private GraphicsDevice graphicsDevice;
        private VertexPositionTexture[] vertexPositions;
        private VertexBuffer vertexBuffer;
        private Timer shadeTimer;
        private bool isShaderOn;
        private Vector2 baseImgSize;

        private Motion motion;
        public Vector2 animSpriteSize;
        private C_Switch3 entityDirect;

        private string nowAnimName;
        private AnimData nowAnim;
        private Dictionary<string, AnimData> animDatas;
        private float size;

        public C_DrawAnimetion(Vector2 animSpriteSize, float depth = 1, float alpha = 1)
        {
            this.alpha = alpha;
            this.depth = depth;
            size = 1;

            animDatas = new Dictionary<string, AnimData>();
            this.animSpriteSize = animSpriteSize;


            graphicsDevice = Renderer_2D.GetGraphicsDevice();

            vertexPositions = new VertexPositionTexture[4];
            shadeTimer = new Timer(3);

            isShaderOn = false;
        }

        public void SetShaderOn(float liveTime, string baseImg, string maskImg) {
            isShaderOn = true;
            baseImgSize = ResouceManager.GetTextureSize(baseImg);
            shadeTimer.SetTimer(liveTime);
            effect = ResouceManager.GetEffect("MaskShader").Clone();
            effect.Parameters["theTexture"].SetValue(ResouceManager.GetTexture(baseImg));
            effect.Parameters["theMask"].SetValue(ResouceManager.GetTexture(maskImg));
            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["WorldViewProjection"].SetValue(Camera2D.GetView() * Camera2D.GetProjection());
            effect.Parameters["Color"].SetValue(new float[4] { 0.5f, 0, 0, 0.5f });
        }

        public void SetShaderCreatment(bool creat) {
            if (creat) {
                effect.CurrentTechnique = effect.Techniques["Technique1"];
            }
            else {
                effect.CurrentTechnique = effect.Techniques["Technique2"];
            }
        }

        public void SetSize(float size) {
            this.size = size;
        }

        public void AddAnim(string animName, AnimData anim) {
            if (animDatas.ContainsKey(animName)) { return; }
            animDatas[animName] = anim;
        }

        public void SetNowAnim(string animName) {
            if (nowAnimName == animName) { return; }
            nowAnimName = animName;
            nowAnim = animDatas[nowAnimName];
            InitializeAnim();
        }

        public string GetNowAnimName() { return nowAnimName; }

        public bool IsAnimEnd() {
            return motion.IsEnd();
        }

        public void InitializeAnim() {
            motion = new Motion(new Range(0, nowAnim.KeyCount - 1), new Timer(nowAnim.KeySecond), nowAnim.IsLoop);
            for (int i = 0; i < nowAnim.KeyCount; i++) {
                motion.Add(
                    i,
                    new Rectangle((i % nowAnim.RowCount) * (int)animSpriteSize.X,
                    (i / nowAnim.RowCount) * (int)animSpriteSize.Y,
                    (int)animSpriteSize.X,
                    (int)animSpriteSize.Y)
                );
            }
        }

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            entityDirect = (C_Switch3)entity.GetNormalComponent("C_Switch3");
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }

        private void VertexUpdate(Vector3 drawPosition)
        {
            float rotateAngle = entity.transform.Angle;

            vertexPositions[0] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(-0.5f * baseImgSize.X, -0.5f * baseImgSize.Y, 0) * size * Camera2D.GetZoom(), rotateAngle), new Vector2(0, 0));
            vertexPositions[1] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(-0.5f * baseImgSize.X, 0.5f * baseImgSize.Y, 0) * size * Camera2D.GetZoom(), rotateAngle), new Vector2(0, 1));
            vertexPositions[2] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(0.5f * baseImgSize.X, -0.5f * baseImgSize.Y, 0) * size * Camera2D.GetZoom(), rotateAngle), new Vector2(1, 0));
            vertexPositions[3] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(0.5f * baseImgSize.X, 0.5f * baseImgSize.Y, 0) * size * Camera2D.GetZoom(), rotateAngle), new Vector2(1, 1));

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), vertexPositions.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionTexture>(vertexPositions);
        }


        public override void Draw() {
            if (motion == null) { return; }

            float radian = MathHelper.ToRadians(entity.transform.Angle);
            Vector2 position = entity.transform.Position;
            Vector2 direction = new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian));

            if (isShaderOn)
            {
                shadeTimer.Update();
                if (shadeTimer.IsTime) { effect.Parameters["theMask"].SetValue(ResouceManager.GetTexture("NoneMask")); }

                graphicsDevice.RasterizerState = RasterizerState.CullClockwise;
                effect.Parameters["Rate"].SetValue(shadeTimer.Rate());

                Vector2 drawPosition = position + Camera2D.GetOffsetPosition() + Methord.RightAngleMove(direction, animSpriteSize.Y / 2);
                Vector3 drawP3 = new Vector3(drawPosition, 0);

                VertexUpdate(drawP3);
                graphicsDevice.SetVertexBuffer(vertexBuffer);
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(
                        PrimitiveType.TriangleStrip,
                        vertexPositions, 0, 2
                    );
                }

                if (shadeTimer.IsTime) { isShaderOn = false; }
            }
            else {
                motion.Update();
                Renderer_2D.Begin(Camera2D.GetTransform());

                while (entity.transform.Angle < 0) { entity.transform.Angle += 360; }
                int angle = (int)(entity.transform.Angle / 90);

                Renderer_2D.DrawTexture(
                    nowAnim.AnimName,
                    position + Methord.RightAngleMove(direction, animSpriteSize.Y / 2),
                    alpha,
                    motion.DrawingRange(),
                    Vector2.One * size,
                    radian,
                    animSpriteSize / 2,
                    angle % 4 == 0 || angle % 4 == 3
                );

                Renderer_2D.End();
            }
        }

    }
}
