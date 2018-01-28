//作成日：　2017.03.14
//作成者：　柏
//クラス内容：　整数の範囲コントロールクラス
//修正内容リスト：
//名前：柏　　　日付：2017.10.6　　　内容：timer機能改善によって改善
//名前：　　　日付：　　　内容：

namespace MyLib.Utility
{
    public class Range
    {
        private int first;
        private int end;

        public Range(int first, int end) {
            this.first = first;
            this.end = end;
        }

        public int First() {
            return first;
        }

        public int End() {
            return end;
        }

        public bool IsWithin(int num) {
            if (num < first) { return false; }
            if (num > end) { return false; }
            return true;
        }

        public bool IsOutOfRange() {
            return first > end;
        }

        public bool IsOutOfRange(int num) {
            return !IsWithin(num);
        }


    }
}
