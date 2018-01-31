//作成日：　2018.01.11
//作成者：　柏
//クラス内容：　GamePlayシーン
//修正内容リスト：
//名前：柏　　　日付：20171011　　　内容：カメラ対応
//名前：　　　日付：　　　内容：

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using StageCreatorForSeason.Def;
using MyLib.Utility;
using StageCreatorForSeason.Objects;
using System.Collections.Generic;

namespace StageCreatorForSeason.Scene.ScenePages
{
    class Creating : IScene
    {
        private GameDevice gameDevice;
        private InputState inputState;

        private Vector2 focus;
        private ObjectManager objectManager;
        private DrawStage drawStage;

        private ToolBar bar;
        private List<Keys> barKeys;
        private bool isEnd;
        private bool isPause;

        public Creating(GameDevice gameDevice) {
            this.gameDevice = gameDevice;
            inputState = gameDevice.GetInputState;

            isEnd = false;
            isPause = false;
            focus = Parameter.ScreenSize;
            bar = new ToolBar();
            objectManager = new ObjectManager(bar);
            barKeys = new List<Keys>() {
                Keys.D1, Keys.D2, Keys.D3,
                Keys.D4, Keys.D5, Keys.D6,
                Keys.D7, Keys.D8, Keys.D9,
                Keys.D0
            };
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() {
            isEnd = false;
            isPause = false;
            focus = Parameter.ScreenSize;
            Camera2D.ZoomInitialize();
            InitializeStage();
        }

        public void InitializeStage() {
            objectManager.Clear();
            objectManager.InitializeStage(Parameter.StageNo);

            CSVReader.Read("StageImgCount");
            int imgCount = CSVReader.GetIntData()[Parameter.StageNo - 1][0];
            Parameter.SetStageWidth(imgCount);
            drawStage = new DrawStage("Stage" + Parameter.StageNo + "Map_", imgCount);
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">時間</param>
        public void Update(GameTime gameTime) {
            if (inputState.WasDown(Keys.Enter)) {
                isEnd = true;
                return;
            }
            if (inputState.WasDown(Keys.C)) {
                objectManager.Save(Parameter.StageNo);
            }

            if (inputState.WasDown(Keys.P)) {
                isPause = !isPause;
            }
            if (isPause) { return; }

            if (inputState.WasDown(Keys.OemMinus)) { bar.ToNext(); }
            if (inputState.WasDown(Keys.OemPlus)) { bar.ToBefore(); }

            for (int i = 0; i < barKeys.Count; i++) {
                if (inputState.WasDown(barKeys[i])) { bar.SetNowTool(i); }
            }

            CameraAction();
            objectManager.Update();
        }

        public void CameraAction() {
            Camera2D.Update(focus);

            if (inputState.IsDown(Keys.Left)) {
                focus.X -= 30;
            }
            if (inputState.IsDown(Keys.Right)) {
                focus.X += 30;
            }
            if (inputState.IsDown(Keys.Up)) {
                focus.Y -= 30;
            }
            if (inputState.IsDown(Keys.Down)) {
                focus.Y += 30;
            }
            focus = Vector2.Clamp(focus, Vector2.Zero, Parameter.StageSize);

            if (inputState.WasDown(Keys.W)) {
                Camera2D.ZoomInitialize();
            }
            else if (inputState.IsDown(Keys.S)) {
                Camera2D.ZoomOut();
            }
        }


        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() {
            drawStage.Draw();
            objectManager.Draw();
            bar.Draw();

            DrawFocus();
            DrawDebug();
            DrawHelpPage();
        }

        private void DrawHelpPage() {
            if (isPause) {
                Renderer_2D.Begin();
                Vector2 imgSize = ResouceManager.GetTextureSize("HelpPage");
                Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
                Renderer_2D.DrawTexture("HelpPage", Parameter.ScreenSize / 2, Color.White, 1, rect, Vector2.One, 0, imgSize / 2);
                Renderer_2D.End();
            }
        }

        private void DrawFocus() {
            Renderer_2D.Begin(Camera2D.GetTransform());
            Vector2 imgSize = ResouceManager.GetTextureSize("Focus");
            Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
            Renderer_2D.DrawTexture("Focus", focus, Color.Yellow, 0.2f, rect, Vector2.One, 0, imgSize / 2);
            Renderer_2D.End();
        }

        private void DrawDebug() {
            Renderer_2D.Begin();
            Renderer_2D.DrawString("Stage1", Vector2.One * 20, Color.Red, 1.8f);
            Renderer_2D.DrawString("ObjectsCount:" + objectManager.ObjectsCount(), new Vector2(20, 100), Color.Red, 1.2f);
            Renderer_2D.DrawString("Position:" + focus, new Vector2(1440, 60), Color.Red, 1.1f);
            Renderer_2D.DrawString("Page:" + (int)(focus.X / 2000), new Vector2(1440, 120), Color.Red, 1.1f);
            Renderer_2D.End();
        }

        /// <summary>
        /// シーンを閉める
        /// </summary>
        public void Shutdown() {
            gameDevice.GetParticleGroup.Clear();
            Camera2D.Update(Parameter.ScreenSize / 2);
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
        public E_Scene Next() {
            return E_Scene.TITLE;
        }
    }
}
