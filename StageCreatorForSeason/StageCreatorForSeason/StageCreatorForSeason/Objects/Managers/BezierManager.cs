using Microsoft.Xna.Framework;
using MyLib.Device;
using StageCreatorForSeason.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageCreatorForSeason.Objects
{
    class BezierManager
    {
        private List<List<List<Vector2>>> bezierCurves;
        private List<List<Vector2>> bezierPositions;
        private List<Object> bezierPoints;

        public BezierManager(List<Object> bezierPoints) {
            bezierCurves = new List<List<List<Vector2>>>();
            bezierPositions = new List<List<Vector2>>();
            this.bezierPoints = bezierPoints;
        }

        public void Initialize() {
            Clear();
        }

        public void Clear() {
            bezierCurves.Clear();
            bezierPositions.Clear();
        }

        public void AddPoint(int teamNo, Vector2 position) {
            bezierPositions[teamNo].Add(position);
        }

        public void SetBezierPoints(List<List<Vector2>> points) {
            bezierPositions = points;
        }

        public void SetBezierPoints() {
            for (int i = 0; i < bezierPositions.Count; i++) {
                bezierPositions[i].Clear();
            }
            for (int i = 0; i < bezierPoints.Count; i++) {
                if (((BezierPoint)bezierPoints[i]).GetTeamNo() >= bezierPositions.Count()) {
                    bezierPositions.Add(new List<Vector2>());
                }
                bezierPositions[((BezierPoint)bezierPoints[i]).GetTeamNo()].Add(bezierPoints[i].Position);
            }
        }

        public List<List<Vector2>> GetBezierPoints() {
            return bezierPositions;
        }

        public void CreatBezierCurves() {
            for (int i = 0; i < bezierPositions.Count; i++) {
                bezierPositions[i].Sort((x, y) => x.X.CompareTo(y.X));
            }

            bezierCurves.Clear();
            bezierCurves.Add(new List<List<Vector2>>());
            for (int i = 0; i < bezierPositions.Count; i++) {
                bezierCurves.Add(new List<List<Vector2>>());
                if (bezierPositions[i].Count < 3) { continue; }
                if (bezierPositions[i].Count % 2 != 1) { continue; }
                bezierCurves[i] = BezierStage.GetBezierCurve(bezierPositions[i]);
            }
        }

        public void Draw() {
            DrawBezierLine();
            DrawBezierCurve();
        }

        private void DrawBezierLine() {
            Renderer_2D.Begin(Camera2D.GetTransform());
            for (int i = 0; i < bezierPositions.Count; i++) {
                for (int j = 0; j < bezierPositions[i].Count - 1; j++)
                {
                    Renderer_2D.DrawLine(
                        bezierPositions[i][j],
                        bezierPositions[i][j + 1],
                        Color.Yellow
                    );
                }
            }
            Renderer_2D.End();
        }

        private void DrawBezierCurve() {
            if (bezierCurves.Count == 0) { return; }

            Renderer_2D.Begin(Camera2D.GetTransform());
            Vector2 imgSize = ResouceManager.GetTextureSize("Point");
            Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
            for (int k = 0; k < bezierCurves.Count; k++) {
                for (int i = 0; i < bezierCurves[k].Count; i++) {
                    for (int j = 0; j < bezierCurves[k][i].Count; j++) {
                        Renderer_2D.DrawTexture("Point", bezierCurves[k][i][j], Color.LightGreen, 1, rect, Vector2.One * 0.3f, 0, imgSize / 2);
                    }
                }
            }
            Renderer_2D.End();
        }

    }
}
