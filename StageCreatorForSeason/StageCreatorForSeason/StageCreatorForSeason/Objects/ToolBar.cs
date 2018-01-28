using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageCreatorForSeason.Objects
{
    enum eToolType {
        BezierStage,
        Wall,
        Boar,
        None,
    }

    class ToolBar
    {
        private eToolType nowType;
        private Vector2 startPosition;

        public ToolBar() {
            nowType = eToolType.BezierStage;
            startPosition = new Vector2(512, 50);
        }

        public void SetNowTool(int index) {
            nowType = (eToolType)index;
        }

        public int GetNowTool() { return (int)nowType; }

        public void Draw() {
            Renderer_2D.Begin();
            Renderer_2D.DrawTexture("ToolBar", startPosition);
            Renderer_2D.DrawTexture("ToolBarMask", startPosition + new Vector2(80 * ((int)nowType), 0));
            for (int i = 0; i < 9; i++){
                Renderer_2D.DrawString((i + 1).ToString(), startPosition + new Vector2(30, -35) + new Vector2(80 * i, 0), Color.Red, 0.8f);
            }

            Renderer_2D.End();
        }
        
    }
}
