using Microsoft.Xna.Framework;
using MyLib.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawSpriteAutoSize : DrawComponent
    {
        private Vector2 size;
        private string name;
        private Vector2 offset;
        private Color color;
        public C_DrawSpriteAutoSize(string name, Vector2 offset, Vector2 size, float depth = 1, float alpha = 1)
        {
            this.alpha = alpha;
            this.depth = depth;
            this.size = size;
            this.name = name;
            this.offset = Vector2.Zero;   //offset;
            color = Color.LightGreen;
        }

        public void SetColor(Color color) {
            this.color = color;
        }

        public void SetSize(Vector2 size) {
            this.size = size;
        }

        public override void Draw() {
            Renderer_2D.Begin(Camera2D.GetTransform());

            Vector2 position = entity.transform.Position + offset;
            Vector2 imgSize = ResouceManager.GetTextureSize(name);
            Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
            Vector2 drawSize = new Vector2(size.X / imgSize.X, size.Y / imgSize.Y) * 2;
            Renderer_2D.DrawTexture(name, position, color, alpha, rect, drawSize, 0, imgSize / 2);

            Renderer_2D.End();
        }
    }
}
