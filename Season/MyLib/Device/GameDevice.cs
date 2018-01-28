//作成日：　2017.03.14
//作成者：　柏
//クラス内容：  gameDeviceまとめ管理
//修正内容リスト：
//名前：柏　　　日付：2017.10.11　　　内容：パーティクル機能追加
//名前：柏　　　日付：2017.11.20　　　内容：RendererとSoundメソッドのstatic化によって修正
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MyLib.Device.ParticleManager;

namespace MyLib.Device
{
    public class GameDevice
    {
        private ResouceManager resouceManager;  //リソース管理用
        private Renderer_2D Renderer_2D;          //2D描画用
        private InputState inputState;      //入力用
        private Sound Sound;                //音声用
        private GraphicsDevice graphics;    //描画用
        private ParticleGroup paticles;   //パーティクル用

        public GameDevice(ContentManager content, GraphicsDevice graphics) {
            resouceManager = new ResouceManager(content);
            Renderer_2D = new Renderer_2D(content, graphics);
            inputState = new InputState();
            Sound = new Sound();
            paticles = new ParticleGroup(graphics, this);
            this.graphics = graphics;
            Initialize();
        }

        /// <summary>
        /// ロードシーンで使う最低限のリソースを読み込む
        /// </summary>
        public void Initialize() {
            //resouceManager.LoadEffect("TargetMark");
            //resouceManager.LoadTextures("number");
            //resouceManager.LoadTextures("Loading");
            //resouceManager.LoadBGM("title");
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update() {
            inputState.Update();
            paticles.Update();
        }

        /// <summary>
        /// リセット
        /// </summary>
        public void UnLoad() {
            Sound.Unload();
            resouceManager.UnLoad();
            paticles.Clear();
        }

        /// <summary>
        /// 入力デバイス取得
        /// </summary>
        public InputState GetInputState { get { return inputState; } }

        /// <summary>
        /// リソース管理取得
        /// </summary>
        public ResouceManager GetResouceManager { get { return resouceManager; } }

        /// <summary>
        /// パーティクル管理取得
        /// </summary>
        public ParticleGroup GetParticleGroup { get { return paticles; } }

        public int GetParticlesCount() {
            return paticles.GetParticlesCount();
        }
    }
}
