//作成日：　2017.02.06
//クラス内容：　ローダーの抽象クラス
//修正内容リスト：
//

namespace MyLib.Scene.Loaders
{
    abstract public class Loader
    {
        protected string[,] resources;
        protected int counter;  //今ロード中のリソースの番号
        protected int maxNum;   //リソースの総量
        protected bool endFlag;

        public Loader(string[,] resources) {
            this.resources = resources;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize() {
            counter = 0;
            endFlag = false;
            maxNum = (resources == null) ? 0 : resources.GetLength(0);
        }

        /// <summary>
        /// 今ロード中のリソースの総量を取得
        /// </summary>
        public int Count {
            get { return maxNum; }
        }

        /// <summary>
        /// 今ロード中のリソースの番号を取得
        /// </summary>
        public int CurrentCount {
            get { return counter; }
        }

        /// <summary>
        /// 終わりフラッグが取得
        /// </summary>
        public bool IsEnd { get { return endFlag; } }

        /// <summary>
        /// 更新
        /// </summary>
        public abstract void Update();
    }
}
