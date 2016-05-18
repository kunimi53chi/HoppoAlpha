using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Microsoft.VisualBasic.FileIO;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;

namespace HoppoAlpha.DataLibrary.DataObject
{
    public class ExMasterSlotitemCollection : Dictionary<int, ExMasterSlotitem>
    {
        AutoResetEvent _autoResetEvent; //APIとの同期を取るため

        public static string FolderPath { get; private set; }
        public static string FilePath { get; private set; }

        static ExMasterSlotitemCollection()
        {
            FolderPath = Environment.CurrentDirectory + @"\config";
            FilePath = Environment.CurrentDirectory + @"\config\slotitems.csv";
        }

        /// <summary>
        /// デフォルトで値をセットしないコンストラクタ
        /// </summary>
        /// <param name="notInitialSet">値は無視されます</param>
        public ExMasterSlotitemCollection(bool notInitialSet)
            : base()
        { }

        public ExMasterSlotitemCollection()
            :this(null)
        { }

        public ExMasterSlotitemCollection(Stream stream)
            : base()
        {
            _autoResetEvent = new AutoResetEvent(false);
            
            List<List<string>> data = new List<List<string>>();
            //リソースからデフォルトデータの取得
            Action getDefaultData = () =>
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                using (var parser = new TextFieldParser(assembly.GetManifestResourceStream("HoppoAlpha.DataLibrary.slotitems160422.csv"), Encoding.GetEncoding("shift-jis")))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        var line = parser.ReadFields();
                        //データに格納
                        List<string> row = new List<string>();
                        row.AddRange(line);
                        data.Add(row);
                    }
                }
            };
            //指定したファイルがある場合
            if(stream != null && stream is FileStream && File.Exists((stream as FileStream).Name))
            {
                using (var parser = new TextFieldParser((stream as FileStream).Name, Encoding.GetEncoding("shift-jis")))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        var line = parser.ReadFields();
                        //データに格納
                        List<string> row = new List<string>();
                        row.AddRange(line);
                        data.Add(row);
                    }
                }
            }
            //ない場合
            else
            {
                getDefaultData();
            }
            //CSVデータ→パラメーターの取得
            if (data.Count < 2) throw new InvalidDataException();

            //データのバージョンチェック
            int version = 0;
            int startRow = 1;
            if(data[0].Count > 1 && data[0][0] == "Version")
            {
                int.TryParse(data[0][1], out version);
            }
            //バージョンが低かったら上書き
            if(Version.ExMasterSlotitemDataVersion > version)
            {
                data = new List<List<string>>();
                getDefaultData();
                version = Version.ExMasterSlotitemDataVersion;
            }
            if (version > 0) startRow++;

            //データの取得
            List<string> header = data[startRow - 1];
            for (int i = startRow; i < data.Count; i++)
            {
                var slotitem = new ExMasterSlotitem();

                for (int j = 0; j < data[i].Count; j++)
                {
                    string h = header[j];
                    string v = data[i][j];

                    slotitem.SetValueFromCsv(h, v);
                }

                this[slotitem.api_id] = slotitem;
            }

            _autoResetEvent.Set();//CSVの読み込みが終わってからAPIとマージする
        }

        /// <summary>
        /// マスターデータとのマージをします
        /// </summary>
        /// <param name="slots">マスターデータ</param>
        public void MergeMasterData(List<ApiMstSlotitem> slots)
        {
            _autoResetEvent.WaitOne();//CSVの読み込みを待つ

            foreach(var x in slots)
            {
                //該当データが既にCSVからインポートしてある場合
                ExMasterSlotitem data;
                if(this.TryGetValue(x.api_id, out data))
                {
                    data.MergeValueFromMasterData(x);
                }
                //ない場合
                else
                {
                    this[x.api_id] = new ExMasterSlotitem(x);
                }
            }

            _autoResetEvent.Set();
        }

        /// <summary>
        /// CSVファイルに保存します
        /// <param name="stream">保存するストリーム</param>
        /// </summary>
        public string Save(Stream stream)
        {
            //初期化されていない場合
            if (this == null || this.Count == 0) return "success";

            //フォルダがない場合
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
            //CSVファイルの作成
            StringBuilder sb = new StringBuilder();
            //バージョン
            sb.AppendLine("Version," + Version.ExMasterSlotitemDataVersion);
            //ヘッダー
            sb.AppendLine(string.Join(",", ExMasterSlotitem.ExportCSVHeader()));
            //各行
            foreach(var x in this.Values)
            {
                sb.AppendLine(string.Join(",", x.ExportCSV()));
            }
            //保存
            try
            {
                //usingをするとFiles.TryLoad側でStreamが閉じられてしまうので
                var sw = new StreamWriter(stream, Encoding.GetEncoding("shift-jis"));
                sw.WriteLine(sb.ToString());
                sw.Flush();
                sw = null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "success";
        }
    }
}
