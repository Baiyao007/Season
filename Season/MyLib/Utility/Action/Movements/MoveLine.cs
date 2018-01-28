//作成日：　2017.10.11
//作成者：　柏
//クラス内容：  移動更新用実装（移動変数いじらない）
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;

namespace MyLib.Utility.Action.Movements
{
    public class MoveLine : IMoveAble
    {
        public MoveLine() { }

        private MoveLine(MoveLine other)
            : this()
        { }

        public IMoveAble Clone()
        {
            return new MoveLine(this);
        }
        public void Move(ref Vector2 velocity, ref float speed){ }
    }
}
