using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyLib.Device;
using MyLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawWithShader : DrawComponent
    {
        private Effect effect;
        private GraphicsDevice graphicsDevice;
        private VertexPositionTexture[] vertexPositions;
        private VertexBuffer vertexBuffer;

        private string imgName;
        string maskName;
        private Vector2 offsetPosition;
        private Timer timer;

        public C_DrawWithShader(string imgName, string maskName, Vector2 offsetPosition, float depth = 1, float alpha = 1)
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
        }


        private void VertexUpdate(Vector3 drawPosition)
        {
            float size = 1;
            float rotateAngle = entity.transform.Angle;
            Vector2 imgSize = ResouceManager.GetTextureSize(imgName);

            vertexPositions[0] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(-0.5f * imgSize.X, -0.5f * imgSize.Y, 0) * size * Camera2D.GetZoom(), rotateAngle), new Vector2(0, 0));
            vertexPositions[1] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(-0.5f * imgSize.X, 0.5f * imgSize.Y, 0) * size * Camera2D.GetZoom(), rotateAngle), new Vector2(0, 1));
            vertexPositions[2] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(0.5f * imgSize.X, -0.5f * imgSize.Y, 0) * size * Camera2D.GetZoom(), rotateAngle), new Vector2(1, 0));
            vertexPositions[3] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(0.5f * imgSize.X, 0.5f * imgSize.Y, 0) * size * Camera2D.GetZoom(), rotateAngle), new Vector2(1, 1));

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), vertexPositions.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionTexture>(vertexPositions);
        }


        public override void Draw()
        {
            timer.Update();
            if (timer.IsTime) { timer.Initialize(); }

            graphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            effect.Parameters["WorldViewProjection"].SetValue(Camera2D.GetView() * Camera2D.GetProjection());
            effect.Parameters["Rate"].SetValue(timer.Rate());

            float radian = MathHelper.ToRadians(entity.transform.Angle);
            Vector2 direction = new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian));
            Vector2 drawPosition = entity.transform.Position + Camera2D.GetOffsetPosition() + offsetPosition;
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
