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
            stageNo = 0;
            pointNo = 0;

            stageNoBefore = stageNo;
            pointNoBefore = pointNo;

            checkPoints = new List<C_CheckPoint>();
            for (int i = 0; i < Parameter.StageSize.X / 10000; i++) {
                checkPoints.Add(new C_CheckPoint(new Vector2(10000 * i, 500), i));
            }

        }

        public int GetCheckPoint() { return pointNo; }

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
            string name_map = "Stage" + stageNoBefore + "Map_";
            string name_front = "Stage" + stageNoBefore + "FrontLayer0_";
            string name_far = "Stage" + stageNoBefore + "FarLayer0_";
            //いらないリソースを削除
            for (int i = 0; i < Parameter.StageSize.X / Parameter.BackGroundSize; i++) {
                ResouceManager.DeleteTextures(name_map + i);
                ResouceManager.DeleteTextures(name_front + i);
                ResouceManager.DeleteTextures(name_far + i);
            }

            name_map = "Stage" + stageNo + "Map_";
            name_front = "Stage" + stageNo + "FrontLayer0_";
            name_far = "Stage" + stageNo + "FarLayer0_";

            int addIndex = checkPoints[pointNo].GetArea().Last();
            //取り入れる部分をロード
            for (int i = 0; i < addIndex * 5; i++) {
                resouceManager.LoadTextures(name_map + i , "./IMAGE/StageLayers/");
                resouceManager.LoadTextures(name_front + i, "./IMAGE/StageLayers/");
                resouceManager.LoadTextures(name_far + i, "./IMAGE/StageLayers/");
            }

        }

        private void UpdateResource() {
            string name_map = "Stage" + stageNoBefore + "Map_";
            string name_front = "Stage" + stageNoBefore + "FrontLayer0_";
            string name_far = "Stage" + stageNoBefore + "FarLayer0_";


            if (pointNo - pointNoBefore > 0) {

                int deleteIndex = checkPoints[pointNoBefore].GetArea().First();
                if (deleteIndex != checkPoints[pointNo].GetArea().First()) {
                    //いらないリソースを削除
                    for (int i = deleteIndex * 5; i < (deleteIndex + 1) * 5; i++) {
                        ResouceManager.DeleteTextures(name_map + i);
                        ResouceManager.DeleteTextures(name_front + i);
                        ResouceManager.DeleteTextures(name_far + i);
                    }
                }

                int addIndex = checkPoints[pointNo].GetArea().Last();
                //取り入れる部分をロード
                for (int i = (addIndex - 1) * 5; i < addIndex * 5; i++) {
                    resouceManager.LoadTextures(name_map + i, "./IMAGE/StageLayers/");
                    resouceManager.LoadTextures(name_front + i, "./IMAGE/StageLayers/");
                    resouceManager.LoadTextures(name_far + i, "./IMAGE/StageLayers/");
                }
            }
            else {
                int deleteIndex = checkPoints[pointNo].GetArea().Last();
                if (deleteIndex != checkPoints[pointNoBefore].GetArea().Last()) {
                    //いらないリソースを削除
                    for (int i = (deleteIndex - 1) * 5; i < deleteIndex * 5; i++) {
                        ResouceManager.DeleteTextures(name_map + i);
                        ResouceManager.DeleteTextures(name_front + i);
                        ResouceManager.DeleteTextures(name_far + i);
                    }
                }

                int addIndex = checkPoints[pointNoBefore].GetArea().First();
                //取り入れる部分をロード
                for (int i = addIndex * 5; i < (addIndex + 1) * 5; i++) {
                    resouceManager.LoadTextures(name_map + i, "./IMAGE/StageLayers/");
                    resouceManager.LoadTextures(name_front + i, "./IMAGE/StageLayers/");
                    resouceManager.LoadTextures(name_far + i, "./IMAGE/StageLayers/");
                }

            }

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
