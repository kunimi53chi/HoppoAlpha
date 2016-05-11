using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// ユニットクエリのデータ
    /// </summary>
    [ProtoContract]
    public class UnitQuery
    {
        /// <summary>
        /// IDを表します
        /// </summary>
        [ProtoMember(1)]
        public int ID { get; set; }
        /// <summary>
        /// クエリの名前を表します
        /// </summary>
        [ProtoMember(2)]
        public string Name { get; set; }
        /// <summary>
        /// TRUEならばQuery以下が全OR、FALSEならば全ANDで結合されます
        /// </summary>
        [ProtoMember(3)]
        public bool IsOr { get; set; }
        /// <summary>
        /// 子クエリアイテムを表します
        /// </summary>
        [ProtoMember(4)]
        public List<UnitQueryItem> Query { get; set; }

        public UnitQuery()
        {
            Query = new List<UnitQueryItem>();
        }

        /// <summary>
        /// クエリが空かどうかを判定します
        /// </summary>
        public bool IsEmpty()
        {
            if (this.IsOr) return false;
            if (!string.IsNullOrEmpty(this.Name) && this.Name != "なし") return false;
            if (this.Query.Count > 0) return false;
            return true;
        }
    }

    #region クエリの1項目
    /// <summary>
    /// クエリの1項目
    /// </summary>
    [ProtoContract]
    public class UnitQueryItem
    {
        /// <summary>
        /// 判定する項目を表します
        /// </summary>
        [ProtoMember(1)]
        public string Target { get; set; }
        /// <summary>
        /// 値をAND検索するかどうかを表します
        /// </summary>
        [ProtoMember(2)]
        public bool IsAnd { get; set; }
        /// <summary>
        /// 検索結果を反転するかどうかを表します
        /// </summary>
        [ProtoMember(3)]
        public bool IsNot { get; set; }
        /// <summary>
        /// 入れ子となる検索条件を表します
        /// </summary>
        [ProtoMember(4)]
        public List<UnitQueryItemSearchBase> Searches { get; set; }

        //インスタンス
        //private object instance = null;
        //Targetの内部列挙体
        private UnitQueryMode tenum;
        /// <summary>
        /// 判定する項目の列挙体を表します
        /// </summary>
        public UnitQueryMode TargetEnum
        {
            get { return tenum; }
        }

        //コンストラクタ
        public UnitQueryItem()
        { }
        /// <summary>
        /// インスタンスをStringから初期化します
        /// </summary>
        /// <param name="target">判定する項目</param>
        public UnitQueryItem(string target)
        {
            Searches = new List<UnitQueryItemSearchBase>();
            Target = target;
            tenum = (UnitQueryMode)Enum.Parse(typeof(UnitQueryMode), target);
        }
        /// <summary>
        /// インスタンスを判定する項目の列挙体から初期化します（おすすめ）
        /// </summary>
        /// <param name="mode">判定する項目の列挙体</param>
        public UnitQueryItem(UnitQueryMode mode)
        {
            Searches = new List<UnitQueryItemSearchBase>();
            tenum = mode;
            Target = mode.ToString();
        }

        //逆シリアル化の際
        [ProtoAfterDeserialization]
        protected void OnDeserialized()
        {
            if (Target != null) tenum = (UnitQueryMode)Enum.Parse(typeof(UnitQueryMode), Target);
        }

        /// <summary>
        /// 子の検索条件を追加します
        /// </summary>
        /// <param name="value">検索値</param>
        /// <param name="matchenum">検索条件の列挙体</param>
        public void SearchesAdd(string value, UnitQueryMatchMode matchenum)
        {
            int match = (int)matchenum;
            UnitQueryItemSearchBase b = new UnitQueryItemSearchBase();
            b.Value = value;
            b.Match = match;
            Searches.Add(b);
        }
        /// <summary>
        /// 子の検索条件を追加します
        /// </summary>
        /// <param name="value">検索値</param>
        /// <param name="matchenum">検索条件の列挙体</param>
        public void SearchesAdd(int value, UnitQueryRangeMode rangeenum)
        {
            int range = (int)rangeenum;
            UnitQueryItemSearchBase b = new UnitQueryItemSearchBase();
            b.Value = value;
            b.Range = range;
            Searches.Add(b);
        }
        /// <summary>
        /// 子の検索条件を追加します
        /// </summary>
        /// <param name="value">検索値</param>
        /// <param name="matchenum">検索条件の列挙体</param>
        public void SearchesAdd(double value, UnitQueryRangeMode rangeenum)
        {
            int range = (int)rangeenum;
            UnitQueryItemSearchBase b = new UnitQueryItemSearchBase();
            b.Value = value;
            b.Range = range;
            Searches.Add(b);
        }

        private void AddHPCondition_base()
        {
            if (tenum != UnitQueryMode.HPRatio) throw new FormatException();
        }
        /// <summary>
        /// 無傷または小破の条件を追加します
        /// </summary>
        public void AddHPCondition_NoneOrSmall()
        {
            AddHPCondition_base();
            SearchesAdd(0.5, UnitQueryRangeMode.MoreThan);
        }
        /// <summary>
        /// 中破以上の条件を追加します
        /// </summary>
        public void AddHPCondition_MiddleOver()
        {
            AddHPCondition_base();
            SearchesAdd(0.5, UnitQueryRangeMode.LessThanEquals);
        }
        /// <summary>
        /// 小破の条件を追加します
        /// </summary>
        public void AddHPCondition_Small()
        {
            AddHPCondition_base();
            IsAnd = true;
            SearchesAdd(0.5, UnitQueryRangeMode.MoreThan);
            SearchesAdd(0.75, UnitQueryRangeMode.LessThanEquals);
        }

    }
    #endregion

    //クエリの1条件
    #region クエリの1条件
    /// <summary>
    /// クエリの1条件
    /// </summary>
    [ProtoContract]
    public class UnitQueryItemSearchBase
    {
        /// <summary>
        /// 検索条件を表します
        /// Object型をシリアル化できないのでシリアル化の際は、ValueInt, ValueDouble, ValueStringを利用してください
        /// int or double or string
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 一致条件を表します。Valueがstringのときのみ参照されます
        /// </summary>
        [ProtoMember(1)]
        public int Match { get; set; }
        /// <summary>
        /// 範囲条件を表します。Valueがint or doubleのときのみ参照されます
        /// </summary>
        [ProtoMember(2)]
        public int Range { get; set; }
        /// <summary>
        /// ValueがProtoBufで出力できないのでその代用プロパティ
        /// </summary>
        [ProtoMember(3)]
        internal int ValueInt { get; set; }
        /// <summary>
        /// ValueがProtoBufで出力できないのでその代用プロパティ
        /// </summary>
        [ProtoMember(4)]
        internal double ValueDouble { get; set; }
        /// <summary>
        /// ValueがProtoBufで出力できないのでその代用プロパティ
        /// </summary>
        [ProtoMember(5)]
        internal string ValueString { get; set; }

        /// <summary>
        /// 一致条件の上限を表す定数
        /// </summary>
        public static readonly int MaxMatch = 5;
        /// <summary>
        /// 範囲条件の上限を表す定数
        /// </summary>
        public static readonly int MaxRange = 7;


        //シリアル化の処理
        [ProtoBeforeSerialization]
        protected void OnSerializing()
        {
            if (Value is int) ValueInt = (int)Value;
            else if (Value is double) ValueDouble = (double)Value;
            else if (Value is string) ValueString = (string)Value;
        }

        //逆シリアル化の処理
        [ProtoAfterDeserialization]
        protected void OnDeserialized()
        {
            if (!string.IsNullOrEmpty(ValueString)) Value = ValueString;
            else if (ValueDouble != 0) Value = ValueDouble;
            else if (ValueString != null) Value = ValueString;
            else Value = ValueInt;
        }

        //チェック
        public bool Checker(ApiShip oship, object instatnce)
        {
            //instanceがstring型の場合
            if (instatnce is string)
            {
                switch (Match)
                {
                    //全てfalse
                    case 0: return false;
                    //完全一致
                    case 1: return (string)instatnce == Value.ToString();
                    //前方一致
                    case 2: return ((string)instatnce).StartsWith(Value.ToString());
                    //後方一致
                    case 3: return ((string)instatnce).EndsWith(Value.ToString());
                    //部分一致
                    case 4: return ((string)instatnce).Contains(Value.ToString());
                    //全てtrue
                    case 5: return true;
                    //その他
                    default: throw new ArgumentException();
                }
            }
            //instanceが数値型の場合
            else if (instatnce is int || instatnce is double)
            {
                double insval;
                if (instatnce is int) insval = (double)((int)instatnce);
                else insval = (double)instatnce;
                double valval;
                if (Value is int) valval = (double)((int)Value);
                else if (Value is double) valval = (double)Value;
                else throw new InvalidCastException();
                switch (Range)
                {
                    // 0 = なし　1=一致　2=＝大きい　4=小さい
                    //全てfalse
                    case 0: return false;
                    //＝
                    case 1: return insval == valval;
                    //＞
                    case 2: return insval > valval;
                    //≧
                    case 3: return insval >= valval;
                    //＜
                    case 4: return insval < valval;
                    //≦
                    case 5: return insval <= valval;
                    //≠
                    case 6: return insval != valval;
                    //全てtrue
                    case 7: return true;
                    //その他
                    default: throw new ArgumentException();
                }
            }
            throw new ArgumentException();
        }
    }
    #endregion

    #region フィルタリングオブジェクト
    /// <summary>
    /// クエリ用フィルタリングオブジェクト
    /// </summary>
    public class UnitQueryFilter
    {
        /// <summary>
        /// 小破を非表示します
        /// </summary>
        public bool NotShowSmallDamage { get; set; }
        /// <summary>
        /// 入渠一定時間以上を無視します
        /// </summary>
        public bool NotShowOverThresholdHour { get; set; }
        /// <summary>
        /// NotShowOverThresholdHourで参照する値を表します
        /// </summary>
        public double ThresholdHour { get; set; }

        /// <summary>
        /// 所属艦を表示しないオプションを表します
        /// </summary>
        public bool NotShowFleetAssignFlag { get; set; }
        /// <summary>
        /// ☆を表示しないオプションを表します
        /// </summary>
        public bool NotShowStar { get; set; }
    }
    #endregion

    #region 列挙体
    //クエリのモード
    /// <summary>
    /// クエリモードの列挙体
    /// </summary>
    public enum UnitQueryMode
    {
        ID, Name, Lv, ShipType, ShipTypeName,
        Cond, NdockTime, HP, MaxHP, HPRatio,
        IsLatestRemodeling, EXP, Fire, Torpedo, NightFire,
        AirSup, CarrierFire, AntiSubmarine, AntiAir, Defense,
        Evasion, AircraftNum, Range, Search, Luck,
        IsLocked, FuelNow, FuelMax, FuelRatio, BullNow,
        BullMax, BullRatio, IsFleetAssign, IsBath, SallyArea,
    }

    /// <summary>
    /// クエリの文字列一致条件
    /// </summary>
    public enum UnitQueryMatchMode
    {
        /// <summary>
        /// 常にNO
        /// </summary>
        None, 
        /// <summary>
        /// 完全一致
        /// </summary>
        ExactlyMatch, 
        /// <summary>
        /// 前方一致
        /// </summary>
        StartsWith, 
        /// <summary>
        /// 後方一致
        /// </summary>
        EndsWith, 
        /// <summary>
        /// 部分一致
        /// </summary>
        Contains, 
        /// <summary>
        /// 常にYES
        /// </summary>
        Free,
    }

    /// <summary>
    /// クエリの数値一致条件
    /// </summary>
    public enum UnitQueryRangeMode
    {
        /// <summary>
        /// 常にNO
        /// </summary>
        None,
        /// <summary>
        /// 等しい(＝)
        /// </summary>
        Equals, 
        /// <summary>
        /// より大きい(＞)
        /// </summary>
        MoreThan, 
        /// <summary>
        /// 以上(≧)
        /// </summary>
        MoreThanEquals, 
        /// <summary>
        /// より小さい(＜)
        /// </summary>
        LessThan, 
        /// <summary>
        /// 以下(≦)
        /// </summary>
        LessThanEquals, 
        /// <summary>
        /// 等しくない(≠)
        /// </summary>
        NotEquals, 
        /// <summary>
        /// 常にYES
        /// </summary>
        Free,
    }

    /// <summary>
    /// クエリモード列挙体の拡張メソッド
    /// </summary>
    public static class UnitQueryEnumExt
    {
        /// <summary>
        /// 列挙体を文字列に変換します
        /// </summary>
        /// <param name="u">クエリモードの列挙体</param>
        /// <returns>文字列</returns>
        public static string ToStr(this UnitQueryMode u)
        {
            switch (u)
            {
                case UnitQueryMode.ID: return "ID";
                case UnitQueryMode.Name: return "艦名";
                case UnitQueryMode.Lv: return "レベル";
                case UnitQueryMode.ShipType: return "艦種ID(数字)";
                case UnitQueryMode.ShipTypeName: return "艦種名";//5
                case UnitQueryMode.Cond: return "Cond値";
                case UnitQueryMode.NdockTime: return "入渠時間(ms)";
                case UnitQueryMode.HP: return "HP";
                case UnitQueryMode.MaxHP: return "最大HP";
                case UnitQueryMode.HPRatio: return "HP割合";//10
                case UnitQueryMode.IsLatestRemodeling: return "改造済みフラグ";
                case UnitQueryMode.EXP: return "累積経験値";
                case UnitQueryMode.Fire: return "火力";
                case UnitQueryMode.Torpedo: return "雷装";
                case UnitQueryMode.NightFire: return "夜戦火力";//15
                case UnitQueryMode.AirSup: return "制空値";
                case UnitQueryMode.CarrierFire: return "空母実火力";
                case UnitQueryMode.AntiSubmarine: return "対潜";
                case UnitQueryMode.AntiAir: return "対空";
                case UnitQueryMode.Defense: return "装甲";//20
                case UnitQueryMode.Evasion: return "回避";
                case UnitQueryMode.AircraftNum: return "搭載";
                case UnitQueryMode.Range: return "射程(数字)";
                case UnitQueryMode.Search: return "索敵";
                case UnitQueryMode.Luck: return "運";//25
                case UnitQueryMode.IsLocked: return "ロック済フラグ";
                case UnitQueryMode.FuelNow: return "燃料残量";
                case UnitQueryMode.FuelMax: return "燃料最大";
                case UnitQueryMode.FuelRatio: return "燃料残量比率";
                case UnitQueryMode.BullNow: return "弾薬残量";//30
                case UnitQueryMode.BullMax: return "弾薬最大";
                case UnitQueryMode.BullRatio: return "弾薬残量比率";
                case UnitQueryMode.IsFleetAssign: return "艦隊配属フラグ";
                case UnitQueryMode.IsBath: return "入渠フラグ";
                case UnitQueryMode.SallyArea: return "出撃制限（お札）";
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// クエリモード列挙体に対応する値の型を取得します
        /// </summary>
        /// <param name="u">クエリモード列挙体</param>
        /// <returns>クエリモードに対応する値の型</returns>
        public static Type GetInstanceType(this UnitQueryMode u)
        {
            switch (u)
            {
                case UnitQueryMode.ID: return typeof(int);
                case UnitQueryMode.Name: return typeof(string);
                case UnitQueryMode.Lv: return typeof(int);
                case UnitQueryMode.ShipType: return typeof(int);
                case UnitQueryMode.ShipTypeName: return typeof(string);//5
                case UnitQueryMode.Cond: return typeof(int);
                case UnitQueryMode.NdockTime: return typeof(int);
                case UnitQueryMode.HP: return typeof(int);
                case UnitQueryMode.MaxHP: return typeof(int);
                case UnitQueryMode.HPRatio: return typeof(double);//10
                case UnitQueryMode.IsLatestRemodeling: return typeof(int);
                case UnitQueryMode.EXP: return typeof(int);
                case UnitQueryMode.Fire: return typeof(int);
                case UnitQueryMode.Torpedo: return typeof(int);
                case UnitQueryMode.NightFire: return typeof(int);//15
                case UnitQueryMode.AirSup: return typeof(int);
                case UnitQueryMode.CarrierFire: return typeof(int);
                case UnitQueryMode.AntiSubmarine: return typeof(int);
                case UnitQueryMode.AntiAir: return typeof(int);
                case UnitQueryMode.Defense: return typeof(int);//20
                case UnitQueryMode.Evasion: return typeof(int);
                case UnitQueryMode.AircraftNum: return typeof(int);
                case UnitQueryMode.Range: return typeof(int);
                case UnitQueryMode.Search: return typeof(int);
                case UnitQueryMode.Luck: return typeof(int);//25
                case UnitQueryMode.IsLocked: return typeof(int);
                case UnitQueryMode.FuelNow: return typeof(int);
                case UnitQueryMode.FuelMax: return typeof(int);
                case UnitQueryMode.FuelRatio: return typeof(double);
                case UnitQueryMode.BullNow: return typeof(int);//30
                case UnitQueryMode.BullMax: return typeof(int);
                case UnitQueryMode.BullRatio: return typeof(double);
                case UnitQueryMode.IsFleetAssign: return typeof(int);
                case UnitQueryMode.IsBath: return typeof(int);
                case UnitQueryMode.SallyArea: return typeof(int);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// クエリの文字列判定の一致条件を文字列に変換します
        /// </summary>
        /// <param name="u">文字列判定条件の列挙体</param>
        /// <returns>文字列</returns>
        public static string ToStr(this UnitQueryMatchMode u)
        {
            switch (u)
            {
                case UnitQueryMatchMode.None: return "常にNO";
                case UnitQueryMatchMode.ExactlyMatch: return "完全一致";
                case UnitQueryMatchMode.StartsWith: return "前方一致";
                case UnitQueryMatchMode.EndsWith: return "後方一致";
                case UnitQueryMatchMode.Contains: return "部分一致";
                case UnitQueryMatchMode.Free: return "常にYES";
                default: throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// クエリの数値判定の一致条件を文字列に変換します
        /// </summary>
        /// <param name="u">数値判定条件の列挙体</param>
        /// <returns>文字列</returns>
        public static string ToStr(this UnitQueryRangeMode u)
        {
            switch (u)
            {
                case UnitQueryRangeMode.None: return "常にNO";
                case UnitQueryRangeMode.Equals: return "等しい(＝)";
                case UnitQueryRangeMode.MoreThan: return "より大きい(＞)";
                case UnitQueryRangeMode.MoreThanEquals: return "以上(≧)";
                case UnitQueryRangeMode.LessThan: return "より小さい(＜)";
                case UnitQueryRangeMode.LessThanEquals: return "以下(≦)";
                case UnitQueryRangeMode.NotEquals: return "等しくない(≠)";
                case UnitQueryRangeMode.Free: return "常にYES";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
    #endregion
}
