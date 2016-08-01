using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;

namespace HoppoAlpha.DataLibrary.KancolleCalcConvert
{
    public class ToDeckBuilder
    {
        public InternalList<DeckItem> Decks { get; set; }
        public const int Version = 4;

        public ToDeckBuilder()
        {
            this.Decks = new InternalList<DeckItem>();
        }

        /// <summary>
        /// デッキビルダー用のJSONに変換します
        /// </summary>
        /// <returns>JSON文字列</returns>
        public override string ToString()
        {
            InternalList<string> str = new InternalList<string>();
            str.Add(string.Format("\"version\":{0}", Version));

            foreach(int i in Enumerable.Range(0, this.Decks.Count))
            {
                var f = this.Decks[i];
                str.Add(string.Format("\"f{0}\":{1}", (i + 1).ToString(), f.ToString()));
            }

            return str.ToStr();
        }

        /// <summary>
        /// デッキビルダーに変換するための艦隊のオブジェクト（内部リストをラップ）
        /// </summary>
        public class DeckItem : InternalList<ShipItem>
        {
            /// <summary>
            /// 船のリストをJSON文字列に変換します
            /// </summary>
            /// <returns>JSON文字列</returns>
            public override string ToString()
            {
                InternalList<string> ships = new InternalList<string>();
                foreach(int i in Enumerable.Range(0, this.Count))
                {
                    var s = this[i];
                    ships.Add(string.Format("\"s{0}\":{1}", (i + 1).ToString(), s.ToString()));
                }

                return ships.ToStr();
            }
        }

        /// <summary>
        /// デッキビルダーに変換するためのキャラのオブジェクト
        /// </summary>
        public class ShipItem
        {
            /// <summary>
            /// キャラのマスターIDを指定します
            /// </summary>
            public int ShipID { get; set; }
            /// <summary>
            /// キャラのレベルを表します
            /// </summary>
            public int Level { get; set; }
            /// <summary>
            /// キャラの運を表します
            /// </summary>
            public int Luck { get; set; }
            /// <summary>
            /// キャラの装備のリストを表します
            /// </summary>
            public InternalList<SlotItem> Items { get; set; }
            /// <summary>
            /// キャラの拡張スロットを表します
            /// </summary>
            public SlotItem ExItem { get; set; }

            public ShipItem()
            {
                this.Luck = -1;
                this.Items = new InternalList<SlotItem>();
            }

            /// 例：{"id":"136","lv":0,"luck":-1,"items":{"i1":{"id":128,"rf":4},"i2":{"id":128,"rf":3},"i3":{"id":59,"rf":2},"i4":{"id":116,"rf":1}}}

            /// <summary>
            /// 船をJSON文字列に変換
            /// 例：{version: 4, f1: {s1: {id: '100', lv: 40, luck: -1, items:{i1:{id:1, rf: 4, mas:7},{i2:{id:3, rf: 0}}...,ix:{id:43}}}, s2:{}...},...}
            /// </summary>
            /// <returns>船のJSON文字列</returns>
            public override string ToString()
            {
                InternalList<string> buf = new InternalList<string>();
                buf.Add(string.Format("\"id\":\"{0}\"", this.ShipID));//IDだけ""でかこむ
                buf.Add(string.Format("\"lv\":{0}", this.Level));
                buf.Add(string.Format("\"luck\":{0}", this.Luck));

                //アイテムの文字列を作成
                InternalList<string> itemstr = new InternalList<string>();
                foreach(int i in Enumerable.Range(0, this.Items.Count))
                {
                    var item = this.Items[i];
                    itemstr.Add(string.Format("\"i{0}\":{1}", (i + 1).ToString(), item.ToString()));
                }

                //拡張スロット
                if (ExItem != null) itemstr.Add(string.Format("\"ix\":{0}", ExItem.ToString()));

                buf.Add(string.Format("\"items\":{0}", itemstr.ToStr()));

                return buf.ToStr();
            }
        }

        /// <summary>
        /// 装備1個あたりのオブジェクト
        /// </summary>
        public class SlotItem
        {
            /// <summary>
            /// 装備のマスターIDを表します
            /// </summary>
            public int Id { get; set; }
            /// <summary>
            /// 改修レベルを表します
            /// </summary>
            public int ReinforcedLevel { get; set; }
            /// <summary>
            /// 熟練度レベルを表します
            /// </summary>
            public int MasterLevel { get; set; }

            /// <summary>
            /// 装備1個をJSONに変換（例：{id:1, rf: 4, mas:7}）
            /// </summary>
            /// <returns>JSON文字列</returns>
            public override string ToString()
            {
                InternalList<string> buf = new InternalList<string>();
                buf.Add(string.Format("\"id\":{0}", this.Id));
                if(ReinforcedLevel > 0) buf.Add(string.Format("\"rf\":{0}", this.ReinforcedLevel));
                if (MasterLevel > 0) buf.Add(string.Format("\"mas\":{0}", this.MasterLevel));
                return buf.ToStr();
            }

        }

        // Internalなのにpublicなのなんでー(´・ω・｀)
        /// <summary>
        /// JSONライクなstringを作成するためのリスト内部クラス
        /// </summary>
        /// <typeparam name="T">TはListに対応</typeparam>
        public class InternalList<T> : List<T>
        {
            /// <summary>
            /// JSONライクな配列文字列を作成します
            /// </summary>
            /// <returns>string : [Value1,Value2,…]</returns>
            public string ToStr()
            {
                return "{" + string.Join(",", this) + "}";
            }
        }
    }
}
