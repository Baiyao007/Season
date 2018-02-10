using Microsoft.Xna.Framework;
using MyLib.Utility;
using Season.Components.DrawComponents;
using Season.Def;
using Season.Entitys;
using Season.Utility;
using System.Collections.Generic;

namespace Season.Components.ColliderComponents
{
    class C_Collider_Square : ColliderComponent
    {
        private C_DrawSpriteAutoSize drawSquare;
        private Entity drawEntity;
        private Vector2 size;

        public C_Collider_Square(
            string colliderName,
            Vector2 position,
            Vector2 size
            ) : base (colliderName, Vector2.One, 0, eCollitionType.Through, eCollitionForm.Hint, false)
        {
            centerPosition = position;
            this.size = size;
            drawEntity = Entity.CreateEntity("Empty", "Empty", new Transform2D());
            InitializeCollision();
            InitializePoints();
        }

        private void InitializePoints() {
            points = new List<Vector2>() {
                centerPosition - size / 2,
                centerPosition + new Vector2( size.X / 2, -size.Y / 2),
                centerPosition + size / 2,
                centerPosition + new Vector2(-size.X / 2,  size.Y / 2),
            };
        }
        

        public override void Update() {
            base.Update();
            if (isLocal) { InitializePoints(); }


            if (Parameter.IsDebug) {
                if (drawSquare == null) {
                    drawSquare = new C_DrawSpriteAutoSize("UnitLine", offsetPosition, size / 2, 100, 0.2f);
                    drawSquare.SetColor(Color.LightYellow);
                    drawEntity.RegisterComponent(drawSquare);
                }
            }
            else {
                if (drawSquare != null) {
                    drawSquare.DeActive();
                    drawSquare = null;
                }
            }

            if (drawSquare == null) { return; }

            if (results.Count > 0) {
                bool isCollide = false;
                for (int i = 0; i < results.Count; i++) {
                    isCollide = results[i].IsCollide();
                    if (isCollide) { break; }
                }
                if (isCollide) { drawSquare.SetColor(Color.Red); }
                else { drawSquare.SetColor(Color.LightYellow); }
            }
            else { drawSquare.SetColor(Color.LightYellow); }

            
        }

        //public override void Collition(ColliderComponent other) { base.Collition(other); }

        protected override void DoJostleCollision(ColliderComponent otherComp) {
            Jostle_Circle_Square(otherComp);
        }
        protected override void DoThroughCollision(ColliderComponent otherComp) {
            Through_Circle_Square(otherComp);
        }

        private void Through_Circle_Square(ColliderComponent otherComp) {
            bool isThroughThis = false;
            if (otherComp.collisionForm == eCollitionForm.Circle) {
                isThroughThis = CollitionCheck.CircleSquare(this, (C_Collider_Circle)otherComp);
            }
            SetResultDataThrough(isThroughThis, otherComp);
        }

        private void Jostle_Circle_Square(ColliderComponent otherComp) {
            bool isJostleThis = false;
            if (otherComp.collisionForm == eCollitionForm.Circle) {
                isJostleThis = CollitionCheck.CircleSquare(this, (C_Collider_Circle)otherComp);
            }
            SetResultDataJostle(isJostleThis, otherComp);
        }

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            drawEntity.transform.Position = centerPosition;
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
            drawEntity.DeActive();
            InitializeCollision();
        }
    }
}
