//作成日：　2017.10.06
//作成者：　柏
//クラス内容：　ボタン
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using MyLib.Device;
using MyLib.Utility.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLib.Utility
{
    public class Button
    {
        private string imgName;
        private Vector2 startPosition;
        private Vector2 position;   //中心座標
        private Vector2 velocity;
        private float alpha;
        private float size;
        IButtonAction iAction1;
        IButtonAction iAction2;
        IButtonAction iAction3;

        public Button(string imgName, Vector2 position, IButtonAction iAction1)
        {
            this.imgName = imgName;
            this.position = position;
            this.iAction1 = iAction1;
            startPosition = position;
            Initialize();
        }

        public Button(string imgName, Vector2 position, IButtonAction iAction1, IButtonAction iAction2) :
            this(imgName, position, iAction1)
        {
            this.iAction2 = iAction2;
        }

        public Button(string imgName, Vector2 position, IButtonAction iAction1, IButtonAction iAction2, IButtonAction iAction3):
            this(imgName, position, iAction1, iAction2) 
        {
            this.iAction3 = iAction3;
        }

        public void Initialize() {
            position = startPosition;
            velocity = Vector2.Zero;
            alpha = 1;
            size = 1;
        }

        public void Update() {
            iAction1.Update(ref position, ref alpha, ref size);

            if (iAction2 != null) {
                iAction2.Update(ref position, ref alpha, ref size);
            }
            if (iAction3 != null){
                iAction3.Update(ref position, ref alpha, ref size);
            }
        }

        public void Draw() {
            Vector2 imgSize = ResouceManager.GetTextureSize(imgName);
            Rectangle rect = new Rectangle(0, 0, (int)imgSize.X, (int)imgSize.Y);
            Renderer_2D.DrawTexture(imgName, position, alpha, rect, Vector2.One * size, 0, imgSize / 2);
        }

        public void SetAction(IButtonAction action){
            iAction1 = action;
            Initialize();
        }

    }
}
