//作成日：　2017.10.18
//作成者：　柏
//クラス内容：  移動更新用実装（Ｕターンする）
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using System;
using Microsoft.Xna.Framework;

namespace MyLib.Utility.Action.Movements
{
    public class MoveUturn : IMoveAble
    {
        public MoveUturn() { }

        private MoveUturn(MoveUturn other)
            : this()
        { }

        public IMoveAble Clone()
        {
            return new MoveUturn(this);
        }

        public void Move(ref Vector2 velocity, ref float speed){
            velocity = -velocity;
        }
    }
}
