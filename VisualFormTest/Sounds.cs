using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using HoppoAlpha.SoundLibrary;
using System.IO.Compression;

namespace VisualFormTest
{
    public class Sounds
    {
        //とりあえずアドホックに与えておく（あとで修正）
        static string exeFile = Environment.CurrentDirectory + @"\HoppoAlpha.SoundSrv.exe";
        //サーバーのプロセス
        static Process serverProcess = null;
        //最後に再生した音量
        static int lastPlayedVolume = -1;
        //ロックオブジェクト
        static object lockObject = new object();

        //中間オブジェクト
        static IPCSound midObject = null;

        static Sounds()
        {
            //クライアントのチャンネルを生成
            IpcClientChannel channel = new IpcClientChannel();
            //チャンネルを登録
            ChannelServices.RegisterChannel(channel, true);

            //リモートオブジェクトの取得
            midObject = Activator.GetObject(typeof(IPCSound), "ipc://HoppoAlphaSound/SoundData") as IPCSound;
        }

        static void CreateExeFile()
        {
            try
            {
                //既存のファイルがある場合は削除する
                File.Delete(exeFile);
                //Zipを展開する
                using(ZipArchive archive = ZipFile.OpenRead("external.zip"))
                {
                    var soundExeFile = archive.Entries.Where(e => e.FullName.EndsWith("HoppoAlpha.SoundSrv.exe"));
                    var entry = soundExeFile.FirstOrDefault();
                    if(entry != null)
                    {
                        entry.ExtractToFile(exeFile);
                    }
                }
            }
            catch(Exception ex)
            {
                LogSystem.AddLogMessage(ex.ToString());
            }
        }

        //サーバーに接続する
        public static void ConnectToServer()
        {
            try
            {
                //サーバーを起動
                using (Process p = Process.GetCurrentProcess())
                {
                    if (!File.Exists(exeFile))
                    {
                        CreateExeFile();
                    }
                    else
                    {
                        //Exeが存在する場合でもバージョンが違えば
                        var info = FileVersionInfo.GetVersionInfo(exeFile);
                        //バージョン
                        int existVersion = Convert.ToInt32(info.FileVersion.Replace(".", ""));
                        //バージョンが指定したものより低ければ書き換え
                        if (existVersion < 1010)
                        {
                            CreateExeFile();
                        }
                    }

                    serverProcess = Process.Start(exeFile, p.Id.ToString());
                }
            }
            catch(Exception ex)
            {
                LogSystem.AddLogMessage(ex.ToString());
            }
        }

        public static void DisconnectToServer()
        {
            try
            {
                if (serverProcess != null)
                {
                    serverProcess.Close();
                    serverProcess.Dispose();
                }
            }
            catch(Exception ex)
            {
                LogSystem.AddLogMessage(ex.ToString());
            }
        }


        //音を再生
        public static void PlaySounds(string playFileName)
        {
            try
            {
                lock (lockObject)
                {
                    if (serverProcess == null || serverProcess.HasExited == true)
                    {
                        ConnectToServer();
                    }
                    if (!File.Exists(playFileName)) return;
                    //音量の設定
                    int volume;
                    if (Config.SoundIsMuted) volume = 0;
                    else volume = Config.SoundVolume;
                    if (volume != lastPlayedVolume) ChangeVolume(volume);
                    midObject.SoundPlay(playFileName);
                }
            }
            catch(Exception ex)
            {
                lastPlayedVolume = -1;//暴発を防ぐために音量の設定をしなかったことにする
                LogSystem.AddLogMessage(ex.ToString());
            }
        }

        public static void PlaySounds(string playFileName, bool volumeNotChanged)
        {
            try
            {
                lock (lockObject)
                {
                    if (!volumeNotChanged) return;
                    if (serverProcess == null || serverProcess.HasExited == true)
                    {
                        ConnectToServer();
                    }
                    if (!File.Exists(playFileName)) return;
                    midObject.SoundPlay(playFileName);
                }
            }
            catch(Exception ex)
            {
                lastPlayedVolume = -1;//暴発を防ぐために音量の設定をしなかったことにする
                LogSystem.AddLogMessage(ex.ToString());
            }
        }

        //音量を設定
        public static void ChangeVolume(int volume)
        {
            try
            {
                if (volume < 0 || volume > 100) return;
                if (serverProcess == null || serverProcess.HasExited == true)
                {
                    ConnectToServer();
                }
                midObject.VolumeChange(volume);
                //音量の登録
                lastPlayedVolume = volume;
            }
            catch(Exception ex)
            {
                lastPlayedVolume = -1;//暴発を防ぐために音量の設定をしなかったことにする
                LogSystem.AddLogMessage(ex.ToString());
            }
        }
    }
}
