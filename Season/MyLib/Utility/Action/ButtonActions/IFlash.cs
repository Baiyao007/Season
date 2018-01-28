//作成日：　2017.10.06
//作成者：　柏
//クラス内容：　行動種類：点滅
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
    public class IFlash : IButtonAction
    {
        private Timer timer;
        private float alpha;
        private bool isAlphaUp;


        public IFlash(float second = 0.01f){
            timer = new Timer(second);
            Initialize();
        }

        private void Initialize() {
            timer.Dt = new Timer.timerDelegate(UpdateAlpha);
            isAlphaUp = false;
            alpha = 1;
        }

        public void Update(ref Vector2 velocity, ref float alpha, ref float size)
        {
            timer.Update();
            alpha = this.alpha;
        }

        private void UpdateAlpha()
        {
            if (alpha > 1.0f) {
                isAlphaUp = false;
            }
            else if (alpha < 0.6f) {
                isAlphaUp = true;
            }

            if (isAlphaUp) {
                alpha *= 1.02f;
            }
            else {
                alpha *= 0.98f;
            }
        }
    }
}
