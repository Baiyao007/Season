using Microsoft.Xna.Framework;
using MyLib.Device;
using MyLib.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.DrawComponents
{
    class C_DrawSkillCircle : DrawComponent
    {
        private Vector2 imgSize;
        private string name;
        private Timer actionTimer;
        private int actionCount;
        private float nowSize;
        private float limitSize;
        private float loopTime;

        private Timer creatSecondCircleTimer;
        private bool isCreatSecond;

        public C_DrawSkillCircle(bool isCreatSecond, float depth = 1, float alpha = 1)
        {
            this.alpha = alpha;
            this.depth = depth;
            nowSize = 0;

            limitSize = 1.5f;
            loopTime = 1.2f;
            
            name = "E_SkillCircle";
            imgSize = ResouceManager.GetTextureSize(name);

            actionCount = 0;
            actionTimer = new Timer(loopTime);
            actionTimer.Dt = new Timer.timerDelegate(ActionCount);

            creatSecondCircleTimer = new Timer(loopTime / 2);
            this.isCreatSecond = isCreatSecond;
        }
        public override void Draw()
        {
            actionTimer.Update();
            CreatSecondCircle();
            SkillAction();

            Renderer_2D.Begin(Camera2D.GetTransform());

            Vector2 position = entity.transform.Position - new Vector2(0 , 130);
            Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
            Renderer_2D.DrawTexture(name, position, alpha * 0.8f, rect, Vector2.One * nowSize, 0, imgSize / 2);

            Renderer_2D.End();
        }

        private void SkillAction() {
            if (actionTimer.Rate() < 0.5f) {
                alpha = actionTimer.Rate() * 2;
            }
            nowSize += limitSize / (loopTime * 60);
        }

        private void CreatSecondCircle() {
            if (!isCreatSecond) { return; }
            creatSecondCircleTimer.Update();
            if (creatSecondCircleTimer.IsTime) {
                DrawComponent drawSkill = new C_DrawSkillCircle(false);
                entity.RegisterComponent(drawSkill);
                isCreatSecond = false;
            }
        }

        private void ActionCount() {
            actionCount++;
            if (actionCount >= 3) {
                DeActive();
            }
            alpha = 1;
            nowSize = 0;
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }


    }
}
