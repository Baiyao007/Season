//作成日：　2017.11.20
//作成者：　柏
//クラス内容：　描画用Component - 2DSprite
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Device;
using Season.Def;

namespace Season.Components.DrawComponents
{
    class C_DrawSpriteNormal : DrawComponent
    {
        public C_DrawSpriteNormal(float depth = 1, float alpha = 1){
            this.alpha = alpha;
            this.depth = depth;
        }
        public override void Draw() {
            Renderer_2D.Begin(Camera2D.GetTransform());

            string name = entity.GetName();
            float radian = MathHelper.ToRadians(entity.transform.Angle);
            Vector2 position = entity.transform.Position;
            Vector2 imgSize = ResouceManager.GetTextureSize(name);
            Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
            Renderer_2D.DrawTexture(name, position, alpha, rect, Vector2.One, radian, imgSize / 2);

            Renderer_2D.End();
        }
    }
}
