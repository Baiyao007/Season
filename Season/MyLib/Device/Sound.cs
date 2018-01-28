//作成日：　2017.03.14
//作成者：　柏
//クラス内容：　Sound管理クラス
//修正内容リスト：
//名前：柏　　　日付：2017.11.20　　　内容：メソッドのstatic化
//名前：　　　日付：　　　内容：

using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;    //wav
using Microsoft.Xna.Framework.Media;    //mp3

namespace MyLib.Device
{
    public class Sound
    {
        private static Dictionary<string, SoundEffectInstance> seInstances;    //WAVインスタンス管理用
        private static List<SoundEffectInstance> sePlayList;   //WAVインスタンスの再生リスト

        private static string currentBGM;  //現在再生中のアセット名

        public Sound() {
            MediaPlayer.IsRepeating = true; //mp3の再生を循環する

            seInstances = new Dictionary<string, SoundEffectInstance>();
            sePlayList = new List<SoundEffectInstance>();
            currentBGM = null;

            MediaPlayer.Volume = 1.0f;
            SoundEffect.MasterVolume = 0.8f;
        }

        #region BGM関連

        /// <summary>
        /// BGM再生中止
        /// </summary>
        public static void StopBGM() {
            MediaPlayer.Stop();
            currentBGM = null;
        }

        /// <summary>
        /// BGM再生
        /// </summary>
        /// <param name="name"></param>
        public static void PlayBGM(string name) {
            if (currentBGM == name) { return; }
            if (IsPlayingBGM()) {
                StopBGM();
            }
            currentBGM = name;
            Song bgm = ResouceManager.GetBGM(currentBGM);
            MediaPlayer.Play(bgm);
        }

        /// <summary>
        /// BGMのループ設定
        /// </summary>
        /// <param name="loopFlag">循環スイッチ</param>
        public static void ChangeBGMLoopFlag(bool loopFlag) {
            MediaPlayer.IsRepeating = loopFlag;
        }

        /// <summary>
        /// ゲット再生中止情報
        /// </summary>
        /// <returns></returns>
        public static bool IsStoppedBGM() {
            return (MediaPlayer.State == MediaState.Stopped);
        }

        /// <summary>
        /// ゲット再生一時停止情報
        /// </summary>
        /// <returns></returns>
        public static bool IsPausedBGM() {
            return (MediaPlayer.State == MediaState.Paused);
        }

        /// <summary>
        /// ゲット再生情報
        /// </summary>
        /// <returns></returns>
        public static bool IsPlayingBGM() {
            return (MediaPlayer.State == MediaState.Playing);
        }

        #endregion

        #region WAV関連

        /// <summary>
        /// SE再生リストを生成
        /// </summary>
        /// <param name="name">SEアセット名</param>
        public static void CreateSEInstance(string name) {
            if (seInstances.ContainsKey(name)) { return; }
            SoundEffect se = ResouceManager.GetSE(name);
            seInstances.Add(name, se.CreateInstance());
        }

        /// <summary>
        /// SE再生
        /// </summary>
        /// <param name="name">SEアセット名</param>
        public static void PlaySE(string name) {
            SoundEffect se = ResouceManager.GetSE(name);
            se.Play();
        }

        /// <summary>
        /// SEリストからSE再生
        /// </summary>
        /// <param name="name">SEアセット名</param>
        /// <param name="loopFlag">ループスイッチ</param>
        public static void PlaySEInstance(string name, bool loopFlag = false) {
            var data = seInstances[name];
            data.IsLooped = loopFlag;
            data.Play();
            sePlayList.Add(data);
        }

        /// <summary>
        /// SE再生の一時停止
        /// </summary>
        /// <param name="name">SEアセット名</param>
        public static void PausedSE(string name) {
            foreach (var se in sePlayList) {
                if (se.State == SoundState.Playing) { se.Stop(); }
            }
        }

        /// <summary>
        /// 再生リストを空にする
        /// </summary>
        public static void RemoveSE() {
            sePlayList.RemoveAll(se => se.State == SoundState.Stopped);
        }

        #endregion

        public static void SetBGMVolume(float volume) {
            MediaPlayer.Volume = volume;
        }

        public static void SetSEVolume(float volume){
            SoundEffect.MasterVolume = volume;
        }

        /// <summary>
        /// 使ったlistとdictionaryを初期化する
        /// </summary>
        public static void Unload() {
            sePlayList.Clear();
        }

    }
}
