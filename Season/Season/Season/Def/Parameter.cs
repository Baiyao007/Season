//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　常数管理
//修正内容リスト：
//名前：柏　　　日付：20171020　　　内容：buttonとkeyに関する部分MyLibに移動
//名前：柏　　　日付：20171102　　　内容：反射機能のために、フレーム枠の計算を加える

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Season.Def
{
    static class Parameter
    {
        public static Vector2 ScreenSize = new Vector2(1920, 1080);
        public static Vector2 StageSize = new Vector2(100000, 2048);
        public static readonly int CollitionRadius_O = 25;
        public static readonly int CollideAbleDistance = 500;
        public static readonly int CollideAbleDistanceSquare = CollideAbleDistance * CollideAbleDistance;
        public static readonly int BackGroundSize = 2000;

        public static readonly float PlayerLimitSpeed = 10;
        public static readonly float IceRouteTime = 5;

        public static Vector2 FrameLT = Vector2.Zero;
        public static Vector2 FrameRB = ScreenSize;
        public static bool IsDebug = false;

        public static bool Debug {
            get { return IsDebug; }
            set { IsDebug = value; }
        }
    }
}
