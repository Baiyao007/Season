//作成日：　2017.10.11
//作成者：　柏
//クラス内容：　パーティクル用
//修正内容リスト：
//名前：柏　　　日付：20171106　　　内容：テクスチャ回転を追加した
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyLib.Device;
using MyLib.Utility.Action;

namespace MyLib.Utility
{
    public class BroadPolygon
    {
        private GraphicsDevice graphicsDevice;
        private VertexPositionTexture[] vertexPositions;
        private VertexBuffer vertexBuffer;
        private Effect effect;
        private Texture2D texture;
        private Vector3 targetPosition;

        private Vector2 size;
        private float alpha;
        private float startAlpha;
        private Vector2 broadSize;
        private float radian;
        private Timer aliveTimer;
        IChangeAble change;

        public BroadPolygon(GraphicsDevice graphicsDevice, Effect effect, Texture2D texture, float startAlpha, Vector2 size, Timer aliveTimer, IChangeAble change) {
            this.graphicsDevice = graphicsDevice;
            this.effect = effect.Clone();
            this.texture = texture;
            this.startAlpha = startAlpha;
            this.size = size;
            this.change = change.Clone();

            vertexPositions = new VertexPositionTexture[4];
            targetPosition = Vector3.Zero;
            broadSize = new Vector2(texture.Width, texture.Height);
            alpha = 1;

            InitializeEffect();
            VertexUpdate(Vector3.Zero);
            this.aliveTimer = aliveTimer;
        }

        private void InitializeEffect() {
            effect.Parameters["theTexture"].SetValue(texture);
        }

        public void Update(Vector3 position, float radian) {
            change.Change(ref size, ref alpha);
            targetPosition = position;
            this.radian = radian;

            effect.Parameters["Alpha"].SetValue(alpha * startAlpha);
        }

        private void VertexUpdate(Vector3 drawPosition) {
            float rotateAngle = MathHelper.ToDegrees(radian);
            vertexPositions[0] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(-0.5f * broadSize.X, -0.5f * broadSize.Y, 0) * size.X * Camera2D.GetZoom(), rotateAngle), new Vector2(0, 0));
            vertexPositions[1] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(-0.5f * broadSize.X,  0.5f * broadSize.Y, 0) * size.X * Camera2D.GetZoom(), rotateAngle), new Vector2(0, 1));
            vertexPositions[2] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3( 0.5f * broadSize.X, -0.5f * broadSize.Y, 0) * size.X * Camera2D.GetZoom(), rotateAngle), new Vector2(1, 0));
            vertexPositions[3] = new VertexPositionTexture(drawPosition + Methord.RotateVector3(new Vector3(0.5f * broadSize.X, 0.5f * broadSize.Y, 0) * size.X * Camera2D.GetZoom(), rotateAngle), new Vector2(1, 1));

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), vertexPositions.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionTexture>(vertexPositions);
        }

        public void Draw() {
            effect.Parameters["WorldViewProjection"].SetValue(Camera2D.GetView() * Camera2D.GetProjection());

            Vector3 drawPosition = targetPosition + Camera2D.GetOffsetPosition3();
            drawPosition.Y -= broadSize.Y / 2;
            drawPosition.X -= broadSize.X / 2;
            VertexUpdate(drawPosition);

            graphicsDevice.SetVertexBuffer(vertexBuffer);
            foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                pass.Apply();
                graphicsDevice.DrawUserPrimitives<VertexPositionTexture>(
                    PrimitiveType.TriangleStrip,
                    vertexPositions, 0, 2
                );
            }
        }

        public void SetTechnique(int techniqueNum) {
            effect.CurrentTechnique = effect.Techniques["Technique" + techniqueNum];
        }
    }
}
