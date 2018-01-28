//作成日：　2017.03.14
//作成者：　柏
//クラス内容：　CSV読込クラス
//修正内容リスト：
//名前：　　　日付：　　　内容：
//名前：　　　日付：　　　内容：

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MyLib.Utility
{
    public static class CSVReader
    {
        static List<string[]> stringData = new List<string[]>();

        public static void Read(string filename, int offset = 0) {
            try {
                stringData.Clear();
                //開かれたファイルを自動的に閉じる書き方（IDisposable）
                using (var sr = new StreamReader("Content/CSV/" + filename + ".csv")) {
                    if (!sr.EndOfStream) {
                        for (int i = 0; i < offset; i++){
                            sr.ReadLine();
                        }
                    }
                    while (!sr.EndOfStream) {
                        var line = sr.ReadLine();
                        var values = line.Split(',');
                        stringData.Add(values);
                        //foreach (var v in values)
                        //{
                        //    System.Console.Write("{0}", v);
                        //}
                        //System.Console.WriteLine();
                    }
                }
            }
            catch (System.Exception e) {
                System.Console.WriteLine(e.Message);
            }
        }

        public static void Save(string filename, List<List<Vector2>> data)
        {
            FileStream fileStream = new FileStream("Content/CSV/" + filename +  ".csv", FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding("Shift_JIS"));

            for (int i = 0; i < data.Count; i++) {
                if (data[i].Count == 0) { continue; }
                streamWriter.WriteLine("0,0");
                for (int j = 0; j < data[i].Count; j++) {
                    streamWriter.Write((int)data[i][j].X + ",");
                    streamWriter.Write((int)data[i][j].Y);
                    streamWriter.WriteLine();
                }
            }

            streamWriter.Close();
            fileStream.Close();
        }



        public static List<string[]> GetData() {
            return stringData;
        }

        public static string[][] GetArrayData() {
            return stringData.ToArray();
        }

        public static int[][] GetIntData() {
            var data = GetArrayData();
            int y = data.Count();
            int x = data[0].Count();

            int[][] intData = new int[y][];
            for (int i = 0; i < y; i++) {
                intData[i] = new int[x];
            }
            for (int i = 0; i < y; i++) {
                for (int j = 0; j < x; j++) {
                    intData[i][j] = int.Parse(data[i][j]);
                }
            }
            return intData;
        }

        public static string[,] GetStringMatrix() {
            var data = GetArrayData();
            int y = data.Count();
            int x = data[0].Count();

            string[,] result = new string[y, x];
            for (int i = 0; i < y; i++) {
                for (int j = 0; j < x; j++) {
                    result[i, j] = data[i][j];
                }
            }
            return result;
        }

        public static int[,] GetIntMatrix() {
            var data = GetArrayData();
            if (data.GetLength(0) == 0) { return new int[0, 0]; }
            int y = data.Count();
            int x = data[0].Count();

            int[,] result = new int[y, x];
            for (int i = 0; i < y; i++) {
                for (int j = 0; j < x; j++) {
                    result[i, j] = int.Parse(data[i][j]);
                }
            }
            return result;
        }
    }
}
