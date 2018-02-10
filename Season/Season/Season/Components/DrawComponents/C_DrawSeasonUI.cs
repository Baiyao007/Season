using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyLib.Device;
using MyLib.Utility;
using Season.Components.UpdateComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawSeasonUI : DrawComponent
    {
        private Effect effect;
        private GraphicsDevice graphicsDevice;
        private VertexPositionTexture[] vertexPositions;
        private VertexBuffer vertexBuffer;

        private C_SeasonState seasonState;

        public C_DrawSeasonUI(C_SeasonState seasonState, float depth = 100, float alpha = 1)
        {
            this.seasonState = seasonState;
            this.alpha = alpha;
            this.depth = depth;

            graphicsDevice = Renderer_2D.GetGraphicsDevice();

            effect = ResouceManager.GetEffect("CircleBar").Clone();
            effect.Parameters["WorldViewProjection"].SetValue(Camera2D.GetView() * Camera2D.GetProjection());
            effect.CurrentTechnique = effect.Techniques["Technique1"];
            vertexPositions = new VertexPositionTexture[4];
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
            float rotateAngle = 0;  //entity.transform.Angle;
            Vector2 imgSize = Vector2.One * 48;

            vertexPositions[0] = new VertexPositionTexture(drawPosition + Method.RotateVector3(new Vector3(-0.5f * imgSize.X, -0.5f * imgSize.Y, 0) * size * Camera2D.GetZoom(), rotateAngle), new Vector2(0, 0));
            vertexPositions[1] = new VertexPositionTexture(drawPosition + Method.RotateVector3(new Vector3(-0.5f * imgSize.X, 0.5f * imgSize.Y, 0) * size * Camera2D.GetZoom(), rotateAngle), new Vector2(0, 1));
            vertexPositions[2] = new VertexPositionTexture(drawPosition + Method.RotateVector3(new Vector3(0.5f * imgSize.X, -0.5f * imgSize.Y, 0) * size * Camera2D.GetZoom(), rotateAngle), new Vector2(1, 0));
            vertexPositions[3] = new VertexPositionTexture(drawPosition + Method.RotateVector3(new Vector3(0.5f * imgSize.X, 0.5f * imgSize.Y, 0) * size * Camera2D.GetZoom(), rotateAngle), new Vector2(1, 1));

            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), vertexPositions.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionTexture>(vertexPositions);
        }


        public override void Draw()
        {
            Vector2 position = entity.transform.Position;

            float angle = entity.transform.Angle;
            if (angle % 180 == 0) { angle--; }

            float radian = MathHelper.ToRadians(angle);
            Vector2 direction = new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian));
            Vector2 offsetVert = Method.RightAngleMove(direction, 270);

            angle += 90;
            int area = (int)(angle / 90);
            radian = MathHelper.ToRadians(angle);
            direction = new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian));

            Vector2 offsetHori = Vector2.Zero;
            if (area % 4 == 0 || area % 4 == 2) {
                offsetHori = Method.RightAngleMove(direction, -80);
            } else {
                offsetHori = Method.RightAngleMove(direction, 80);
            }

            Timer skillCD = seasonState.GetSkillCD();
            float rate = skillCD.Rate();
            if (skillCD.IsMax()) {
                effect.Parameters["Alpha"].SetValue(1.0f);
                rate = 0;
            } else {
                effect.Parameters["Alpha"].SetValue(0.7f);
            }

            graphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            effect.Parameters["Rate"].SetValue(rate);
            effect.Parameters["theTexture"].SetValue(ResouceManager.GetTexture("SeasonUI_0" + (int)seasonState.GetNowSeason()));
            Vector2 drawPosition = entity.transform.Position + Camera2D.GetOffsetPosition() + offsetVert + offsetHori;
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

















//using Microsoft.Xna.Framework;
//using MyLib.Device;
//using MyLib.Utility;
//using Season.Components.UpdateComponents;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Season.Components.DrawComponents
//{
//    class C_DrawSeasonUI : DrawComponent
//    {
//        private C_SeasonState seasonState;
//        private string name;

//        public C_DrawSeasonUI(C_SeasonState seasonState, float alpha = 1, float depth = 100)
//        {
//            this.seasonState = seasonState;
//            this.alpha = alpha;
//            this.depth = depth;
//            name = "SeasonUI";
//        }
//        public override void Draw()
//        {
//            Renderer_2D.Begin(Camera2D.GetTransform());

//            Vector2 position = entity.transform.Position;
//            while (entity.transform.Angle < 0) { entity.transform.Angle += 360; }
//            float angle = entity.transform.Angle;
//            float radian = MathHelper.ToRadians(angle);
//            Vector2 direction = new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian));
//            Vector2 offsetVert = Method.RightAngleMove(direction, 270);

//            angle += 90;
//            int area = (int)(angle / 90);
//            radian = MathHelper.ToRadians(angle);
//            direction = new Vector2((float)Math.Cos(radian), (float)Math.Sin(radian));

//            Vector2 offsetHori = Vector2.Zero;
//            if (area % 4 == 0 || area % 4 == 2) {
//                offsetHori = Method.RightAngleMove(direction, -80);
//            }
//            else {
//                offsetHori = Method.RightAngleMove(direction, 80);
//            }

//            for (int i = 0; i < (int)eSeason.None; i++)
//            {
//                Renderer_2D.DrawTexture(
//                    name,
//                    position + offsetVert + offsetHori,
//                    1,
//                    seasonState.GetRect((eSeason)i),
//                    Vector2.One,
//                    0,
//                    Vector2.One * 48 / 2
//                    );
//            }

//            Renderer_2D.End();
//        }

//        public override void Active()
//        {
//            base.Active();
//            //TODO 更新コンテナに自分を入れる
//        }

//        public override void DeActive()
//        {
//            base.DeActive();
//            //TODO 更新コンテナから自分を削除

//        }



//    }
//}