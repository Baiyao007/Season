//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　シーンの管理クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：


using System.Collections.Generic;

using Microsoft.Xna.Framework;
using MyLib.Scene;
using MyLib.Device;

namespace StageCreatorForSeason.Scene
{
    class SceneManager
    {
        private Dictionary<E_Scene, IScene> scenes = new Dictionary<E_Scene, IScene>();
        private IScene currentScene;
        private E_Scene currentType;
        private SceneFade fade;
        private bool isFade;

        public SceneManager() {
            fade = new SceneFade();
            currentScene = null;
            currentType = E_Scene.NONE;
            isFade = false;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">時間</param>
        public void Update(GameTime gameTime) {
            if (currentScene == null) { return; }
            currentScene.Update(gameTime);

            if (currentScene.IsEnd()) {
                if (!isFade) {
                    fade.GetFadeSwitch = E_FadeSwitch.On;
                    isFade = true;
                }
                else {
                    if (fade.GetFadeState == E_FadeState.In) {
                        Change(currentScene.Next());
                    }
                }
            }
            if (fade.GetFadeSwitch == E_FadeSwitch.Off) { return; }
            fade.Update();
        }

        /// <summary>
        /// 新しいシーンの登録
        /// </summary>
        /// <param name="name">シーンの名前</param>
        /// <param name="scene">シーン</param>
        public void Add(E_Scene name, IScene scene) {
            if (scenes.ContainsKey(name)) { return; }
            scenes.Add(name, scene);
        }

        /// <summary>
        /// シーンを変える
        /// </summary>
        /// <param name="name">変え先</param>
        public void Change(E_Scene name) {
            if (currentScene != null) { currentScene.Shutdown(); }
            isFade = false;
            currentScene = scenes[name];
            currentType = name;
            currentScene.Initialize();
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() {
            if (currentScene == null) { return; }
            currentScene.Draw();
            if (fade.GetFadeSwitch == E_FadeSwitch.Off) { return; }
            fade.Draw();
        }

        public E_Scene GetSceneType() {
            return currentType;
        }
    }
}
