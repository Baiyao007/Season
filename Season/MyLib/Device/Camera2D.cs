//作成日：　2017.10.11
//作成者：　柏
//クラス内容：　カメラクラス
//修正内容リスト：
//名前：柏　　　日付：2017.11.20　　　内容：メソッドのstatic化
//名前：　　　日付：　　　内容：


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Device
{
    public class Camera2D
    {
        private static Vector2 startPosition;       //targetは左上の（0,0）から描画したい座標
        private static Vector2 targetForcus;
        private static Vector2 nowForcus;
        private static Vector2 offsetPosition;
        private static Vector2 priviousOffset;
        private static Rectangle cameraRect;

        private static float zoomSize;
        private static Matrix transform = Matrix.CreateTranslation(Vector3.Zero);

        //初期化してから変わらない
        private static Matrix projection;
        private static Matrix view;
        private static Vector2 stageSize;
        private static Vector2 screenSize;
        private static Vector2 offsetDirect;


        public Camera2D(Viewport viewport, Vector2 stageS) {
            stageSize = stageS;
            screenSize = new Vector2(viewport.Width, viewport.Height);
            offsetDirect = new Vector2(-viewport.Width / 4, 0);
            projection = Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height, 0, 0, 1);
            view = Matrix.Identity;

            offsetPosition = Vector2.Zero;

            //targetは画面の中心からちょっと下の位置に描画するよう
            startPosition = new Vector2(viewport.Width, viewport.Height) / 2;
            startPosition.Y += 400;
            targetForcus = startPosition + offsetDirect;
            nowForcus = startPosition + offsetDirect;

            cameraRect = new Rectangle();

            zoomSize = 1;
            transform = Matrix.CreateTranslation(Vector3.Zero);
        }

        public static float GetZoom() { return zoomSize; }
        public static Matrix GetTransform() { return transform; }
        public static void ZoomIn() { zoomSize += 0.01f; }
        public static void ZoomOut() { zoomSize -= 0.01f; }
        public static void ZoomInitialize() { zoomSize = 1; }
        public static void TurnRight() { targetForcus.X = (startPosition + offsetDirect).X; }
        public static void TurnLeft() { targetForcus.X = (startPosition - offsetDirect).X; }


        public static void Initialize() {
            cameraRect = new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y);
            Vector2 targetPosition = screenSize / 2;
            InScreenClip(ref targetPosition);

            //2Dカメラ処理(ZoomInの状況に合わせて移動)
            transform =
                Matrix.CreateTranslation(new Vector3(-targetPosition, 0)) *     //targetのところに移動
                Matrix.CreateScale(zoomSize, zoomSize, 1) *                     //Zoom処理
                Matrix.CreateRotationZ(0) *                                     //回転処理
                Matrix.CreateTranslation(new Vector3(startPosition, 0));        //targetを画面に描画したい所に映りための移動

            //Shader描画できるEntityはEffect使って描画するため、Entityの映すPositionを算出
            priviousOffset = startPosition - targetPosition;
            offsetPosition = priviousOffset;
        }

        public static void Update(Vector2 targetPosition) {
            cameraRect = new Rectangle(
                (int)(targetPosition - nowForcus).X,
                (int)(targetPosition - nowForcus).Y,
                (int)screenSize.X,
                (int)screenSize.Y
            );
            InScreenClip(ref targetPosition);
            LocusSlowMove(ref targetPosition);
            LocusTurnMove();
            
            //2Dカメラ処理(ZoomInの状況に合わせて移動)
            transform =
                Matrix.CreateTranslation(new Vector3(-targetPosition, 0)) *     //targetのところに移動
                Matrix.CreateScale(zoomSize, zoomSize, 1) *                     //Zoom処理
                Matrix.CreateRotationZ(0) *                                     //回転処理
                Matrix.CreateTranslation(new Vector3(nowForcus, 0));        //targetを画面に描画したい所に映りための移動

            //Shader描画できるEntityはEffect使って描画するため、Entityの映すPositionを算出
            priviousOffset = offsetPosition;
            offsetPosition = nowForcus - targetPosition;
        }

        private static void LocusTurnMove() {
            nowForcus += (targetForcus - nowForcus) * 0.02f;
        }

        private static void LocusSlowMove(ref Vector2 targetPosition) {
            Vector2 privioustargetPosition = nowForcus - priviousOffset;
            targetPosition = privioustargetPosition + (targetPosition - privioustargetPosition) * 0.2f;
        }

        private static void InScreenClip(ref Vector2 targetPosition) {
            if (cameraRect.Left <= 0) {
                targetPosition.X -= cameraRect.Left;
            }
            else if (cameraRect.Right >= stageSize.X) {
                targetPosition.X -= cameraRect.Right - stageSize.X;
            }
            if (cameraRect.Top <= 0) {
                targetPosition.Y -= cameraRect.Top;
            }
            else if (cameraRect.Bottom >= stageSize.Y) {
                targetPosition.Y -= cameraRect.Bottom - stageSize.Y;
            }
        }

        public static Vector2 GetCameraMove() { return offsetPosition - priviousOffset; }
        public static Vector2 GetOffsetPosition() { return offsetPosition; }
        public static Vector3 GetOffsetPosition3() { return new Vector3(offsetPosition, 0); }
        public static Matrix GetProjection() { return projection; }
        public static Matrix GetView() { return view; }

    }
}
