//作成日：　2017.10.11
//作成者：　柏
//クラス内容：  移動更新用実装（コントローラー使用）
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Device;

namespace MyLib.Utility.Action.Movements
{
    public class MoveByControl : IMoveAble
    {
        InputState inputState;

        public MoveByControl(InputState inputState) {
            this.inputState = inputState;
        }

        private MoveByControl(MoveByControl other)
            : this(other.inputState)
        { }

        public IMoveAble Clone()
        {
            return new MoveByControl(this);
        }

        public void Move(ref Vector2 velocity, ref float speed)
        {
            speed *= 0.98f;
            Vector2 stickLDir = inputState.GetLeftStickDirection();
            if (stickLDir != Vector2.Zero)
            {
                velocity = stickLDir;
                speed = 1;
            }

            if (speed <= 0.01f)
            {
                speed = 0;
                velocity = Vector2.Zero;
            }
        }
    }
}
