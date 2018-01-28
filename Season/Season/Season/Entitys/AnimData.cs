using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Entitys
{

    public class AnimData
    {
        public AnimData(int keyCount, int rowCount, float keySecond, string animName, bool isLoop = true) {
            KeyCount = keyCount;
            RowCount = rowCount;
            KeySecond = keySecond;
            AnimName = animName;
            IsLoop = isLoop;
        }

        public int RowCount { get; set; }
        public int KeyCount { get; set; }
        public float KeySecond { get; set; }
        public string AnimName { get; set; }
        public bool IsLoop { get; set; }

    }
}
