//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　Clearシーン
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib;
using MyLib.Device;
using MyLib.Utility;
using MyLib.Utility.Action.Movements;
using MyLib.Utility.Action.TheChange;
using Season.Def;

namespace Season.Scene.ScenePages
{
    class Clear : IScene
    {
        private InputState inputState;
        private GameDevice gameDevice;

        private bool isEnd;

        public Clear(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            inputState = gameDevice.GetInputState;
            isEnd = false;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() {
            CreatExplode();
            isEnd = false;
        }


        private void CreatExplode() {
            gameDevice.GetParticleGroup.AddParticles(
                "P_Sakura",         //name
                50, 80,             //count
                Parameter.ScreenSize / 2 + new Vector2(-100, -300),
                Parameter.ScreenSize / 2 + new Vector2(100, -100),     //position
                3.0f, 5.0f,         //speed
                1.5f, 3.0f,         //size
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
            Sound.PlayBGM("Clear");
            if (inputState.WasDown(InputParameter.ConfirmKey, InputParameter.ConfirmButton)) {
                Sound.PlaySE("Laser");
                isEnd = true;
            }
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="renderer">2D描画管理</param>
        public void Draw() {
            Renderer_2D.DrawTexture("Page_Clear", Vector2.Zero);
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
