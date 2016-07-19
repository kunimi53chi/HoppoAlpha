using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.VisualBasic.FileIO;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;

namespace HoppoAlpha.DataLibrary.DataObject
{
    public class ExMasterShipCollection : Dictionary<int, ExMasterShip>
    {
        AutoResetEvent _autoResetEvent;//Master側の読み込みがいつくるかわからないので
        AutoResetEvent _autoResetEvent2;

        public static string FolderPath { get; private set; }
        public static string FilePath { get; private set; }

        static ExMasterShipCollection()
        {
            FolderPath = Environment.CurrentDirectory + @"\config";
            FilePath = Environment.CurrentDirectory + @"\config\ships.csv";
        }

        /// <summary>
        /// デフォルトで値をセットしないコンストラクタ
        /// </summary>
        /// <param name="notInitialSet">値は無視されます</param>
        public ExMasterShipCollection(bool notInitialSet)
            : base()
        { }

        public ExMasterShipCollection()
            : this(null)
        { }

        public ExMasterShipCollection(Stream stream)
            : base()
        {
            _autoResetEvent = new AutoResetEvent(false);
            _autoResetEvent2 = new AutoResetEvent(false);

            List<List<string>> data = new List<List<string>>();
            //リソースからデフォルトデータの取得
            Action getDefaultData = () =>
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();

                    using (var parser = new TextFieldParser(assembly.GetManifestResourceStream("HoppoAlpha.DataLibrary.ships160718.csv"), Encoding.GetEncoding("shift-jis")))
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
            if (stream != null && stream is FileStream && File.Exists((stream as FileStream).Name))
            {
                using(var parser = new TextFieldParser((stream as FileStream).Name, Encoding.GetEncoding("shift-jis")))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while(!parser.EndOfData)
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
            if(data.Count < 2) throw new InvalidDataException();

            //バージョンチェック
            int version = 0;
            int startRow = 1;
            if (data[0].Count > 1 && data[0][0] == "Version")
            {
                int.TryParse(data[0][1], out version);
            }
            //バージョンが低かったら上書き
            if(Version.ExMasterShipDataVersion > version)
            {
                data = new List<List<string>>();
                getDefaultData();
                version = Version.ExMasterShipDataVersion;
            }
            if (version > 0) startRow++;
            //データの取得
            List<string> header = data[startRow - 1];
            for(int i=startRow; i<data.Count; i++)
            {
                var ship = new ExMasterShip();

                for(int j=0; j<data[i].Count; j++)
                {
                    string h = header[j];
                    string v = data[i][j];

                    ship.SetValueFromCsv(h, v);
                }

                this[ship.api_id] = ship;
            }

            _autoResetEvent.Set();//CSVデータの読み込みが終わってからMasterを読み込ませる
        }

        /// <summary>
        /// マスターデータとのマージをします
        /// </summary>
        /// <param name="ships">マスターデータ</param>
        public void MergeMasterData(List<ApiMstShip> ships)
        {
            _autoResetEvent.WaitOne();

            foreach(var x in ships)
            {
                //該当のデータが既にCSVからのインポートで場合
                ExMasterShip data;
                if(this.TryGetValue(x.api_id, out data))
                {
                    data.MergeValueFromMasterData(x);
                }
                //ない場合
                else
                {
                    this[x.api_id] = new ExMasterShip(x);
                }
            }

            _autoResetEvent.Set();
            _autoResetEvent2.Set();
        }

        /// <summary>
        /// キャラクターのデフォルトの制空値と加重対空値を計算します
        /// </summary>
        /// <param name="slotdata">装備のマスターデータ</param>
        /// <param name="enforce">強制的に再計算するか</param>
        public void CalcMasterAirSuperiorityAndWeightedAntiAir(ExMasterSlotitemCollection slotdata, bool enforce)
        {
            _autoResetEvent2.WaitOne();

            
            foreach(var x in this.Values)
            {
                //強制モードor制空値0で再計算
                if(enforce || x.AirSuperiority == 0)
                {
                    x.CalcDefaultAirSuperiority(slotdata);
                }
                if(enforce || x.WeightedAntiAir == 0)
                {
                    x.CalcDefaultWeightedAntiAir(slotdata);
                }
            }

            _autoResetEvent2.Set();
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
            if(!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
            //CSVファイルの作成
            StringBuilder sb = new StringBuilder();
            //バージョン
            sb.AppendLine("Version," + Version.ExMasterShipDataVersion);
            //ヘッダー
            sb.AppendLine(string.Join(",", ExMasterShip.ExportCSVHeader()));
            //各行
            foreach(var x in this.Values)
            {
                sb.AppendLine(string.Join(",", x.ExportCSV()));
            }
            //保存
            try
            {
                //usingをするとFIles.TryLoad側でStreamが閉じられてしまうので
                var sw = new StreamWriter(stream, Encoding.GetEncoding("shift-jis"));
                sw.WriteLine(sb.ToString());
                sw.Flush();
                sw = null;
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
            return "success";
        }
    }
}
