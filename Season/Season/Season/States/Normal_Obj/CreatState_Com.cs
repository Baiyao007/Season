//作成日：　2017.10.20
//作成者：　柏
//クラス内容：  ステート実装（生成パータン）
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using MyLib.Device;
using MyLib.Utility;
using MyLib.Utility.Action;
using Season.Components;
using Season.Entitys;
using Season.Utility;

namespace Season.States.Normal_Obj
{
    class CreatState_Com : IState<Entity>
    {
        private GameDevice gameDevice;
        private Timer timer;

        public CreatState_Com(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            timer = new Timer(1);
        }

        protected override void Initialize(Entity entity)
        {
            timer.Update();
        }

        protected override eStateTrans UpdateAction(Entity entity, ref IState<Entity> nextState)
        {
            timer.Update();

            //entity.GetDrawComponent().alpha = 1 - timer.Rate();

            if (timer.IsTime)
            {
                nextState = new MoveState_Com_Player(gameDevice, 0);
                return eStateTrans.ToNext;
            }

            nextState = this;
            return eStateTrans.ToThis;
        }

        protected override void ExitAction(Entity entity) { }
    }
}
