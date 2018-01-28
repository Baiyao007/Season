using Microsoft.Xna.Framework;
using Season.Components.DrawComponents;
using Season.Entitys;

namespace Season.Components.ColliderComponents
{
    class C_Collider_Circle : ColliderComponent
    {
        private C_DrawSpriteAutoSize drawCircle;
        private Entity drawEntity;

        public C_Collider_Circle(
            string colliderName,
            Vector2 offsetPosition,
            int radius,
            eCollitionType collisionType = eCollitionType.Through,
            bool isLocal = true
            ) : base (colliderName, offsetPosition, radius, collisionType, eCollitionForm.Circle, isLocal)
        {
            drawEntity = Entity.CreateEntity("Empty", "Empty", new Transform2D());
            InitializeCollision();
        }

        public override void Update()
        {
            base.Update();
                       
            //if (isLocal) { return; }
            drawEntity.transform.Position = centerPosition;
            drawCircle.SetRudius(radius);
        }

        //public override void Collition(ColliderComponent other) { base.Collition(other); }

        protected override void DoJostleCollision(ColliderComponent otherComp) {
            if (otherComp.collisionForm == eCollitionForm.Circle) {
                Jostle_Circle_Circle(otherComp);
            }
            else if (otherComp.collisionForm == eCollitionForm.Line) {
                Jostle_Circle_Line(otherComp);
            }
        }

        protected override void DoThroughCollision(ColliderComponent otherComp) {
            if (otherComp.collisionForm == eCollitionForm.Circle) {
                Through_Circle_Circle(otherComp);
            }
            else if (otherComp.collisionForm == eCollitionForm.Line) {
                Through_Circle_Line(otherComp);
            }
        }

        public override void Active() {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            if (colliderName == "OverseeCircle") {
                drawCircle = new C_DrawSpriteAutoSize("E_CheckArea", offsetPosition, radius, 15, 0.5f);
                drawEntity.RegisterComponent(drawCircle);
            }
            else {
                drawCircle = new C_DrawSpriteAutoSize("CollisionArea", offsetPosition, radius, 100);
                //drawEntity.RegisterComponent(drawCircle);
            }
            centerPosition = entity.transform.Position + offsetPosition;
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
            drawEntity.DeActive();
            InitializeCollision();
        }
    }
}
