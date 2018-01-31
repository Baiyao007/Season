using Microsoft.Xna.Framework;
using MyLib.Utility;
using System;
using System.Collections.Generic;

namespace StageCreatorForSeason.Utility
{
    static class Extensions
    {
        public static T Begin<T>(this List<T> list) { return list[0]; }
        public static T End<T>(this List<T> list) { return list[list.Count - 1]; }

    }


    static class BezierStage
    {
        static private List<List<Vector2>> controllPoints = new List<List<Vector2>>();
        static private List<List<Vector2>> blockMapList = new List<List<Vector2>>();

        public static List<List<Vector2>> InitializeStage(int stageNo) {
            controllPoints.Clear();
            CSVReader.Read("PointData_S" + stageNo);
            int[,] result = CSVReader.GetIntMatrix();
          
            for (int i = 0; i < result.GetLength(0); i++) {
                if (result[i, 0] == 0 && result[i, 1] == 0) {
                    controllPoints.Add(new List<Vector2>());
                    continue;
                }
                controllPoints[controllPoints.Count - 1].Add(new Vector2(result[i, 0], result[i, 1]));
            }
            return controllPoints;
        }

        public static List<Vector2> GetNowRoute(Vector2 v1, Vector2 v2, Vector2 v3) {
            List<Vector2> route = new List<Vector2>();
            Vector2 previous = v1;
            for (float time = 0.0f; time <= 1.0f; time += 0.0002f) {
                Vector2 result = (1 - time) * (1 - time) * v1 +
                        2 * time * (1 - time) * v2 +
                        time * time * v3;

                if (Vector2.DistanceSquared(previous, result) >= 40) {
                    previous = result;
                    route.Add(result);
                }
            }
            return route;
        }

        public static List<List<Vector2>> GetBezierCurve(List<Vector2> points) {
            List<List<Vector2>> curves = new List<List<Vector2>>();
            for (int i = 0; i < points.Count - 1; i += 2) {
                curves.Add(GetNowRoute(points[i], points[i + 1], points[i + 2]));
            }
            return curves;
        }



        public static List<int> GetIndexList(Vector2 nowPosition) {
            List<int> indexs = new List<int>();
            for (int i = 0; i < controllPoints.Count; i++) {
                int lastIndex = controllPoints[i].Count - 1;

                if (nowPosition.X > controllPoints[i].End().X) { continue; }
                if (nowPosition.X >= controllPoints[i].Begin().X) {
                    indexs.Add(i);
                }
            }
            return indexs;
        }



    }
}
