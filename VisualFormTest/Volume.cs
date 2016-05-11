using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CoreAudioApi;
using CoreAudioApi.Interfaces;

namespace VisualFormTest
{
    /*
     * 提督業は忙しいからのだいたいコピペ
     */

    public class Volume
    {
        private SimpleAudioVolume simpleAudioVolume;

        #region IsMute 変更通知プロパティ

        private bool _IsMute;

        public bool IsMute
        {
            get { return this._IsMute; }
            private set
            {
                if (this._IsMute != value)
                {
                    this._IsMute = value;
                }
            }
        }

        #endregion

        #region Value 変更通知プロパティ

        private int _Value;

        public int Value
        {
            get { return this._Value; }
            private set
            {
                if (this._Value != value)
                {
                    this._Value = value;
                }
            }
        }

        #endregion


        private Volume() { }

        public static Volume GetInstance()
        {
            var volume = new Volume();
            var processId = Process.GetCurrentProcess().Id;

            var devenum = new MMDeviceEnumerator();
            var device = devenum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);

            for (var i = 0; i < device.AudioSessionManager.Sessions.Count; i++)
            {
                var session = device.AudioSessionManager.Sessions[i];
                if (session.ProcessID == processId)
                {
                    volume.simpleAudioVolume = session.SimpleAudioVolume;
                    volume.IsMute = session.SimpleAudioVolume.Mute;
                    volume.Value = (int)(session.SimpleAudioVolume.MasterVolume * 100);
                    return volume;
                }
            }

            throw new Exception("Session is not found.");
        }

        public void ToggleMute()
        {
            this.simpleAudioVolume.Mute = !this.simpleAudioVolume.Mute;
            this.IsMute = this.simpleAudioVolume.Mute;
        }

        public void SetVolume(int volume)
        {
            this.simpleAudioVolume.MasterVolume = volume / 100.0f;
            this.Value = (int)(this.simpleAudioVolume.MasterVolume * 100);
        }

    }
}
