////�쐬���F�@2017.11.28
//�쐬�ҁF�@��
//�N���X���e�F�@���C���N���X
//�C�����e���X�g�F
//���O�F���@�@�@���t�F20171011�@�@�@���e�F�J�����Ή�
//���O�F�@�@�@���t�F�@�@�@���e�F


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


            //�V�[���ݒ�
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

            //Window�̑傫���`�F���W
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

            //�RD�����̐ݒ�ύX
            //SpriteBatch���ύX�����ݒ�����ɖ߂��B�i����̓J�����O�̐ݒ�݂̂�OK�j
            //GraphicsDevice.BlendState = BlendState.Opaque;
            //GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            gameDevice.GetParticleGroup.Draw();

            
        }
    }
}
