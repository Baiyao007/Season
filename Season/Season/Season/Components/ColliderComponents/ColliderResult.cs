using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components.ColliderComponents
{
    public class ColliderResult
    {
        public ColliderComponent otherCollider { get; set; }
        public bool isThroughCurrent { get; set; }
        public bool isThroughPrivious { get; set; }
        public bool isJostleCurrent { get; set; }
        public bool isJostlePrivious { get; set; }

        public ColliderResult() {
            otherCollider = null;
            isThroughCurrent = false;
            isThroughPrivious = false;
            isJostleCurrent = false;
            isJostlePrivious = false;
        }

        public bool ThroughStart() { return !isThroughPrivious && isThroughCurrent; }
        public bool IsThrough() { return isThroughPrivious && isThroughCurrent; }
        public bool ThroughEnd() { return isThroughPrivious && !isThroughCurrent; }


        public bool JostleStart() { return !isJostlePrivious && isJostleCurrent; }
        public bool IsJostle() { return isJostlePrivious && isJostleCurrent; }
        public bool JostleEnd() { return isJostlePrivious && !isJostleCurrent; }

        public bool IsCollide() {
            bool through = isThroughCurrent || isThroughPrivious;
            bool jostle = isJostleCurrent || isJostlePrivious;
            return through || jostle;
        }

        public void InitialzeResultPerFrame() {
            isThroughPrivious = isThroughCurrent;
            isThroughCurrent = false;     //多数の判定になるとbug
            isJostlePrivious = isJostleCurrent;
            isJostleCurrent = false;     //多数の判定になるとbug
        }

    }
}
