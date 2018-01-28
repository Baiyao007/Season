//作成日：　2017.10.06
//作成者：　柏
//クラス内容：　行動種類：拡大縮小
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
    public class IResize : IButtonAction
    {
        private Timer timer;
        private float size;
        private bool isSizeUp;

        public IResize(float second = 0.01f)
        {
            timer = new Timer(second);
            Initialize();
        }

        private void Initialize() {
            timer.Dt = new Timer.timerDelegate(UpdateSize);
            isSizeUp = true;
            size = 1;
        }

        public void Update(ref Vector2 velocity, ref float alpha, ref float size)
        {
            timer.Update();
            size = this.size;
        }

        private void UpdateSize()
        {
            if (size > 1.3f)
            {
                isSizeUp = false;
            }
            else if (size < 0.8f)
            {
                isSizeUp = true;
            }

            if (isSizeUp)
            {
                size *= 1.02f;
            }
            else {
                size *= 0.98f;
            }
        }
    }
}
