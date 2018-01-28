using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MyLib.Device;
using Season.Def;


namespace Season.Components.DrawComponents
{
    class C_DrawLocus : DrawComponent
    {
        private List<Vector2> locusPositions;
        private Vector2 imgSize;
        private Rectangle rect;
        private string name;

        public C_DrawLocus(float alpha = 1, float depth = 100)
        {
            this.alpha = alpha;
            this.depth = depth;
            name = "P_Circle";
            imgSize = ResouceManager.GetTextureSize(name);
            rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
            locusPositions = new List<Vector2>();
        }
        public override void Draw()
        {
            locusPositions.Add(entity.transform.Position);
            if (locusPositions.Count > 100) { locusPositions.RemoveAt(0); }

            Renderer_2D.Begin(Camera2D.GetTransform());

            locusPositions.ForEach(locus => 
                    Renderer_2D.DrawTexture(name, locus, 1, rect, Vector2.One, 0, imgSize / 2, depth)
            );

            Renderer_2D.End();
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

            locusPositions.Clear();
        }



    }
}