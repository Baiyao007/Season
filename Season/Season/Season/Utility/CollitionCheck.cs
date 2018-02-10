using Microsoft.Xna.Framework;
using MyLib.Utility;
using Season.Components;
using Season.Components.ColliderComponents;
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


        public static bool CircleSquare(C_Collider_Square square, C_Collider_Circle circle)
        {
            bool isCollide = false;
            for (int i = 0; i < square.points.Count; i++) {
                Vector2 centre = circle.centerPosition;
                float radius = circle.radius;

                int nextIndex = (int)Method.Warp(0, square.points.Count, i + 1);
                Vector2 thisPoint = square.points[i];
                Vector2 nextPoint = square.points[nextIndex];
                Vector2 normal = Vector2.Zero;

                isCollide = Method.CircleSegment(ref centre, radius, thisPoint, nextPoint, ref normal);
                if (isCollide) { return isCollide; }
            }
            return isCollide;
        }

    }
}
