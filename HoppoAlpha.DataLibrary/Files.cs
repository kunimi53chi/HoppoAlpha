using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ProtoBuf;

namespace HoppoAlpha.DataLibrary
{
    /// <summary>
    /// ファイル操作を行うクラス
    /// </summary>
    public static class Files
    {
        //内部クラス
        #region 内部クラス
        /// <summary>
        /// ファイル操作の帰り値用のクラス
        /// </summary>
        public class FileOperationResult
        {
            /// <summary>
            /// メソッドが成功したかどうかのフラグ
            /// </summary>
            public bool IsSuccess { get; internal set; }
            /// <summary>
            /// エラーが発生した場合は理由が記されます
            /// </summary>
            public string ErrorReason { get; internal set; }
            /// <summary>
            /// 読み込みメソッドの場合はここにインスタンスが格納されます
            /// </summary>
            public object Instance { get; internal set; }

            /// <summary>
            /// 読み込み結果を表示します（デバッグ用）
            /// </summary>
            public string DisplayConsole()
            {
                return string.Format("IsSuccess : {0}, ErrorReason : {1}", this.IsSuccess, this.ErrorReason);
            }
        }
        #endregion


        //ファイル操作をロックするためのオブジェクト
        public static object FileLockObject { get; set; }

        static Files()
        {
            FileLockObject = new object();
        }

        //読み込み
        #region Load
        /// <summary>
        /// ファイルから読み込み逆シリアル化します
        /// 読み込みに失敗した場合はinstanceにはデフォルトの値が出力されます
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dataType">DataType列挙体</param>
        /// <param name="instance">出力するインスタンス</param>
        /// <returns>読み込み結果</returns>
        public static FileOperationResult TryLoad(string filePath, DataType dataType)
        {
            lock (FileLockObject)
            {
                //結果の格納用
                FileOperationResult result = new FileOperationResult();

                Type actType = dataType.GetCollectionType();
                //ファイルが存在しない場合
                if (!File.Exists(filePath))
                {
                    result.IsSuccess = false;
                    result.ErrorReason = "File not found.";
                    result.Instance = Activator.CreateInstance(dataType.GetCollectionType());
                    return result;
                }
                //読み込み
                FileStream fs = null;
                try
                {
                    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    TryLoad_Internal(fs, dataType, ref result);
                    //型のチェック
                    if(result.Instance.GetType() != actType)
                    {
                        throw new TypeLoadException(string.Format("Type {0} is unexpected.", result.Instance.GetType()));
                        throw new FormatException("Format is unexpected.");
                    }
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.ErrorReason = ex.ToString();
                    result.Instance = Activator.CreateInstance(actType);
                    return result;
                }
                finally
                {
                    if (fs != null) fs.Close();
                }
                //IsNullOrEmptyのチェック
                if (dataType.IsNullOrEmpty(result.Instance))
                {
                    result.IsSuccess = false;
                    result.ErrorReason = "The instance seems to be null or empty.";
                    result.Instance = Activator.CreateInstance(actType);
                    return result;
                }
                else
                {
                    result.IsSuccess = true;
                    result.ErrorReason = "";
                    return result;
                }
            }
        }

        private static void TryLoad_Internal(Stream stream, DataType dataType, ref FileOperationResult result)
        {
            switch (dataType)
            {
                case DataType.Config:
                    result.Instance = Serializer.Deserialize<DataObject.ConfigSerializeItem>(stream);
                    break;
                case DataType.Counter:
                    result.Instance = Serializer.Deserialize<List<DataObject.CounterItem>>(stream);
                    break;
                case DataType.DefaultSlotItem:
                    break;
                case DataType.DropRecord:
                    result.Instance = Serializer.Deserialize<DataObject.DropRecordCollection>(stream);
                    break;
                case DataType.EnemyFleet:
                    result.Instance = Serializer.Deserialize<Dictionary<int, DataObject.EnemyFleetRecord>>(stream);
                    break;
                case DataType.Experience:
                    result.Instance = Serializer.Deserialize<List<DataObject.ExpRecord>>(stream);
                    break;
                case DataType.Material:
                    result.Instance = Serializer.Deserialize<List<DataObject.MaterialRecord>>(stream);
                    break;
                case DataType.Query:
                    result.Instance = Serializer.Deserialize<List<DataObject.UnitQuery>>(stream);
                    break;
                case DataType.Quest:
                    result.Instance = Serializer.Deserialize<DataObject.QuestRecord>(stream);
                    break;
                case DataType.Ranking:
                    result.Instance = Serializer.Deserialize<SortedDictionary<int, RawApi.ApiReqRanking.ApiRanking.ApiList>>(stream);
                    break;
                case DataType.Senka:
                    result.Instance = Serializer.Deserialize<List<DataObject.SenkaRecord>>(stream);
                    break;
                case DataType.ExMasterShip:
                    result.Instance = new DataObject.ExMasterShipCollection(stream);
                    break;
                case DataType.ExMasterSlotitem:
                    result.Instance = new DataObject.ExMasterSlotitemCollection(stream);
                    break;
                case DataType.SortieReport:
                    result.Instance = Serializer.Deserialize<DataObject.SortieReportCollection>(stream);
                    break;
                case DataType.PracticeInfo:
                    result.Instance = Serializer.Deserialize<DataObject.PracticeInfoCollection>(stream);
                    break;
                default:
                    throw new NotImplementedException("要求されたDataTypeに対する、TryLoad_Internalが実装されていません");
            }
        }
        #endregion

        //保存
        #region Save
        /// <summary>
        /// ファイルにシリアル化します
        /// </summary>
        /// <param name="filePath">ファイル名</param>
        /// <param name="dataType">DataType列挙体</param>
        /// <param name="instance">インスタンス</param>
        /// <param name="checkIncreasing">同名ファイルが存在したとき、前より大きくなっていた場合のみ保存するオプション（デフォルトでtrue）</param>
        /// <returns>保存結果</returns>
        public static FileOperationResult Save(string filePath, DataType dataType, object instance, bool checkIncreasing = true)
        {
            FileOperationResult result = new FileOperationResult();

            //型のチェック
            var collectType = dataType.GetCollectionType();
            if (collectType != instance.GetType())
            {
                throw new ArgumentException("要求されたDataTypeに対するinstanceの型が正しくないため、ファイルを保存することができません");
            }

            //ファイルが存在しない場合 → そのまま保存
            if(!File.Exists(filePath))
            {
                return Save_internal(filePath, dataType, instance);//ロック区間
            }

            //ファイルが存在する場合
            var oldload = TryLoad(filePath, dataType);//ロック区間
            if(!oldload.IsSuccess)
            {
                //古いファイルが読めない場合　→ そのまま上書き保存
                return Save_internal(filePath, dataType, instance);//ロック区間                
            }

            //  →古いファイルが読める場合　または　大きくなっているのをチェックしない場合
            //　→古いファイルより大きくなっている場合
            if(!checkIncreasing || dataType.IsIncreasing(instance, oldload.Instance))
            {
                string backup = filePath + ".bak";
                try
                {
                    //.bakファイルが存在した場合はMoveで例外が発生するので対策
                    if(File.Exists(backup))
                    {
                        File.Delete(backup);
                    }
                    //→古いファイルを.bakに置き換える
                    File.Move(filePath, backup);
                    //→新しいファイルを保存する
                    result = Save_internal(filePath, dataType, instance);//ロック区間
                    return result;
                }
                catch(Exception ex)
                {
                    result.IsSuccess = false;
                    result.ErrorReason = ex.ToString();
                    return result;
                }
                finally
                {
                    //→[成功時のみ]古い.bakを削除する
                    if (result.IsSuccess) File.Delete(backup);
                    //→[失敗時のみ]古い.bakを戻す
                    else
                    {
                       if(File.Exists(backup)) File.Move(backup, filePath);
                    }
                    //成功時
                }
            }
            // 古いファイルより大きくなっていない場合
            else
            {
                result.IsSuccess = false;
                result.ErrorReason = "The new-file's data is not bigger than the old-file's";
                return result;
            }
        }

        
        private static FileOperationResult Save_internal(string filePath, DataType dataType, object instance)
        {
            FileOperationResult result = new FileOperationResult();

            //NullOrEmptyの場合
            if(dataType.IsNullOrEmpty(instance))
            {
                result.IsSuccess = false;
                result.ErrorReason = "The instance seems to be null or empty.";
                return result;
            }

            //ロックして保存
            lock(FileLockObject)
            {
                MemoryStream ms = null;
                try
                {
                    ms = new MemoryStream();

                    //いきなりFileStreamに格納すると失敗した際に0KBで上書きされるので、いったんMemoryStreamに落としこむ
                    switch(dataType)
                    {
                        case DataType.Config:
                            Serializer.Serialize<DataObject.ConfigSerializeItem>(ms, (DataObject.ConfigSerializeItem)instance);
                            break;
                        case DataType.Counter:
                            Serializer.Serialize<List<DataObject.CounterItem>>(ms, (List<DataObject.CounterItem>)instance);
                            break;
                        case DataType.DefaultSlotItem:
                            break;
                        case DataType.DropRecord:
                            Serializer.Serialize<DataObject.DropRecordCollection>(ms, (DataObject.DropRecordCollection)instance);
                            break;
                        case DataType.EnemyFleet:
                            Serializer.Serialize<Dictionary<int, DataObject.EnemyFleetRecord>>(ms, (Dictionary<int, DataObject.EnemyFleetRecord>)instance);
                            break;
                        case DataType.Experience:
                            Serializer.Serialize<List<DataObject.ExpRecord>>(ms, (List<DataObject.ExpRecord>)instance);
                            break;
                        case DataType.Material:
                            Serializer.Serialize<List<DataObject.MaterialRecord>>(ms, (List<DataObject.MaterialRecord>)instance);
                            break;
                        case DataType.Query:
                            Serializer.Serialize<List<DataObject.UnitQuery>>(ms, (List<DataObject.UnitQuery>)instance);
                            break;
                        case DataType.Quest:
                            Serializer.Serialize<DataObject.QuestRecord>(ms, (DataObject.QuestRecord)instance);
                            break;
                        case DataType.Ranking:
                            Serializer.Serialize<SortedDictionary<int, RawApi.ApiReqRanking.ApiRanking.ApiList>>(ms, (SortedDictionary<int, RawApi.ApiReqRanking.ApiRanking.ApiList>)instance);
                            break;
                        case DataType.Senka:
                            Serializer.Serialize<List<DataObject.SenkaRecord>>(ms, (List<DataObject.SenkaRecord>)instance);
                            break;
                        case DataType.ExMasterShip:
                            ((DataObject.ExMasterShipCollection)instance).Save(ms);
                            break;
                        case DataType.ExMasterSlotitem:
                            ((DataObject.ExMasterSlotitemCollection)instance).Save(ms);
                            break;
                        case DataType.SortieReport:
                            Serializer.Serialize<DataObject.SortieReportCollection>(ms, (DataObject.SortieReportCollection)instance);
                            break;
                        case DataType.PracticeInfo:
                            Serializer.Serialize<DataObject.PracticeInfoCollection>(ms, (DataObject.PracticeInfoCollection)instance);
                            break;
                        default:
                            throw new NotImplementedException("要求されたDataTypeに対する、Save_internalが実装されていません");
                    }

                    //ベリファイ
                    ms.Position = 0;//大事
                    FileOperationResult verifyResult = new FileOperationResult();//Instance以外は更新されない
                    TryLoad_Internal(ms, dataType, ref verifyResult);
                    if(dataType.IsNullOrEmpty(verifyResult.Instance))
                    {
                        result.IsSuccess = false;
                        result.ErrorReason = "Verify failed.";
                        return result;
                    }

                    //書き込み
                    if(ms.Capacity > 0)
                    {
                        File.WriteAllBytes(filePath, ms.ToArray());
                    }
                }
                catch (Exception ex)
                {
                    result.IsSuccess = false;
                    result.ErrorReason = ex.ToString();
                    return result;
                }
                finally
                {
                    if (ms != null) ms.Close();
                }
                //正常に終了した場合
                result.IsSuccess = true;
                result.ErrorReason = "";
                return result;
            }
        }
        #endregion

    }
}
