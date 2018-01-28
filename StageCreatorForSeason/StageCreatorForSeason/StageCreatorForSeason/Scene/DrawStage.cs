using Microsoft.Xna.Framework;
using MyLib.Device;
using StageCreatorForSeason.Def;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageCreatorForSeason.Scene
{
    class DrawStage
    {
        private string name;
        private int imgCount;
        private List<Vector2> layerPositions;

        public DrawStage(string name, int imgCount) {
            this.name = name;
            this.imgCount = imgCount;

            layerPositions = new List<Vector2>();
            int x = 0;
            for (int i = 0; i < imgCount; i++) {
                x = i * Parameter.BackGroundSize;
                layerPositions.Add(new Vector2(x, 0));
            }
        }
        public void Draw() {
            Renderer_2D.Begin(Camera2D.GetTransform());

            for (int i = 0; i < imgCount; i++) {
                string imageName = name + i;
                Renderer_2D.DrawTexture(imageName, layerPositions[i]);
            }

            Renderer_2D.End();
        }

    }
}
