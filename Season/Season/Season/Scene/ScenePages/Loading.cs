//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　Loadingシーン
//修正内容リスト：
//名前：柏　　　日付：20171103　　　内容：リソースリストの管理はCSVに
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Device;
using MyLib.Scene.Loaders;
using MyLib.Utility;
using MyLib.Utility.Action.Movements;
using MyLib.Utility.Action.TheChange;
using Season.Components.DrawComponents;
using Season.Components.NormalComponents;
using Season.Components.UpdateComponents;
using Season.Def;
using Season.Entitys;
using System.Collections.Generic;

namespace Season.Scene.ScenePages
{
    class Loading : IScene
    {
        private ResouceManager resourceManager;
        private ResouceLoader resourceLoader;
        private bool isEnd;
        private Dictionary<string, string> resourceTypes;

        private int totalResouceNum;    //全種類リソース合計数

        private Entity entity;

        private float startX;
        private float endX;

        private Timer effectTimer;
        private GameDevice gameDevice;

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
            this.gameDevice = gameDevice;
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

            startX = 100;
            endX = Parameter.ScreenSize.X - startX;

            effectTimer = new Timer(0.15f); //effectの生成間隔を設定
            effectTimer.Dt = new Timer.timerDelegate(CreatSeasonEffect);
        }

        private void CreatSeasonEffect()
        {
            if (entity == null) { return; }
            Vector2 creatPosition = entity.transform.Position - new Vector2(0, 180);

            gameDevice.GetParticleGroup.AddParticles(
                "P_Leaf",           //name
                1, 1,                               //count
                creatPosition - new Vector2(65, 100),
                creatPosition + new Vector2(135, 100),       //position
                0.6f, 1.5f,         //speed
                0.15f, 0.4f,         //size
                1, 1,               //alpha
                230, 250,           //angle
                2.0f, 2.0f,         //alive
                new MoveLine(),     //moveType
                new ChangeToLucency(new Timer(2))   //changeType
            );

            gameDevice.GetParticleGroup.AddParticles(
                "P_Rain",           //name
                1, 1,                               //count
                creatPosition - new Vector2(65, 100),
                creatPosition + new Vector2(135, 100),       //position
                1.5f, 2.0f,         //speed
                0.15f, 0.4f,         //size
                1, 1,               //alpha
                230, 250,           //angle
                2.0f, 2.0f,         //alive
                new MoveLine(),     //moveType
                new ChangeToLucency(new Timer(2))   //changeType
            );
        }


        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() {
            isEnd = false;
            resourceLoader.Initialize();
            totalResouceNum = resourceLoader.Count;

            resourceManager.LoadFont("HGPop");
            resourceManager.LoadTextures("A_Player_Run", "./IMAGE/Animetions/");
            for (int i = 0; i < 4; i++) {
                resourceManager.LoadTextures("P_Flower_" + i, "./IMAGE/Particles/");
            }
            resourceManager.LoadTextures("P_Rain", "./IMAGE/Particles/");
            resourceManager.LoadTextures("P_Leaf", "./IMAGE/Particles/");
            resourceManager.LoadEffect("PaticleShader", "./EFFECT/");
            resourceManager.LoadEffect("MaskShader", "./EFFECT/");


            entity = Entity.CreateEntity("Empty", "Empty", new Transform2D());
            entity.transform.Position = new Vector2(startX, 1020);

            entity.RegisterComponent(new C_Switch3());
            C_BezierPoint bezier = new C_BezierPoint();
            entity.RegisterComponent(bezier);

            C_DrawAnimetion drawComp = new C_DrawAnimetion(Vector2.One * 256, 16);
            drawComp.AddAnim("Run", new AnimData(13, 4, 0.02f, "A_Player_Run"));
            entity.RegisterComponent(drawComp);
            drawComp.SetNowAnim("Run");
            drawComp.SetSize(0.7f);

            C_SeasonState season = new C_SeasonState(eSeason.Summer);
            C_DrawRouteEffect route = new C_DrawRouteEffect();
            route.RegisterImage("P_Flower_", 4);
            route.SetState(0.02f, 10, -85);
            route.SetAliveSecond(2);
            route.SetSize(new Vector2(0.5f, 0.5f));
            entity.RegisterComponent(season);
            entity.RegisterComponent(route);

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">時間</param>
        public void Update(GameTime gameTime) {
            //Sound.PlayBGM("title");
            effectTimer.Update();
            if (!resourceLoader.IsEnd) { resourceLoader.Update(); }
            else { isEnd = true; }
        }

        /// <summary>
        /// 描画
        /// </summary>
        public void Draw() {
            //Renderer_2D.DrawTexture("Loading", Vector2.Zero);

            int currentCount = resourceLoader.CurrentCount;
            int pasent = (int)(currentCount / (float)totalResouceNum * 100f);

            //完成率を表示
            entity.transform.SetPositionX = startX + (endX - startX) * pasent / 100;

            //完成率を数字で表示
            if (totalResouceNum != 0) {
                Renderer_2D.DrawString(pasent + "%", new Vector2(1800, 1000), Color.White, 1.5f);
            }
        }

        /// <summary>
        /// シーンを閉める
        /// </summary>
        public void Shutdown() {
            entity.DeActive();
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
