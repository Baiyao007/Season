//作成日：　2017.10.11
//作成者：　柏
//クラス内容：  移動更新用インタフェース
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility.Action
{
    public interface IMoveAble
    {
        void Move(ref Vector2 velocity, ref float speed);

        IMoveAble Clone();
    }
}
