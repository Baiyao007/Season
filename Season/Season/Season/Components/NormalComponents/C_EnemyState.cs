using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.NormalComponents
{
    class C_EnemyState : Component
    {
        private bool isCaughtChild;
        public C_EnemyState() {
            isCaughtChild = false;
        }

        public void SetCaughtChild() { isCaughtChild = true; }

        public bool IsCaughtChild() { return isCaughtChild; }

    }
}
