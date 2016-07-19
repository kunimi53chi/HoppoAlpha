using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary.RawApi.ApiReqRanking
{
    /// <summary>
    /// ランキングのAPIデータ
    /// </summary>
    public class ApiRanking
    {
        /// <summary>
        /// この項目はシリアル化されません。ランキングで表示される最大順位を表します
        /// </summary>
        public int api_count { get; set; }
        /// <summary>
        /// この項目はシリアル化されません。ランキングで表示される最大ページ数を表します
        /// </summary>
        public int api_page_count { get; set; }
        /// <summary>
        /// この項目はシリアル化されません。現在表示されているランキングのページ番号を表します
        /// </summary>
        public int api_disp_page { get; set; }
        /// <summary>
        /// この項目はシリアル化されません。ランキングのデータ部分を表します
        /// </summary>
        public List<ApiList> api_list { get; set; }

        //ユーザーデータ
        /// <summary>
        /// ランキングのユーザーデータ
        /// </summary>
        [ProtoContract]
        public class ApiList
        {
            /// <summary>
            /// 順位を表します
            /// </summary>
            [ProtoMember(1)]
            public int api_no { get; set; }
            /// <summary>
            /// ユーザーIDを表します
            /// </summary>
            [Obsolete]
            [ProtoMember(2)]
            public int api_member_id { get; set; }
            /// <summary>  
            /// ユーザーの司令部レベルを表します
            /// </summary>
            [ProtoMember(3)]
            public int api_level { get; set; }
            /// <summary>
            /// ユーザーの階級(元帥=1, 大将=2 …)を表します
            /// </summary>
            [ProtoMember(4)]
            public int api_rank { get; set; }
            /// <summary>
            /// ユーザーの名前を表します
            /// </summary>
            [ProtoMember(5)]
            public string api_nickname { get; set; }
            /// <summary>
            /// ユーザーの提督経験値を表します
            /// </summary>
            [ProtoMember(6)]
            public int api_experience { get; set; }
            /// <summary>
            /// ユーザーのコメントを表します
            /// </summary>
            [ProtoMember(7)]
            public string api_comment { get; set; }
            /// <summary>
            /// ユーザーの戦果を表します（測定値）
            /// </summary>
            [Obsolete]
            [ProtoMember(8)]
            public int api_rate { get; set; }
            [ProtoMember(9)]
            public int api_flag { get; set; }
            /// <summary>
            /// ユーザーの名前のIDを表します
            /// </summary>
            [ProtoMember(10)]
            public string api_nickname_id { get; set; }
            /// <summary>
            /// ユーザーのコメントのIDを表します
            /// </summary>
            [ProtoMember(11)]
            public string api_comment_id { get; set; }
            /// <summary>
            /// ユーザーの甲種勲章の数を表します
            /// </summary>
            [ProtoMember(12)]
            public int api_medals { get; set; }

            /// <summary>
            /// ユーザーによって入力された情報解析用の戦果値（表示値）
            /// </summary>
            [ProtoMember(100)]
            public int ForAnalyzeSenkaValue { get; set; }
            /// <summary>
            /// ForAnalyzeSenkaValueが入力されたかどうか
            /// </summary>
            [ProtoMember(101)]
            public bool IsForAnalyzeSenkaValueSet { get; set; }
            /// <summary>
            /// 戦果の測定値の形式（2016/7/15以前は0, 以降は1）
            /// </summary>
            [ProtoMember(102)]
            public int ApiRateStyle { get; set; }

            /// <summary>
            /// EOメモ（EO検出が自動で出なくなったので）
            /// </summary>
            [ProtoMember(200)]
            public int EoMemoValue { get; set; }

            /// <summary>
            /// 表示する戦果
            /// </summary>
            public int ViewSenka
            {
                get
                {
                    switch(ApiRateStyle)
                    {
                        //2016/7/15以前
                        case 0:
                            return this.api_rate;
                        //2016/7/15以降
                        case 1:
                            if (IsForAnalyzeSenkaValueSet) return this.ForAnalyzeSenkaValue;
                            else return -1;
                        default:
                            return -1;
                    }
                }
            }

            /// <summary>
            /// インスタンスをディープコピーします
            /// </summary>
            /// <returns>複製されたインスタンス</returns>
            public ApiList DeepCopy()
            {
                ApiList result = new ApiList();

                result.api_no = this.api_no;
                result.api_member_id = this.api_member_id;
                result.api_level = this.api_level;
                result.api_rank = this.api_rank;
                result.api_nickname = this.api_nickname;

                result.api_experience = this.api_experience;
                result.api_comment = this.api_comment;
                result.api_rate = this.api_rate;
                result.api_flag = this.api_flag;
                result.api_nickname_id = this.api_nickname_id;

                result.api_comment_id = this.api_comment_id;
                result.api_medals = this.api_medals;

                result.ForAnalyzeSenkaValue = this.ForAnalyzeSenkaValue;
                result.IsForAnalyzeSenkaValueSet = this.IsForAnalyzeSenkaValueSet;
                result.ApiRateStyle = this.ApiRateStyle;

                result.EoMemoValue = this.EoMemoValue;

                return result;
            }

            /// <summary>
            /// 同一名用のアカウントの類似度計算（api_member_idが消えて提督名だけでは一意性を保証できなくなったため）
            /// </summary>
            /// <param name="previousRankingData">比較対象のランキングデータ</param>
            /// <returns>アカウントの類似度</returns>
            public double CalcAccountSimilarity(ApiList previousRankingData)
            {
                if(previousRankingData == null) return 0.0;

                var simi = 0.0;
                //コメント一致したら+1点
                if (this.api_comment == previousRankingData.api_comment) simi += 1.0;
                //階級による類似点
                simi += Similarity(this.api_rank, previousRankingData.api_rank);
                //戦果による類似点
                simi += Similarity(this.ViewSenka, previousRankingData.ViewSenka);
                //甲種勲章よる類似点
                simi += Similarity(this.api_medals, previousRankingData.api_medals);

                return simi;
            }

            private double Similarity(int newValue, int oldValue)
            {
                if(oldValue == 0)
                {
                    if (newValue == oldValue) return 1.0;
                    else return 0.0;
                }
                else
                {
                    var ratio = Math.Abs((double)newValue / (double)oldValue);
                    //1を超えた場合は逆数を返す
                    if (ratio > 1) return 1 / ratio;
                    else return ratio;
                }
            }

            /// <summary>
            /// 以前のデータから、提督名＋パラメーター類似度で抽出（多少同一提督名に配慮）
            /// </summary>
            /// <param name="previousData">以前のデータ</param>
            /// <returns>抽出されたメンバー</returns>
            public ApiRanking.ApiList GetPreviousDataFromSimilarity(SortedDictionary<int, ApiList> previousData)
            {
                if (previousData == null) return null;

                var prevPlayerDataLists = previousData.Values.Where(x => x.api_nickname == this.api_nickname).ToList();
                if (prevPlayerDataLists.Count == 0) return null;
                else if (prevPlayerDataLists.Count == 1) return prevPlayerDataLists[0];
                else
                {
                    var sims = prevPlayerDataLists
                        .Select(x => new Tuple<ApiRanking.ApiList, double>(x, this.CalcAccountSimilarity(x)));
                    return sims.OrderByDescending(x => x.Item2).Select(x => x.Item1).FirstOrDefault();
                }
            }

            /// <summary>
            /// CSV形式の行に変換
            /// </summary>
            /// <param name="rankName">階級の日本語名</param>
            /// <returns>CSVの形式の行</returns>
            public Const.CsvList<string> ExportToCsvRow(string rankName)
            {
                var row = new Const.CsvList<string>();
                //順位
                row.Add(this.api_no.ToString());
                //提督ID
                row.Add(this.api_member_id.ToString());
                //司令部レベル
                row.Add(this.api_level.ToString());
                //階級
                row.Add(rankName);
                //名前
                row.Add(this.api_nickname);
                //-
                //提督経験値
                row.Add(this.api_experience.ToString());
                //コメント
                row.Add(this.api_comment);
                //戦果
                row.Add(this.ViewSenka.ToString());
                //api_flag
                row.Add(this.api_flag.ToString());
                //名前ID
                row.Add(this.api_nickname_id);
                //--
                //コメントID
                row.Add(this.api_comment_id);
                //甲種勲賞
                row.Add(this.api_medals.ToString());

                return row;
            }
        }

        /// <summary>
        /// ランキングの差分を計算します
        /// </summary>
        public class RankingDiff
        {
            /// <summary>
            /// 戦果の増分を表します
            /// </summary>
            public int? DiffSenka { get; private set; }
            /// <summary>
            /// EOの増分を表します
            /// </summary>
            public int? DiffEO { get; private set; }

            /// <summary>
            /// ランキングの差分を計算します
            /// </summary>
            /// <param name="calcMemberName">計算するメンバーの名前</param>
            /// <param name="selectedData">現在のランキングデータ</param>
            /// <param name="calcMemberNowPosition">計算するメンバーの現在の順位</param>
            /// <param name="previousData">直前のセクションのランキングデータ</param>
            /// <returns>戦果差分の結果</returns>
            public static RankingDiff CalcDiff(string calcMemberName, SortedDictionary<int, ApiRanking.ApiList> selectedData, int calcMemberNowPosition, SortedDictionary<int, ApiRanking.ApiList> previousData)
            {
                RankingDiff result = new RankingDiff();
                result.DiffSenka = null; result.DiffEO = null;

                if (selectedData == null || previousData == null) return result;

                ApiRanking.ApiList nowPlayerData = selectedData.Values.Where(x => x.api_no == calcMemberNowPosition).FirstOrDefault();
                if (nowPlayerData == null) return result;

                //同一提督名があった場合の類似度検索
                ApiRanking.ApiList prevPlayerData = nowPlayerData.GetPreviousDataFromSimilarity(previousData);

                //選択したメンバーのデータが取得できない場合
                if(prevPlayerData == null) return result;
                //両方データが取得できた場合
                else
                {
                    //戦果差分
                    result.DiffSenka = nowPlayerData.ViewSenka - prevPlayerData.ViewSenka;
                    ////経験値差分
                    //int expdiff = nowPlayerData.api_experience - prevPlayerData.api_experience;
                    ////経験値増加による戦果増加
                    //double netdiffsenka = (double)expdiff / 10000.0 * 7.0;
                    //EO増加分
                    result.DiffEO = nowPlayerData.EoMemoValue;

                    return result;
                }
            }
        }

        public class RankingEOCalc
        {
            /// <summary>
            /// 月初戦果
            /// </summary>
            public int? FirstSenka { get; set; }
            /// <summary>
            /// 月初戦果のレコード名
            /// </summary>
            public string FirstSenkaRecordName { get; set; }
            /// <summary>
            /// 破壊済みEO
            /// </summary>
            public int? DestroiedEO { get; set; }
            /// <summary>
            /// EO補正済み戦果
            /// </summary>
            public int? CorrelatedWithEOSenka { get; set; }
            /// <summary>
            /// EO・潜水補正済み戦果
            /// </summary>
            public int? CorrelatedWithEOSubmarineSenka { get; set; }


            /// <summary>
            /// 月初データに依存する破壊済みEO、EO補正済み戦果を計算
            /// </summary>
            /// <param name="calcMemberName">計算するメンバーの名前</param>
            /// <param name="selectedData">現在のランキングデータ</param>
            /// <param name="calcMemberNowPosition">計算するメンバーの現在の順位</param>
            /// <param name="allMonthDatas">月全体のランキングデータ（複数可能）</param>
            /// <param name="submarinerHandicap">潜水マンのEO不足ハンデ。水上マンなら0にする</param>
            /// <returns></returns>
            public static RankingEOCalc CalcEO(string calcMemberName, SortedDictionary<int, ApiRanking.ApiList> selectedData, int calcMemberNowPosition,
                Dictionary<string, SortedDictionary<int, ApiRanking.ApiList>>allMonthDatas, int submarinerHandicap)
            {
                //結果が取得できない場合の値のセット
                RankingEOCalc result = new RankingEOCalc();
                result.FirstSenka = null; result.DestroiedEO = null; result.CorrelatedWithEOSenka = null; result.CorrelatedWithEOSubmarineSenka = null;

                if (selectedData == null || allMonthDatas == null) return result;

                //現在のデータの取得
                var nowPlayerData = selectedData.Values.Where(x => x.api_no == calcMemberNowPosition).FirstOrDefault();
                if (nowPlayerData == null) return result;

                //月初戦果データの取得
                ApiRanking.ApiList firstPlayerData = null;
                foreach(var d in allMonthDatas.Take(4))
                {
                    //月初データのコレクションを頭から検索してあったら登録
                    if (d.Value != null) firstPlayerData = nowPlayerData.GetPreviousDataFromSimilarity(d.Value);
                    if (firstPlayerData != null)
                    {
                        //現在のデータも取得できた場合は最初の戦果レコード名を登録
                        if (nowPlayerData != null) result.FirstSenkaRecordName = d.Key;
                        break;
                    }
                }

                //選択したメンバーのデータが取得できない場合
                if(firstPlayerData == null)
                {
                    return result;
                }
                //両方データが取得できた場合
                else
                {
                    //月初戦果
                    result.FirstSenka = firstPlayerData.ViewSenka;
                    //破壊済みEO
                    // = [最新戦果] - [月初戦果] - ([最新経験値] - [月初経験値]) ÷ r （※r = 10000/7）
                    //double destroied = (double)nowPlayerData.api_rate - (double)firstPlayerData.api_rate
                    //    - ((double)nowPlayerData.api_experience - (double)firstPlayerData.api_experience) / 10000.0 * 7.0;
                    //result.DestroiedEO = (int)destroied;
                    bool isCalcStarted = false; result.DestroiedEO = 0;
                    foreach(var d in allMonthDatas)
                    {
                        if(!isCalcStarted)
                        {
                            if (d.Key != result.FirstSenkaRecordName) continue;
                            else isCalcStarted = true;
                        }

                        if(isCalcStarted)
                        {
                            var origData = nowPlayerData.GetPreviousDataFromSimilarity(d.Value);
                            if (origData != null) result.DestroiedEO += origData.EoMemoValue;
                        }
                    }
                    //EO補正済み戦果
                    // = [EO合計] + [最新戦果] - [破壊済みEO]
                    // ※EO合計は0と仮定して良い
                    //result.CorrelatedWithEOSenka = (int)((double)nowPlayerData.api_rate - destroied);
                    result.CorrelatedWithEOSenka = nowPlayerData.ViewSenka - result.DestroiedEO;
                    //EO・潜水補正済み戦果
                    // = [EO補正済み戦果] - [潜水マンハンデ]
                    result.CorrelatedWithEOSubmarineSenka = result.CorrelatedWithEOSenka - submarinerHandicap;

                    return result;
                }
            }
        }
    }

    /*
     * -----補正戦果メモ-----
     * 1日AMランキング		提督経験値をE0　戦果をA0　とする
     * 現在のランキング	提督経験値をEn　戦果をAn　とする
     * 
     * 定数
     * ・EO合計　s=現在は、75+75+100+150+180+200=780
     * ・提督経験値←→戦果の変換倍率　r=10000/7
     * 
     * 現在の見かけ戦果：
     * 　An = (En - E0) / r + A0 + [破壊済みEO]
     * 
     * 今月の純増（経験値による増加分の）戦果
     * 　= (En - E0) / r
     * 
     * 破壊済みEO
     * 　= An - A0 - (En - E0) / r
     * 
     * 残EO
     * 　= s - [破壊済みEO]
     * 　= s - {An - A0 - (En - E0) / r}
     * 
     * 残EO補正済み戦果
     * 　= An + [残EO]
     * 　= An + s - {An - A0 - (En - E0) / r}
     * 　= s + A0 + (En - E0) / r
     * 　= s + A0 + [今月の純増戦果]
     * 　= s + An - [破壊済みEO]
     * →sは定数なので、残EO補正済み戦果のソート結果は、A0+[今月の純増戦果]のソート結果と等しい
     * →【sを0と仮定してもソートにおいては何ら問題がない！】
     * 
     * ただし、sは厳密には定数ではなく、「全員が全てのEOを最終的に破壊すると仮定した場合、定数と扱える」という条件つき。
     * したがって、潜水マンの場合はsが-200されるので、残EO補正済み戦果に-200してソートするとより正確な結果が得られる
     */

    public static class ApiRankingListExt
    {
        public static DataTable ToDatatable(this SortedDictionary<int, ApiRanking.ApiList> ranking)
        {
            var dt = new DataTable();

            dt.Columns.Add("Rank", Type.GetType("System.String"));
            dt.Columns.Add("Name", Type.GetType("System.String"));
            dt.Columns.Add("MeasuredValue", Type.GetType("System.Int32"));
            dt.Columns.Add("ViewValue", Type.GetType("System.Int32"));
            dt.Columns.Add("IsSet", Type.GetType("System.Boolean"));

            //データの追加
            foreach(var r in ranking)
            {
                var row = dt.NewRow();
                row["Rank"] = r.Value.api_no + "位";
                row["Name"] = r.Value.api_nickname;
                row["MeasuredValue"] = r.Value.api_rate;
                row["ViewValue"] = r.Value.ForAnalyzeSenkaValue;
                row["IsSet"] = r.Value.IsForAnalyzeSenkaValueSet;

                dt.Rows.Add(row);
            }

            dt.TableName = "TableSenka";

            return dt;
        }

        /// <summary>
        /// ApiRateを取得するだけのメソッド
        /// </summary>
        /// <param name="apilists"></param>
        /// <returns></returns>
        public static int[] GetApiRates(this List<ApiRanking.ApiList> apilists)
        {
            return apilists.Select(x => x.api_rate).ToArray();
        }
    }
}
