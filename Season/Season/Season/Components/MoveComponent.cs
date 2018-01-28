//作成日：　2017.11.20
//作成者：　柏
//クラス内容：　移動用Component親クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Season.Components
{
    class MoveComponent : UpdateComponent
    {
        public float speed;
        public Vector2 velocity;

        public MoveComponent(float speed, Vector2 velocity) {
            this.speed = speed;
            this.velocity = velocity;
        }


        protected virtual void UpdateMove() {
            entity.transform.Position += velocity * speed;
        }

        public override void Update() {
            base.Update();
            UpdateMove();
        }

        public override void Active()
        {
            base.Active();
            //TODO 更新コンテナに自分を入れる
        }

        public override void DeActive()
        {
            base.DeActive();
            //TODO 更新コンテナから自分を削除
        }
    }
}
