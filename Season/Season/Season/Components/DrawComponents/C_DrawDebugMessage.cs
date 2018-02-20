using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using Season.Components.NormalComponents;
using Season.Def;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawDebugMessage : DrawComponent
    {
        private InputState inputState;
        private C_BezierPoint bezier;
        private ColliderComponent collider;
        public C_DrawDebugMessage(GameDevice gameDevice, C_BezierPoint bezier, float alpha = 1, float depth = 100)
        {
            this.alpha = alpha;
            this.depth = depth;
            this.bezier = bezier;
            inputState = gameDevice.GetInputState;
        }
        public override void Draw() {
            if (!Parameter.IsDebug) { return; }

            if (inputState.IsDown(Keys.W)) { Camera2D.ZoomIn(); }
            if (inputState.IsDown(Keys.S)) { Camera2D.ZoomOut(); }

            Renderer_2D.Begin();
            DrawTextMessage();
            Renderer_2D.End();

            Renderer_2D.Begin(Camera2D.GetTransform());
            DrawEntityPosition();
            Renderer_2D.End();
        }

        private void DrawTextMessage() {
            Vector2 position = entity.transform.Position;
            Renderer_2D.DrawString(entity.GetName() + " - Position:" + position, new Vector2(1000, 50), Color.Red, 0.8f);
            Renderer_2D.DrawString(entity.GetName() + " - Angle:" + entity.transform.Angle, new Vector2(1000, 100), Color.Red, 0.8f);
            Renderer_2D.DrawString(entity.GetName() + " - Comps:" + entity.GetComponentCount(), new Vector2(1000, 150), Color.Red, 0.8f);
            Renderer_2D.DrawString(bezier.Print(), new Vector2(1000, 200), Color.Red, 0.8f);

            collider = entity.GetColliderComponent(entity.GetName());
            if (collider == null) { return; }
            Renderer_2D.DrawString("ColliderCount:" + collider.results.Count, new Vector2(1000, 250), Color.Red, 0.8f);
        }


        private void DrawEntityPosition() {
            string name = "P_Cross";
            Vector2 position = entity.transform.Position;
            Vector2 imgSize = ResouceManager.GetTextureSize(name);
            float radian = MathHelper.ToRadians(entity.transform.Angle);
            Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
            Renderer_2D.DrawTexture(name, position, Color.Red, alpha, rect, Vector2.One * 1.5f, radian, imgSize / 2);
        }

    }
}
