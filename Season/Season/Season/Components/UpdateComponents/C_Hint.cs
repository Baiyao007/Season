using Microsoft.Xna.Framework;
using Season.Components.ColliderComponents;
using Season.Components.DrawComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.UpdateComponents
{
    class C_Hint : UpdateComponent
    {
        private C_Collider_HintArea hintArea;
        private C_DrawSpriteNormal draw;
        private Vector2 imgPosition;
        private Vector2 areaCentre;
        private Vector2 areaSize;
        private string imgName;

        private static List<string> canViewList = new List<string>() {
            "F", "X", "Z",
            "Jump",
            "Right",
            "Left",
        };

        public C_Hint(string name, List<Vector2> positionData) {
            imgName = name;
            imgPosition = positionData[0];
            areaCentre = positionData[1];
            areaSize = positionData[2];
        }

        public override void Update() {
            CheckDraw();
        }

        private void CheckDraw() {
            if (!hintArea.IsThrough("Player")) {
                if (draw != null) {
                    TaskManager.RemoveTask(draw);
                    draw = null;
                }
                return;
            }

            if (draw != null) { return; }
            bool isDraw = canViewList.Find(l => l == imgName) != null;
            if (isDraw) {
                draw = new C_DrawSpriteNormal(imgName, imgPosition, 100);
                draw.Active();
                TaskManager.AddTask(draw);
            }
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる
            hintArea = new C_Collider_HintArea(imgName, areaCentre, areaSize);
            hintArea.Active();
            TaskManager.AddTask(hintArea);
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

            hintArea.DeActive();
        }


    }
}
