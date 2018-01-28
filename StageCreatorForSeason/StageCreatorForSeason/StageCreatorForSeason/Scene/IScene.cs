//作//作成日：　2017.10.04
//作成者：　柏
//クラス内容：　シーンの抽象クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Device;

namespace StageCreatorForSeason.Scene
{
    public interface IScene
    {
        /// <summary>
        /// 初期化
        /// </summary>
        void Initialize();

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="gameTime">時間</param>
        void Update(GameTime gameTime);

        /// <summary>
        /// 描画
        /// </summary>
        void Draw();

        /// <summary>
        /// シーンを閉める
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 終わりフラッグを取得
        /// </summary>
        /// <returns>エンドフラッグ</returns>
        bool IsEnd();

        /// <summary>
        /// 次のシーン
        /// </summary>
        /// <returns>シーンのEnum</returns>
        E_Scene Next();
    }
}
