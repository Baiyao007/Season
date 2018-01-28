using Microsoft.Xna.Framework;
using MyLib.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageCreatorForSeason.Objects
{
    class BezierPoint : Object
    {
        public BezierPoint(Vector2 position, int teamNo)
            : base("Point", position, 20, eObjectType.BezierStage, teamNo)
        {
            isSelected = false;
        }

        private BezierPoint(Object other, Vector2 position, int teamNo)
                : this( position, teamNo)
        { }

        public  BezierPoint Clone(Vector2 position, int teamNo) {
            return new BezierPoint(this, position, teamNo);
        }

        protected override void DrawObject() {
            Renderer_2D.Begin(Camera2D.GetTransform());

            Color color = isSelected ? Color.Red : Color.White;

            Vector2 imgSize = ResouceManager.GetTextureSize(name);
            Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
            Renderer_2D.DrawTexture(name, position, color, 1, rect, Vector2.One, 0, imgSize / 2);

            Renderer_2D.End();
        }


    }
}
