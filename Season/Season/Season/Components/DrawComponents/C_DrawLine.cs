using Microsoft.Xna.Framework;
using MyLib.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawLine : DrawComponent
    {
        private List<Vector2> linePoint;

        public C_DrawLine(List<Vector2> linePoint, float alpha = 1, float depth = 100) {
            this.alpha = alpha;
            this.depth = depth;
            this.linePoint = linePoint;
        }

        public override void Draw() {
            Renderer_2D.Begin(Camera2D.GetTransform());

            for (int i = 0; i < linePoint.Count - 1; i++) {
                Renderer_2D.DrawLine(
                    linePoint[i],
                    linePoint[i + 1],
                    Color.Yellow
                );
            }

            Renderer_2D.End();
        }

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

            linePoint.Clear();
        }


    }
}