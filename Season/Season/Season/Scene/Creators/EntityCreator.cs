using Microsoft.Xna.Framework;
using MyLib.Device;
using MyLib.Utility;
using Season.Components;
using Season.Components.ColliderComponents;
using Season.Components.DrawComponents;
using Season.Components.NormalComponents;
using Season.Components.UpdateComponents;
using Season.Entitys;
using Season.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Scene.Creators
{
    class EntityCreator
    {

        private GameDevice gameDevice;
        private Dictionary<string, Action<Vector2>> createWithPosition;
        private Dictionary<string, Action<int, int, int>> createWithLBC;

        public EntityCreator(GameDevice gameDevice) {
            this.gameDevice = gameDevice;

            createWithLBC = new Dictionary<string, Action<int, int, int>>() {
                { "Shrub", CreateShrub },
            };

            createWithPosition = new Dictionary<string, Action<Vector2>>() {
                { "Squirrel", CreateSquirrel },
                { "Bird", CreateBird },
                { "Boar", CreateBoar },
                { "Eagle", CreateEagle },
                { "Branch", CreateBranch }
            };
        }

        public void CreateEntitys(int stageNo) {
            EntityManager.Clear();
            TaskManager.CloseAllTask();
            CreatPlayer();
            CreateWithPosition(stageNo);
            CreateWithLBC(stageNo);
        }

        private void CreatPlayer()
        {
            //生成地を決める
            Transform2D trans = new Transform2D();
            trans.Position = new Vector2(500, 100);

            //実体生成
            Entity player = Entity.CreateEntity("Player", "Player", trans);

            //機能登録(順序注意 - 関連性ある)
            player.RegisterComponent(new C_Switch3());
            C_BezierPoint bezier = new C_BezierPoint();
            player.RegisterComponent(bezier);
            player.RegisterComponent(new C_EntityDeadCheck());
            player.RegisterComponent(new C_PlayerState());
            player.RegisterComponent(new C_DrawRouteEffect());
            //player.RegisterComponent(new C_DrawLocus());
            //player.RegisterComponent(new C_DrawDebugMessage(bezier));

            C_DrawAnimetion drawComp = new C_DrawAnimetion(Vector2.One * 256, 16);
            drawComp.AddAnim("Idle", new AnimData(1, 1, 1, "A_Player_Idle"));
            drawComp.AddAnim("Walk", new AnimData(4, 4, 0.12f, "A_Player_Walk"));
            drawComp.AddAnim("Jump", new AnimData(2, 3, 0.1f, "A_Player_Jump", false));
            drawComp.AddAnim("Run", new AnimData(13, 4, 0.05f, "A_Player_Run"));
            drawComp.AddAnim("Attack", new AnimData(1, 1, 1, "A_Player_Attack"));
            player.RegisterComponent(drawComp);
            drawComp.SetNowAnim("Idle");

            player.RegisterComponent(new C_Energy("Bar", 10, 270));
            player.RegisterComponent(new C_UpdatePlayerState(gameDevice));
        }

        #region CreateFun
        private void CreateWithPosition(int stageNo) {
            CSVReader.Read("EntityPositionData_S" + stageNo);
            List<string[]> data = CSVReader.GetData();
            for (int i = 0; i < data.Count; i++) {
                createWithPosition[data[i][0]](new Vector2(int.Parse(data[i][1]), int.Parse(data[i][2])));
            }
        }
        private void CreateWithLBC(int stageNo) {
            CSVReader.Read("EntityLbcData_S" + stageNo);
            List<string[]> data = CSVReader.GetData();
            for (int i = 0; i < data.Count; i++) {
                createWithLBC[data[i][0]](int.Parse(data[i][1]), int.Parse(data[i][2]), int.Parse(data[i][3]));
            }
        }
        #endregion

        #region CreateWithPositon
        private void CreateBranch(Vector2 position)
        {
            Transform2D trans = new Transform2D();
            trans.Position = position;

            Entity branch = Entity.CreateEntity("Branch", "Branch", trans);
            branch.RegisterComponent(new C_Switch3());

            C_DrawAnimetion drawComp = new C_DrawAnimetion(new Vector2(30, 200), 12);
            drawComp.AddAnim("Idle", new AnimData(1, 1, 1, "A_Branch_Idle", false));
            drawComp.AddAnim("Break", new AnimData(4, 4, 0.08f, "A_Branch_Break", false));
            branch.RegisterComponent(drawComp);
            drawComp.SetNowAnim("Idle");

            //Message用
            //C_BezierPoint bezier = new C_BezierPoint();
            //branch.RegisterComponent(bezier);
            //branch.RegisterComponent(new C_DrawDebugMessage(bezier));

            branch.RegisterComponent(new C_UpdateBranchState(gameDevice));
        }
        private void CreateBoar(Vector2 position)
        {
            Transform2D trans = new Transform2D();
            trans.Position = position;

            Entity boar = Entity.CreateEntity("Boar", "Enemy", trans);

            boar.RegisterComponent(new C_Switch3());
            boar.RegisterComponent(new C_BezierPoint());
            boar.RegisterComponent(new C_EntityDeadCheck());
            boar.RegisterComponent(new C_UpdateBoarState(gameDevice));

            C_DrawAnimetion drawComp = new C_DrawAnimetion(new Vector2(320, 200), 19);
            drawComp.AddAnim("Run", new AnimData(6, 3, 0.08f, "A_Boar_Run"));
            boar.RegisterComponent(drawComp);
            drawComp.SetNowAnim("Run");
        }
        private void CreateSquirrel(Vector2 position)
        {
            Transform2D trans = new Transform2D();
            trans.Position = position;

            Entity child = Entity.CreateEntity("Squirrel", "Child", trans);

            child.RegisterComponent(new C_Switch3());
            child.RegisterComponent(new C_ChildState());
            C_BezierPoint bezier = new C_BezierPoint();
            child.RegisterComponent(bezier);
            //child.RegisterComponent(new C_DrawDebugMessage(bezier));
            child.RegisterComponent(new C_EntityDeadCheck());
            child.RegisterComponent(new C_Energy("", 10, 90));

            C_DrawAnimetion drawComp = new C_DrawAnimetion(new Vector2(80, 60), 14);
            drawComp.AddAnim("Run", new AnimData(7, 7, 0.08f, "A_Squirrel_Run"));
            drawComp.AddAnim("Eat", new AnimData(1, 1, 1f, "A_Squirrel_Eat"));
            drawComp.AddAnim("Catch", new AnimData(1, 1, 1f, "A_Squirrel_Catch"));
            child.RegisterComponent(drawComp);
            drawComp.SetNowAnim("Run");

            child.RegisterComponent(new C_UpdateSquirrelState(gameDevice));
        }
        private void CreateBird(Vector2 position)
        {
            Transform2D trans = new Transform2D();
            trans.Position = position;

            Entity child = Entity.CreateEntity("Bird", "Child", trans);

            child.RegisterComponent(new C_Switch3());
            child.RegisterComponent(new C_ChildState());
            child.RegisterComponent(new C_EntityDeadCheck());
            child.RegisterComponent(new C_UpdateBirdState(gameDevice));
            //child.RegisterComponent(new C_DrawDebugMessage());

            C_DrawAnimetion drawComp = new C_DrawAnimetion(new Vector2(80, 60), 14);
            drawComp.AddAnim("Fly", new AnimData(6, 6, 0.08f, "A_Bird_Fly"));
            child.RegisterComponent(drawComp);
            drawComp.SetNowAnim("Fly");
        }
        private void CreateEagle(Vector2 position)
        {
            Transform2D trans = new Transform2D();
            trans.Position = position;

            Entity eagle = Entity.CreateEntity("Eagle", "Enemy", trans);

            eagle.RegisterComponent(new C_Switch3());
            eagle.RegisterComponent(new C_EntityDeadCheck());
            eagle.RegisterComponent(new C_UpdateEagleState(gameDevice));

            C_DrawAnimetion drawComp = new C_DrawAnimetion(new Vector2(320, 200), 19);
            drawComp.AddAnim("Fly", new AnimData(5, 3, 0.15f, "A_Eagle_Fly"));
            drawComp.AddAnim("Attack", new AnimData(1, 1, 1f, "A_Eagle_Attack"));
            eagle.RegisterComponent(drawComp);
            drawComp.SetNowAnim("Fly");
        }
        #endregion

        #region CreateWithLBC
        private void CreateShrub(int l, int b, int c)
        {
            //実体生成
            Entity shrub = Entity.CreateEntity("Tree", "Tree", new Transform2D());

            //位置設定
            C_BezierPoint bezier = new C_BezierPoint();
            shrub.RegisterComponent(bezier);
            bezier.SetBezierData(l, b, c);
            bezier.SetRoute(BezierStage.GetNowRoute(l, b));
            shrub.transform.Position = bezier.GetNowPosition();

            shrub.RegisterComponent(new C_Switch3());

            C_DrawAnimetion drawComp = new C_DrawAnimetion(new Vector2(452, 276), 12);
            drawComp.AddAnim("Pollution", new AnimData(1, 1, 1, "A_Shrub_Pollution"));
            drawComp.AddAnim("Spring", new AnimData(1, 1, 1, "A_Shrub_Spring"));
            drawComp.AddAnim("Summer", new AnimData(1, 1, 1, "A_Shrub_Summer"));
            drawComp.AddAnim("Autumn", new AnimData(1, 1, 1, "A_Shrub_Autumn"));
            drawComp.AddAnim("Winter", new AnimData(1, 1, 1, "A_Shrub_Winter"));
            drawComp.AddAnim("Hidden", new AnimData(1, 1, 1, "A_Shrub_Hidden"));
            shrub.RegisterComponent(drawComp);
            drawComp.SetNowAnim("Pollution");

            shrub.RegisterComponent(new C_UpdateShrubState(gameDevice));
        }
        #endregion

    }


}
