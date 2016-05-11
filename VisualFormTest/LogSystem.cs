using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HoppoAlpha.DataLibrary;

namespace VisualFormTest
{
    //システムログを表すクラス
    public static class LogSystem
    {
        public static List<string> LogMessage { get; private set; }

        public const string FileName = "SystemLog.txt";

        static LogSystem()
        {
            LogMessage = new List<string>();
        }

        public static void AddLogMessage(DataType dataType, Files.FileOperationResult fileResult, bool isSave)
        {
            //自動バックアップを開始します
            //[読込]config : 成功 (2015/6/1 13:12:05)
            //[保存]config : 成功 (2015/6/1 13:12:07)
            //[保存]Material : 失敗 ErrorReason (2015/6/1 13:12:75)
            string mode = isSave ? "保存" : "読込";
            string issuccess = fileResult.IsSuccess ? "成功" : "失敗";
            string str = string.Format("[{0}]{1} : {2} {3} ({4})",
                mode, dataType.ToString(), issuccess,
                fileResult.ErrorReason, DateTime.Now.ToString());
            LogMessage.Add(str);
        }

        public static void AddLogMessage(string str)
        {
            string item = string.Format("{0} ({1})", str, DateTime.Now.ToString());
            LogMessage.Add(item);
        }

        public static void SaveSystemLog()
        {
            Encoding enc = Encoding.GetEncoding("shift_jis");

            if(File.Exists(FileName))
            {
                File.AppendAllLines(FileName, LogMessage, enc);
            }
            else
            {
                File.WriteAllLines(FileName, LogMessage, enc);
            }

        }
    }
}
