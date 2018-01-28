//作成日：　2017.10.20
//作成者：　柏
//クラス内容：　ゲームルール管理
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Def
{
    static class GameConst
    {
        public static int Score = 0;
        public static int Level = 1;
        public static bool IsClear = false;
        public static bool IsEnding = false;

        public static void AddScore(int point)
        {
            Score += point;
            if (Score >= 50) { Level = 2; }
            if (Score >= 100) { Level = 3; }
        }

        public static void SetClear() {
            IsClear = true;
        }

        public static void SetEnding() {
            IsEnding = true;
        }

        public static void Initialize() {
            Score = 0;
            Level = 1;
            IsClear = false;
            IsEnding = false;
        }
    }
}
