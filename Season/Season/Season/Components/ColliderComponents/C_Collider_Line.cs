using Microsoft.Xna.Framework;
using Season.Components.DrawComponents;
using Season.Def;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.ColliderComponents
{
    class C_Collider_Line : ColliderComponent
    {
        public Vector2 Position1;
        public Vector2 Position2;

        private C_DrawLine drawLine;
        private Entity drawEntity;

        public C_Collider_Line(
            string colliderName,
            Vector2 Position1,
            Vector2 Position2,
            eCollitionType collisionType = eCollitionType.Through,
            bool isLocal = false
            ) : base (colliderName, Vector2.Zero, 0, collisionType, eCollitionForm.Line, isLocal)
        {
            this.Position1 = Position1;
            this.Position2 = Position2;
            centerPosition = (Position1 + Position2) / 2;

            drawEntity = Entity.CreateEntity("Empty", "Empty", new Transform2D());
            InitializeCollision();
        }

        public override void Update() {
            base.Update();

            if (Parameter.IsDebug) {
                if (drawLine == null) {
                    List<Vector2> linePoint = new List<Vector2>() { Position1, Position2 };
                    drawLine = new C_DrawLine(linePoint);
                    drawEntity.RegisterComponent(drawLine);
                }
            }
            else {
                if (drawLine != null) {
                    drawLine.DeActive();
                    drawLine = null;
                }
            }

        }
        //public override void Collition(ColliderComponent other) { base.Collition(other); }

        protected override void DoJostleCollision(ColliderComponent otherComp) {
            if (otherComp.collisionForm == eCollitionForm.Line) { }
            else {
                Jostle_Circle_Line(otherComp);
            }
        }

        protected override void DoThroughCollision(ColliderComponent otherComp) {
            if (otherComp.collisionForm == eCollitionForm.Line) { }
            else {
                Through_Circle_Line(otherComp);
            }
        }


        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

            drawEntity.DeActive();
            InitializeCollision();
        }
    }
}
