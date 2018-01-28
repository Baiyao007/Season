//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　Endingシーン
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using Season.Def;
using MyLib.Device;
using MyLib.Utility;
using MyLib.Utility.Action;
using MyLib;
using MyLib.Utility.Action.Movements;
using MyLib.Utility.Action.TheChange;

namespace Season.Scene.ScenePages
{
    class Ending : IScene
    {
        private InputState inputState;
        private GameDevice gameDevice;
        private Button promptButton;
        private bool isEnd;

        public Ending(GameDevice gameDevice) {
            this.gameDevice = gameDevice;
            inputState = gameDevice.GetInputState;
            promptButton = new Button("Text_Prompt", new Vector2(Parameter.ScreenSize.X / 2, 550), new IFlash(0.02f));
            isEnd = false;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() {
            isEnd = false;
            CreatExplode();
        }

        private void CreatExplode() {
            gameDevice.GetParticleGroup.AddParticles(
                "P_Sakura",         //name
                50, 80,             //count
                Parameter.ScreenSize / 2 + new Vector2(-100,-300),
                Parameter.ScreenSize / 2 + new Vector2(100, -100),     //position
                3.0f, 5.0f,       //speed
                1.5f, 3.0f,            //size
                0.5f, 1,            //alpha
                0, 360,             //angle
                10, 10,             //alive
                new MoveExplode(),  //moveType
                new ChangeLuceBig(new Timer(1.5f))   //changeType
            );
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">時間</param>
        public void Update(GameTime gameTime) {
            Sound.PlayBGM("Ending");
            if (inputState.WasDown(InputParameter.ConfirmKey, InputParameter.ConfirmButton)) {
                Sound.PlaySE("Laser");
                isEnd = true;
            }
            promptButton.Update();
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">2D描画管理</param>
        public void Draw() {
            Vector2 operSize = ResouceManager.GetTextureSize("Page_Ending");
            Vector2 operPosition = new Vector2((Parameter.ScreenSize.X - operSize.X) / 2, 0);
            Renderer_2D.DrawTexture("Page_Ending", operPosition);

            promptButton.Draw();
        }

        /// <summary>
        /// シーンを閉める
        /// </summary>
        public void Shutdown() { Initialize(); }

        /// <summary>
        /// 終わりフラッグを取得
        /// </summary>
        /// <returns>エンドフラッグ</returns>
        public bool IsEnd() { return isEnd; }

        /// <summary>
        /// 次のシーン
        /// </summary>
        /// <returns>シーンのEnum</returns>
        public E_Scene Next() {
            return E_Scene.TITLE; 
        }

    }
}
