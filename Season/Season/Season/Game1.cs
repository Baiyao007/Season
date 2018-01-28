////作成日：　2017.11.28
//作成者：　柏
//クラス内容：　メインクラス
//修正内容リスト：
//名前：柏　　　日付：20171011　　　内容：カメラ対応
//名前：　　　日付：　　　内容：


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MyLib.Device;
using Season.Def;
using Season.Entitys;
using Season.Scene;
using Season.Scene.ScenePages;

namespace Season
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private GameDevice gameDevice;
        private SceneManager sceneManager;
        private Camera2D camera2D;
        //private BloomComponent bloom;

        public Game1()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            graphicsDeviceManager.PreferredBackBufferHeight = (int)Parameter.ScreenSize.Y;
            graphicsDeviceManager.PreferredBackBufferWidth = (int)Parameter.ScreenSize.X;

            
            graphicsDeviceManager.IsFullScreen = false;
        }


        protected override void Initialize()
        {
            gameDevice = new GameDevice(Content, graphicsDeviceManager.GraphicsDevice);
            camera2D = new Camera2D(GraphicsDevice.Viewport, Parameter.StageSize);
            //bloom = new BloomComponent(this);
            //Components.Add(bloom);
            //bloom.Settings = new BloomSettings(null, 0.25f, 4, 2, 1, 1.5f, 1);


            //シーン設定
            sceneManager = new SceneManager();
            sceneManager.Add(E_Scene.LOADING, new Loading(gameDevice));
            sceneManager.Add(E_Scene.TITLE, new Title(gameDevice));
            sceneManager.Add(E_Scene.GAMEPLAY, new GamePlay(gameDevice));
            sceneManager.Add(E_Scene.OPERATE, new Operate(gameDevice));
            sceneManager.Add(E_Scene.STAFFROLL, new StaffRoll(gameDevice));
            sceneManager.Add(E_Scene.ENDING, new Ending(gameDevice));
            sceneManager.Add(E_Scene.CLEAR, new Clear(gameDevice));
            sceneManager.Change(E_Scene.LOADING);

            Window.Title = "Season";

            //Windowの大きさチェンジ
            //Window.AllowUserResizing = false;
            //Window.BeginScreenDeviceChange(false);
            //Window.EndScreenDeviceChange(Window.ScreenDeviceName, (int)(Parameter.ScreenSize.X * 1.5f), (int)(Parameter.ScreenSize.Y * 1.5f));

            base.Initialize();
        }


        protected override void LoadContent() { }


        protected override void UnloadContent()
        {
            gameDevice.UnLoad();
            TaskManager.CloseAllTask();
            TaskManager.Update();
            EntityManager.Clear();
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape)
                )
            {
                UnloadContent();
                Exit();
            } 

            gameDevice.Update();
            sceneManager.Update(gameTime);
            TaskManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //bloom.BeginDraw();
            GraphicsDevice.Clear(Color.Black);

            //if (sceneManager.GetSceneType() == E_Scene.GAMEPLAY)
            //{
            //    Renderer_2D.BeginBlend();
            //    sceneManager.Draw();
            //    Renderer_2D.End();
            //    base.Draw(gameTime);
            //}
            //else {
            //    base.Draw(gameTime);
            //    Renderer_2D.Begin();
            //    sceneManager.Draw();
            //    Renderer_2D.End();
            //}

            base.Draw(gameTime);

            TaskManager.Draw();

            Renderer_2D.Begin();
            sceneManager.Draw();
            Renderer_2D.End();

            //３D向けの設定変更
            //SpriteBatchが変更した設定を元に戻す。（今回はカリングの設定のみでOK）
            //GraphicsDevice.BlendState = BlendState.Opaque;
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            gameDevice.GetParticleGroup.Draw();

            
        }
    }
}
