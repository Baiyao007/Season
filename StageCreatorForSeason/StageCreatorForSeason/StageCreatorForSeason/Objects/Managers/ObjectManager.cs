using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using MyLib.Utility;
using StageCreatorForSeason.Def;
using StageCreatorForSeason.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageCreatorForSeason.Objects
{
    class ObjectManager
    {
        private MyMouse mouse;
        private Dictionary<string, eObjectType> typeChange;

        private BezierManager bezierManager;

        private Dictionary<eObjectType, Func<Vector2, Object>> creatFuncs;
        private Dictionary<eObjectType, List<Object>> mapPointsList;
        private List<Object> entitysList;
        private List<List<Object>> hintsList;

        private int bezierIndex;
        private int wallIndex;
        private ToolBar bar;

        public ObjectManager(ToolBar bar) {
            mouse = new MyMouse(this);
            this.bar = bar;

            mapPointsList = new Dictionary<eObjectType, List<Object>>() {
                { eObjectType.BezierStage, new List<Object>()},
                { eObjectType.Wall, new List<Object>()},
            };

            creatFuncs = new Dictionary<eObjectType, Func<Vector2, Object>> {
                { eObjectType.BezierStage,  CreatBezierPoint },
                { eObjectType.Wall,  CreatWallPoint },
                { eObjectType.Boar,  CreatEntity },
                { eObjectType.Eagle,  CreatEntity },
                { eObjectType.Shrub,  CreatEntity },
                { eObjectType.F,CreatHint },
                { eObjectType.X,CreatHint },
                { eObjectType.Z,CreatHint },
                { eObjectType.Jump,CreatHint },
                { eObjectType.Right,CreatHint },
                { eObjectType.Left,CreatHint },
                { eObjectType.HintNoLeft,CreatHint },
                { eObjectType.HintNoRight,CreatHint },
                { eObjectType.HintJump,CreatHint },
                { eObjectType.HintDown,CreatHint },
            };

            typeChange = new Dictionary<string, eObjectType>() {
                { "Boar", eObjectType.Boar },
                { "Eagle", eObjectType.Eagle },
                { "Shrub", eObjectType.Shrub },
                { "Squirrel", eObjectType.Squirrel },
            };

            entitysList = new List<Object>();
            hintsList = new List<List<Object>>();

            bezierManager = new BezierManager(mapPointsList[eObjectType.BezierStage]);
            bezierIndex = 0;
            wallIndex = 0;
        }

        private Hint CreatHint(Vector2 position) {
            hintsList.Add(new List<Object>());
            Hint hint = new Hint(position, (eObjectType)bar.GetNowTool());
            int lastOne = hintsList.Count - 1;
            hintsList[lastOne].Add(hint);
            hintsList[lastOne].Add(hint.GetLeftTop());
            hintsList[lastOne].Add(hint.GetRightBottom());
            return hint;
        }

        private BezierPoint CreatBezierPoint(Vector2 position) {
            return new BezierPoint(position, bezierIndex);
        }

        private WallPoint CreatWallPoint(Vector2 position) {
            wallIndex++;
            mapPointsList[eObjectType.Wall].Add(new WallPoint(position + new Vector2(0,-100), wallIndex));
            return new WallPoint(position + new Vector2(0, 100), wallIndex);
        }

        private Object CreatEntity(Vector2 position) {
            return new Object(
                ((eObjectType)bar.GetNowTool()).ToString(), 
                position, 50, 
                (eObjectType)bar.GetNowTool()
            );
        }


        public void CreatObject(eObjectType type, Vector2 position) {
            if ((int)type < 2) {
                mapPointsList[type].Add(creatFuncs[type](position));
            }
            else if ((int)type >= 10) {
                creatFuncs[type](position);
            }
            else {
                entitysList.Add(creatFuncs[type](position));
            }
            
        }


        public void InitializeStage(int stageNo) {
            //初期化ルート
            List<List<Vector2>> bezierPoints = BezierStage.InitializeStage(Parameter.StageNo);
            for (int i = 0; i < bezierPoints.Count; i++) {
                for (int j = 0; j < bezierPoints[i].Count; j++) {
                    CreatObjectFromData(eObjectType.BezierStage, bezierPoints[i][j], i);
                }
            }
            bezierManager.SetBezierPoints(bezierPoints);


            //初期化実体
            entitysList.Clear();
            CSVReader.Read("EntityPositionData_S" + stageNo);
            List<string[]> result = CSVReader.GetData();

            for (int i = 0; i < result.Count; i++) {
                Vector2 position = new Vector2(int.Parse(result[i][1]), int.Parse(result[i][2]));
                eObjectType type = typeChange[result[i][0]];
                entitysList.Add(new Object(result[i][0], position, 50, type));
            }


            //初期化ヒント
            hintsList.Clear();
            CSVReader.Read("HintsData_S" + stageNo);
            result = CSVReader.GetData();

            for (int i = 0; i < result.Count; i++) {
                Hint hint = new Hint(new Vector2(int.Parse(result[i][1]), int.Parse(result[i][2])), Hint.NameToType(result[i][0]));
                Vector2 center = new Vector2(int.Parse(result[i][3]), int.Parse(result[i][4]));
                Vector2 size = new Vector2(int.Parse(result[i][5]), int.Parse(result[i][6]));

                hint.SetLeft(center - size / 2);
                hint.SetRight(center + size / 2);
                hintsList.Add(new List<Object>());
                int lastOne = hintsList.Count - 1;
                hintsList[lastOne].Add(hint);
                hintsList[lastOne].Add(hint.GetLeftTop());
                hintsList[lastOne].Add(hint.GetRightBottom());
            }

        }

        private void CreatObjectFromData(eObjectType type, Vector2 position, int index) {
            BezierPoint obj = new BezierPoint(position, index);
            mapPointsList[type].Add(obj);
        }

        public void DeleteObjects(List<Object> objs) {
            for (int i = 0; i < objs.Count(); i++) {
                if (CanDelectBezier(objs[i])) {
                    objs.Remove(objs[i]);
                    i--;
                    continue;
                }

                if (CanDelectWall(objs[i])) {
                    objs.Remove(objs[i]);
                    i--;
                    continue;
                }

                if (CanDelectEntity(objs[i])) {
                    objs.Remove(objs[i]);
                    i--;
                    continue;
                }

                if (CanDelectHint(objs[i], objs)) {
                    i = 0;
                    continue;
                }
            }
        }

        private bool CanDelectHint(Object obj, List<Object> objs) {
            int typeNo = (int)obj.GetType();
            if (typeNo < 10 || typeNo >= 20) { return false; }

            int index = 0;
            for (int x = 0; x < hintsList.Count; x++) {
                for (int y = 0; y < 3; y++) {
                    if (hintsList[x][y] == obj ) {
                        index = x;
                    }
                }
            }
            
            for (int j = 0; j < 3; j++) {
                for (int i = 0; i < objs.Count(); i++)
                {
                    if (objs[i] == hintsList[index][j]) {
                        objs.Remove(objs[i]);
                        i--;
                    }
                }
            }
            hintsList.RemoveAt(index);

            return true;
        }

        private bool CanDelectEntity(Object obj) {
            if (obj.GetType() == eObjectType.BezierStage) { return false; }
            if (obj.GetType() == eObjectType.Wall) { return false; }
            int typeNo = (int)obj.GetType();
            if (typeNo >= 10 && typeNo < 20) { return false; }
            entitysList.Remove(obj);
            return true;
        }


        private bool CanDelectBezier(Object obj) {
            if (obj.GetType() != eObjectType.BezierStage) { return false; }
            mapPointsList[eObjectType.BezierStage].Remove(obj);
            return true;
        }

        private bool CanDelectWall(Object obj) {
            if (obj.GetType() != eObjectType.Wall) { return false; }
            int teamNo = obj.GetTeamNo();
            mapPointsList[eObjectType.Wall].RemoveAll(o => o.GetTeamNo() == teamNo);
            return true;
        }



        public void Clear() {
            for (int i = 0; i < mapPointsList.Count; i++) {
                mapPointsList[(eObjectType)i].Clear();
            }
            entitysList.Clear();
            hintsList.Clear();
        }
        public int ObjectsCount() {
            int count = 0;
            for (int i = 0; i < mapPointsList.Count; i++) {
                count += mapPointsList[(eObjectType)i].Count;
            }
            return count;
        }

        public void Save(int stageNo) {
            SavePoints(stageNo);
            SaveEntitys(stageNo);
            SaveHints(stageNo);
        }

        private void SaveHints(int stageNo) {
            List<string> saveList = new List<string>();

            for (int i = 0; i < hintsList.Count; i++) {
                string data = ((Hint)hintsList[i][0]).Print();
                saveList.Add(data);
            }

            CSVReader.Save("HintsData_S" + stageNo, saveList);
        }

        private void SavePoints(int stageNo) {
            List<List<Vector2>> saveList = bezierManager.GetBezierPoints();
            List<List<Vector2>> wallList = saveList.FindAll(x => x.Count == 2);
            saveList.RemoveAll(x => x.Count == 0 || x.Count == 2);
            saveList.Sort((x, y) => x[0].X.CompareTo(y[0].X));
            for (int i = 0; i < mapPointsList[eObjectType.Wall].Count; i += 2) {
                saveList.Add(new List<Vector2>());
                saveList[saveList.Count - 1].Add(mapPointsList[eObjectType.Wall][i].Position);
                saveList[saveList.Count - 1].Add(mapPointsList[eObjectType.Wall][i + 1].Position);
            }
            saveList.AddRange(wallList);

            CSVReader.Save("PointData_S" + stageNo, saveList);
        }

        private void SaveEntitys(int stageNo) {
            List<string> saveList = new List<string>();
 
            for (int i = 0; i < entitysList.Count;i++) {
                Object obj = entitysList[i];
                string data = obj.GetName() + "," + (int)obj.PostionX + "," + (int)obj.PostionY;
                saveList.Add(data);
            }

            CSVReader.Save("EntityPositionData_S" + stageNo, saveList);
        }

        public void Update() {
            if (Camera2D.GetZoom() != 1) { return; }

            mouse.ClearTargets();
            mouse.Update();
            CheckMouseOnTarget();

            //点の移動
            if (mouse.IsPressingLeft()) { mouse.SetTargetsPosition(); }

            //点の追加
            else if (mouse.WasPressedLeft()) {
                if (mouse.GetTargets().Count != 0) { return; }
                CreatObject((eObjectType)bar.GetNowTool(), mouse.GetPosition());
                bezierManager.SetBezierPoints();
                bezierManager.CreatBezierCurves();
            }

            //点の削除
            else if (mouse.WasPressedRight()) {
                DeleteObjects(mouse.GetTargets());
                bezierManager.SetBezierPoints();
                bezierManager.CreatBezierCurves();
            }

            //新たなグループを生成か次のグループを選択
            else if (mouse.WasScrollWheelPlus()) {
                ToNextIndex();
            }

            //前のグループを選択
            else if (mouse.WasScrollWheelMinus()) {
                bezierIndex--;
                bezierIndex = (int)Method.MinClamp(bezierIndex, 0);
            }

        }

        private void CheckMouseOnTarget() {
            for (int i = 0; i < mapPointsList.Count; i++)
            {
                mapPointsList[(eObjectType)i].ForEach(o => {
                    o.Update(mouse.GetPosition());
                    o.CheckSelected(bezierIndex);
                    if (o.IsMouseIn) { mouse.AddTarget(o); }
                });
            }

            entitysList.ForEach(o => {
                o.Update(mouse.GetPosition());
                o.CheckSelected(bezierIndex);
                if (o.IsMouseIn) { mouse.AddTarget(o); }
            });

            hintsList.ForEach(l => l.ForEach(h => {
                h.Update(mouse.GetPosition());
                h.CheckSelected(bezierIndex);
                if (h.IsMouseIn) { mouse.AddTarget(h); }
            }));
        }


        private void ToNextIndex() {
            List<List<Vector2>> list = bezierManager.GetBezierPoints();
            if (list.Count - 1 >= bezierIndex) { bezierIndex++; }
            else if (list.Count - 1 == bezierIndex) {
                bezierIndex++;
            }

        }

        public void Draw() {
            Renderer_2D.Begin();
            Renderer_2D.DrawString("BezierIndex:" +bezierIndex, new Vector2(20, 160), Color.Red, 1.2f);
            Renderer_2D.End();

            bezierManager.Draw();
            DrawWall();

            for (int i = 0; i < mapPointsList.Count; i++) {
                mapPointsList[(eObjectType)i].ForEach(o => o.Draw());
            }
            mouse.Draw();

            entitysList.ForEach(e => e.Draw());

            hintsList.ForEach(l => l.ForEach(h => h.Draw()));
        }

        private void DrawWall() {
            Renderer_2D.Begin(Camera2D.GetTransform());
            for (int i = 0; i < mapPointsList[eObjectType.Wall].Count; i += 2) {
                Renderer_2D.DrawLine(
                    mapPointsList[eObjectType.Wall][i].Position,
                    mapPointsList[eObjectType.Wall][i + 1].Position,
                    Color.LightGreen
                );
            }
            Renderer_2D.End();
        }

    }
}
