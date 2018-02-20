using Microsoft.Xna.Framework;
using MyLib.Device;
using MyLib.Utility;
using Season.Def;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawStringTemporary : DrawComponent
    {
        private string content;
        private Timer aliveTimer;
      
        public C_DrawStringTemporary(string content, float second, float alpha = 1, float depth = 100)
        {
            this.alpha = alpha;
            this.depth = depth;
            this.content = content;

            aliveTimer = new Timer(second);
            aliveTimer.Dt = new Timer.timerDelegate(DeActive);
        }

        public override void Draw() {
            Renderer_2D.Begin();
            Renderer_2D.DrawString(content, Parameter.ScreenSize - new Vector2(300, 100), Color.White, 2);
            Renderer_2D.End();

            aliveTimer.Update();
        }

        public override void DeActive() {
            base.DeActive();
        }

    }
}
