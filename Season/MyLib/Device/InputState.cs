//作成日：　2017.03.14
//作成者：　柏
//クラス内容：　入力管理クラス
//修正内容リスト：
//名前：柏　　日付：2017.10.05　　　内容：マウス状態の追加
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace MyLib.Device
{
    public class InputState
    {
        private KeyboardState currentKey;   //今のキーボード入力状態
        private KeyboardState previousKey;  //前フレームのキーボード入力状態
        private GamePadState currentPad;    //今のコントローラー入力状態
        private GamePadState previousPad;   //前フレームの今のコントローラー入力状態入力状態
        public MouseState mouseState { get; set; }

        public InputState() { }

        /// <summary>
        /// キーボードとコントローラーの入力状態更新
        /// </summary>
        public void Update() {
            var keyState = Keyboard.GetState();
            var buttonState = GamePad.GetState(PlayerIndex.One);
            UpdateState(keyState, buttonState);
        }

        /// <summary>
        /// キーボードとコントローラーの入力状態更新
        /// </summary>
        /// <param name="keyState">キーボード状態</param>
        /// <param name="padState">コントローラー状態</param>
        public void UpdateState(KeyboardState keyState, GamePadState padState) {
            previousKey = currentKey;
            currentKey = keyState;

            previousPad = currentPad;
            currentPad = padState;
        }


        /// <summary>
        /// 押しっぱなし
        /// </summary>
        /// <param name="key">keyboardのkey</param>
        /// <param name="button">padのbutton</param>
        /// <returns></returns>
        public bool IsDown(Keys key, Buttons button) {
            return IsDown(key) || IsDown(button);
        }

        public bool IsDown(Keys key) { return currentKey.IsKeyDown(key); }
        public bool IsDown(Buttons button) { return currentPad.IsButtonDown(button); }

        /// <summary>
        /// 押した瞬間
        /// </summary>
        /// <param name="key">keyboardのkey</param>
        /// <param name="button">padのbutton</param>
        /// <returns></returns>
        public bool WasDown(Keys key, Buttons button) {
            return WasDown(key) || WasDown(button);
        }

        public bool WasDown(Keys key) {
            bool current_Key = currentKey.IsKeyDown(key);
            bool previous_Key = previousKey.IsKeyDown(key);
            return current_Key && !previous_Key;
        }

        public bool WasDown(Buttons button) {
            bool currentButton = currentPad.IsButtonDown(button);
            bool previousButton = previousPad.IsButtonDown(button);
            return currentButton && !previousButton;
        }

        /// <summary>
        /// 上がった瞬間
        /// </summary>
        /// <param name="key">keyboardのkey</param>
        /// <param name="button">padのbutton</param>
        /// <returns></returns>
        public bool IsUp(Keys key, Buttons button) {
            bool current_Key = currentKey.IsKeyDown(key);
            bool previous_Key = previousKey.IsKeyDown(key);

            bool currentButton = currentPad.IsButtonDown(button);
            bool previousButton = previousPad.IsButtonDown(button);

            return (!current_Key && previous_Key) || (!currentButton && previousButton);
        }

        public Vector2 GetMousePosition() {
            return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        public Vector2 GetLeftStickDirection() {
            Vector2 stickDirection = new Vector2(
                currentPad.ThumbSticks.Left.X,
                -currentPad.ThumbSticks.Left.Y
            );
            return stickDirection;
        }

        public Vector2 GetRightStickDirection(){
            Vector2 stickDirection = new Vector2(
                currentPad.ThumbSticks.Right.X,
                -currentPad.ThumbSticks.Right.Y
            );
            return stickDirection;
        }

    }
}
