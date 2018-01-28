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

        private BezierManager bezierManager;

        private Dictionary<eObjectType, Func<Vector2, Object>> creatFuncs;
        private Dictionary<eObjectType, List<Object>> objectList;

        private int bezierIndex;
        private int wallIndex;
        private ToolBar bar;

        public ObjectManager(ToolBar bar) {
            mouse = new MyMouse(this);
            this.bar = bar;

            objectList = new Dictionary<eObjectType, List<Object>>() {
                { eObjectType.BezierStage, new List<Object>()},
                { eObjectType.Wall, new List<Object>()},
            };

            creatFuncs = new Dictionary<eObjectType, Func<Vector2, Object>> {
                { eObjectType.BezierStage,  CreatBezierPoint },
                { eObjectType.Wall,  CreatWallPoint },
            };

            bezierManager = new BezierManager(objectList[eObjectType.BezierStage]);
            bezierIndex = 0;
            wallIndex = 0;
        }

        private BezierPoint CreatBezierPoint(Vector2 position) {
            return new BezierPoint(position, bezierIndex);
        }

        private WallPoint CreatWallPoint(Vector2 position) {
            wallIndex++;
            objectList[eObjectType.Wall].Add(new WallPoint(position + new Vector2(0,-100), wallIndex));
            return new WallPoint(position + new Vector2(0, 100), wallIndex);
        }

        public void CreatObject(eObjectType type, Vector2 position) {
            objectList[type].Add(creatFuncs[type](position));
        }


        public void InitializeStage(int stageNo) {
            List<List<Vector2>> bezierPoints = BezierStage.InitializeStage(Parameter.StageNo);
            for (int i = 0; i < bezierPoints.Count; i++) {
                for (int j = 0; j < bezierPoints[i].Count; j++) {
                    CreatObjectFromData(eObjectType.BezierStage, bezierPoints[i][j], i);
                }
            }
            bezierManager.SetBezierPoints(bezierPoints);
        }

        private void CreatObjectFromData(eObjectType type, Vector2 position, int index) {
            BezierPoint obj = new BezierPoint(position, index);
            objectList[type].Add(obj);
        }

        public void DeleteObjects(List<Object> objs) {
            for (int i = 0; i < objs.Count(); i++) {

                if (DelectBezier(objs[i])) {
                    objs.Remove(objs[i]);
                    i--;
                    continue;
                }

                if (DelectWall(objs[i])) {
                    objs.Remove(objs[i]);
                    i--;
                    continue;
                }
            }

            
        }

        private bool DelectBezier(Object obj) {
            if (obj.GetType() != eObjectType.BezierStage) { return false; }
            objectList[eObjectType.BezierStage].Remove(obj);
            return true;
        }

        private bool DelectWall(Object obj) {
            if (obj.GetType() != eObjectType.Wall) { return false; }
            int teamNo = obj.GetTeamNo();
            objectList[eObjectType.Wall].RemoveAll(o => o.GetTeamNo() == teamNo);
            return true;
        }



        public void Clear() {
            for (int i = 0; i < (int)eObjectType.None; i++) {
                objectList[(eObjectType)i].Clear();
            }
        }
        public int ObjectsCount() {
            int count = 0;
            for (int i = 0; i < (int)eObjectType.None; i++) {
                count += objectList[(eObjectType)i].Count;
            }
            return count;
        }

        public void SavePoints(int stageNo) {
            List<List<Vector2>> saveList = bezierManager.GetBezierPoints();
            List<List<Vector2>> wallList = saveList.FindAll(x => x.Count == 2);
            saveList.RemoveAll(x => x.Count == 0 || x.Count == 2);
            saveList.Sort((x, y) => x[0].X.CompareTo(y[0].X));
            for (int i = 0; i < objectList[eObjectType.Wall].Count; i += 2) {
                saveList.Add(new List<Vector2>());
                saveList[saveList.Count - 1].Add(objectList[eObjectType.Wall][i].Position);
                saveList[saveList.Count - 1].Add(objectList[eObjectType.Wall][i + 1].Position);
            }
            saveList.AddRange(wallList);

            CSVReader.Save("PointData_S" + stageNo, saveList);
        } 

        public void Update() {
            if (Camera2D.GetZoom() != 1) { return; }

            mouse.ClearTargets();
            mouse.Update();

            for (int i = 0; i < (int)eObjectType.None; i++) {
                objectList[(eObjectType)i].ForEach(o=> {
                    o.Update(mouse.GetPosition());
                    o.CheckSelected(bezierIndex);
                    if (o.IsMouseIn()) { mouse.AddTarget(o); }
                });
            }

            //点の移動
            if (mouse.IsPressingLeft()) {
                mouse.SetTargetsPosition();
                bezierManager.CreatBezierCurves();
            }
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
                bezierIndex = (int)Methord.MinClamp(bezierIndex, 0);
            }
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

            for (int i = 0; i < (int)eObjectType.None; i++) {
                objectList[(eObjectType)i].ForEach(o => o.Draw());
            }
            mouse.Draw();
        }

        private void DrawWall() {
            Renderer_2D.Begin(Camera2D.GetTransform());
            for (int i = 0; i < objectList[eObjectType.Wall].Count; i += 2) {
                Renderer_2D.DrawLine(
                    objectList[eObjectType.Wall][i].Position,
                    objectList[eObjectType.Wall][i + 1].Position,
                    Color.LightGreen
                );
            }
            Renderer_2D.End();
        }

    }
}
