//作成日：　2017.11.20
//作成者：　柏
//クラス内容：　変換の基本情報クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Entitys
{
    public class Transform2D
    {
        public Vector2 Position { get; set; }
        public float Angle { get; set; }
        
        public float Scale { get; set; }

        public float SetPositionX {
            get{ return Position.X; }
            set { Position = new Vector2(value, Position.Y); }
        }
        public float SetPositionY {
            get { return Position.Y; }
            set { Position = new Vector2(Position.X, value); }
        }

    }
}
