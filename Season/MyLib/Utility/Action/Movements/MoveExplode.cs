using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility.Action.Movements
{
    public class MoveExplode : IMoveAble
    {
        private Timer timer;
        private float startSpeed;
        private bool isTurned;

        public MoveExplode() {
            timer = new Timer(1.0f);
            startSpeed = 0;
            isTurned = false;
        }

        private MoveExplode(MoveExplode other)
            : this()
        { }

        public IMoveAble Clone() {
            return new MoveExplode(this);
        }
        public void Move(ref Vector2 velocity, ref float speed) {
            if (startSpeed == 0) { startSpeed = speed; }
            if (isTurned) {
                speed *= 1.03f;
                return;
            }


            timer.Update();
            speed = startSpeed * timer.Rate();

            if (timer.IsTime) {
                isTurned = true;
                velocity *= -1;
                speed = startSpeed * 5;
            }
        }

    }
}
