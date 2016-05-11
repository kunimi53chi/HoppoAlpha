using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    var saveResult = HoppoAlpha.DataLibrary.Files.Save(
                        HoppoAlpha.DataLibrary.DataObject.ExMasterShipCollection.FilePath,
                        HoppoAlpha.DataLibrary.DataType.ExMasterShip, APIMaster.MstShips);
                    LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.ExMasterShip, saveResult, true);
                    if (onclosing && !saveResult.IsSuccess)
                    {
                        while (true)
                        {
                            var result = System.Windows.Forms.MessageBox.Show("船のマスターデータの保存に失敗しました\n" + saveResult.ErrorReason, "保存",
                                System.Windows.Forms.MessageBoxButtons.RetryCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                            //再試行
                            if (result == System.Windows.Forms.DialogResult.Retry)
                            {
                                saveResult = HoppoAlpha.DataLibrary.Files.Save(
                                    HoppoAlpha.DataLibrary.DataObject.ExMasterShipCollection.FilePath,
                                    HoppoAlpha.DataLibrary.DataType.ExMasterShip, APIMaster.MstShips);
                                LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.ExMasterShip, saveResult, true);
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
                    var saveResult = HoppoAlpha.DataLibrary.Files.Save(
                        HoppoAlpha.DataLibrary.DataObject.ExMasterSlotitemCollection.FilePath,
                        HoppoAlpha.DataLibrary.DataType.ExMasterSlotitem, APIMaster.MstSlotitems);
                    LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.ExMasterSlotitem, saveResult, true);
                    if (onclosing && !saveResult.IsSuccess)
                    {
                        while (true)
                        {
                            var result = System.Windows.Forms.MessageBox.Show("装備のマスターデータの保存に失敗しました\n" + saveResult.ErrorReason, "保存",
                                System.Windows.Forms.MessageBoxButtons.RetryCancel, System.Windows.Forms.MessageBoxIcon.Exclamation);
                            //再試行
                            if (result == System.Windows.Forms.DialogResult.Retry)
                            {
                                saveResult = HoppoAlpha.DataLibrary.Files.Save(
                                        HoppoAlpha.DataLibrary.DataObject.ExMasterSlotitemCollection.FilePath,
                                        HoppoAlpha.DataLibrary.DataType.ExMasterSlotitem, APIMaster.MstSlotitems);
                                LogSystem.AddLogMessage(HoppoAlpha.DataLibrary.DataType.ExMasterSlotitem, saveResult, true);
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
