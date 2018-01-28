//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　Titleシーン
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using Season.Def;
using MyLib.Device;
using MyLib.Utility;
using MyLib.Utility.Action;
using System.Collections.Generic;
using MyLib;

namespace Season.Scene.ScenePages
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
                new Button("Text_GameStart", new Vector2(Parameter.ScreenSize.X / 2, 680), new IWave_Y()),
                new Button("Text_Operation", new Vector2(Parameter.ScreenSize.X / 2, 800), new INone()),
                new Button("Text_StaffScroll", new Vector2(Parameter.ScreenSize.X / 2, 920), new INone())
            };
            selector = new Selector(3, false);
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
            Sound.PlayBGM("Title");
            buttons.ForEach(b => b.Update());

            InputCheck();
        }

        private void InputCheck() {
            if (inputState.WasDown(InputParameter.ConfirmKey, InputParameter.ConfirmButton))
            {
                Sound.PlaySE("Laser");
                isEnd = true;
            }
            if (inputState.WasDown(InputParameter.DownKey, InputParameter.DownButton))
            {
                buttons[selector.GetSelection()].SetAction(new INone());

                selector.ToNext();
                buttons[selector.GetSelection()].SetAction(new IWave_Y());

                Sound.PlaySE("Shoot");
            }
            if (inputState.WasDown(InputParameter.UpKey, InputParameter.UpButton))
            {
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
            Renderer_2D.DrawTexture("Page_Title", Vector2.Zero);

            buttons.ForEach(b => b.Draw());
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
            switch (selector.GetSelection())
            {
                case 0: return E_Scene.GAMEPLAY;
                case 1: return E_Scene.OPERATE;
                case 2: return E_Scene.STAFFROLL;
                default: return E_Scene.GAMEPLAY;
            }
        }

    }
}
