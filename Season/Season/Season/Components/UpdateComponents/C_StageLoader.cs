using Microsoft.Xna.Framework;
using MyLib.Device;
using Season.Components.NormalComponents;
using Season.Def;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.UpdateComponents
{
    class C_StageLoader : UpdateComponent
    {
        private ResouceManager resouceManager;
        private List<C_CheckPoint> checkPoints;
        private int stageNo;
        private int pointNo;
        private int pointNoBefore;
        private int stageNoBefore;

        public C_StageLoader(GameDevice gameDevice) {
            resouceManager = gameDevice.GetResouceManager;
            stageNo = 1;
            pointNo = 0;

            stageNoBefore = stageNo;
            pointNoBefore = pointNo;

            checkPoints = new List<C_CheckPoint>();
            for (int i = 0; i < Parameter.StageSize.X / 10000; i++) {
                checkPoints.Add(new C_CheckPoint(new Vector2(10000 * i, 500), i));
            }
        }

        public void SetPoint(int pointNo) {
            if (this.pointNo == pointNo) { return; }
            pointNoBefore = this.pointNo;
            this.pointNo = pointNo;
            UpdateResource();
        }

        public void SetStage(int stageNo) {
            if (this.stageNo == stageNo) { return; }

            this.stageNo = stageNo;
            pointNo = 0;
            InitializeStage();

            stageNoBefore = this.stageNo;
            pointNoBefore = 0;
        }

        private void InitializeStage() {
            string name_back = "Stage" + stageNoBefore + "Back_";
            string name_map = "Stage" + stageNoBefore + "Map_";
            string name_front = "Stage" + stageNoBefore + "FrontLayer0_";
            string name_far = "Stage" + stageNoBefore + "FarLayer0_";
            //いらないリソースを削除
            for (int i = 0; i < Parameter.StageSize.X / Parameter.BackGroundSize; i++) {
                ResouceManager.DeleteTextures(name_back + i);
                ResouceManager.DeleteTextures(name_map + i);
                ResouceManager.DeleteTextures(name_front + i);
                ResouceManager.DeleteTextures(name_far + i);
            }

            //取り入れる部分をロード
            

        }

        private void UpdateResource() {
            //いらないリソースを削除

            //取り入れる部分をロード
        }

        public override void Update()
        {
            base.Update();



        }

        public override void Active()
        {
            base.Active();
        }

        public override void DeActive()
        {
            base.DeActive();
        }

    }
}
