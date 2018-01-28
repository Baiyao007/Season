using Microsoft.Xna.Framework;
using MyLib.Device;
using Season.Def;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawScrollLayer : DrawComponent
    {
        private string name;
        private int imgCount;
        private List<Vector2> layerPositions;
        private Vector2 scrollPosition;
        private float scrollSpeed;

        private Entity player;

        public C_DrawScrollLayer(string name, int imgCount, float scrollSpeed = 0, float depth = 1, float alpha = 1)
        {
            this.alpha = alpha;
            this.depth = depth;
            this.name = name;
            this.imgCount = imgCount;
            this.scrollSpeed = scrollSpeed;

            layerPositions = new List<Vector2>();
            int x = 0;
            for (int i = 0; i < imgCount; i++) {
                x = i * Parameter.BackGroundSize;   //imageSize_X
                layerPositions.Add(new Vector2(x, 0));
            }

            player = EntityManager.FindWithName("Player")[0];
        }
        public override void Draw() {
            Renderer_2D.Begin(Camera2D.GetTransform());

            if (scrollSpeed == 0) { DrawNoScoll(); }
            else { DrawScoll(); }

            Renderer_2D.End();
        }

        private void DrawNoScoll() {
            for (int i = 0; i < imgCount; i++) {
                if ((layerPositions[i] + Camera2D.GetOffsetPosition()).X < -Parameter.BackGroundSize) { continue; }
                if ((layerPositions[i] + Camera2D.GetOffsetPosition()).X > Parameter.ScreenSize.X) { continue; }

                string imageName = name + i;
                Renderer_2D.DrawTexture(imageName, layerPositions[i]);
            }
        }

        private void DrawScoll() {
            scrollPosition.X += scrollSpeed * Camera2D.GetCameraMove().X;
            for (int i = 0; i < imgCount; i++) {
                string imageName = name + i;
                Renderer_2D.DrawTexture(imageName, layerPositions[i] + scrollPosition);

                if (layerPositions[i].X + scrollPosition.X + Camera2D.GetOffsetPosition().X <= -Parameter.BackGroundSize) {
                    layerPositions[i] += new Vector2(Parameter.BackGroundSize * imgCount, 0);
                }
                else if (layerPositions[i].X + scrollPosition.X + Camera2D.GetOffsetPosition().X >= Parameter.BackGroundSize) {
                    layerPositions[i] -= new Vector2(Parameter.BackGroundSize * imgCount, 0);
                }
            }
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
            layerPositions.Clear();
        }
    }
}
