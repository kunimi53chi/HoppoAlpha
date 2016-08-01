using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;

namespace VisualFormTest
{
    public partial class KanmusuList : Form
    {
        public KanmusuList()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://www.kancolle-calc.net/kanmusu_list.html");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //テーブルの初期化
            if (!KanmusuRemodelingTable.IsInited) KanmusuRemodelingTable.Init();
            //未改造艦を基準にしたディクショナリー
            Dictionary<int, List<string>> notremodel = new Dictionary<int, List<string>>();
            foreach(ApiShip s in APIPort.ShipsDictionary.Values)
            {
                //Lv1の艦を弾くか
                if(checkBox1.Checked)
                {
                    if (s.api_lv == 1) continue;
                }
                //マスターデータのノード
                var m = KanmusuRemodelingTable.Nodes[s.api_ship_id];
                //未改造データまでノードを遡る
                var m_base = m;
                while(true)
                {
                    if (m_base.Previous == null) break;
                    m_base = m_base.Previous;
                }
                //出力用のレベルを取得
                string result = m.CheckLevel(s.api_lv);
                //ディクショナリーに追加
                List<string> oldlist;
                int base_id = m_base.Data.api_id;
                if(notremodel.ContainsKey(base_id))
                {
                    oldlist = notremodel[base_id];
                }
                else
                {
                    oldlist = new List<string>();
                }
                oldlist.Add(result);
                notremodel[base_id] = oldlist;
            }
            //出力用のテキスト
            StringBuilder sb = new StringBuilder(".2");
            foreach(var dicitem in notremodel)
            {
                //先頭区切り
                sb.Append("|");
                //未改造艦のID
                sb.Append(dicitem.Key);
                sb.Append(":");
                //レベル
                sb.Append(string.Join(",", dicitem.Value));
            }
            //クリップボードへ
            Clipboard.SetDataObject(sb.ToString(), true);
            //メッセージボックス
            MessageBox.Show("出力結果をクリップボードにコピーしました");
        }
    }

    //改造データの参照
    public class KanmusuRemodelingTable
    {
        public static Dictionary<int, KanmusuRemodelingNode> Nodes { get; set; }
        public static bool IsInited { get; set; }

        private static List<int> UsedKey { get; set; }

        //内部クラス
        //ノード
        public class KanmusuRemodelingNode
        {
            public ApiMstShip Data { get; set; }
            public KanmusuRemodelingNode Previous { get; set; }
            public KanmusuRemodelingNode Next { get; set; }
            public int Depth { get; set; }

            public void Display()
            {
                string prev = this.Previous == null ? "null" : this.Previous.Data.api_id.ToString();
                string next = this.Next == null ? "null" : this.Next.Data.api_id.ToString();
                Console.WriteLine("{0}{1}(id = {4}, depth = {5}) Prev = {2} Next = {3}"
                    , new string('　', this.Depth), this.Data.api_name,
                    prev, next, this.Data.api_id, this.Depth);
            }

            //チェック
            public string CheckLevel(int level)
            {
                //Nextがnull→そのまま
                //改造Level未満→そのまま
                //改造Level以上→明示
                if (this.Next == null) return level.ToString();
                if (level < this.Data.api_afterlv) return level.ToString();
                else return level.ToString() + "." + (this.Depth + 1).ToString();
            }
        }


        //初期化
        public static void Init()
        {
            Nodes = new Dictionary<int, KanmusuRemodelingNode>();
            UsedKey = new List<int>();
            foreach (ApiMstShip ship in APIMaster.MstShips.Values)
            {
                MakeTree(ship, 0);
            }
            IsInited = true;
        }

        //ツリーを作る
        public static void MakeTree(ApiMstShip ship, int depth)
        {
            //オブジェクト
            KanmusuRemodelingNode node;
            int id = ship.api_id;
            if (UsedKey.Contains(id))
            {
                node = Nodes[id];
            }
            else
            {
                node = new KanmusuRemodelingNode();
                UsedKey.Add(id);
            }
            /*
             *      	before 		after	 　　depth	data
            進化n	　 そのまま 	new n+1		　n	    d_n
            進化n+1　  this		    null		　n+1	null

             */
            //進化後がない場合
            if (ship.api_aftershipid == "0" || ship.api_aftershipid == null || ship.api_afterlv <= 0)
            {
                node.Data = ship;
                Nodes[id] = node;
                return;
            }
            //進化後がある場合
            else
            {
                if (node.Data != null) return;
                //次のデータ
                int nextid = Convert.ToInt32(ship.api_aftershipid);
                ApiMstShip nextship = APIMaster.MstShips[nextid];
                //次のノード
                KanmusuRemodelingNode nextnode = new KanmusuRemodelingNode();
                nextnode.Previous = node;
                nextnode.Depth = depth + 1;
                //自分自身
                node.Next = nextnode;
                //node.Depth = depth;
                node.Data = ship;
                //追加
                Nodes[id] = node;

                //循環改装対策
                List<int> prevs = new List<int>();
                var prevnode = node.Previous;
                while(prevnode != null)
                {
                    prevs.Add(prevnode.Data.api_id);
                    prevnode = prevnode.Previous;
                }
                bool isRecursive = prevs.IndexOf(nextid) != -1;//ループしているかどうか

                if(!isRecursive)
                {
                    Nodes[nextid] = nextnode;
                    //キー
                    UsedKey.Add(nextid);
                    //再帰呼び出し
                    MakeTree(nextship, depth + 1);
                }
                else
                {
                    return;
                }

            }
        }
    }
}
