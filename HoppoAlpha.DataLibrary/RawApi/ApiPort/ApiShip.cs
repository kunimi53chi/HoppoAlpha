using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using HoppoAlpha.DataLibrary.DataObject;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;

namespace HoppoAlpha.DataLibrary.RawApi.ApiPort
{
    [Serializable]
    public class ApiShip
    {
        int _api_fuel = 0, _api_bull = 0, _api_slot_ex = 0, _api_nowhp = 0, _api_maxhp = 0;
        List<int> _api_slot = null, _api_onslot = null, _api_taiku = null;

        private static object _lockObject = new object();//Dslotitem計算中に装備が変更されないように排他処理

        public int api_id { get; set; }
        public int api_sortno { get; set; }
        public int api_ship_id { get; set; }
        public int api_lv { get; set; }
        public List<int> api_exp { get; set; }
        public int api_nowhp
        {
            get { return this._api_nowhp; }
            set
            {
                this._api_nowhp = value;
                _hpCondition = null; _warnState = null;
            }
        }
        public int api_maxhp
        {
            get { return this._api_maxhp; }
            set
            {
                this._api_maxhp = value;
                _hpCondition = null; _warnState = null;
            }
        }
        public int api_leng { get; set; }
        public List<int> api_slot
        {
            get { return this._api_slot; }
            set 
            {
                lock (_lockObject)
                {
                    this._api_slot = value;
                    _slotItems = null; _exMasterSlotitems = null; _apiMstSlotitemEquiptypes = null; _airsup = null; _weightedAntiAir = null;
                    _numDrum = null; _numRadar = null; 
                    _numDaihatsu = null;
                }
            }
        }
        public List<int> api_onslot
        {
            get { return this._api_onslot; }
            set 
            {
                lock (_lockObject)
                {
                    this._api_onslot = value;
                    _airsup = null;
                }
            }
        }
        public int api_slot_ex 
        {
            get { return this._api_slot_ex; }
            set
            {
                lock(_lockObject)
                {
                    this._api_slot_ex = value;
                    _exslot_slotItem = null; _exslot_exMasterSlotitem = null;
                }
            }
        }
        public List<int> api_kyouka { get; set; }
        public int api_backs { get; set; }
        public int api_fuel
        {
            get { return this._api_fuel; }
            set { this._api_fuel = value; this._isFuelMax = null; }
        }
        public int api_bull
        {
            get { return this._api_bull; }
            set { this._api_bull = value; this._isBullMax = null;}
        }
        public int api_slotnum { get; set; }
        public int api_ndock_time { get; set; }
        public List<int> api_ndock_item { get; set; }
        public int api_srate { get; set; }
        public int api_cond { get; set; }
        public List<int> api_karyoku { get; set; }
        public List<int> api_raisou { get; set; }
        public List<int> api_taiku 
        {
            get { return this._api_taiku; }
            set { this._api_taiku = value; this._weightedAntiAir = null; }
        }
        public List<int> api_soukou { get; set; }
        public List<int> api_kaihi { get; set; }
        public List<int> api_taisen { get; set; }
        public List<int> api_sakuteki { get; set; }
        public List<int> api_lucky { get; set; }
        public int api_locked { get; set; }
        public int api_locked_equip { get; set; }
        public int api_sally_area { get; set; }

        //拡張スロット込みの全てのスロット
        public List<int> AllSlots
        {
            get
            {
                if (api_slot == null) return new List<int>();
                List<int> all = new List<int>();
                all.AddRange(this.api_slot);
                all.Add(this.api_slot_ex);
                return all;
            }
        }

        //マスターデータ
        /// <summary>
        /// 船のマスターデータ
        /// </summary>
        public static ExMasterShipCollection MasterShips { get; set; }
        /// <summary>
        /// 船の種類のマスターデータ
        /// </summary>
        public static Dictionary<int, ApiMstStype> MasterSTypes { get; set; }
        /// <summary>
        /// 装備のマスターデータ
        /// </summary>
        public static ExMasterSlotitemCollection MasterSlotitems { get; set; }
        /// <summary>
        /// 装備の種類のマスターデータ
        /// </summary>
        public static List<ApiMstSlotitemEquiptype> MasterSlotitemEquipTypes { get; set; }

        //マスターデータを必要とするプロパティ
        #region マスターデータから取得するプロパティ
        //マスターデータのオブジェクト
        private ExMasterShip _exMasterShip = null;
        private string _shipName = null;//艦名
        private bool? _isFuelMax = null;//補給
        private bool? _isBullMax = null;
        private int? _weightedAntiAir = null;//加重対空値

        private ApiMstStype _apiMstStype = null;

        private List<SlotItem> _slotItems = null;
        private List<ExMasterSlotitem> _exMasterSlotitems = null;
        private List<ApiMstSlotitemEquiptype> _apiMstSlotitemEquiptypes = null;
        private SlotItem _exslot_slotItem = null;
        private ExMasterSlotitem _exslot_exMasterSlotitem = null;
        private AirSupResult _airsup = null;
        private bool[,] _lastWithdrawnArray = null;

        private HPCondition? _hpCondition = null;
        private WarnState? _warnState = null;

        private int? _numDrum = null, _numRadar = null;//ドラム数、レーダー数
        private DaihatsuResult _numDaihatsu = null;

        /// <summary>
        /// HPが変化していない場合（入渠など）で状態を更新させる場合に読み込むメソッド
        /// </summary>
        public void RaiseConditionChanged()
        {
            _hpCondition = null; _warnState = null;
        }


        #region ExMasterShip関連
        /// <summary>
        /// 該当の船のマスターデータを取得
        /// </summary>
        public ExMasterShip DShip
        {
            get
            {
                //キャッシュされていない場合
                if(_exMasterShip == null)
                {
                    //マスターデータが登録されていない場合
                    if (MasterShips == null) throw new NullReferenceException("MasterShipsがNullです");
                    //マスターデータのキャッシュ
                    MasterShips.TryGetValue(this.api_ship_id, out _exMasterShip);
                }
                return _exMasterShip;
            }
        }

        /// <summary>
        /// 艦名を取得
        /// </summary>
        public string ShipName
        {
            get
            {
                //キャッシュされていない場合
                if(_shipName == null)
                {
                    _shipName = DShip.api_name;
                }
                return _shipName;
            }
        }

        /// <summary>
        /// 燃料満載かどうか
        /// </summary>
        public bool IsFuelMax
        {
            get
            {
                if(_isFuelMax == null)
                {
                    _isFuelMax = DShip.api_fuel_max == this.api_fuel;
                }
                return (bool)_isFuelMax;
            }
        }

        /// <summary>
        /// 弾薬満載かどうか
        /// </summary>
        public bool IsBullMax
        {
            get
            {
                if(_isBullMax == null)
                {
                    _isBullMax = DShip.api_bull_max == this.api_bull;
                }
                return (bool)_isBullMax;
            }
        }
        #endregion

        #region ApiMstStype関連
        /// <summary>
        /// ShipTypeのマスターデータを参照します
        /// </summary>
        public ApiMstStype DStype
        {
            get
            {
                //キャッシュされていない場合
                if(_apiMstStype == null)
                {
                    //マスターデータが登録されていない場合
                    if (MasterSTypes == null) throw new NullReferenceException("MasterStypeがNullです");
                    //マスターデータのキャッシュ
                    MasterSTypes.TryGetValue(DShip.api_stype, out _apiMstStype);
                }
                return _apiMstStype;
            }
        }

        /// <summary>
        /// 当該艦の艦種名を表します
        /// </summary>
        public string ShipTypeName
        {
            get
            {
                return DStype.api_name;
            }
        }
        #endregion

        #region Slotitem関連（これだけメソッドで）
        /// <summary>
        /// 装備中のアイテムのオブジェクトを取得します
        /// </summary>
        /// <param name="itemdata">アイテムのデータ</param>
        /// <returns>装備中のアイテムのオブジェクトのリスト</returns>
        public List<SlotItem> GetOSlotitems(Dictionary<int, SlotItem> itemdata)
        {
            lock (_lockObject)
            {
                //キャッシュされていない場合
                if (_slotItems == null && this.api_slot != null)
                {
                    _slotItems = new List<SlotItem>();
                    foreach (var i in this.api_slot)
                    {
                        SlotItem item;
                        if (itemdata.TryGetValue(i, out item))
                        {
                            _slotItems.Add(item);
                        }
                    }
                }
                return _slotItems;
            }
        }

        /// <summary>
        /// 装備中のアイテムのマスターデータを取得します
        /// </summary>
        /// <param name="itemdata">アイテムのデータ</param>
        /// <returns>装備中のアイテムのマスターデータのリスト</returns>
        public List<ExMasterSlotitem> GetDSlotitems(Dictionary<int, SlotItem> itemdata)
        {
            lock (_lockObject)
            {
                //キャッシュされていない場合
                if (_exMasterSlotitems == null)
                {
                    //マスターデータが登録されていない場合
                    if (MasterSlotitems == null) throw new NullReferenceException("MasterSlotitemsがNullです");
                    //OSlotitemsの取得
                    var oslots = GetOSlotitems(itemdata);
                    //DSlotitemsに変換
                    _exMasterSlotitems = new List<ExMasterSlotitem>();
                    foreach (var x in oslots)
                    {
                        ExMasterSlotitem ditem;
                        if (MasterSlotitems.TryGetValue(x.api_slotitem_id, out ditem))
                        {
                            _exMasterSlotitems.Add(ditem);
                        }
                    }
                }
                return _exMasterSlotitems;
            }
        }

        /// <summary>
        /// 装備中のアイテムの種類のマスターデータを取得します
        /// </summary>
        /// <param name="itemdata">アイテムのデータ</param>
        /// <returns>装備中のアイテムの種類のマスターデータのリスト</returns>
        public List<ApiMstSlotitemEquiptype> GetDSlotitemEquiptypes(Dictionary<int, SlotItem> itemdata)
        {
            lock (_lockObject)
            {
                //キャッシュされていない場合
                if (_apiMstSlotitemEquiptypes == null)
                {
                    //マスターデータが登録されていない場合
                    if (MasterSlotitemEquipTypes == null) throw new NullReferenceException("MasterSlotitemEquiptypesがNULLです");
                    //DSlotitemsの取得
                    var dslots = GetDSlotitems(itemdata);
                    //DSlotitemEquiptypesに変換
                    _apiMstSlotitemEquiptypes = new List<ApiMstSlotitemEquiptype>();
                    foreach (var x in dslots)
                    {
                        foreach (var target in MasterSlotitemEquipTypes)
                        {
                            if (x.EquipType == target.api_id)
                            {
                                _apiMstSlotitemEquiptypes.Add(target);
                                break;
                            }
                        }
                    }
                }
                return _apiMstSlotitemEquiptypes;
            }
        }

        /// <summary>
        /// 拡張スロットのアイテムオブジェクトを取得します
        /// </summary>
        /// <param name="itemdata">アイテムのデータ</param>
        /// <returns>装備中の拡張スロットのデータ</returns>
        public SlotItem GetExslotOSlotitem(Dictionary<int, SlotItem> itemdata)
        {
            lock(_lockObject)
            {
                //キャッシュされていない場合
                if(_exslot_slotItem == null && this.api_slot_ex != -1)
                {
                    itemdata.TryGetValue(this.api_slot_ex, out _exslot_slotItem);
                }
                return _exslot_slotItem;
            }
        }

        /// <summary>
        /// 拡張スロットのアイテムのマスターデータを取得します
        /// </summary>
        /// <param name="itemdata">アイテムのデータ</param>
        /// <returns>装備中の拡張スロットのマスターデータ</returns>
        public ExMasterSlotitem GetExslotDSlotitem(Dictionary<int, SlotItem> itemdata)
        {
            lock(_lockObject)
            {
                //キャッシュされていない場合
                if(_exslot_exMasterSlotitem == null)
                {
                    //マスターデータが登録されていない場合
                    if (MasterSlotitems == null) throw new NullReferenceException("MasterSlotitemsがNullです");
                    //拡張スロットのOslotitemを取得
                    var oslotex = GetExslotOSlotitem(itemdata);
                    //Dslotitemに変換
                    if(oslotex != null)
                    {
                        MasterSlotitems.TryGetValue(oslotex.api_slotitem_id, out _exslot_exMasterSlotitem);
                    }
                }
                return _exslot_exMasterSlotitem;
            }
        }

        /// <summary>
        /// 制空値を計算します
        /// </summary>
        /// <param name="itemdata">アイテムのデータ</param>
        /// <param name="withdrawnArray">護衛退避の情報</param>
        /// <param name="deckports">艦隊の情報</param>
        /// <returns>制空値</returns>
        public AirSupResult GetAirSupValue(Dictionary<int, SlotItem> itemdata, bool[,] withdrawnArray, List<ApiDeckPort> deckports)
        {
            lock (_lockObject)
            {
                //計算が必要かどうか
                bool calcrequired = false;
                if (_airsup == null) calcrequired = true;//キャッシュされていない場合
                else if (_lastWithdrawnArray == null || _lastWithdrawnArray != withdrawnArray) calcrequired = true;
                //再計算が必要な場合
                if (calcrequired)
                {
                    _airsup = new AirSupResult();

                    //護衛退避しているかどうか
                    bool iswithdrawn = false;
                    foreach (int i in Enumerable.Range(0, deckports.Count))
                    {
                        var d = deckports[i];
                        foreach (int j in Enumerable.Range(0, d.api_ship.Count))
                        {
                            var shipid = d.api_ship[j];
                            //艦隊リストの中に該当艦を発見した場合
                            if (shipid == this.api_id)
                            {
                                if (i >= 0 && i < withdrawnArray.GetLength(0) && j >= 0 && j < withdrawnArray.GetLength(1))
                                {
                                    iswithdrawn = withdrawnArray[i, j];
                                    break;
                                }
                            }
                        }
                        if (iswithdrawn) break;
                    }

                    //退避していない場合に限り制空を計算
                    if (!iswithdrawn)
                    {
                        var oslots = GetOSlotitems(itemdata);
                        var dslots = GetDSlotitems(itemdata);

                        foreach (int i in Enumerable.Range(0, dslots.Count))
                        {
                            var oequip = oslots[i];
                            var equip = dslots[i];
                            //このスロットの制空値
                            var slot_airsup = AirSupResult.SingleSlotitemAirSup(equip, api_onslot[i], oequip.api_alv);

                            //全体にマージ
                            _airsup = _airsup.Merge(slot_airsup);
                        }
                    }

                    _lastWithdrawnArray = withdrawnArray;
                }
                return _airsup;
            }
        }

        public int GetWeightedAntiAirValue(Dictionary<int, SlotItem> itemdata)
        {
            lock(_lockObject)
            {
                if(_weightedAntiAir == null)
                {
                    //素の対空値
                    double taiku = this.api_taiku[0];
                    //Oslots, DSlotsの取得
                    var oslots = GetOSlotitems(itemdata);
                    var dslots = GetDSlotitems(itemdata);

                    foreach(int i in Enumerable.Range(0, dslots.Count))
                    {
                        var oequip = oslots[i];
                        var dequip = dslots[i];

                        //装備の加重対空値の加算
                        taiku += (double)dequip.WeightedAntiAirEquipmentRatio * ((double)dequip.api_tyku + 0.7 * Math.Sqrt(oequip.api_level));
                    }

                    //Intに変換
                    _weightedAntiAir = (int)taiku;
                }
                return (int)_weightedAntiAir;
            }
        }
        #endregion

        #region HPCondition（これもメソッド）
        /// <summary>
        /// 艦娘の状態を取得します
        /// </summary>
        /// <param name="isbath">入渠しているかどうか</param>
        /// <param name="iswithdraw">護衛退避しているかどうか</param>
        /// <param name="hpRatioSelected">指定したHP率</param>
        /// <param name="slotitemdata">装備データ</param>
        /// <returns>艦娘の状態</returns>
        public HPCondition GetHPCondition(bool isbath, bool iswithdraw, double hpRatioSelected, Dictionary<int, SlotItem> slotitemdata)
        {
            lock (_lockObject)
            {
                //キャッシュされていない場合
                if (_hpCondition == null)
                {
                    //コア部分から取得
                    _hpCondition = GetHPCondtionCore(this.api_nowhp, this.api_maxhp, isbath, iswithdraw, hpRatioSelected);

                    //--ダメコンフラグを追加
                    //ダメコンを所持しているかどうか
                    var dslotitems = this.GetDSlotitems(slotitemdata).Select(x => x).ToList();//Addで元が置き換わらないようにディープコピー
                    SlotItem exslot;
                    if (slotitemdata.TryGetValue(this.api_slot_ex, out exslot)) dslotitems.Add(this.GetExslotDSlotitem(slotitemdata));
                    foreach (var d in dslotitems)
                    {
                        //通常ダメコン
                        if (d.api_id == 42) _hpCondition = HPCondition.HasDamecon | _hpCondition;
                        //女神
                        if (d.api_id == 43) _hpCondition = HPCondition.HasDamecon | HPCondition.HasDameconGoddess | _hpCondition;
                    }
                }

                return (HPCondition)_hpCondition;
            }
        }

        /// <summary>
        /// GetHPConditionのコア部分。このメソッドではダメコン所持のフラグは取得できません
        /// </summary>
        /// <param name="nowhp">現在のHP</param>
        /// <param name="maxhp">最大HP</param>
        /// <param name="isBathing">入渠しているかどうか</param>
        /// <param name="isWithdrawn">護衛退避してるかどうか</param>
        /// <param name="hpRatioSelected">指定したHP率以下かどうか</param>
        /// <returns></returns>
        public static HPCondition GetHPCondtionCore(int nowhp, int maxhp, bool isBathing, bool isWithdrawn, double hpRatioSelected)
        {
            var hpcond = HPCondition.Full;

            if (nowhp >= maxhp) hpcond = HPCondition.Full;
            else if (nowhp > maxhp * 0.75) hpcond = HPCondition.None;
            else if (nowhp > maxhp * 0.5) hpcond = HPCondition.SmallDamage;
            else if (nowhp > maxhp * 0.25) hpcond = HPCondition.MiddleDamage;
            else if (nowhp > 0) hpcond = HPCondition.HeavyDamage;
            else if (nowhp == 0) hpcond = HPCondition.Sank;
            else throw new ArgumentException();

            //--フラグを追加
            //入渠
            if (isBathing) hpcond = HPCondition.IsBathing | hpcond;
            //護衛退避
            if (isWithdrawn) hpcond = HPCondition.IsWithdrawn | hpcond;
            //一定HP率以下
            if (nowhp <= maxhp * hpRatioSelected) hpcond = HPCondition.IsUnderSelectedHPRatio | hpcond;

            return hpcond;
        }
        #endregion

        #region WarnState（これもメソッド）
        public WarnState GetWarnState(bool isflagship, bool iswithdraw, double unitHpRatioBorder, Dictionary<int, SlotItem> slotitemdata)
        {
            lock (_lockObject)
            {
                //キャッシュされていない場合
                if (_warnState == null)
                {
                    //損傷チェック
                    var hpcond = GetHPCondition(false, iswithdraw, unitHpRatioBorder, slotitemdata);

                    //退避または入渠してる場合はConditionGreen
                    if(hpcond.HasFlag(HPCondition.IsWithdrawn) || hpcond.HasFlag(HPCondition.IsBathing))
                        return WarnState.ConditionGreen;

                    //大破していて
                    if (hpcond.HasFlag(HPCondition.HeavyDamage))
                    {
                        //ロックされている船の場合 旗艦→state=3, それ以外→state=5
                        if (this.api_locked == 1)
                        {
                            //ダメコン装備しているか（応急修理要員＝42、女神＝43） state2
                            if (hpcond.HasFlag(HPCondition.HasDamecon))
                            {
                                _warnState = WarnState.HasDameconDamaged;
                            }
                            else
                            {
                                if (isflagship) _warnState = WarnState.FlagshipDamaged;//旗艦の場合 state3
                                else _warnState = WarnState.LockedShipDamagedWarning;//それ以外 state5
                            }
                        }
                        //非ロックであるが装備がロックされている場合 state=4
                        else if (this.api_locked_equip == 1)
                        {
                            _warnState = WarnState.ShipUnlockedAndEquipsLockedDamaged;//state 4
                        }
                        //非ロックで装備ロックなしの船の場合 state=1
                        else
                        {
                            _warnState = WarnState.ShipUnlockedDamaged;
                        }
                    }
                    else
                    {
                        _warnState = WarnState.ConditionGreen;
                    }
                }

                return (WarnState)_warnState;
            }
        }
        #endregion

        #region ドラム数、電探数など
        /// <summary>
        /// ドラム数を取得します
        /// </summary>
        /// <param name="itemdata">アイテムデータ</param>
        /// <returns>ドラム缶装備数</returns>
        public int GetDrumNum(Dictionary<int, SlotItem> itemdata)
        {
            lock(_lockObject)
            {
                //キャッシュされていない場合
                if(_numDrum == null)
                {
                    var dslots = GetDSlotitems(itemdata);
                    _numDrum = 0;
                    foreach(var d in dslots)
                    {
                        if (d.api_type == null) continue;
                        //ドラム缶 type = 30 "api_type":[9,19,30,25]
                        if (d.EquipType == 30) _numDrum++;
                    }
                }

                return (int)_numDrum;
            }
        }

        /// <summary>
        /// 電探数を取得します
        /// </summary>
        /// <param name="itemdata">アイテムデータ</param>
        /// <returns>電探装備数</returns>
        public int GetRadarNum(Dictionary<int, SlotItem> itemdata)
        {
            lock(_lockObject)
            {
                //キャッシュされていない場合
                if (_numRadar == null)
                {
                    var dslots = GetDSlotitems(itemdata);
                    _numRadar = 0;
                    foreach(var d in dslots)
                    {
                        if (d.api_type == null) continue;
                        //電探 type : 12(小型電探)、13（大型電探）、93（大型電探II）
                        if (d.EquipType == 12 || d.EquipType == 13 || d.EquipType == 93) _numRadar++;
                    }
                }

                return (int)_numRadar;
            }
        }


        public class DaihatsuResult
        {
            /// <summary>
            /// 個別：大発装備数
            /// </summary>
            public int PerShipNumDaihatsu { get; set; }
            /// <summary>
            /// 個別：大発改修★数合計
            /// </summary>
            public int PerShipNumStars { get; set; }
            /// <summary>
            /// 個別：大発系素補正
            /// </summary>
            public double PerShipBasicBonusRatio { get; set; }
            /// <summary>
            /// 艦隊：大発合計数
            /// </summary>
            public int FleetNumDaihatsu { get; set; }
            /// <summary>
            /// 艦隊：キャップ適用前の大発ボーナス
            /// </summary>
            public double FleetNonCappedBonusRatio { get; set; }
            /// <summary>
            /// 艦隊：キャップ適用後の大発ボーナス
            /// </summary>
            public double FleetCappedBonusRatio { get; set; }
            /// <summary>
            /// 艦隊：キャップが適用されたか
            /// </summary>
            public bool IsCapped { get; set; }

            /// <summary>
            /// 個別艦の結果を合計し、艦隊全体の結果を計算
            /// </summary>
            /// <param name="ships">個別の結果</param>
            /// <returns>艦隊全体の結果</returns>
            public static DaihatsuResult ToFleetResult(IEnumerable<DaihatsuResult> ships)
            {
                DaihatsuResult result = new DaihatsuResult();
                double basic_noncap = 0.0;//キャップ適用前の素補正
                int starts = 0;//改修レベル合計
                foreach(var s in ships)
                {
                    result.FleetNumDaihatsu += s.PerShipNumDaihatsu;
                    starts += s.PerShipNumStars;
                    basic_noncap += s.PerShipBasicBonusRatio;
                }
                double basic_capped = Math.Min(basic_noncap, 0.20);//キャップ適用後の素補正

                if(result.FleetNumDaihatsu == 0)
                {
                    result.FleetCappedBonusRatio = 0.0;
                    result.FleetNonCappedBonusRatio = 0.0;
                }
                else
                {
                    //遠征ボーナス（キャップ適用前）
                    result.FleetNonCappedBonusRatio = basic_noncap + 0.01 * (double)starts * basic_noncap / (double)result.FleetNumDaihatsu;
                    //遠征ボーナス（キャップ適用後）
                    result.FleetCappedBonusRatio = basic_capped + 0.01 * (double)starts * basic_capped / (double)result.FleetNumDaihatsu;
                }
                //キャップが適用されたか
                result.IsCapped = basic_capped != basic_noncap;

                return result;
            }
        }

        /// <summary>
        /// 大発装備数を取得します
        /// </summary>
        /// <param name="itemdata">アイテムデータ</param>
        /// <returns>大発装備の個別データ</returns>
        public DaihatsuResult GetDaihatsuNum(Dictionary<int, SlotItem> itemdata)
        {
            lock(_lockObject)
            {
                if(_numDaihatsu == null)
                {
                    var oslots = GetOSlotitems(itemdata);
                    var dslots = GetDSlotitems(itemdata);
                    _numDaihatsu = new DaihatsuResult();
                    foreach(var i in Enumerable.Range(0, Math.Min(oslots.Count, dslots.Count)))
                    {
                        var o = oslots[i];
                        var d = dslots[i];

                        switch(d.api_id)
                        {
                            //68 : 大発動艇
                            case 68:
                                _numDaihatsu.PerShipBasicBonusRatio += 0.05;
                                goto case -1;
                            //166 : 大発動艇(八九式中戦車&陸戦隊)
                            case 166:
                                _numDaihatsu.PerShipBasicBonusRatio += 0.02;
                                goto case -1;
                            //167 : 特二式内火艇
                            case 167:
                                _numDaihatsu.PerShipBasicBonusRatio += 0.01;
                                goto case -1;
                            //共通処理
                            case -1:
                                _numDaihatsu.PerShipNumDaihatsu++;
                                _numDaihatsu.PerShipNumStars += o.api_level;
                                break;
                        }
                    }
                }

                return _numDaihatsu;
            }
        }
        #endregion

        #endregion
    }
}
