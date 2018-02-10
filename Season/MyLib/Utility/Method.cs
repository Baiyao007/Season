//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　自分用メソッド
//修正内容リスト：
//名前：柏　　　日付：20171020　　　内容：線形補間追加
//名前：　　　日付：　　　内容：


using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MyLib.Utility
{
    public static class Method
    {
        //for内forのまとめ
        public static void MyForeach(Action<int, int> action, Vector2 xy)
        {
            for (int y = 0; y < xy.Y; y++) {
                for (int x = 0; x < xy.X; x++) {
                    action(x, y);
                }
            }
        }

        //点の四角い内判定（外積法）
        public static bool IsInScale(Vector2 position, Vector2 leftTop, Vector2 scaleXY)
        {
            bool isIn1 = Vector2Cross(new Vector2(-scaleXY.X, 0), position - new Vector2(leftTop.X + scaleXY.X, leftTop.Y)) < 0;
            bool isIn2 = Vector2Cross(new Vector2(scaleXY.X, 0), position - new Vector2(leftTop.X, leftTop.Y + scaleXY.Y)) < 0;
            bool isIn3 = Vector2Cross(new Vector2(0, -scaleXY.Y), position - new Vector2(leftTop.X + scaleXY.X, leftTop.Y + scaleXY.Y)) < 0;
            bool isIn4 = Vector2Cross(new Vector2(0, scaleXY.Y), position - leftTop) < 0;
            return isIn1 && isIn2 && isIn3 && isIn4;
        }

        //二次元外積
        public static float Vector2Cross(Vector2 v1, Vector2 v2)
        {
            Vector3 cross = Vector3.Cross(new Vector3(v1, 0), new Vector3(v2, 0));
            return cross.Z;
            //return v1.X * v2.Y - v1.Y * v2.X;
        }

        //二次元ベクトル回す
        public static Vector2 RotateVector2(Vector2 vec, float angle) {
            float radian = MathHelper.ToRadians(angle);
            Vector2 newVec = new Vector2(
                vec.X * (float)Math.Cos(-radian) - vec.Y * (float)Math.Sin(-radian),
                vec.X * (float)Math.Sin(-radian) + vec.Y * (float)Math.Cos(-radian)
            );
            return newVec;
        }

        public static Vector3 RotateVector3(Vector3 vec, float angle) {
            float radian = MathHelper.ToRadians(angle);
            Vector3 newVec = new Vector3(
                vec.X * (float)Math.Cos(-radian) - vec.Y * (float)Math.Sin(-radian),
                vec.X * (float)Math.Sin(-radian) + vec.Y * (float)Math.Cos(-radian),
                vec.Z
            );
            return newVec;
        }


        //二次線形補間
        public static float GetQuadraticInterpolateValue(float timeRate)
        {
            float interpolateValue = timeRate * (2 - timeRate);
            return interpolateValue;
        }

        //三次線形補間
        public static float GetCubicInterpolateValue(float timeRate)
        {
            float interpolateValue = timeRate * timeRate * (3 - 2 * timeRate);
            return interpolateValue;
        }

        public static float GetSinInterpolateValue(float timeRate) {
            float interpolateValue =  (float)Math.Sin(Math.PI * timeRate);
            return interpolateValue;
        }

        public static float GetCosInterpolateValue(float timeRate)
        {
            float interpolateValue = (float)Math.Cos(Math.PI * timeRate);
            return interpolateValue;
        }


        //直角方向移動
        public static Vector2 RightAngleMove(Vector2 direction, float distance) {
            bool isRight = direction.X > 0;
            if (isRight) {
                direction = RotateVector2(direction, 90);
            }
            else {
                direction = RotateVector2(direction, -90);
            }
            direction.Normalize();
            direction *= distance;
            return direction;
        }

        public static int GetQuadrant(float angle) {
            int quadrant = (int)(angle / 90) % 4;
            return quadrant;
        }

        public static float AngleClamp(float angle) {
            if (angle >= 360) { angle -= 360; }
            if (angle <= 0) { angle += 360; }
            return angle;
        }

        public static float ToDegree(float radian) {
            float angle = MathHelper.ToDegrees(radian);
            angle = AngleClamp(angle);
            return angle;
        }

        public static bool CircleSegment(ref Vector2 center, float radius, Vector2 p1, Vector2 p2, ref Vector2 normal) {
            Vector2 v = p2 - p1;
            float r2 = radius * radius;

            //p1の外側にあって境界円より中心までの距離が半径以下か？
            //p1の端点で接触している
            Vector2 v1 = center - p1; //中心からp1までのベクトル
            normal = Vector2.Normalize(v);
            float t = Vector2.Dot(v1, normal) / (float)Math.Sqrt(Vector2.Dot(v, v));//中心から線分に下した点とp1までの距離の比

            if ((t < 0) && (v1.LengthSquared() <= r2)) {
                normal = Vector2.Normalize(v1);
                center = p1 + radius * normal;//中心を移動
                return true;
            }

            //p2の外側にあって境界円より中心までの距離が半径以下か？
            //p2の端点で接触している
            Vector2 v2 = center - p2;   //positionからp2までのベクトル
            if ((t > 1) && (v2.LengthSquared() <= r2)) {
                normal = Vector2.Normalize(v2);
                center = p2 + radius * normal;//中心を移動
                return true;
            }

            //境界円の中心がp1p2の間にあって線分に接触している場合
            Vector2 vn = v1 - v * t; //法線方向のベクトル
            normal = Vector2.Normalize(vn);
            float L = vn.LengthSquared();

            //線分の間
            if ((t >= 0) && (t <= 1) && (L <= r2)) {
                center = p1 + v * t + radius * normal - vn; //中心点を移動
                return true;
            }

            return false;
        }

        public static bool Circle_Ray(
            ref Vector2 centre, 
            Vector2 L_startPoint, 
            Vector2 n_Ray, 
            float radius, 
            ref Vector2 cp1, 
            ref Vector2 cp2)
        {
            Vector2 v1 = L_startPoint - centre;
            float D = Vector2.Dot(n_Ray, v1) * Vector2.Dot(n_Ray, v1) - Vector2.Dot(v1, v1) + radius * radius;
            if (D < 0) { return false; }
            float t1 = -Vector2.Dot(n_Ray, v1) - (float)Math.Sqrt(D);
            float t2 = -Vector2.Dot(n_Ray, v1) + (float)Math.Sqrt(D);

            cp1 = t1 > 0 ? L_startPoint + t1 * n_Ray : cp1;
            cp2 = t2 > 0 ? L_startPoint + t2 * n_Ray : cp2;
            
            return true;
        }

        public static float Warp(float min, float max, float value) {
            if (Math.Min(value, min) == value) { return max; }
            else if (Math.Max(value, max) == value) { return min; }
            return value;
        }

        public static void Warp(int min, int max, ref int value) {
            if (Math.Min(value, min) == value) {
                value = max;
                return;
            }
            else if (Math.Max(value, max) == value) {
                value = min;
                return;
            }
        }

        public static void Clamp(float min, float max, ref float value) {
            value = Math.Min(max, Math.Max(value, min));
        }

        public static int MinClamp(int bezierIndex, int min)
        {
            return Math.Max(bezierIndex, min);
        }


        public static T Begin<T>(this List<T> list) {
            return list[0];
        }

        public static T End<T>(this List<T> list) {
            return list[list.Count - 1];
        }

    }
}
