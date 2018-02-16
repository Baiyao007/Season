using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.NormalComponents
{
    
    class C_CharaState : Component
    {
        private bool isLand;
        private bool isJump;
        private bool isActive;
        private bool isDead;

        public C_CharaState(
            bool isJump = false, 
            bool isActive = false)
        {
            IsJump = isJump;
            IsActive = isActive;
            isDead = false;
        }

        public bool IsActive {
            get { return isActive; }
            set { isActive = value; }
        }

        public bool IsDead {
            get { return isDead; }
            set { isDead = value; }
        }

        public bool IsLand{
            get{ return isLand; }
            set{
                //if (isLand == value) { return; }
                isLand = value;
                //isLand = !isJump;
                if (isLand) {
                    isJump = false;
                }
            }
        }

        public bool IsJump{
            get { return isJump; }
            set {
                //if (isJump == value) { return; }
                isJump = value;
                //isLand = !isJump;
                if (isJump) {
                    isLand = false;
                }
            }
        }





    }
}
