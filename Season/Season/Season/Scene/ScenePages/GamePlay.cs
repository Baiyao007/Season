//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　GamePlayシーン
//修正内容リスト：
//名前：柏　　　日付：20171011　　　内容：カメラ対応
//名前：　　　日付：　　　内容：

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using Season.Def;
using Season.Entitys;
using Season.Utility;
using Season.Components;
using Season.Scene.Creators;
using Season.Components.DrawComponents;
using Season.Components.NormalComponents;
using Season.Components.ColliderComponents;

namespace Season.Scene.ScenePages
{
    class GamePlay : IScene
    {
        private GameDevice gameDevice;
        private InputState inputState;
        private bool isEnd;

        private E_Scene next;
        private StageCreator stageCreator;

        private Entity player;

        private bool isPause;
        private int stageNo;

        public GamePlay(GameDevice gameDevice) {
            this.gameDevice = gameDevice;

            inputState = gameDevice.GetInputState;
            stageCreator = new StageCreator(gameDevice);

            isPause = false;
            stageNo = 1;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() {
            isEnd = false;
            isPause = false;
            GameConst.Initialize();

            //Entity初期化
            stageCreator.CreateStage(stageNo);
            player = EntityManager.FindWithName("Player")[0];

            //Debug用
            DebugInitialize();
        }

        private void DebugInitialize() {
            //BezierLine描画
            DrawComponent com = new C_DrawBezier(BezierStage.GetControllPoints());
            com.Active();
            TaskManager.AddTask(com);

            //Wall描画
            com = new C_DrawBezier(BezierStage.GetBlockMapList());
            com.Active();
            TaskManager.AddTask(com);

            //ShaderTest用
            //CreatShaderTest();

            //StageCheck
            if (inputState.IsDown(Keys.W, Buttons.LeftShoulder)) {
                Camera2D.ZoomIn();
            }
            if (inputState.IsDown(Keys.S, Buttons.RightShoulder)) {
                Camera2D.ZoomOut();
            }


            //毒沼生成（機能改善中）
            if (stageNo == 1) {
                List<int> lb = new List<int>() { 4, 4 };
                CreatWasteland(lb);
            }
        }

        private void CreatShaderTest() {
            Entity test = Entity.CreateEntity("Test","Test", new Transform2D());
            test.transform.Position = new Vector2(1000, 1200);

            test.RegisterComponent(new C_DrawWithShader("TestImg", "UIMask", Vector2.Zero, 100));   //TestMask
        }

        private void CreatWasteland(List<int> lb) {
            //実体生成
            Entity wasteland = Entity.CreateEntity("Wasteland", "Wasteland", new Transform2D());
            
            //位置設定
            C_BezierPoint bezier = new C_BezierPoint();

            //初期化毒沼
            C_WastelandState wastelandState = new C_WastelandState(lb);
            
            bezier.SetBezierData(lb[0], lb[1], 0);
            bezier.SetRoute(BezierStage.GetNowRoute(lb[0], lb[1]));
            wastelandState.startPosition = bezier.GetNowPosition();
            wastelandState.SetTargetY(wastelandState.startPosition.Y);

            bezier.SetBezierData(lb[0], lb[1] + 1, 0);
            bezier.SetRoute(BezierStage.GetNowRoute(lb[0], lb[1] + 1));
            wastelandState.SetStartY(bezier.GetNowPosition().Y);

            bezier.SetBezierData(lb[0], lb[1] + 4, 0);
            bezier.SetRoute(BezierStage.GetNowRoute(lb[0], lb[1] + 4));
            wastelandState.endPosition = bezier.GetNowPosition();

            wasteland.transform.Position = wastelandState.startPosition;

            wasteland.RegisterComponent(new C_DrawWasteland(gameDevice, wastelandState));


            Vector2 offset = new Vector2(0, -120);
            wasteland.RegisterComponent(new C_Collider_Circle("WastelandStart", offset, 80, eCollitionType.Through, false));
            offset.X += (wastelandState.endPosition - wastelandState.startPosition).X;
            wasteland.RegisterComponent(new C_Collider_Circle("WastelandEnd", offset, 80, eCollitionType.Through, false));

            wasteland.RegisterComponent(wastelandState);
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">時間</param>
        public void Update(GameTime gameTime) {
            if (isEnd) { return; }
            if (inputState.WasDown(Keys.P, Buttons.X)) {
                TaskManager.ChangePause();
                isPause = !isPause;
            }
            if (isPause) { return; }

            if (GameConst.IsEnding) {
                next = E_Scene.ENDING;
                isEnd = true;
                return;
            }
            if (player.transform.Position.X >= Parameter.StageSize.X - 500) {
                GameConst.SetClear();
            }
            if (GameConst.IsClear) {
                stageNo++;
                Shutdown();
                Initialize();
                return;
            }
            if (EntityManager.GetEntityCount() > 0) {
                Camera2D.Update(player.transform.Position);
            }
            Sound.PlayBGM("GamePlay");
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() {
            //Renderer_2D.DrawString("ObjectsCount:" + EntityManager.GetEntityCount(), new Vector2(10, 520), Color.Red, 0.5f);
            //Renderer_2D.DrawString("ParticlesCount:" + gameDevice.GetParticlesCount(), new Vector2(10, 550), Color.Red, 0.5f);
        }


        /// <summary>
        /// シーンを閉める
        /// </summary>
        public void Shutdown() {
            gameDevice.GetParticleGroup.Clear();
            TaskManager.CloseAllTask();
            EntityManager.Clear();
            Camera2D.Initialize();
        }

        /// <summary>
        /// 終わりフラッグを取得
        /// </summary>
        /// <returns>エンドフラッグ</returns>
        public bool IsEnd() { return isEnd; }

        /// <summary>
        /// 次のシーン
        /// </summary>
        /// <returns>シーンのEnum</returns>
        public E_Scene Next() { return next; }
    }
}
