using Microsoft.Xna.Framework;
using MyLib.Utility;
using Season.Components.DrawComponents;
using Season.Entitys;

namespace Season.Components.ColliderComponents
{
    class C_Collider_HintArea : ColliderComponent
    {
        private C_DrawSpriteAutoSize drawSquare;
        private Entity drawEntity;
        private Vector2 size;

        public C_Collider_HintArea(
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
            bool isThroughThis = Methord.IsInScale(otherPosition, centerPosition - size / 2, size);
            SetResultDataThrough(isThroughThis, otherComp);
        }
        private void Jostle_Point_Square(ColliderComponent otherComp) {
            Vector2 otherPosition = otherComp.GetEntity().transform.Position;
            bool isJostleThis = Methord.IsInScale(otherPosition, centerPosition - size / 2, size);
            SetResultDataJostle(isJostleThis, otherComp);
        }


        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            drawSquare = new C_DrawSpriteAutoSize("UnitLine", offsetPosition, size / 2, 100, 0.5f);
            drawSquare.SetColor(Color.LightYellow);
            drawEntity.transform.Position = centerPosition;
            drawEntity.RegisterComponent(drawSquare);
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
            drawEntity.DeActive();
            InitializeCollision();
        }
    }
}
