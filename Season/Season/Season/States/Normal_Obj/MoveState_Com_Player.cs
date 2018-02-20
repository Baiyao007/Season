//作成日：　2017.10.10
//作成者：　柏
//クラス内容：  ステート実装（移動パータン）
//修正内容リスト：
//名前：柏　　　日付：2017.11.20　　　内容：Component実装によって修正
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using System;
using Season.Components;
using Season.Components.ColliderComponents;
using Season.Components.DrawComponents;
using Season.Components.MoveComponents;
using Season.Components.NormalComponents;
using Season.Def;
using Season.Entitys;
using Season.States.Normal_Obj;
using Season.Utility;
using Season.Components.UpdateComponents;

namespace Season.States
{
    class MoveState_Com_Player : IState<Entity> {
        private GameDevice gameDevice;
        private InputState inputState;
        private C_MoveWithController moveComp;
        private C_DrawRouteEffect routeEffect;

        private ColliderComponent collider;
        private C_SeasonState seasonState;
        private C_CharaState state;
        private C_Energy energy;

        public MoveState_Com_Player(GameDevice gameDevice, float nowSpeed) {
            this.gameDevice = gameDevice;
            inputState = gameDevice.GetInputState;

            moveComp = new C_MoveWithController(Parameter.PlayerLimitSpeed, inputState);
            moveComp.speed = Math.Abs(nowSpeed);
        }

        protected override void Initialize(Entity entity) {
            entity.RegisterComponent(moveComp);

            routeEffect = (C_DrawRouteEffect)entity.GetDrawComponent("C_DrawRouteEffect");
            routeEffect.Awake();

            seasonState = (C_SeasonState)entity.GetUpdateComponent("C_SeasonState");
            energy = (C_Energy)entity.GetNormalComponent("C_Energy");

            state = (C_CharaState)entity.GetNormalComponent("C_CharaState");
        }

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState) {
            //下ジャンプに遷移
            if (JumpDownCheck(entity)) {
                routeEffect.Sleep();
                float nowSpeed = moveComp.speed;
                entity.RemoveComponent(moveComp);
                nextState = new JumpState_Com_Player(gameDevice, eJumpType.Down, nowSpeed);
                return eStateTrans.ToNext;
            }

            //落下に遷移
            if (state.IsJump) { 
                routeEffect.Sleep();
                float nowSpeed = moveComp.speed;
                entity.RemoveComponent(moveComp);
                nextState = new JumpState_Com_Player(gameDevice, eJumpType.Fall, nowSpeed);
                return eStateTrans.ToNext;
            }

            //Jumpに遷移
            if (inputState.WasDown(Keys.Space, Buttons.A)) {
                //子生物誘導ヒントを作る
                C_Collider_PointInHintArea hint = new C_Collider_PointInHintArea("ChildJump", entity.transform.Position, Vector2.One * 40);
                hint.Active();
                hint.Destroy(2);
                TaskManager.AddTask(hint);

                routeEffect.Sleep();
                float nowSpeed = moveComp.speed;
                entity.RemoveComponent(moveComp);
                nextState = new JumpState_Com_Player(gameDevice, eJumpType.Up, nowSpeed);
                return eStateTrans.ToNext;
            }


            //攻撃
            if (inputState.WasDown(Keys.F, Buttons.B)) {
                routeEffect.Sleep();
                entity.RemoveComponent(moveComp);
                nextState = new AttackState_Com_Player(gameDevice);
                return eStateTrans.ToNext;
            }

            //Skill選択に遷移
            if (inputState.WasDown(Keys.Z, Buttons.LeftShoulder)) {
                routeEffect.Sleep();
                entity.RemoveComponent(moveComp);
                nextState = new SkillSelect_Com(gameDevice);
                return eStateTrans.ToNext;
            }

            //Damage判定
            if (CollitionCheck(entity)) {
                entity.RemoveComponent(moveComp);
                collider.DeActive();
                nextState = new DeathState_Com("Bom", gameDevice);
                return eStateTrans.ToNext;
            }

            //Skill発動
            if (seasonState.IsCanSkill() && inputState.WasDown(Keys.X, Buttons.RightShoulder)) {
                DrawComponent drawSkill = new C_DrawSkillCircle(true, 100);
                entity.RegisterComponent(drawSkill);

                ColliderComponent skillCollider = new C_Collider_Circle("PlayerSkill", new Vector2(0, -120), 400);
                entity.RegisterComponent(skillCollider);
                skillCollider.Destroy(3);

                seasonState.CoolingSkill();
            }

            nextState = this;
            return eStateTrans.ToThis;
        }

        private bool IsColliderValid(Entity entity) {
            collider = entity.GetColliderComponent("Player");
            if (collider == null) { return false; }
            return true;
        }

        private bool JumpDownCheck(Entity entity) {
            if (!IsColliderValid(entity)) { return false; }
            if (collider.IsThrough("JumpDown") && 
                inputState.IsDown(Keys.Down, Buttons.DPadDown))
            {
                return true;
            }
            return false;
        }


        private bool CollitionCheck(Entity entity) {
            if (energy.IsDead()) { return true; }
            if (!IsColliderValid(entity)) { return false; }
            if (collider.IsThrough("Boar")) { 
                return true;
            }
            if (collider.ThroughStart("Eagle")) {
                energy.Damage(5);
                SurrenderChild(entity);
                return energy.IsDead();
            }
            return false;
        }

        private void SurrenderChild(Entity entity) {
            Entity child = ((C_PlayerState)entity.GetNormalComponent("C_PlayerState")).GetOneChild();
            if (child.GetName() == "Null") { return; }
            C_DrawAnimetion drawChild = (C_DrawAnimetion)child.GetDrawComponent("C_DrawAnimetion");
            C_ChildState childState = (C_ChildState)child.GetNormalComponent("C_ChildState");
            drawChild.SetNowAnim("Catch");
            Entity eagle = collider.GetOtherEntity("Eagle");
            C_EnemyState eagleState = (C_EnemyState)eagle.GetNormalComponent("C_EnemyState");
            eagleState.SetCaughtChild();
            childState.SetEnemyCatchMe(eagle);
        }


        protected override void ExitAction(Entity entity) { }
    }
}
