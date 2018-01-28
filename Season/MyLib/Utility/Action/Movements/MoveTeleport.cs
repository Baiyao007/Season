//作成日：　2017.11.07
//作成者：　柏
//クラス内容：  移動更新用実装（瞬間移動運動）
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using System;
using Microsoft.Xna.Framework;

namespace MyLib.Utility.Action.Movements
{
    public class MoveTeleport : IMoveAble
    {
        private bool isClockwise;
        private Timer timer;
        private static Random rand = new Random();

        public MoveTeleport(bool isClockwise)
        {
            this.isClockwise = isClockwise;
            timer = new Timer((float)rand.NextDouble() / 2);
        }

        private MoveTeleport(MoveTeleport other)
                : this(other.isClockwise)
        { }

        public IMoveAble Clone()
        {
            return new MoveTeleport(this);
        }

        public void Move(ref Vector2 velocity, ref float speed)
        {
            timer.Update();
            if (timer.IsTime)
            {
                timer.SetTimer((float)rand.NextDouble() / 2);

                if (isClockwise) {
                    velocity = Methord.RotateVector2(velocity, -90);
                }
                else {
                    velocity = Methord.RotateVector2(velocity, 90);
                }
            }
        }

    }
}
