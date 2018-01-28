//作成日：　2017.10.10
//作成者：　柏
//クラス内容：  ステートパタン親クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.States
{

    enum eStateTrans {
        ToThis,
        ToNext,
    }

    abstract class IState<T>
    {
        protected bool isInit = true;

        public IState<T> Update(T target) {
            if (isInit) {
                Initialize(target);
                isInit = false;
            }
            IState<T> nextState = null;
            eStateTrans trans = UpdateAction(target, ref nextState);

            if (trans == eStateTrans.ToNext) {
                ExitAction(target);
            }
            return nextState;
        }

        protected abstract void Initialize(T target);
        protected abstract eStateTrans UpdateAction(T target, ref IState<T> nextState);
        protected abstract void ExitAction(T target);
    }
}
