//作成日：　2017.03.07
//作成者：　柏
//クラス内容：　リソース読込クラス(ResouceManagerできたによって作った)
//修正内容リスト：
//名前：柏　　　日付：20171103　　　内容：フォントの管理を加える
//名前：　　　日付：　　　内容：

using MyLib.Device;
using System.Collections.Generic;
using System;

namespace MyLib.Scene.Loaders
{
    public class ResouceLoader : Loader
    {
        private ResouceManager resouceManager;  //リソース管理

        public ResouceLoader(ResouceManager resouceManager, string[,] resources)
            : base(resources)
        {
            this.resouceManager = resouceManager;
            Initialize();
        }

        /// <summary>
        /// 更新
        /// </summary>
        public override void Update() {
            endFlag = true;
            if (counter < maxNum) { //読込終わってない場合
                LoadRecouce(counter);
                counter++;
                endFlag = false;
            }
        }

        /// <summary>
        /// リソースを種類によってロード
        /// </summary>
        /// <param name="counter">ロード中番号</param>
        private void LoadRecouce(int counter) {
            string folderName = resources[counter, 2] == "" ? resources[counter, 2] : resources[counter, 2] + "/";

            if (resources[counter, 1] == "./IMAGE/") {
                resouceManager.LoadTextures(resources[counter, 0], resources[counter, 1] + folderName);
            }
            else if (resources[counter, 1] == "./EFFECT/") {
                resouceManager.LoadEffect(resources[counter, 0], resources[counter, 1] + folderName);
            }
            else if (resources[counter, 1] == "./MODEL/") {
                resouceManager.LoadModels(resources[counter, 0], resources[counter, 1] + folderName);
            }
            else if (resources[counter, 1] == "./MP3/") {
                resouceManager.LoadBGM(resources[counter, 0], resources[counter, 1] + folderName);
            }
            else if (resources[counter, 1] == "./WAV/") {
                resouceManager.LoadSE(resources[counter, 0], resources[counter, 1] + folderName);
            }
            else if (resources[counter, 1] == "./FONT/") {
                resouceManager.LoadFont(resources[counter, 0], resources[counter, 1] + folderName);
            }
        }
    }
}
