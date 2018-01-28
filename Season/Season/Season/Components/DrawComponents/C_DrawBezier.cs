using Microsoft.Xna.Framework;
using MyLib.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawBezier : DrawComponent
    {
        private List<List<Vector2>> bezierPoints;

        public C_DrawBezier(List<List<Vector2>> bezierPoints, float alpha = 1, float depth = 100)
        {
            this.alpha = alpha;
            this.depth = depth;
            this.bezierPoints = bezierPoints;
        }

        public override void Draw()
        {
            Renderer_2D.Begin(Camera2D.GetTransform());

            for (int i = 0; i < bezierPoints.Count; i++) {
                for (int j = 0; j < bezierPoints[i].Count - 1; j++) {
                    Renderer_2D.DrawLine(
                        bezierPoints[i][j], 
                        bezierPoints[i][j + 1], 
                        Color.Yellow
                    );
                }
            }

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

            bezierPoints.Clear();
        }


    }
}