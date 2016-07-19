using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.DataLibrary
{
    /// <summary>
    /// データタイプ列挙体の拡張メソッド
    /// </summary>
    public static class DataTypeExt
    {
        /// <summary>
        /// ほっぽアルファ側で定義されている、DataType列挙体に対応する型を取得します
        /// </summary>
        /// <param name="d">DataType列挙体</param>
        /// <returns>コレクションの型</returns>
        public static Type GetCollectionType(this DataType d)
        {
            switch(d)
            {
                case DataType.Config:
                    return typeof(DataObject.ConfigSerializeItem);
                case DataType.Counter:
                    return typeof(List<DataObject.CounterItem>);
                case DataType.DefaultSlotItem:
                    return typeof(object);
                case DataType.DropRecord:
                    return typeof(DataObject.DropRecordCollection);
                case DataType.EnemyFleet:
                    return typeof(Dictionary<int, DataObject.EnemyFleetRecord>);
                case DataType.Experience:
                    return typeof(List<DataObject.ExpRecord>);
                case DataType.Material:
                    return typeof(List<DataObject.MaterialRecord>);
                case DataType.Query:
                    return typeof(List<DataObject.UnitQuery>);
                case DataType.Quest:
                    return typeof(DataObject.QuestRecord);
                case DataType.Ranking:
                    return typeof(SortedDictionary<int, RawApi.ApiReqRanking.ApiRanking.ApiList>);
                case DataType.Senka:
                    return typeof(List<DataObject.SenkaRecord>);
                case DataType.ExMasterShip:
                    return typeof(DataObject.ExMasterShipCollection);
                case DataType.ExMasterSlotitem:
                    return typeof(DataObject.ExMasterSlotitemCollection);
                case DataType.SortieReport:
                    return typeof(DataObject.SortieReportCollection);
                case DataType.PracticeInfo:
                    return typeof(DataObject.PracticeInfoCollection);
                default:
                    throw new NotImplementedException("要求されたDataTypeに対する、GetCollectionTypeが実装されていません");
            }
        }

        //IsNullOrEmpty拡張メソッド
        #region IsNullOrEmpty
        /// <summary>
        /// コレクションがNullもしくはEmptyかどうかを判定します
        /// </summary>
        /// <param name="d">DataType列挙体</param>
        /// <param name="collection">コレクションのインスタンス</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this DataType d, dynamic collection)
        {
            //インスタンスがNullかどうか
            if (collection == null) return true;

            //型のチェック
            var collectType = d.GetCollectionType();
            if(collectType != collection.GetType())
            {
                throw new ArgumentException("要求されたDataTypeに対するcollectionの型が正しくないため、IsNullOrEmptyを比較できません");
            }

            //個別のチェック
            //(´・ω・｀)コレクションを独自のクラスでラップしてインターフェースかければよかったよ
            //(´・ω・｀)らんらん♪
            switch(d)
            {
                case DataType.Config:
                    return collection == null;
                case DataType.Counter:
                    return _IsNullOrEmpty(collection);
                case DataType.DefaultSlotItem:
                    return true;
                case DataType.DropRecord:
                    var dr = (DataObject.DropRecordCollection)collection;
                    return dr.IsNullOrEmpty();
                case DataType.EnemyFleet:
                    return _IsNullOrEmpty(collection);
                case DataType.Experience:
                    return _IsNullOrEmpty(collection);
                case DataType.Material:
                    return _IsNullOrEmpty(collection);
                case DataType.Query:
                    return _IsNullOrEmpty(collection);
                case DataType.Quest:
                    var quest = (DataObject.QuestRecord)collection;
                    return quest.IsNullOrEmpty();
                case DataType.Ranking:
                    return _IsNullOrEmpty(collection);
                case DataType.Senka:
                    return _IsNullOrEmpty(collection);
                case DataType.ExMasterShip:
                    return _IsNullOrEmpty(collection);
                case DataType.ExMasterSlotitem:
                    return _IsNullOrEmpty(collection);
                case DataType.SortieReport:
                    var sortie = (DataObject.SortieReportCollection)collection;
                    return sortie.IsNullOrEmpty();
                case DataType.PracticeInfo:
                    var prac = (DataObject.PracticeInfoCollection)collection;
                    return prac.IsNullOfEmpty();
                default:
                    throw new NotImplementedException("要求されたDataTypeに対する、IsNullOrEmptyが実装されていません");
            }
        }

        //ListやDictionaryやSortedDictionaryなど.Countプロパティを持つ用
        private static bool _IsNullOrEmpty(dynamic collection)
        {
            if (collection == null) return true;
            if (collection.Count == 0) return true;
            return false;
        }
        #endregion

        //IsIncreasing拡張メソッド
        #region IsIncreasing
        /// <summary>
        /// 2つのコレクションについて、現在のコレクションがより多くのデータを持っているかどうかを比較します
        /// </summary>
        /// <param name="d">DataType列挙体</param>
        /// <param name="newSource">新しいコレクション</param>
        /// <param name="oldSource">古いコレクション</param>
        /// <returns>新しいコレクションの要素数＞古いコレクションの要素数</returns>
        public static bool IsIncreasing(this DataType d,  dynamic newSource, dynamic oldSource)
        {
            //Nullチェック
            if (newSource == null || oldSource == null)
            {
                throw new NullReferenceException("newSourceまたはoldSourceがNullのため、IsIncreasingを比較できません");
            }

            //型判定
            var collectType = d.GetCollectionType();
            if(newSource.GetType() != collectType || oldSource.GetType() != collectType)
            {
                throw new ArgumentException("newSourceまたはoldSourceの型が、要求されたDataTypeに対して適切ではないため、IsIncreasingを比較できません");
            }

            //個別のチェック
            switch(d)
            {
                case DataType.Config:
                    return true;//ちょっとアレだけど
                case DataType.Counter:
                    return true;//これもちょっとアレ
                case DataType.DefaultSlotItem:
                    return false;//使わなくなったので
                case DataType.DropRecord:
                    return ((DataObject.DropRecordCollection)newSource).IsIncreasing((DataObject.DropRecordCollection)oldSource);
                case DataType.EnemyFleet:
                    return _IsIncreasing(newSource, oldSource);
                case DataType.Experience:
                    return _IsIncreasing(newSource, oldSource);
                case DataType.Material:
                    return _IsIncreasing(newSource, oldSource);
                case DataType.Query:
                    return _IsIncreasing(newSource, oldSource) || (newSource.Count == oldSource.Count);
                case DataType.Quest:
                    return ((DataObject.QuestRecord)newSource).IsIncreasing((DataObject.QuestRecord)oldSource);
                case DataType.Ranking:
                    return _IsIncreasing(newSource, oldSource) || (newSource.Count == oldSource.Count);
                case DataType.Senka:
                    var oldSenka = (List<DataObject.SenkaRecord>)oldSource;
                    var newSenka = (List<DataObject.SenkaRecord>)newSource;
                    if(!d.IsNullOrEmpty(oldSenka) && !d.IsNullOrEmpty(newSenka) && oldSenka.Count == newSenka.Count)
                    {
                        int nsenka = oldSenka.Count - 1;
                        return newSenka[nsenka].EndExp > oldSenka[nsenka].EndExp;
                    }
                    return _IsIncreasing(newSource, oldSource);
                case DataType.ExMasterShip:
                case DataType.ExMasterSlotitem:
                    return _IsIncreasing(newSource, oldSource) || (newSource.Count == oldSource.Count);
                case DataType.SortieReport:
                    return ((DataObject.SortieReportCollection)newSource).IsIncreasing((DataObject.SortieReportCollection)oldSource);
                case DataType.PracticeInfo:
                    return ((DataObject.PracticeInfoCollection)newSource).IsIncreasing((DataObject.PracticeInfoCollection)oldSource);
                default:
                    throw new NotImplementedException("要求されたDataTypeに対する、IsIncreasingが実装されていません");
            }

        }

        private static bool _IsIncreasing(dynamic newSource, dynamic oldSource)
        {
            return newSource.Count > oldSource.Count;
        }
        #endregion
    }
}
