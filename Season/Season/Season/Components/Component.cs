//作成日：　2017.11.20
//作成者：　柏
//クラス内容：　Component親クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components
{
    public class Component
    {
        protected bool isActive;
        protected Entity entity;

        public  bool IsAtive {
            get { return isActive; }
            set { isActive = value; }
        }
        public virtual void Active() { IsAtive = true; }
        public virtual void DeActive() { IsAtive = false; }
        public void Register(Entity owner) { entity = owner; }
        public Entity GetEntity() { return entity; }
    }
}
