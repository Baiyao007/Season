//作成日：　2017.03.14
//作成者：　柏
//クラス内容：　リソース管理クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace MyLib.Device
{
    public class ResouceManager
    {
        private ContentManager contentManager;

        private static Dictionary<string, Model> models;
        private static Dictionary<string, Texture2D> textures;
        private static Dictionary<string, Effect> effects;
        private static Dictionary<string, SpriteFont> fonts;
        private static Dictionary<string, Song> bgms;           //MP3管理用
        private static Dictionary<string, SoundEffect> SoundEffects;    //WAV管理用

        public ResouceManager(ContentManager content)
        {
            contentManager = content;
            models = new Dictionary<string, Model>();
            textures = new Dictionary<string, Texture2D>();
            effects = new Dictionary<string, Effect>();
            fonts = new Dictionary<string, SpriteFont>();

            bgms = new Dictionary<string, Song>();
            SoundEffects = new Dictionary<string, SoundEffect>();
        }

        /// <summary>
        /// 3Dモデル読み込む
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="path">ファイルアドレス</param>
        public void LoadModels(string name, string path = "./MODEL/") {
            models.Add(name, contentManager.Load<Model>(path + name));
        }

        /// <summary>
        /// 2D画像の読み込み
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="texture">画像</param>
        public void LoadTexture(string name, Texture2D texture) {
            textures.Add(name, texture);
        }

        /// <summary>
        /// 2D画像の読み込み
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="filepath">ファイルアドレス</param>
        public void LoadTextures(string name, string filepath = "./IMAGE/") {
            if (textures.ContainsKey(name)) { return; }
            textures.Add(name, contentManager.Load<Texture2D>(filepath + name));
        }

        /// <summary>
        /// エフェクトの読込　by柏　2017.02.21
        /// </summary>
        /// <param name="name">エフェクトのアセット名</param>
        /// <param name="filepath">ファイルアドレス</param>
        public void LoadEffect(string name, string filepath = "./EFFECT/") {
            if (effects.ContainsKey(name)) { return; }
            effects.Add(name, contentManager.Load<Effect>(filepath + name));
        }

        /// <summary>
        /// フォントの読込　by柏　2017.02.08
        /// </summary>
        /// <param name="fontName">フォントの名前</param>
        /// <param name="filepath">ファイルアドレス</param>
        public void LoadFont(string fontName, string filepath = "./FONT/") {
            if (fonts.ContainsKey(fontName)) { return; }
            fonts.Add(fontName, contentManager.Load<SpriteFont>(filepath + fontName));
        }

        /// <summary>
        /// BGMファイルを読み取る
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="filepath">保存アドレス</param>
        public void LoadBGM(string name, string filepath = "./MP3/") {
            if (bgms.ContainsKey(name)) { return; }
            bgms.Add(name, contentManager.Load<Song>(filepath + name));
        }

        /// <summary>
        /// SEファイルを読み取る
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <param name="filepath">保存アドレス</param>
        public void LoadSE(string name, string filepath = "./WAV/") {
            if (SoundEffects.ContainsKey(name)) { return; }
            SoundEffects.Add(name, contentManager.Load<SoundEffect>(filepath + name));
        }

        /// <summary>
        /// 読み込んだ画像、エフェクト、フォントのリセット
        /// </summary>
        public void UnLoad() {
            models.Clear();
            textures.Clear();
            effects.Clear();
            fonts.Clear();
            bgms.Clear();
            SoundEffects.Clear();
        }

        /// <summary>
        /// 3Dモデル取得
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <returns>3Dモデル</returns>
        public static Model GetModel(string name) {
            Model m = models.ContainsKey(name) ? models[name] : null;
            return m;
        }

        /// <summary>
        /// 画像の幅をゲット
        /// </summary>
        /// <param name="name">画像のアセット名</param>
        /// <returns>画像の幅</returns>
        public static int GetTextureWidth(string name) {
            return textures[name].Width;
        }

        /// <summary>
        /// 画像の高さをゲット
        /// </summary>
        /// <param name="name">画像のアセット名</param>
        /// <returns>画像の高さ</returns>
        public static int GetTextureHeight(string name) {
            return textures[name].Height;
        }


        /// <summary>
        /// 画像のサイズをゲット
        /// </summary>
        /// <param name="name">画像のアセット名</param>
        /// <returns>画像のサイズ</returns>
        public static Vector2 GetTextureSize(string name) {
            return new Vector2(textures[name].Width, textures[name].Height);
        }

        /// <summary>
        /// 画像をゲット
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <returns>画像</returns>
        public static Texture2D GetTexture(string name) {
            if (!textures.ContainsKey(name)) { return null; }
            return textures[name];
        }

        /// <summary>
        /// エフェクト取得
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <returns>エフェクト</returns>
        public static Effect GetEffect(string name) {
            if (!effects.ContainsKey(name)) { return null; }
            return effects[name];
        }

        /// <summary>
        /// フォント取得
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <returns>フォント</returns>
        public static SpriteFont GetFont(string name) {
            if (!fonts.ContainsKey(name)) { return null; }
            return fonts[name];
        }

        /// <summary>
        /// エフェクト音取得
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <returns>エフェクト音</returns>
        public static SoundEffect GetSE(string name) {
            if (!SoundEffects.ContainsKey(name)) { return null; }
            return SoundEffects[name];
        }

        /// <summary>
        /// 背景音楽取得
        /// </summary>
        /// <param name="name">アセット名</param>
        /// <returns>背景音楽</returns>
        public static Song GetBGM(string name) {
            if (!bgms.ContainsKey(name)) { return null; }
            return bgms[name];
        }
    }
}
