//作成日：　2017.03.14
//作成者：　柏
//クラス内容：　2Dアニメーション描画クラス
//修正内容リスト：
//名前：柏　　　日付：2017.10.6　　　内容：delegate利用し、機能改善
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MyLib.Utility
{
    public class Motion
    {
        private Range range;
        private Timer timer;
        private int motionNumber;
        private bool isLoop;
        private bool isEnd;

        private Dictionary<int, Rectangle> rectangles = new Dictionary<int, Rectangle>();

        public Motion() {
            Initialize(new Range(0, 0), new Timer(0.5f), true);
        }

        public Motion(Range range, Timer timer, bool isLoop = true) {
            Initialize(range, timer,isLoop);
        }

        public void Initialize(Range range, Timer timer, bool isLoop = true) {
            this.range = range;
            this.timer = timer;
            this.isLoop = isLoop;
            timer.Dt = new Timer.timerDelegate(MotionUpdate);
            motionNumber = range.First();
            isEnd = false;
        }

        public void Add(int index, Rectangle rect) {
            if (rectangles.ContainsKey(index)) {
                return;
            }
            rectangles.Add(index, rect);
        }

        public void RemoveKey(int index) {
            rectangles.Remove(index);
        }

        private void MotionUpdate() {
            motionNumber += 1;
            if (range.IsOutOfRange(motionNumber)) {
                if (isLoop) {
                    motionNumber = range.First();
                } else {
                    motionNumber = range.End();
                    isEnd = true;
                }
            }
        }

        public void Update() {
            if (range.IsOutOfRange()) { return; }
            timer.Update();
        }

        public bool IsEnd() {
            return isEnd;
        }

        public Rectangle DrawingRange() {
            return rectangles[motionNumber];
        }

    }
}
