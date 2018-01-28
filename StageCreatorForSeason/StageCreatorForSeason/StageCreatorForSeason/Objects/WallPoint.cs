using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StageCreatorForSeason.Objects
{
    class WallPoint : Object
    {
        public WallPoint(Vector2 position, int teamNo)
            : base("Point", position, 20, eObjectType.Wall, teamNo)
        { }

    }
}
