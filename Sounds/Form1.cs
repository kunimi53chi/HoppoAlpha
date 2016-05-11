using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using HoppoAlpha.SoundLibrary;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

namespace HoppoAlpha.SoundSrv
{
    public partial class Form1 : Form
    {
        //IPCのオブジェクト
        IPCSound midObject = null;
        //ホストのプロセス
        Process hostProcess = null;

        //MediaPlayer
        private readonly dynamic _mediaPlayer = null;

        //音量のキャッシュ
        private int Volume = -1;

        //IPC : サーバーとして登録
        public Form1(string[] args)
        {
            InitializeComponent();

            //サーバーのチャンネル登録
            IpcServerChannel channel = new IpcServerChannel("HoppoAlphaSound");

            //チャンネルの登録
            ChannelServices.RegisterChannel(channel, true);

            midObject = new IPCSound();
            midObject.SoundPlayed += new IPCSound.SoundPlayEventHandler(midObject_SoundPlayed);
            midObject.SoundVolumeChanged += new IPCSound.SoundVolumeEventHandler(midObject_SoundVolumeChanged);

            RemotingServices.Marshal(midObject, "SoundData", typeof(IPCSound));

            //ホストプロセスの取得
            if (args.Length <= 0) return;
            int hostId = Convert.ToInt32(args[0]);
            hostProcess = Process.GetProcessById(hostId);
            hostProcess.SynchronizingObject = this;
            hostProcess.Exited += hostProcess_Exited;
            hostProcess.EnableRaisingEvents = true;

            //WMPの起動
            _mediaPlayer = Activator.CreateInstance(Type.GetTypeFromProgID("WMPlayer.OCX.7"));
            _mediaPlayer.settings.autoStart = true;//自動再生をTrueにする
        }

        void hostProcess_Exited(object sender, EventArgs e)
        {
            //ホストが終了したときにサーバーも終了
            hostProcess.Close();
            hostProcess.Dispose();
            Environment.Exit(0);
        }

        void midObject_SoundPlayed(IPCSound.SoundPlayEventArgs e)
        {
            if (Volume != -1 && _mediaPlayer.settings.volume != Volume) _mediaPlayer.settings.volume = Volume;
            _mediaPlayer.URL = e.FileName;
        }

        void midObject_SoundVolumeChanged(IPCSound.SoundVolumeEventArgs e)
        {
            _mediaPlayer.settings.volume = (int)e.Volume;
            Volume = (int)e.Volume;
        }
    }
}
