//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　Titleシーン
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using StageCreatorForSeason.Def;
using MyLib.Device;
using MyLib.Utility;
using MyLib.Utility.Action;
using System.Collections.Generic;
using MyLib;

namespace StageCreatorForSeason.Scene.ScenePages
{
    class Title : IScene
    {
        private InputState inputState;

        private List<Button> buttons;
        private Selector selector;

        private bool isEnd;

        public Title(GameDevice gameDevice) {
            inputState = gameDevice.GetInputState;

            buttons = new List<Button> {
                new Button("StageSelection1", new Vector2(Parameter.ScreenSize.X / 2 - 250, 380), new IWave_Y()),
                new Button("StageSelection2", new Vector2(Parameter.ScreenSize.X / 2 - 250, 600), new INone()),
                new Button("StageSelection3", new Vector2(Parameter.ScreenSize.X / 2 + 250, 380), new INone()),
                new Button("StageSelection4", new Vector2(Parameter.ScreenSize.X / 2 + 250, 600), new INone()),
            };
            selector = new Selector(4, false);
            selector.Initialize();
            isEnd = false;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() {
            isEnd = false;
            selector.Initialize();

            buttons.ForEach(b => b.SetAction(new INone()));
            buttons[0].SetAction(new IWave_Y());
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">時間</param>
        public void Update(GameTime gameTime) {
            buttons.ForEach(b => b.Update());

            InputCheck();
        }

        private void InputCheck() {
            if (inputState.WasDown(InputParameter.ConfirmKey, InputParameter.ConfirmButton)) {
                Parameter.SetStageNo(selector.GetSelection() + 1);
                Sound.PlaySE("Laser");
                isEnd = true;
            }
            if (inputState.WasDown(InputParameter.DownKey, InputParameter.DownButton)) {
                buttons[selector.GetSelection()].SetAction(new INone());

                selector.ToNext();
                buttons[selector.GetSelection()].SetAction(new IWave_Y());

                Sound.PlaySE("Shoot");
            }
            if (inputState.WasDown(InputParameter.UpKey, InputParameter.UpButton)) {
                buttons[selector.GetSelection()].SetAction(new INone());

                selector.ToBehind();
                buttons[selector.GetSelection()].SetAction(new IWave_Y());

                Sound.PlaySE("Shoot");
            }
        }


        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() {
            Renderer_2D.Begin();
            buttons.ForEach(b => b.Draw());
            Renderer_2D.End();
        }

        /// <summary>
        /// シーンを閉める
        /// </summary>
        public void Shutdown() { }

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
            return E_Scene.CREATING;
        }

    }
}
