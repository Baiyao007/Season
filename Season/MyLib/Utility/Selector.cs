//作成日：　2017.10.06
//作成者：　柏
//クラス内容：　選択肢操作用
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility
{
    public class Selector
    {
        private int selectCount;
        private bool isClamp;
        private int index;

        public Selector(int selectCount, bool isClamp) {
            this.selectCount = selectCount;
            this.isClamp = isClamp;
            Initialize();
        }

        public void Initialize() {
            index = 0;
        }

        public void ToNext(){
            index++;
            CheckSelection();
        }

        public void ToBehind() {
            index--;
            CheckSelection();
        }

        public int GetSelection() {
            return index;
        }

        private void CheckSelection() {
            if (isClamp) { Clamp(); }
            else { Warp(); }
        }

        private void Clamp() {
            index = Math.Min(index, selectCount - 1);
            index = Math.Max(index, 0);
        }

        private void Warp() {
            if (index < 0) {
                index = selectCount - 1;
            }
            else if (index > selectCount - 1) {
                index = 0;
            }
        }

    }
}
