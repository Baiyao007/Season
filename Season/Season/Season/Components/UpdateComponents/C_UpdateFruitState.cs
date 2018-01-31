using Microsoft.Xna.Framework;
using MyLib.Device;
using MyLib.Utility;
using MyLib.Utility.Action.Movements;
using MyLib.Utility.Action.TheChange;
using Season.Components.ColliderComponents;
using Season.Components.DrawComponents;
using Season.Components.MoveComponents;
using Season.Components.NormalComponents;

namespace Season.Components.UpdateComponents
{
    class C_UpdateFruitState : UpdateComponent
    {
        private GameDevice gameDevice;

        private ColliderComponent collider;
        private Timer disapearTimer;
        private bool isDisapear;

        private C_DrawAnimetion drawComp;

        public C_UpdateFruitState(GameDevice gameDevice) {
            this.gameDevice = gameDevice;
            disapearTimer = new Timer(3);
            isDisapear = false;
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる
            
            UpdateComponent fallComp = new C_StaticEntityFall();
            entity.RegisterComponent(fallComp);

            collider = new C_Collider_Circle("Fruit", new Vector2(0, -15), 15);
            entity.RegisterComponent(collider);

            drawComp = (C_DrawAnimetion)entity.GetDrawComponent("C_DrawAnimetion");
        }

        public override void DeActive() {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }

        public override void Update() {
            if (isDisapear) {
                disapearTimer.Update();
                if (disapearTimer.IsTime) {
                    CreatEffect();
                    entity.DeActive();
                }
            }
            else {
                if (collider.IsThrough("Squirrel")) {
                    isDisapear = true;
                    C_Energy childHP = (C_Energy)collider.GetOtherEntity("Squirrel").GetNormalComponent("C_Energy");
                    childHP.Heal(childHP.GetLimitEnery()); 
                    drawComp.SetShaderOn(disapearTimer.GetLimitTime() + 0.1f, "A_Apple_Idle", "ShadeMask_Circle");
                    drawComp.SetShaderCreatment(false);
                }
            }
        }

        private void CreatEffect() {
            Vector2 effectPosition = entity.transform.Position;
            gameDevice.GetParticleGroup.AddParticles(
                "P_Cross",          //name
                40, 60,             //count
                effectPosition,
                effectPosition,     //position
                7, 14,              //speed
                0.8f, 1.3f,         //size
                0.3f, 0.8f,               //alpha
                0, 360,             //angle
                0.8f, 1.2f,         //alive
                new MoveAccelerate(false),          //moveType
                new ChangeToLucency(new Timer(1))   //changeType
            );

        }

    }
}
