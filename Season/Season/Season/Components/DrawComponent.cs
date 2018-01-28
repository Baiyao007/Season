//作成日：　2017.11.20
//作成者：　柏
//クラス内容：　描画用Component親クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

namespace Season.Components
{
    public class DrawComponent : Component
    {
        public float alpha;
        public float depth;

        public virtual void Draw() { }

        public void SetDepth(float depth) {
            this.depth = depth;
            TaskManager.SordDepth();
        }

        public override void Active()
        {
            base.Active();
            //TODO 描画コンテナに自分を入れる

        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 描画コンテナから自分を削除

        }


    }
}
