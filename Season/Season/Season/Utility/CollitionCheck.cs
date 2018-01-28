using Microsoft.Xna.Framework;
using Season.Components;
using Season.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Utility
{
    static class CollitionCheck
    {
        //円同士の当たり判定
        public static bool CollitionCheck_Circle(ColliderComponent obj1, ColliderComponent obj2)
        {
            float distanseSquare = Vector2.DistanceSquared(obj1.centerPosition, obj2.centerPosition);
            float radiusTogether = obj1.radius + obj2.radius;
            float radiusSquare = radiusTogether * radiusTogether;
            return distanseSquare <= radiusSquare;
        }

    }
}
