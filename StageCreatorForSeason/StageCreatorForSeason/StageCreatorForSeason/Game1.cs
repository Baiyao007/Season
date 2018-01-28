using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MyLib.Device;
using StageCreatorForSeason.Scene;
using StageCreatorForSeason.Def;
using StageCreatorForSeason.Scene.ScenePages;

namespace StageCreatorForSeason
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private GameDevice gameDevice;
        private SceneManager sceneManager;
        private Camera2D camera2D;

        public Game1()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphicsDeviceManager.PreferredBackBufferHeight = (int)Parameter.ScreenSize.Y;
            graphicsDeviceManager.PreferredBackBufferWidth = (int)Parameter.ScreenSize.X;

            graphicsDeviceManager.IsFullScreen = false;
        }

        protected override void Initialize() {
            gameDevice = new GameDevice(Content, graphicsDeviceManager.GraphicsDevice);
            camera2D = new Camera2D(GraphicsDevice.Viewport, Parameter.StageSize);

            //ÉVÅ[Éìê›íË
            sceneManager = new SceneManager();
            sceneManager.Add(E_Scene.LOADING, new Loading(gameDevice));
            sceneManager.Add(E_Scene.TITLE, new Title(gameDevice));
            sceneManager.Add(E_Scene.CREATING, new Creating(gameDevice));
            sceneManager.Change(E_Scene.LOADING);

            Window.Title = "StageCreator";
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent() { }


        protected override void UnloadContent() {
            gameDevice.UnLoad();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) this.Exit();

            gameDevice.Update();
            sceneManager.Update(gameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            sceneManager.Draw();
            
            GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            gameDevice.GetParticleGroup.Draw();

            base.Draw(gameTime);
        }
    }
}
