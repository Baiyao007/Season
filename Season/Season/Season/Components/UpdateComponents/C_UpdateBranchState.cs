using Microsoft.Xna.Framework;
using MyLib.Device;
using Season.Components.ColliderComponents;
using Season.Components.DrawComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.UpdateComponents
{
    class C_UpdateBranchState : UpdateComponent
    {
        private GameDevice gameDevice;

        private ColliderComponent collider;
        private C_DrawAnimetion drawComp;

        private bool isBreak;

        public C_UpdateBranchState(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            isBreak = false;
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる
            drawComp = (C_DrawAnimetion)entity.GetDrawComponent("C_DrawAnimetion");

            Vector2 position = entity.transform.Position;
            collider = new C_Collider_Line("Wall", position - new Vector2(0, drawComp.animSpriteSize.Y), position, eCollitionType.Jostle);
            entity.RegisterComponent(collider);
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }

        public override void Update() {
            DoBreak();
            CheckBreakEnd();
        }

        private void DoBreak() {
            if (isBreak) { return; }
            if (collider.ThroughStart("PlayerAttack")) {
                drawComp.SetNowAnim("Break");
                isBreak = true;
            }
        }

        private void CheckBreakEnd() {
            if (!isBreak) { return; }
            if (drawComp.IsAnimEnd()) {
                entity.DeActive();
            }
        }

    }
}
