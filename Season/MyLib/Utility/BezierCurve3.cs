//作成日：　2017.10.17
//作成者：　柏
//クラス内容：　ベジュ曲線（3点対応）
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：


using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility
{
    public class BezierCurve3
    {
        private Timer timer;
        private Vector2 position;
        private List<Vector2> controlPoints;
        private Vector2 direction;

        public BezierCurve3(float second) {
            timer = new Timer(second);
            timer.Dt = new Timer.timerDelegate(UpdateCurve);

            controlPoints = new List<Vector2>() {
                Vector2.Zero,
                Vector2.Zero,
                Vector2.Zero
            };
        }

        public void SetControlPoints(List<Vector2> controlPoints) {
            if (controlPoints.Count < 3) { return; }
            this.controlPoints = controlPoints;
        }

        public void SetTimer(float second) {
            timer.SetTimer(second);
        }

        public void Update() {
            timer.Update();
        }

        private void UpdateCurve() {
            float timeRate = 1 - timer.Rate();
            Vector2 previousPosition = position;
            position = timer.Rate() * timer.Rate() * controlPoints[0] +
                2 * timer.Rate() * timeRate * controlPoints[1] +
                timeRate * timeRate * controlPoints[2];
            direction = position - previousPosition;
        }

        public Vector2 GetPosiiton() {
            return position;
        }

        public Vector2 GetDirection() {
            return direction;
        }
    }
}
