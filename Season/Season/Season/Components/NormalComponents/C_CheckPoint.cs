using Microsoft.Xna.Framework;
using Season.Def;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.NormalComponents
{
    class C_CheckPoint
    {
        private List<int> loadArea;
        private Vector2 position;
        private bool isCheck;

        public C_CheckPoint(Vector2 position, int index) {
            this.position = position;
            isCheck = false;
            InitializeArea(index);
        }

        //常に40000ピクセルのリソースが持ってる
        //ステージを10000単位で分割する
        private void InitializeArea(int index) {
            loadArea = new List<int>();
            for (int i = 0; i < 4; i++) {
                loadArea.Add(index + i - 1);
            }
            loadArea.RemoveAll(a => a < 0 || a > Parameter.StageSize.X / 10000);
        }

        public List<int> GetArea() { return loadArea; }
    }
}
