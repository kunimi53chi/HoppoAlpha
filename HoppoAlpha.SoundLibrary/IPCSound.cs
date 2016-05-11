using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoppoAlpha.SoundLibrary
{
    /// <summary>
    /// SEを再生する際のプロセス間通信のためのオブジェクト
    /// </summary>
    public class IPCSound : MarshalByRefObject
    {
        /// <summary>
        /// SE再生の情報を引き渡すイベント引数クラス
        /// </summary>
        public class SoundPlayEventArgs : EventArgs
        {
            /// <summary>
            /// SEのファイル名
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// 再生するファイルを指定します
            /// </summary>
            /// <param name="filename">ファイルのパス</param>
            public SoundPlayEventArgs(string filename)
            {
                this.FileName = filename;
            }
        }

        /// <summary>
        /// SEの音量を調整する際のプロセス間通信のためのオブジェクト
        /// </summary>
        public class SoundVolumeEventArgs : EventArgs
        {
            /// <summary>
            /// 音量
            /// </summary>
            public int Volume { get; set; }

            /// <summary>
            /// 音量を指定します
            /// </summary>
            /// <param name="volume">音量（0～100）</param>
            public SoundVolumeEventArgs(int volume)
            {
                this.Volume = volume;
            }
        }

        public delegate void SoundPlayEventHandler(SoundPlayEventArgs e);
        public event SoundPlayEventHandler SoundPlayed;

        public delegate void SoundVolumeEventHandler(SoundVolumeEventArgs e);
        public event SoundVolumeEventHandler SoundVolumeChanged;

        /// <summary>
        /// 自動的に切断されるのを回避する
        /// </summary>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// サウンドを再生します
        /// </summary>
        /// <param name="filename">ファイル名を指定</param>
        public void SoundPlay(string filename)
        {
            if (SoundPlayed != null)
            {
                SoundPlayed(new SoundPlayEventArgs(filename));
            }
        }

        /// <summary>
        /// 音量を変更します
        /// </summary>
        /// <param name="volume">音量（0～100）</param>
        public void VolumeChange(int volume)
        {
            if(SoundVolumeChanged != null)
            {
                SoundVolumeChanged(new SoundVolumeEventArgs(volume));
            }
        }
    }
}
