//作成日：　2017.11.20
//作成者：　柏
//クラス内容：　更新用Component親クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components
{
    public class UpdateComponent : Component
    {
        public virtual void Update() { }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる


        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

            
        }
    }
}
