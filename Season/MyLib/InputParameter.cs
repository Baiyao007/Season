//作成日：　2017.10.20
//作成者：　柏
//クラス内容：　入力常数管理
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework.Input;

namespace MyLib
{
    public static class InputParameter
    {

        //キーボード
        public static Keys LeftKey = Keys.Left;
        public static Keys RightKey = Keys.Right;
        public static Keys UpKey = Keys.Up;
        public static Keys DownKey = Keys.Down;

        public static Keys JumpKey = Keys.Space;
        public static Keys ConfirmKey = Keys.Enter;

        //コントローラー
        public static Buttons LeftButton = Buttons.LeftThumbstickLeft;
        public static Buttons RightButton = Buttons.LeftThumbstickRight;
        public static Buttons UpButton = Buttons.LeftThumbstickUp;
        public static Buttons DownButton = Buttons.LeftThumbstickDown;

        public static Buttons JumpButton = Buttons.B;
        public static Buttons ConfirmButton = Buttons.A;
    }
}
