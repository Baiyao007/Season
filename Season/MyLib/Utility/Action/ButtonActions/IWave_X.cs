//作成日：　2017.10.06
//作成者：　柏
//クラス内容：　行動種類：横揺れる
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility.Action
{
    public class IWave_X : IButtonAction
    {
        private Timer timer;
        private Vector2 velocity;
        private float radian;

        public IWave_X(float second = 0.05f)
        {
            timer = new Timer(second);
            Initialize();
        }

        private void Initialize() {
            timer.Dt = new Timer.timerDelegate(UpdateMove);
            velocity = Vector2.Zero;
            radian = 0;
        }

        public void Update(ref Vector2 position, ref float alpha, ref float size)
        {
            timer.Update();
            position += velocity;
        }

        private void UpdateMove()
        {
            velocity.X = (float)Math.Sin(radian);
            radian++;
            if (radian >= MathHelper.Pi * 2) { radian -= MathHelper.Pi * 2; }
        }
    }
}
