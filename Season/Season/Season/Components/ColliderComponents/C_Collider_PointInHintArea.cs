using Microsoft.Xna.Framework;
using MyLib.Utility;
using Season.Components.DrawComponents;
using Season.Def;
using Season.Entitys;

namespace Season.Components.ColliderComponents
{
    class C_Collider_PointInHintArea : ColliderComponent
    {
        private C_DrawSpriteAutoSize drawSquare;
        private Entity drawEntity;
        private Vector2 size;

        public C_Collider_PointInHintArea(
            string colliderName,
            Vector2 position,
            Vector2 size
            ) : base (colliderName, Vector2.One, 0, eCollitionType.Through, eCollitionForm.Hint, false)
        {
            centerPosition = position;
            this.size = size;
            drawEntity = Entity.CreateEntity("Empty", "Empty", new Transform2D());
            InitializeCollision();
        }

        public override void Update() {
            base.Update();

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

        }

        //public override void Collition(ColliderComponent other) { base.Collition(other); }

        protected override void DoJostleCollision(ColliderComponent otherComp) {
            Jostle_Point_Square(otherComp);
        }
        protected override void DoThroughCollision(ColliderComponent otherComp) {
            Through_Point_Square(otherComp);
        }

        private void Through_Point_Square(ColliderComponent otherComp) {
            Vector2 otherPosition = otherComp.GetEntity().transform.Position;
            bool isThroughThis = Method.IsInScale(otherPosition, centerPosition - size / 2, size);
            SetResultDataThrough(isThroughThis, otherComp);
        }
        private void Jostle_Point_Square(ColliderComponent otherComp) {
            Vector2 otherPosition = otherComp.GetEntity().transform.Position;
            bool isJostleThis = Method.IsInScale(otherPosition, centerPosition - size / 2, size);
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
