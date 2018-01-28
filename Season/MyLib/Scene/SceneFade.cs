//作成日：　2017.03.14
//作成者：　柏
//クラス内容：　シーンのフェード管理用クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Device;

namespace MyLib.Scene
{
    public enum E_FadeSwitch {
        Off,
        On,
    }

    public enum E_FadeState {
        None,
        Out,
        In,
    }

    public class SceneFade
    {
        private float fadeAlpha;            // fadeIn/fadeOutの透明度
        private E_FadeSwitch fadeSwitch;    // fadeIn/fadeOutのスイッチ
        private E_FadeState fadeState;      // fadeIn or fadeOut
        private float unitFade;             // 単位fadeInOutの量

        public SceneFade() {
            fadeState = E_FadeState.None;
            unitFade = 0.03f;
            fadeAlpha = 0;
            Initalize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initalize() {
            fadeSwitch = E_FadeSwitch.Off;
            fadeState = E_FadeState.None;
        }

        /// <summary>
        /// 更新（fadeIn → fadeOut → Off）
        /// </summary>
        public void Update() {
            if (fadeSwitch == E_FadeSwitch.Off) { return; }
            fadeAlpha += unitFade;
            if (fadeAlpha > 1.0f) {
                unitFade *= -1;
                fadeState = E_FadeState.In;
            }
            else if (fadeAlpha < 0.0f) {
                unitFade *= -1;
                fadeAlpha = 0;
                fadeSwitch = E_FadeSwitch.Off;
                fadeState = E_FadeState.None;
            }
        }

        /// <summary>
        /// fadeSwitchの取得と設定
        /// </summary>
        public E_FadeSwitch GetFadeSwitch {
            get { return fadeSwitch; }
            set { fadeSwitch = value; }
        }

        /// <summary>
        /// fade状態の取得と設定
        /// </summary>
        public E_FadeState GetFadeState {
            get { return fadeState; }
            set { fadeState = value; }
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() {
            //Renderer_2D.DrawTexture("fade", Vector2.Zero, fadeAlpha);
        }
    }
}
