using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoppoAlpha.DataLibrary;
using HoppoAlpha.DataLibrary.DataObject;

namespace VisualFormTest
{
    public static class DataLibraryCenter
    {
        //ロックオブジェクト
        public static object LockObject { get; set; }

        static DataLibraryCenter()
        {
            LockObject = new object();
        }

        //シリアルの全バックアップ
        public static void BackupSerials(bool onclosing)
        {
            //FormClosingとタイマーが競合しないようにロック
            lock (LockObject)
            {
                //データの記録
                HistoricalData.SaveAll();
                //カウンターの記録
                KancolleInfoCounter.Save();
                //任務の保存
                APIReqQuest.Save();
                //ランキングの記録
                APIReqRanking.Save();
                //設定の保存
                Config.Save();
                //クエリの保存
                KancolleInfoUnitList.SaveQueries();
                //マスターデータの保存
                if (APIMaster.MstShips != null)
                {
                    //IDを昇順ソート
                    var mstships = new ExMasterShipCollection(true);
                    foreach(var m in APIMaster.MstShips.OrderBy(x => x.Key))
                    {
                        mstships[m.Key] = m.Value;
                    }
                    //保存するアクション
                    var saveResult = new Files.FileOperationResult();
                    var saveAction = new Action(() =>
                        {
                            saveResult = Files.Save(ExMasterShipCollection.FilePath, DataType.ExMasterShip, mstships);
                            LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.ExMasterShip, saveResult, true);
                        });
                    //保存
                    saveAction();
                    if (onclosing && !saveResult.IsSuccess)
                    {
                        while (true)
                        {
                            var result = System.Windows.Forms.MessageBox.Show("船のマスターデータの保存に失敗しました\n" + saveResult.ErrorReason, "保存",
                                System.Windows.Forms.MessageBoxButtons.RetryCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                            //再試行
                            if (result == System.Windows.Forms.DialogResult.Retry)
                            {
                                saveAction();
                                if (saveResult.IsSuccess) break;
                            }
                            else if (result == System.Windows.Forms.DialogResult.Cancel)
                            {
                                break;
                            }

                        }
                    }
                }
                //装備のマスターデータの保存
                if(APIMaster.MstSlotitems != null)
                {
                    //IDを昇順ソート
                    var mstslotitems = new ExMasterSlotitemCollection(true);
                    foreach(var m in APIMaster.MstSlotitems.OrderBy(x => x.Key))
                    {
                        mstslotitems[m.Key] = m.Value;
                    }
                    //保存するアクション
                    var saveResult = new Files.FileOperationResult();
                    var saveAction = new Action(() =>
                    {
                        saveResult = Files.Save(ExMasterSlotitemCollection.FilePath, DataType.ExMasterSlotitem, mstslotitems);
                        LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.ExMasterSlotitem, saveResult, true);
                    });
                    saveAction();
                    if (onclosing && !saveResult.IsSuccess)
                    {
                        while (true)
                        {
                            var result = System.Windows.Forms.MessageBox.Show("装備のマスターデータの保存に失敗しました\n" + saveResult.ErrorReason, "保存",
                                System.Windows.Forms.MessageBoxButtons.RetryCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                            //再試行
                            if (result == System.Windows.Forms.DialogResult.Retry)
                            {
                                saveAction();
                                if (saveResult.IsSuccess) break;
                            }
                            else if (result == System.Windows.Forms.DialogResult.Cancel)
                            {
                                break;
                            }
                        }
                    }
                }

                //敵艦DBの保存
                EnemyFleetDataBase.Save();
                //ドロップログの保存
                DropDataBase.Save();
                //出撃報告書の保存
                SortieReportDataBase.Save(onclosing);
                //演習情報の保存
                PracticeInfoDataBase.Save();

                //検証DBのエラーログ
                KancolleVerifyDb.KCVDBObjects.SaveDbErrorLog();
            }
        }

        //上の非同期メソッド
        public static async Task BackupSerialsAsync(bool onclosing)
        {
            await Task.Factory.StartNew(() =>
                {
                    BackupSerials(onclosing);
                });
        }
    }
}
