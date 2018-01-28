//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　Loadingシーン
//修正内容リスト：
//名前：柏　　　日付：20171103　　　内容：リソースリストの管理はCSVに
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyLib.Device;
using MyLib.Scene.Loaders;
using MyLib.Utility;
using StageCreatorForSeason.Def;
using System.Collections.Generic;

namespace StageCreatorForSeason.Scene.ScenePages
{
    class Loading : IScene
    {
        private ResouceManager resourceManager;
        private ResouceLoader resourceLoader;
        private bool isEnd;
        private Dictionary<string, string> resourceTypes;

        private int totalResouceNum;    //全種類リソース合計数

        private float startX;
        private float endX;

        /// <summary>
        /// リソースそろう
        /// </summary>
        /// <returns>リソースリスト</returns>
        private string[,] ResouceList()
        {
            CSVReader.Read("ResourceList");
            string[,] list = CSVReader.GetStringMatrix();
            for (int i = 0; i < list.GetLength(0); i++) {
                list[i, 1] = resourceTypes[list[i, 1]];
            }
            return list;
        }

        public Loading(GameDevice gameDevice) {
            resourceManager = gameDevice.GetResouceManager;
            resourceTypes = new Dictionary<string, string>()
            {
                { "image", "./IMAGE/" },
                { "effect", "./EFFECT/" },
                { "bgm", "./MP3/" },
                { "se", "./WAV/" },
                { "font", "./FONT/" },
            };
            resourceLoader = new ResouceLoader(resourceManager, ResouceList());

            startX = 200;
            endX = Parameter.ScreenSize.X - startX;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() {
            isEnd = false;
            resourceLoader.Initialize();
            totalResouceNum = resourceLoader.Count;

            resourceManager.LoadFont("HGPop");

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">時間</param>
        public void Update(GameTime gameTime) {
            //Sound.PlayBGM("title");
            if (!resourceLoader.IsEnd) { resourceLoader.Update(); }
            else { isEnd = true; }
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() {


            Renderer_2D.Begin();

            int currentCount = resourceLoader.CurrentCount;
            int pasent = (int)(currentCount / (float)totalResouceNum * 100f);

            //完成率を数字で表示
            if (totalResouceNum != 0) {
                Renderer_2D.DrawString(pasent + "%", new Vector2(1800, 1000), Color.White, 1.5f);
            }

            Renderer_2D.End();
        }

        /// <summary>
        /// シーンを閉める
        /// </summary>
        public void Shutdown() {
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
