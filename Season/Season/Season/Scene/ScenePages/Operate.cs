//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　Operateシーン
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using Season.Def;
using MyLib.Device;
using MyLib.Utility;
using MyLib.Utility.Action;
using MyLib;

namespace Season.Scene.ScenePages
{
    class Operate : IScene
    {
        private InputState inputState;
        private Button promptButton;

        private bool isEnd;

        public Operate(GameDevice gameDevice)
        {
            inputState = gameDevice.GetInputState;
            isEnd = false;

            promptButton = new Button("Text_Prompt", new Vector2(Parameter.ScreenSize.X / 2, 500), new IFlash(0.02f));
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() {
            isEnd = false;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">ゲームタイム</param>
        public void Update(GameTime gameTime) {
            Sound.PlayBGM("Operation");
            if (inputState.WasDown(InputParameter.ConfirmKey, InputParameter.ConfirmButton)) {
                isEnd = true;
                Sound.PlaySE("Laser");
            }
            promptButton.Update();
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() {
            //Renderer_2D.DrawTexture("BackGround", Vector2.Zero);
            promptButton.Draw();

            Vector2 operSize = ResouceManager.GetTextureSize("Page_Operation");
            Vector2 operPosition = new Vector2((Parameter.ScreenSize.X - operSize.X) / 2, 0);
            Renderer_2D.DrawTexture("Page_Operation", operPosition);
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
