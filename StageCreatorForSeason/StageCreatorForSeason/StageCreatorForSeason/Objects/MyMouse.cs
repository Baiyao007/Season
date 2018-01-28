using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MyLib.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageCreatorForSeason.Objects
{
    class MyMouse
    {
        private ObjectManager objectManager;

        private ButtonState nowLeftState;
        private ButtonState priviousLeftState;

        private ButtonState nowRightState;
        private ButtonState priviousRightState;

        private int nowScrollWheel;
        private int priviousScrollWheel;

        private List<Object> targets;

        public MyMouse(ObjectManager manager) {
            objectManager = manager;
            targets = new List<Object>();
        }

        public Vector2 GetPosition() {
            return new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - Camera2D.GetOffsetPosition();
        }
        public void AddTarget(Object target) { targets.Add(target); }
        public void ClearTargets() { targets.Clear(); }
        public List<Object> GetTargets() { return targets; }
        public void SetTargetsPosition() { targets.ForEach(t => t.Position = GetPosition()); }

        public bool IsPressingLeft() {
            return nowLeftState == ButtonState.Pressed && priviousLeftState == ButtonState.Pressed;
        }
        public bool WasPressedLeft() {
            return nowLeftState == ButtonState.Pressed && priviousLeftState == ButtonState.Released;
        }
        public bool WasPressedRight() {
            return nowRightState == ButtonState.Pressed && priviousRightState == ButtonState.Released;
        }
        public bool WasScrollWheelPlus() {
            return nowScrollWheel > priviousScrollWheel;
        }
        public bool WasScrollWheelMinus() {
            return nowScrollWheel < priviousScrollWheel;
        }

        public void Update() { UpdateState(); }

        public void Draw()
        {
            Renderer_2D.Begin(Camera2D.GetTransform());

            Vector2 imgSize = ResouceManager.GetTextureSize("Mouse");
            Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
            Renderer_2D.DrawTexture("Mouse", GetPosition(), Color.LightBlue, Vector2.One);

            Renderer_2D.End();

        }

        private void UpdateState() {
            priviousLeftState = nowLeftState;
            nowLeftState = Mouse.GetState().LeftButton;
            
            priviousRightState = nowRightState;
            nowRightState = Mouse.GetState().RightButton;

            priviousScrollWheel = nowScrollWheel;
            nowScrollWheel = Mouse.GetState().ScrollWheelValue;
        }

    }
}
