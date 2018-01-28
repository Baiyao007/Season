using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyLib.Device;
using MyLib.Utility;
using Season.Components.NormalComponents;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawTree : DrawComponent
    {
        private Effect effect;
        private GraphicsDevice graphicsDevice;
        private VertexPositionTexture[] vertexPositions;
        private VertexBuffer vertexBuffer;

        private string imgName;
        string maskName;
        private Vector2 offsetPosition;
        private Timer timer;

        private bool IsShaderOn;

        private Motion motion;
        public Vector2 animSpriteSize;
        private C_Switch3 entityDirect;

        private string nowAnimName;
        private AnimData nowAnim;
        private Dictionary<string, AnimData> animDatas;
        private float size;


        public C_DrawTree(Vector2 animSpriteSize, string imgName, string maskName, Vector2 offsetPosition, float depth = 1, float alpha = 1)
        {
            this.imgName = imgName;
            this.maskName = maskName;
            this.offsetPosition = offsetPosition;
            this.alpha = alpha;
            this.depth = depth;

            graphicsDevice = Renderer_2D.GetGraphicsDevice();

            effect = ResouceManager.GetEffect("MaskShader").Clone();
            effect.Parameters["theTexture"].SetValue(ResouceManager.GetTexture(imgName));
            effect.Parameters["theMask"].SetValue(ResouceManager.GetTexture(maskName));
            effect.CurrentTechnique = effect.Techniques["Technique1"];
            effect.Parameters["Color"].SetValue(new float[4] { 0.5f, 0, 0, 0.5f });

            vertexPositions = new VertexPositionTexture[4];
            timer = new Timer(5);


            IsShaderOn = false;
            size = 1;

            animDatas = new Dictionary<string, AnimData>();
            this.animSpriteSize = animSpriteSize;
        }


        public void SetSize(float size)
        {
            this.size = size;
        }


        public void AddAnim(string animName, AnimData anim)
        {
            if (animDatas.ContainsKey(animName)) { return; }
            animDatas[animName] = anim;
        }

        public void SetNowAnim(string animName)
        {
            if (nowAnimName == animName) { return; }
            nowAnimName = animName;
            nowAnim = animDatas[nowAnimName];
            InitializeAnim();
            IsShaderOn = animName != "Pollution";
        }

        public string GetNowAnimName() { return nowAnimName; }

        public bool IsAnimEnd()
        {
            return motion.IsEnd();
        }

        public void InitializeAnim()
        {
            motion = new Motion(new Range(0, nowAnim.KeyCount - 1), new Timer(nowAnim.KeySecond), nowAnim.IsLoop);
            for (int i = 0; i < nowAnim.KeyCount; i++)
            {
                motion.Add(
                    i,
                    new Rectangle((i % nowAnim.RowCount) * (int)animSpriteSize.X,
                    (i / nowAnim.RowCount) * (int)animSpriteSize.Y,
                    (int)animSpriteSize.X,
                    (int)animSpriteSize.Y)
                );
            }
        }


        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            entityDirect = (C_Switch3)entity.GetNormalComponent("C_Switch3");
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }


        private void VertexUpdate(Vector3 drawPosition)
        {
            float size = 1;
            float rotateAngle = entity.transform.Angle;
            Vector2 broadSize = animSpriteSize;

            vertexPositions[0] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(-0.5f, -0.5f, 0) * size * Camera2D.GetZoom(), rotateAngle) * broadSize.Y, new Vector2(0, 0));
            vertexPositions[1] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(-0.5f, 0.5f, 0) * size * Camera2D.GetZoom(), rotateAngle) * broadSize.Y, new Vector2(0, 1));
            vertexPositions[2] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(0.5f, -0.5f, 0) * size * Camera2D.GetZoom(), rotateAngle) * broadSize.X, new Vector2(1, 0));
            vertexPositions[3] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(0.5f, 0.5f, 0) * size * Camera2D.GetZoom(), rotateAngle) * broadSize.X, new Vector2(1, 1));

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), vertexPositions.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionTexture>(vertexPositions);
        }


        public override void Draw()
        {
            if (motion == null) { return; }
            motion.Update();
            Renderer_2D.Begin(Camera2D.GetTransform());

            float radian = MathHelper.ToRadians(entity.transform.Angle);
            Vector2 position = entity.transform.Position;
            Vector2 direction = new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian));

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


            if (!IsShaderOn) { return; }
            timer.Update();
            if (timer.IsTime) { timer.Initialize(); }

            graphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            effect.Parameters["WorldViewProjection"].SetValue(Camera2D.GetView() * Camera2D.GetProjection());
            effect.Parameters["Rate"].SetValue(timer.Rate());

            Vector2 drawPosition = position + Camera2D.GetOffsetPosition() + offsetPosition;
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
        }

    }
}
