//作成日：　2017.03.14
//作成者：　柏
//クラス内容：　2D描画管理クラス
//修正内容リスト：
//名前：柏　　　日付：2017.11.02　　　内容：線を描く機能追加（画像のリソース依存）
//名前：柏　　　日付：2017.11.20　　　内容：メソッドのstatic化
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MyLib.Device
{
    public class Renderer_2D
    {
        private static ContentManager contentManager;
        private static SpriteBatch spriteBatch;
        private static GraphicsDevice graphics;

        public Renderer_2D(ContentManager content, GraphicsDevice graphic) {
            contentManager = content;
            spriteBatch = new SpriteBatch(graphic);
            graphics = graphic;
        }

        public static GraphicsDevice GetGraphicsDevice() {
            return graphics;
        }

        /// <summary>
        /// 描画始め
        /// </summary>
        public static void Begin()
        {
            spriteBatch.Begin();
        }

        /// <summary>
        /// 描画始め(カメラ付き)
        /// </summary>
        public static void Begin(Matrix trans)
        {
            spriteBatch.Begin(
                SpriteSortMode.Deferred, 
                BlendState.AlphaBlend,
                null, null, null, null, 
                trans
            );
        }

        public static void BeginBlend(Matrix trans)
        {
            spriteBatch.Begin(
                SpriteSortMode.Texture, 
                BlendState.Additive,
                null, null, null, null,
                trans);
        }



        /// <summary>
        /// 描画終わり
        /// </summary>
        public static void End() {
            spriteBatch.End();
        }

        public static void GetTextureSize(Texture2D texture) {

        }

        /// <summary>
        /// 2D画像描画（通常）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        public static void DrawTexture(string name, Vector2 position) {
            Texture2D texture = ResouceManager.GetTexture(name);
            spriteBatch.Draw(texture, position, Color.White);
        }

        /// <summary>
        /// 2D画像描画（透明度あり）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="alpha">透明度</param>
        public static void DrawTexture(string name, Vector2 position, float alpha = 1.0f) {
            Texture2D texture = ResouceManager.GetTexture(name);
            spriteBatch.Draw(texture, position, Color.White * alpha);
        }

        /// <summary>
        /// 2D画像描画（色と大きさの設定あり）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="color">色</param>
        /// <param name="scale">大きさ</param>
        public static void DrawTexture(string name, Vector2 position, Color color, Vector2 scale) {
            Texture2D texture = ResouceManager.GetTexture(name);
            spriteBatch.Draw(texture, position, new Rectangle(0, 0, ResouceManager.GetTextureWidth(name), ResouceManager.GetTextureHeight(name)), color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// 2D画像描画（リソースの描画範囲設定あり）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="rect">描画範囲</param>
        public static void DrawTexture(string name, Vector2 position, Rectangle rect) {
            Texture2D texture = ResouceManager.GetTexture(name);
            spriteBatch.Draw(texture, position, rect, Color.White);
        }

        /// <summary>
        /// 2D画像描画（リソースの描画範囲と透明度の設定あり）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="rect">描画範囲</param>
        /// <param name="alpha">透明度</param>
        public static void DrawTexture(string name, Vector2 position, Rectangle rect, float alpha = 1.0f) {
            Texture2D texture = ResouceManager.GetTexture(name);
            spriteBatch.Draw(texture, position, rect, Color.White * alpha);
        }

        /// <summary>
        /// 2D画像描画(よく使う全設定あり)
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>   //画像の中心標準に描画
        /// <param name="alpha">透明度</param>
        /// <param name="rect">描画範囲</param>
        /// <param name="scale">大きさ</param>
        /// <param name="rocate">回転孤度</param>
        /// <param name="origin">回転中心</param>
        public static void DrawTexture(string name, Vector2 position, float alpha, Rectangle rect, Vector2 scale, float rocate, Vector2 origin, float depth = 1.0f) {
            Texture2D texture = ResouceManager.GetTexture(name);
            spriteBatch.Draw(texture, position, rect, Color.White * alpha, rocate, origin, scale, SpriteEffects.None, depth);
        }

        /// <summary>
        /// 2D画像描画(よく使う全設定あり)
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>   //画像の中心標準に描画
        /// <param name="color">色</param>
        /// <param name="alpha">透明度</param>
        /// <param name="rect">描画範囲</param>
        /// <param name="scale">大きさ</param>
        /// <param name="rocate">回転孤度</param>
        /// <param name="origin">回転中心</param>
        public static void DrawTexture(string name, Vector2 position, Color color, float alpha, Rectangle rect, Vector2 scale, float rocate, Vector2 origin, float depth = 1.0f)
        {
            Texture2D texture = ResouceManager.GetTexture(name);
            spriteBatch.Draw(texture, position, rect, color * alpha, rocate, origin, scale, SpriteEffects.None, depth);
        }

        /// <summary>
        /// 2D画像描画(よく使う全設定あり)
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>   //画像の中心標準に描画
        /// <param name="alpha">透明度</param>
        /// <param name="rect">描画範囲</param>
        /// <param name="scale">大きさ</param>
        /// <param name="rocate">回転孤度</param>
        /// <param name="origin">回転中心</param>
        /// <param name="isRight">横フリップするか</param>
        public static void DrawTexture(string name, Vector2 position, float alpha, Rectangle rect, Vector2 scale, float rocate, Vector2 origin, bool isRight, float depth = 1.0f)
        {
            Texture2D texture = ResouceManager.GetTexture(name);
            if (isRight) {
                spriteBatch.Draw(texture, position, rect, Color.White * alpha, rocate, origin, scale, SpriteEffects.None, depth);
            } else {
                spriteBatch.Draw(texture, position, rect, Color.White * alpha, rocate, origin, scale, SpriteEffects.FlipVertically, depth);
            }
            
        }

        /// <summary>
        /// 数字描画
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="num">数字</param>
        /// <param name="scale">大きさ</param>
        public static void DrawNumber(string name, Vector2 position, int num, float scale = 1f, float depth = 1.0f) {
            Texture2D texture = ResouceManager.GetTexture(name);
            foreach (var n in num.ToString()) {
                spriteBatch.Draw(texture, position,
                    new Rectangle((n - '0') * 16, 0, 16, 32), Color.White
                    , 0f,Vector2.Zero, scale, SpriteEffects.None, depth);
                position.X += 16 * scale;
            }
        }

        /// <summary>
        /// 文字表示用 by柏　2017.02.08
        /// </summary>
        /// <param name="fontName">フォント</param>
        /// <param name="data">表示したい文字</param>
        /// <param name="position">表示位置</param>
        /// <param name="color">色</param>
        /// <param name="scale">大きさ</param>
        public static void DrawString(string data, Vector2 position, Color color, float scale = 1.0f, string fontName = "HGPop", float depth = 1.0f) {
            SpriteFont font = ResouceManager.GetFont(fontName);
            spriteBatch.DrawString(font, data, position, color, 0, Vector2.Zero, scale, SpriteEffects.None, depth);
        }

        /// <summary>
        /// 2D画像描画（線を描く）
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="position">位置</param>
        /// <param name="color">色</param>
        public static void DrawLine(Vector2 startP, Vector2 endP, Color color, bool pointIsVisible = true) {
            string name = "UnitLine";
            Vector2 size = ResouceManager.GetTextureSize(name);
            float distance = Vector2.Distance(startP, endP);
            float radian = (float)Math.Atan2((endP - startP).Y, (endP - startP).X);

            Vector2 scale = new Vector2(distance / size.X, 1);

            DrawTexture(name, (startP + endP) / 2, color, 1,new Rectangle(0,0, (int)size.X, (int)size.Y), scale, radian, size / 2);

            if (pointIsVisible) {
                DrawTexture(name, startP, Color.Red, 1, new Rectangle(0, 0, (int)size.X, (int)size.Y), Vector2.One * 2, 0, size / 2);
                DrawTexture(name, endP, Color.Red, 1, new Rectangle(0, 0, (int)size.X, (int)size.Y), Vector2.One * 2, 0, size / 2);
            }
            
        }

    }
}
