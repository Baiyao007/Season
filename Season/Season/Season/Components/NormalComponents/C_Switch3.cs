using MyLib.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.NormalComponents
{
    enum eSwitchDirect {
        Left = -1,
        None,
        Right,
    }

    class C_Switch3 : Component
    {
        private eSwitchDirect switchValue;
        private eSwitchDirect priviousValue;

        public C_Switch3(bool isRight = true) {
            priviousValue = isRight ? eSwitchDirect.Right : eSwitchDirect.Left;
            switchValue = eSwitchDirect.None;
        }

        public void SetRight(bool isJump) {
            if (IsRight()) { return; }

            if (isJump) { TurnRight_Jump(); }
            else { TurnRight_Walk(); }

            priviousValue = switchValue;
            switchValue = eSwitchDirect.Right;
        }

        public void SetNowRight(bool isJump) {
            if (IsNowRight()) { return; }
            switchValue = eSwitchDirect.Right;

            if (priviousValue == eSwitchDirect.Right) { return; }

            if (isJump) { TurnRight_Jump(); }
            else { TurnRight_Walk(); }

            priviousValue = switchValue;
            switchValue = eSwitchDirect.Right;
        }

        public void SetLeft(bool isJump) {
            if (IsLeft()) { return; }
            if (isJump) { TurnLeft_Jump(); }
            else { TurnLeft_Walk(); }

            priviousValue = switchValue;
            switchValue = eSwitchDirect.Left;
        }


        public void SetNowLeft(bool isJump) {
            if (IsNowRight()) { return; }
            switchValue = eSwitchDirect.Left;

            if (priviousValue == eSwitchDirect.Left) { return; }

            if (isJump) { TurnLeft_Jump(); }
            else { TurnLeft_Walk(); }

            priviousValue = switchValue;
            switchValue = eSwitchDirect.Left;
        }

        private void TurnRight_Jump() { entity.transform.Angle = 180 - entity.transform.Angle; }
        private void TurnLeft_Jump() { entity.transform.Angle = 180 - entity.transform.Angle; }
        private void TurnRight_Walk() { entity.transform.Angle -= 180; }
        private void TurnLeft_Walk() { entity.transform.Angle += 180; }


        public void SetNone() {
            if (IsNone()) { return; }
            priviousValue = switchValue;
            switchValue = eSwitchDirect.None;
        }


        public void UTurn(bool isJump) {
            if (IsRight()) { SetLeft(isJump); }
            else if (IsLeft()) { SetRight(isJump); }
        }

        public bool IsNone() { return switchValue == eSwitchDirect.None; }
        public bool IsNowRight() { return switchValue == eSwitchDirect.Right; }
        public bool IsNowLeft() { return switchValue == eSwitchDirect.Left; }
        public bool WasRight() { return IsNone() && priviousValue == eSwitchDirect.Right; }
        public bool WasLeft() { return IsNone() && priviousValue == eSwitchDirect.Left; }

        public bool IsRight() { return IsNowRight() || WasRight(); }
        public bool IsLeft() { return IsNowLeft() || WasLeft(); }
        
        
        

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる

            if (IsRight()) { entity.transform.Angle = 0; }
            if (IsLeft()) { entity.transform.Angle = 180; }
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除

        }
    }
}
