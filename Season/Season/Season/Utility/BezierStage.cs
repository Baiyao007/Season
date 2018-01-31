using Microsoft.Xna.Framework;
using MyLib.Utility;
using Season.Components;
using Season.Components.ColliderComponents;
using Season.Components.DrawComponents;
using Season.Components.NormalComponents;
using Season.Entitys;
using System;
using System.Collections.Generic;

namespace Season.Utility
{
    static class Extensions
    {
        public static T Begin<T>(this List<T> list) {
            return list[0];
        }

        public static T End<T>(this List<T> list) {
            return list[list.Count - 1];
        }

    }


    static class BezierStage
    {
        static private List<List<Vector2>> controllPoints = new List<List<Vector2>>();
        static private List<List<Vector2>> blockMapList = new List<List<Vector2>>();
        static bool isEnd = false;

        public static void InitializeStage(int stageNo) {
            controllPoints.Clear();
            blockMapList.Clear();
            CSVReader.Read("PointData_S" + stageNo);
            int[,] result = CSVReader.GetIntMatrix();
          
            for (int i = 0; i < result.GetLength(0); i++) {
                if (result[i, 0] == 0 && result[i, 1] == 0) {
                    controllPoints.Add(new List<Vector2>());
                    continue;
                }
                controllPoints[controllPoints.Count - 1].Add(new Vector2(result[i, 0], result[i, 1]));
            }

            for (int i = 0; i < controllPoints.Count; i++) {
                if (controllPoints[i].Count == 2) {
                    blockMapList.Add(controllPoints[i]);
                    controllPoints.RemoveAt(i);
                    i--;
                }
            }

            blockMapList.ForEach(b => {
                Transform2D trans = new Transform2D();
                trans.Position = b[0];
                //実体生成
                Entity wallEntity = Entity.CreateEntity("Wall", "Wall", trans);
                C_Collider_Line wall = new C_Collider_Line("Wall", b[0], b[1], eCollitionType.Jostle, false);
                wallEntity.RegisterComponent(wall);
            });

            isEnd = false;
        }

        public static void UpdateBezierPoint(float newY, List<int> lb) {
            controllPoints[lb[0]][lb[1]] = new Vector2(controllPoints[lb[0]][lb[1]].X, newY);
        }

        public static List<Vector2> GetNowRoute(int nowLineIndex, int nowPoint)
        {
            if (nowLineIndex == -1) {     //isEnd || 
                isEnd = true;
                return new List<Vector2>();
            }
            if (controllPoints.Count == 0) { return new List<Vector2>(); }
            if (nowPoint > controllPoints[nowLineIndex].Count - 3 || nowPoint < 0) {
                //isEnd = true;
                return new List<Vector2>();
            }
            List<Vector2> route = new List<Vector2>();
            Vector2 previous = controllPoints[nowLineIndex][nowPoint];
            for (float time = 0.0f; time <= 1.0f; time += 0.0001f) {
                Vector2 result = (1 - time) * (1 - time) * controllPoints[nowLineIndex][nowPoint] +
                        2 * time * (1 - time) * controllPoints[nowLineIndex][nowPoint + 1] +
                        time * time * controllPoints[nowLineIndex][nowPoint + 2];

                if (Vector2.DistanceSquared(previous, result) >= 1) {
                    previous = result;
                    route.Add(result);
                }
            }
            isEnd = false;
            return route;
        }

        public static List<int> GetIndexList(Vector2 nowPosition) {
            List<int> indexs = new List<int>();
            for (int i = 0; i < controllPoints.Count; i++)
            {
                int lastIndex = controllPoints[i].Count - 1;

                if (nowPosition.X > controllPoints[i].End().X) { continue; }
                if (nowPosition.X >= controllPoints[i].Begin().X)
                {
                    isEnd = false;
                    indexs.Add(i);
                }
            }
            return indexs;
        }


        public static void SetBezierposition(Entity entity) {
            Vector2 nowPosition = entity.transform.Position;

            //横スキャンして、リストを取る
            List<int> indexs = GetIndexList(nowPosition);
            C_BezierPoint bezierComp = (C_BezierPoint)entity.GetNormalComponent("C_BezierPoint");

            int lineIndex = 0;
            int bezierPoint = 0;
            int cursor = 0;
            List<Vector2> route = new List<Vector2>();

            object syncObject = new object();
            lock (syncObject) {
                CheckBezierPosition(bezierComp, ref lineIndex, ref bezierPoint, ref cursor, ref route);
            }

            bezierComp.SetBezierData(lineIndex, bezierPoint, cursor);
            bezierComp.SetRoute(route);
        }

        private static void CheckBezierPosition(C_BezierPoint bezierComp, ref int lineIndex, ref int bezierPoint, ref int cursor, ref List<Vector2> route) {
            Vector2 nowPosition = bezierComp.GetEntity().transform.Position;

            //横スキャンして、リストを取る
            List<int> indexs = GetIndexList(nowPosition);
            List<List<Vector2>> routes = new List<List<Vector2>>();

            //着地できるルートがない場合
            if (indexs.Count == 0) {
                isEnd = true;
                lineIndex = -1;
                return;
            }

            //着地できるルートは1つしかない場合
            else if (indexs.Count == 1) {
                while (nowPosition.X > controllPoints[indexs[0]][bezierPoint + 2].X) {
                    bezierPoint += 2;
                }
                routes.Add(GetNowRoute(indexs[0], bezierPoint));

                while (nowPosition.X > routes[0][cursor].X &&
                    cursor < routes[0].Count - 1)
                {
                    int distance = (int)((nowPosition.X - routes[0][cursor].X) * 0.6f);
                    if (distance <= 1) {
                        cursor++;
                        break;
                    }
                    if (routes[0].Count > cursor + distance) {
                        cursor += distance;
                    }
                    else {
                        cursor++;
                    }
                }
                lineIndex = indexs[0];
                route = routes[0];
                return;
            }

            //着地できるルートは複数の場合
            List<int> cursors = new List<int>();        //ルートの中の位置
            List<int> bezierPoints = new List<int>();   //ルート
            for (int i = 0; i < indexs.Count; i++)  {
                bezierPoint = 0;
                cursor = 0;

                //今のlineを登録
                if (bezierComp.LineIndex == indexs[i])  {
                    while (nowPosition.X > controllPoints[indexs[i]][bezierPoint + 2].X)  {
                        bezierPoint += 2;
                    }
                    routes.Add(GetNowRoute(indexs[i], bezierPoint));
                    bezierPoints.Add(bezierPoint);

                    while (nowPosition.X > routes[i][cursor].X &&
                            cursor < routes[i].Count - 1)
                    {
                        int distance = (int)((nowPosition.X - routes[i][cursor].X) * 0.6f);
                        if (distance <= 1)  {
                            cursor++;
                            break;
                        }
                        if (routes[i].Count > cursor + distance)  {
                            cursor += distance;
                        }
                        else {
                            cursor++;
                        }
                    }
                    cursors.Add(cursor);
                    continue;
                }

                //検索した他のlineを登録
                while (nowPosition.X > controllPoints[indexs[i]][bezierPoint + 2].X)  {
                    bezierPoint += 2;
                }
                routes.Add(GetNowRoute(indexs[i], bezierPoint));
                bezierPoints.Add(bezierPoint);

                //所在位置のx座標のチェック
                while (nowPosition.X > routes[i][cursor].X &&
                        cursor < routes[i].Count - 1)
                {
                    int distance = (int)((nowPosition.X - routes[i][cursor].X) * 0.6f);
                    if (distance <= 1) {
                        cursor++;
                        break;
                    }
                    if (routes[i].Count > cursor + distance) {
                        cursor += distance;
                    }
                    else {
                        cursor++;
                    }
                }
                cursors.Add(cursor);
            }

            int index = 0;
            float lenth = routes[index][cursors[index]].Y - nowPosition.Y;
            //着地できるルートを全チェック
            for (int i = 0; i < indexs.Count; i++) {
                //今の位置はルートの下だとチェックしない
                if (nowPosition.Y > routes[i][cursors[i]].Y) { continue; }
                if (lenth < 0) {
                    index = i;
                    lenth = routes[index][cursors[index]].Y - nowPosition.Y;
                    continue;
                }
                //チェック中のルートと自分の距離はさっきチェックした最短距離より大きい場合は更新しない
                if (lenth < routes[i][cursors[i]].Y - nowPosition.Y) { continue; }
                index = i;
                lenth = routes[index][cursors[index]].Y - nowPosition.Y;
            }
            lineIndex = indexs[index];
            bezierPoint = bezierPoints[index];
            cursor = cursors[index];
            route = routes[index];
        }

        public static List<List<Vector2>> GetControllPoints() { return controllPoints; }
        public static List<List<Vector2>> GetBlockMapList() { return blockMapList; }

        public static bool IsRouteEnd() { return isEnd; }
    }
}
