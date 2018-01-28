using MyLib.Device;
using Season.Components;
using Season.Components.MoveComponents;
using Season.Components.NormalComponents;
using Season.Components.UpdateComponents;
using Season.Def;
using Season.Entitys;

namespace Season.States.Normal_Obj
{
    class JumpState_Com_Player : IState<Entity>
    {
        private GameDevice gameDevice;
        private InputState inputState;
        private C_JumpWithController jumpComp;
        private C_EntityDeadCheck deadCheck;
        private ColliderComponent collider;
        private C_Energy energy;

        public JumpState_Com_Player(GameDevice gameDevice, eJumpType jumpType, float nowSpeed)
        {
            this.gameDevice = gameDevice;
            inputState = gameDevice.GetInputState;

            jumpComp = new C_JumpWithController(Parameter.PlayerLimitSpeed, inputState, jumpType);
            jumpComp.speed = nowSpeed;
        }

        protected override void Initialize(Entity entity)
        {
            deadCheck = (C_EntityDeadCheck)entity.GetUpdateComponent("C_EntityDeadCheck");
            energy = (C_Energy)entity.GetNormalComponent("C_Energy");
            entity.RegisterComponent(jumpComp);
        }

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState)
        {
            if (jumpComp.GetIsLand())
            {
                System.Console.WriteLine("Landed");
                float nowSpeed = jumpComp.speed;
                entity.RemoveComponent(jumpComp);
                nextState = new MoveState_Com_Player(gameDevice, nowSpeed); 
                return eStateTrans.ToNext;
            }

            //Damage判定
            if (CollitionCheck(entity))
            {
                entity.RemoveComponent(jumpComp);
                collider.DeActive();
                nextState = new DeathState_Com("Bom", gameDevice);
                return eStateTrans.ToNext;
            }
            
            nextState = this;
            return eStateTrans.ToThis;
        }

        private bool CollitionCheck(Entity entity)
        {
            if (deadCheck.IsDead()) { return true; }
            collider = entity.GetColliderComponent("Player");
            if (collider == null) { return false; }
            if (collider.IsThrough("Boar")) {
                return true;
            }
            if (collider.ThroughStart("Eagle")) {
                energy.Damage(5);
                return energy.IsDead();
            }
            return false;
        }


        protected override void ExitAction(Entity entity) { }
    }
}
