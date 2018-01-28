//作成日：　2017.11.20
//作成者：　柏
//クラス内容：　直線移動用Component実装
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;

namespace Season.Components.MoveComponents
{
    class C_MoveStraight : MoveComponent
    {
        public C_MoveStraight(float speed, Vector2 velocity)
            : base(speed, velocity)
        { }

        protected override void UpdateMove() {
            base.UpdateMove();
        }
    }
}
