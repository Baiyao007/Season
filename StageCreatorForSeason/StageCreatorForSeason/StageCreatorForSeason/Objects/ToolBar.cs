using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using MyLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageCreatorForSeason.Objects
{

    class ToolBar
    {
        private eObjectType nowType;
        private Vector2 startPosition;
        private int pageNo;

        public ToolBar() {
            nowType = eObjectType.BezierStage;
            startPosition = new Vector2(462, 50);
            pageNo = 0;
        }

        public void ToNext() {
            pageNo++;
            Method.Warp(0, 1, ref pageNo);
            SetNowTool((int)nowType % 10);
        }

        public void ToBefore() {
            pageNo--;
            Method.Warp(0, 1, ref pageNo);
            SetNowTool((int)nowType % 10);
        }

        public void SetNowTool(int index) {
            int nowIndex = index + 10 * pageNo;
            if (nowIndex >= (int)eObjectType.None) { return; }
            nowType = (eObjectType)nowIndex;
            Console.WriteLine(nowIndex);
        }

        public int GetNowTool() { return (int)nowType; }

        public void Draw() {
            Renderer_2D.Begin();
            Renderer_2D.DrawTexture("ToolBar" + pageNo, startPosition);
            Renderer_2D.DrawTexture("ToolBarMask", startPosition + new Vector2(80 * ((int)nowType % 10), 0));
            for (int i = 0; i < 9; i++){
                Renderer_2D.DrawString((i + 1).ToString(), startPosition + new Vector2(30, -35) + new Vector2(80 * i, 0), Color.Red, 0.8f);
            }
            Renderer_2D.DrawString(0.ToString(), startPosition + new Vector2(30, -35) + new Vector2(80 * 9, 0), Color.Red, 0.8f);
            Renderer_2D.DrawString("-".ToString(), startPosition + new Vector2(30, -35) + new Vector2(80 * 10, 0), Color.Red, 0.8f);
            Renderer_2D.DrawString("+".ToString(), startPosition + new Vector2(30, -35) + new Vector2(80 * 11, 0), Color.Red, 0.8f);
            Renderer_2D.End();
        }
        
    }
}
