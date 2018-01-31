using Microsoft.Xna.Framework;
using MyLib.Utility;
using Season.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.NormalComponents
{
    class C_BezierPoint : Component
    {
        private int bezierPoint;
        private List<Vector2> route;
        private int cursor;
        private Vector2 previousPosition;
        private C_Switch3 direction;
        private int lineIndex;

        public int LineIndex {
            get { return lineIndex; }
            set { lineIndex = value; }
        }

        public int BezierPoint { 
            get { return bezierPoint; }
            set { bezierPoint = value; }
        }
        public int Cursor {
            get { return cursor; }
            set { cursor = value; }
        }
        public List<Vector2> Route {
            get { return route; }
            set { route = value; }
        }


        public C_BezierPoint()
        {
            lineIndex = -1;
            bezierPoint = 0;    //一グループの制御点の始点
            cursor = -1;     //移動ルートのポジション番号
            route = new List<Vector2>();
        }

        public bool IsEnd() {
            return BezierStage.IsRouteEnd() || route.Count == 0;
        }

        public void ToBefore() {
             bezierPoint -= 2;
            route = BezierStage.GetNowRoute(lineIndex, bezierPoint);
            cursor = route.Count - 1;
        }

        public void ToNext() {
            bezierPoint += 2;
            route = BezierStage.GetNowRoute(lineIndex, bezierPoint);
            cursor = 0;
        }

        public Vector2 GetNowPosition() {
            if (route.Count == 0) { return Vector2.Zero; }
            return route[cursor];
        }

        public void SetBezierData(int lineIndex, int bezierPoint, int cursor) {
            LineIndex = lineIndex;
            BezierPoint = bezierPoint;
            Cursor = cursor;
            
        }

        public void SetRoute(List<Vector2> route) {
            this.route = route;
        }

        public void ToRight(int speed = 1) {
            if (route.Count != 0 && !direction.IsLeft()) { previousPosition = route[cursor]; }
            cursor += speed;
            if (cursor >= route.Count) { ToNext(); }
        }

        public void ToLeft(int speed = 1) {
            if (route.Count != 0 && !direction.IsRight()) { previousPosition = route[cursor]; }
            cursor -= speed;
            if (cursor < 0) { ToBefore(); }
        }

        public void Rotate() {
            if (cursor >= route.Count || cursor < 0) {
                if (direction.IsRight()) { entity.transform.Angle = 0; }
                else { entity.transform.Angle = 180; }
                return;
            }
            if (route[cursor] == previousPosition) { return; }
            Vector2 direct = route[cursor] - previousPosition;
            float radian = (float)Math.Atan2(direct.Y, direct.X);
            entity.transform.Angle = Methord.ToDegree(radian);
        }

        public void CheckJumpMove() {
            BezierStage.SetBezierposition(entity);
            if (lineIndex == -1) { route = new List<Vector2>(); }
        }

        private void InitializeCursor()
        {
            for (int i = 0; i < route.Count; i++) {
                if (entity.transform.Position.X < route[i].X) {
                    cursor = i;
                    return;
                }
            }
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            BezierStage.SetBezierposition(entity);
            route = BezierStage.GetNowRoute(lineIndex, bezierPoint);
            direction = (C_Switch3)entity.GetNormalComponent("C_Switch3");
            InitializeCursor();
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

            route.Clear();
        }

        public string Print() {
            string result = entity.GetName() + 
                " - Route: line - " + lineIndex + 
                " , bezier - " + bezierPoint +
                " , cursor - " + cursor + "(" + route.Count + ")" ;
            return result;
        }
    }
}