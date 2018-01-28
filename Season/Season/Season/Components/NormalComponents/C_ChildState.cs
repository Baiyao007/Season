using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.NormalComponents
{
    class C_ChildState : Component
    {

        public bool FollowSwitch { get; set; }
        private Entity enemy;

        public C_ChildState() {
            FollowSwitch = false;
            enemy = Entity.CreateEntity("Null","Null",new Transform2D());
        }

        public void SetEnemyCatchMe(Entity enemy) { this.enemy = enemy; }

        public Entity GetEnemyCaughtMe() { return enemy; }

        public bool IsBeCaught() { return enemy.GetName() != "Null"; }

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
