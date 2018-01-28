using MyLib.Device;
using MyLib.Utility;
using Season.Components;
using Season.Components.DrawComponents;
using Season.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Scene.Creators
{
    class StageCreator
    {
        private EntityCreator entityCreator;
        public StageCreator(GameDevice gameDevice) {
            entityCreator = new EntityCreator(gameDevice);
        }

        public void CreateStage(int stageNo) {
            BezierStage.InitializeStage(stageNo);
            entityCreator.CreateEntitys(stageNo);
            CreateStageLayer(stageNo);
        }

        private void CreateStageLayer(int stageNo) {
            CSVReader.Read("LayerList_Stage" + stageNo);
            string[,] data = CSVReader.GetStringMatrix();

            DrawComponent com = null;
            for (int i = 0; i < data.GetLength(0); i++) {
                com = new C_DrawScrollLayer(
                    data[i, 0],
                    int.Parse(data[i, 1]),
                    float.Parse(data[i, 2]),
                    float.Parse(data[i, 3])
                );
                com.Active();
                TaskManager.AddTask(com);
            }
        }

    }
}
