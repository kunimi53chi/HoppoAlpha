using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary.RawApi.ApiPort;
using HoppoAlpha.DataLibrary.RawApi.ApiGetMember;
using HoppoAlpha.DataLibrary.RawApi.ApiMaster;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.DataObject
{
    /// <summary>
    /// 出撃報告書の出撃集計用ハッシュ。これをキーにする
    /// </summary>
    [ProtoContract]
    public struct SortieReportHash : IEquatable<SortieReportHash>
    {
        /// <summary>
        /// 通常艦隊、連合艦隊本体の艦隊編成
        /// </summary>
        [ProtoMember(1)]
        public SortieReportFleetHash Fleet { get; set; }
        /// <summary>
        /// 連合艦隊水雷の艦隊編成
        /// </summary>
        [ProtoMember(2)]
        public SortieReportFleetHash FleetCombined { get; set; }

        private static SortieReportFleetHash zerofleet = new SortieReportFleetHash();
        /// <summary>
        /// 統合モードによって多次元キーをマスキングする
        /// </summary>
        /// <param name="mode">統合するモード</param>
        /// <returns>マスキングされたキー</returns>
        public SortieReportHash Mask(SortieReportShipHashIntegrateMode mode)
        {
            var key = new SortieReportHash();

            if(this.Fleet != zerofleet)
            {
                key.Fleet = this.Fleet.Mask(mode);
            }
            if(this.FleetCombined != zerofleet)
            {
                key.FleetCombined = this.FleetCombined.Mask(mode);
            }

            return key;
        }

        /// <summary>
        /// ハッシュを作成
        /// </summary>
        /// <param name="decks">艦隊のリスト</param>
        /// <param name="ships">船のデータ</param>
        /// <param name="slots">装備のデータ</param>
        /// <param name="isCombined">連合艦隊が結成されているかどうか</param>
        /// <param name="sallyFleetNumber">艦隊番号（第1艦隊なら1）</param>
        /// <returns>出撃集計用のハッシュ</returns>
        public static SortieReportHash CreateInstance
            (List<ApiDeckPort> decks, Dictionary<int, ApiShip> ships, Dictionary<int, SlotItem> slots,
            bool isCombined, int sallyFleetNumber)
        {
            var result = new SortieReportHash();

            //本隊の艦隊
            int mainfleet = isCombined ? 0 : sallyFleetNumber - 1;
            result.Fleet = SortieReportFleetHash.CreateInstance(decks[mainfleet], ships, slots);
            //連合艦隊
            if(isCombined && decks.Count >= 2)
            {
                result.FleetCombined = SortieReportFleetHash.CreateInstance(decks[1], ships, slots);
            }

            return result;
        }

        public bool Equals(SortieReportHash other)
        {
            return Fleet.Equals(other.Fleet) && FleetCombined.Equals(other.FleetCombined);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SortieReportHash)) return false;
            else return Equals((SortieReportHash)obj);
        }

        public override int GetHashCode()
        {
            return Fleet.GetHashCode() ^ FleetCombined.GetHashCode();
        }

        public static bool operator !=(SortieReportHash hash1, SortieReportHash hash2)
        {
            return !hash1.Equals(hash2);
        }

        public static bool operator ==(SortieReportHash hash1, SortieReportHash hash2)
        {
            return hash1.Equals(hash2);
        }
    }

    /// <summary>
    /// 出撃報告書の出撃艦隊構成用のハッシュ
    /// </summary>
    [ProtoContract]
    public struct SortieReportFleetHash : IEquatable<SortieReportFleetHash>
    {
        /// <summary>
        /// 1隻目のキャラと装備
        /// </summary>
        [ProtoMember(1)]
        public SortieReportShipHash Ship1 { get; set; }
        /// <summary>
        /// 2隻目のキャラと装備
        /// </summary>
        [ProtoMember(2)]
        public SortieReportShipHash Ship2 { get; set; }
        /// <summary>
        /// 3隻目のキャラと装備
        /// </summary>
        [ProtoMember(3)]
        public SortieReportShipHash Ship3 { get; set; }
        /// <summary>
        /// 4隻目のキャラと装備
        /// </summary>
        [ProtoMember(4)]
        public SortieReportShipHash Ship4 { get; set; }
        /// <summary>
        /// 5隻目のキャラと装備
        /// </summary>
        [ProtoMember(5)]
        public SortieReportShipHash Ship5 { get; set; }
        /// <summary>
        /// 6隻目のキャラと装備
        /// </summary>
        [ProtoMember(6)]
        public SortieReportShipHash Ship6 { get; set; }

        /// <summary>
        /// 統合モードによって多次元キーをマスキングする
        /// </summary>
        /// <param name="mode">統合するモード</param>
        /// <returns>マスキングされたキー</returns>
        public SortieReportFleetHash Mask(SortieReportShipHashIntegrateMode mode)
        {
            var key = new SortieReportFleetHash();

            key.Ship1 = this.Ship1.Mask(mode);
            key.Ship2 = this.Ship2.Mask(mode);
            key.Ship3 = this.Ship3.Mask(mode);
            key.Ship4 = this.Ship4.Mask(mode);
            key.Ship5 = this.Ship5.Mask(mode);
            key.Ship6 = this.Ship6.Mask(mode);

            //降順ソートの場合はここでする
            if(mode.HasFlag(SortieReportShipHashIntegrateMode._Descending))
            {
                var original = new SortieReportShipHash[]
                {
                    key.Ship1, key.Ship2, key.Ship3,
                    key.Ship4, key.Ship5, key.Ship6,
                };
                //降順ソート
                SortieReportShipHash[] query = Enumerable.Repeat(new SortieReportShipHash(), 6).ToArray();
                if (mode.HasFlag(SortieReportShipHashIntegrateMode._ShipType))
                {
                    query = original.OrderByDescending(x => x.MasterShipTypeID).ToArray();
                }
                else if (mode.HasFlag(SortieReportShipHashIntegrateMode._ShipId) || mode.HasFlag(SortieReportShipHashIntegrateMode._Slotitem))
                {
                    query = original.OrderByDescending(x => x.MasterShipID).ToArray();
                }

                //再度登録
                key.Ship1 = query[0];
                key.Ship2 = query[1];
                key.Ship3 = query[2];
                key.Ship4 = query[3];
                key.Ship5 = query[4];
                key.Ship6 = query[5];
            }

            return key;
        }

        /// <summary>
        /// 艦隊のハッシュを作成
        /// </summary>
        /// <param name="deck">艦隊オブジェクト</param>
        /// <param name="ships">船のデータ</param>
        /// <param name="slots">装備のデータ</param>
        /// <returns>艦隊のハッシュ</returns>
        public static SortieReportFleetHash CreateInstance
            (ApiDeckPort deck, Dictionary<int, ApiShip> ships, Dictionary<int, SlotItem> slots)
        {
            var result = new SortieReportFleetHash();

            //船のインスタンスを登録
            foreach (int i in Enumerable.Range(0, deck.api_ship.Count))
            {
                ApiShip oship;
                if (!ships.TryGetValue(deck.api_ship[i], out oship)) continue;//取得できなかった場合

                var x = SortieReportShipHash.CreateInstance(oship, slots);

                switch (i)
                {
                    case 0: result.Ship1 = x; break;
                    case 1: result.Ship2 = x; break;
                    case 2: result.Ship3 = x; break;
                    case 3: result.Ship4 = x; break;
                    case 4: result.Ship5 = x; break;
                    case 5: result.Ship6 = x; break;
                }
            }

            return result;
        }

        public bool Equals(SortieReportFleetHash other)
        {
            return Ship1.Equals(other.Ship1) && Ship2.Equals(other.Ship2) && Ship3.Equals(other.Ship3) &&
                Ship4.Equals(other.Ship4) && Ship5.Equals(other.Ship5) && Ship6.Equals(other.Ship6);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SortieReportFleetHash)) return false;
            else return Equals((SortieReportFleetHash)obj);
        }

        public override int GetHashCode()
        {
            int result = Ship1.GetHashCode();
            result = result ^ Ship2.GetHashCode();
            result = result ^ Ship3.GetHashCode();
            result = result ^ Ship4.GetHashCode();
            result = result ^ Ship5.GetHashCode();
            result = result ^ Ship6.GetHashCode();

            return result;
        }

        public static bool operator !=(SortieReportFleetHash fleethash1, SortieReportFleetHash fleethash2)
        {
            return !fleethash1.Equals(fleethash2);
        }

        public static bool operator ==(SortieReportFleetHash fleethash1, SortieReportFleetHash fleethash2)
        {
            return fleethash1.Equals(fleethash1);
        }

        //UI用
        #region UI用
        //TreeViewの表示用
        /// <summary>
        /// TreeViewの表示用のテキストの取得
        /// </summary>
        /// <param name="viewmode">統合するモード</param>
        /// <returns></returns>
        public string DisplayTreeView(SortieReportShipHashIntegrateMode viewmode, ExMasterShipCollection shipdata, Dictionary<int, ApiMstStype> stypedata)
        {
            //艦種のみ
            if (viewmode.HasFlag(SortieReportShipHashIntegrateMode._ShipType))
            {
                Dictionary<int, int> stypes = new Dictionary<int, int>();
                AppendDictionary(stypes, Ship1.MasterShipTypeID);
                AppendDictionary(stypes, Ship2.MasterShipTypeID);
                AppendDictionary(stypes, Ship3.MasterShipTypeID);
                AppendDictionary(stypes, Ship4.MasterShipTypeID);
                AppendDictionary(stypes, Ship5.MasterShipTypeID);
                AppendDictionary(stypes, Ship6.MasterShipTypeID);
                //艦種リスト
                List<string> typestr = new List<string>();
                foreach (var x in stypes)
                {
                    if (x.Key == 0) continue;
                    ApiMstStype dstype;
                    if (stypedata.TryGetValue(x.Key, out dstype))
                    {
                        string s;
                        if (dstype.api_name.Length <= 5) s = dstype.api_name;
                        else s = dstype.api_name.Substring(0, 5);
                        typestr.Add(s+x.Value);
                    }
                }

                return string.Join(" ", typestr);
            }
            //艦名・装備込み
            else if (viewmode.HasFlag(SortieReportShipHashIntegrateMode._ShipId) || viewmode.HasFlag(SortieReportShipHashIntegrateMode._Slotitem))
            {
                int[] mastershipid = new int[]
                {
                    Ship1.MasterShipID, Ship2.MasterShipID, Ship3.MasterShipID, Ship4.MasterShipID, Ship5.MasterShipID, Ship6.MasterShipID,
                };
                List<string> shipname = new List<string>();
                foreach (var m in mastershipid)
                {
                    ExMasterShip dship;
                    if (shipdata.TryGetValue(m, out dship))
                    {
                        string s;
                        if (dship.api_name.Length <= 5) s = dship.api_name;
                        else s = dship.api_name.Substring(0, 5);
                        shipname.Add(s);
                    }
                }

                return string.Join(" ", shipname);
            }

            else return "";
        }

        //集計用のDictionaryの加算用
        private static void AppendDictionary(Dictionary<int, int> dict, int key)
        {
            int x;
            dict.TryGetValue(key, out x);
            dict[key] = x + 1;
        }
        #endregion
    }

    /// <summary>
    /// 出撃報告書の出撃艦隊構成用の1隻あたりのハッシュ（SortieReportFleetHashの中身）
    /// </summary>
    [ProtoContract]
    public struct SortieReportShipHash : IEquatable<SortieReportShipHash>
    {
        /// <summary>
        /// 船の艦種ID
        /// </summary>
        [ProtoMember(1)]
        public int MasterShipTypeID { get; set; }
        /// <summary>
        /// 船のマスターID
        /// </summary>
        [ProtoMember(2)]
        public int MasterShipID { get; set; }
        /// <summary>
        /// 1スロ目装備のマスターID
        /// </summary>
        [ProtoMember(3)]
        public int SlotitemMasterID1 { get; set; }
        /// <summary>
        /// 2スロ目装備のマスターID
        /// </summary>
        [ProtoMember(4)]
        public int SlotitemMasterID2 { get; set; }
        /// <summary>
        /// 3スロ目装備のマスターID
        /// </summary>
        [ProtoMember(5)]
        public int SlotitemMasterID3 { get; set; }
        /// <summary>
        /// 4スロ目装備のマスターID
        /// </summary>
        [ProtoMember(6)]
        public int SlotitemMasterID4 { get; set; }
        /// <summary>
        /// 5スロ目装備のマスターID
        /// </summary>
        [ProtoMember(7)]
        public int SlotitemMasterID5 { get; set; }
        /// <summary>
        /// 拡張スロ装備のマスターID
        /// </summary>
        [ProtoMember(8)]
        public int SlotitemMasterIDEx { get; set; }

        /// <summary>
        /// 統合モードによって多次元キーをマスキングする
        /// </summary>
        /// <param name="mode">統合するモード</param>
        /// <returns>マスキングされたキー</returns>
        public SortieReportShipHash Mask(SortieReportShipHashIntegrateMode mode)
        {
            var key = new SortieReportShipHash();
            //艦種キーを入れる場合
            if(mode.HasFlag(SortieReportShipHashIntegrateMode._ShipType))
            {
                key.MasterShipTypeID = this.MasterShipTypeID;
            }
            //艦IDのキーを入れる場合
            if(mode.HasFlag(SortieReportShipHashIntegrateMode._ShipId))
            {
                key.MasterShipID = this.MasterShipID;
            }
            //装備キーを入れる場合
            if(mode.HasFlag(SortieReportShipHashIntegrateMode._Slotitem))
            {
                key.MasterShipTypeID = this.MasterShipTypeID;
                key.MasterShipID = this.MasterShipID;
                key.SlotitemMasterID1 = this.SlotitemMasterID1;
                key.SlotitemMasterID2 = this.SlotitemMasterID2;
                key.SlotitemMasterID3 = this.SlotitemMasterID3;
                key.SlotitemMasterID4 = this.SlotitemMasterID4;
                key.SlotitemMasterID5 = this.SlotitemMasterID5;
                key.SlotitemMasterIDEx = this.SlotitemMasterIDEx;
            }

            return key;
        }

        /// <summary>
        /// 船のハッシュを作成
        /// </summary>
        /// <param name="ship">船のオブジェクト</param>
        /// <param name="slots">装備データ</param>
        /// <returns>船のハッシュ</returns>
        public static SortieReportShipHash CreateInstance(ApiShip ship, Dictionary<int, SlotItem> slots)
        {
            var result = new SortieReportShipHash();

            //船のマスターデータ
            var dship = ship.DShip;
            result.MasterShipID = dship.api_id;
            result.MasterShipTypeID = dship.api_stype;
            //装備
            var oslots = ship.GetOSlotitems(slots);
            foreach (int i in Enumerable.Range(0, oslots.Count))
            {
                //装備のマスターID
                int x;
                if (oslots[i] == null) x = 0;
                else x = oslots[i].api_slotitem_id;
                //装備の登録
                switch (i)
                {
                    case 0: result.SlotitemMasterID1 = x; break;
                    case 1: result.SlotitemMasterID2 = x; break;
                    case 2: result.SlotitemMasterID3 = x; break;
                    case 3: result.SlotitemMasterID4 = x; break;
                    case 4: result.SlotitemMasterID5 = x; break;
                }
            }
            //拡張スロットの装備
            SlotItem exslot;
            if (slots.TryGetValue(ship.api_slot_ex, out exslot))
            {
                result.SlotitemMasterIDEx = exslot.api_slotitem_id;
            }

            return result;
        }


        public bool Equals(SortieReportShipHash other)
        {
            return MasterShipID.Equals(other.MasterShipID) && MasterShipTypeID.Equals(other.MasterShipTypeID) &&
                SlotitemMasterID1.Equals(other.SlotitemMasterID1) && SlotitemMasterID2.Equals(other.SlotitemMasterID2) &&
                SlotitemMasterID3.Equals(other.SlotitemMasterID3) && SlotitemMasterID4.Equals(other.SlotitemMasterID4) &&
                SlotitemMasterID5.Equals(other.SlotitemMasterID5) && SlotitemMasterIDEx.Equals(other.SlotitemMasterIDEx);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SortieReportShipHash)) return false;
            else return Equals((SortieReportShipHash)obj);
        }

        public override int GetHashCode()
        {
            //全ての排他的論理和を取る
            return MasterShipID ^ MasterShipTypeID ^ SlotitemMasterID1 ^ SlotitemMasterID2 ^ SlotitemMasterID3 ^ SlotitemMasterID4 ^ SlotitemMasterID5 ^ SlotitemMasterIDEx;
        }

        public static bool operator !=(SortieReportShipHash shiphash1, SortieReportShipHash shiphash2)
        {
            return !shiphash1.Equals(shiphash2);
        }

        public static bool operator ==(SortieReportShipHash shiphash1, SortieReportShipHash shiphash2)
        {
            return shiphash1.Equals(shiphash2);
        }
    }

    /// <summary>
    /// 船のハッシュキーを統合するモード
    /// </summary>
    [Flags]
    public enum SortieReportShipHashIntegrateMode
    {
        /// <summary>
        /// 初期化用
        /// </summary>
        None = 0,
        /// <summary>
        /// 使用しないでください。内部フラグ
        /// </summary>
        _ShipType = 1,
        /// <summary>
        /// 使用しないでください。内部フラグ
        /// </summary>
        _ShipId = 2,
        /// <summary>
        /// 使用しないでください。内部フラグ
        /// </summary>
        _Slotitem = 4,
        /// <summary>
        /// 使用しないでください。内部フラグ
        /// </summary>
        _Descending = 8,
        /// <summary>
        /// 艦種のIDでマスクし降順ソートして統合する
        /// </summary>
        MaskShipTypeDescending = _ShipType | _Descending,
        /// <summary>
        /// 艦のマスターIDでマスクし統合する
        /// </summary>
        MaskShipId = _ShipType | _ShipId,
        /// <summary>
        /// 艦の装備まで考慮してマスクし統合する
        /// </summary>
        MaskSlotitem = _ShipType | _ShipId | _Slotitem,
    }

    /// <summary>
    /// 期間別のファイルを統合するモード
    /// </summary>
    public enum SortieReportTermIntegrateMode
    {
        None = 0,
        /// <summary>
        /// 週（１ファイル）単位で集計
        /// </summary>
        Week = 1,
        /// <summary>
        /// 約１ヶ月単位で集計
        /// </summary>
        Month = 2,
        /// <summary>
        /// １年単位で集計
        /// </summary>
        Year = 3,
        /// <summary>
        /// 全ファイルを一気に集計
        /// </summary>
        All = 4,
    }

    /// <summary>
    /// 出撃報告書のマップ集計用ハッシュ。これをキーにする
    /// </summary>
    [ProtoContract]
    public struct SortieReportMapHash : IEquatable<SortieReportMapHash>
    {
        /// <summary>
        /// 海域番号（5-4なら5）
        /// </summary>
        [ProtoMember(1)]
        public int MapAreaID { get; set; }
        /// <summary>
        /// マップ番号（5-4なら4）
        /// </summary>
        [ProtoMember(2)]
        public int MapInfoID { get; set; }

        //演算子オーバーロード
        public static bool operator !=(SortieReportMapHash maphash1, SortieReportMapHash maphash2)
        {
            return !maphash1.Equals(maphash2);
        }

        public static bool operator ==(SortieReportMapHash maphash1, SortieReportMapHash maphash2)
        {
            return maphash1.Equals(maphash2);
        }

        public bool Equals(SortieReportMapHash other)
        {
            return Equals(MapAreaID, other.MapAreaID) && Equals(MapInfoID, other.MapInfoID);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SortieReportMapHash)) return false;
            else return Equals((SortieReportMapHash)obj);
        }

        public override int GetHashCode()
        {
            return MapAreaID * 31 + MapInfoID;//MapInfoはたかだか1桁なので十分に大きい素数を取って和を求めればOK？
        }
        
        //UI用
        public string Display()
        {
            return MapAreaID + "-" + MapInfoID;
        }
    }
}
