//作成日：　2018.01.19
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
    public interface IChangeAble
    {
        void Change(ref Vector2 size, ref float alpha);

        IChangeAble Clone();
    }
}
