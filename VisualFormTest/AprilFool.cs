using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using WeifenLuo.WinFormsUI.Docking;

namespace VisualFormTest
{
    public static class AprilFool
    {
        //かもちゃん
        static string[] kamos;
        //乱数
        static Random rand;

        //コンストラクタ
        static AprilFool()
        {
            kamos = new string[]
            {
                "(╹‿ ╹.　)",
                "(╹‿ ╹.　)",
                "(╹‿ ╹.　)",
                "(╹‿ ╹.　)",
                "(╹‿ ╹.　)",
                ">∩(╹‿ ╹.　)∩< ",
                "♪L(╹‿ ╹.　)┘",
                "(人.>▽╹)☆",
                "(╹_ ╹.　)",
                "(╹_ ╹.　)",
                "(⊃‿ ⊂.　)",
                "ｶﾓ₍₍ (　ง.╹‿╹)ว ⁾⁾ｶﾓ",
                "ᕙ(　╹ ‿ ╹.　)ᕗ",
                "(≖‿ ≖.╬╬)",
                "(「.╹‿ ╹)「ｼｬｰ",
                "ｶﾓｫ…┌(╹‿ ╹.　┌ )┐",
                "(╹‸ ╹. )",
                "(╹‿ ╹.⊂彡☆))‿╹) ｶﾓｰﾝ",
                "(「.╹‿ ╹)「ｼｬｰ",
                "┏(　╹‿ ╹.)┛",
                "♪～(╹ε ╹.　) ",
                "ᕙ(　╹ ‿ ╹.　)ᕗ",
                "_(╹‿ ╹. 」∠)_",
                "┗(⌒)(╹‿ ╹.　)(⌒)┛",
                "┏(　╹‿ ╹.)┛",
                "( ･´ｰ･｀. )",
                "⊂(╹‿ ╹.⊂⌒｀つ≡≡≡",
            };
            rand = new Random(Environment.TickCount);
        }

        //エイプリルフールかどうか
        public static bool IsAprilFool
        {
            get
            {
                if (Config.NazonoOmajonai == "kamokamo") return true;
                DateTime today = DateTime.Today;
                if (today.Month == 4 && today.Day == 1) return true;
                else return false;
            }
        }

        public static void DoAprilFoolWindow(DockPanel panel)
        {
            foreach(DockContent c in panel.Contents)
            {
                var form = c.FindForm();
                form.Text = kamos[rand.Next(0, kamos.Length)];
            }
        }

        public static void DoAprilSound()
        {
            string baseDirectory = Environment.CurrentDirectory + "\\sounds";
            string[] targetFiles = new string[]{"k1.mp3", "k2.mp3", "k3.mp3"};

            foreach(var s in targetFiles)
            {
                string file = baseDirectory + "\\" + s;

                if(!File.Exists(file))
                {
                    try
                    {
                        using(var archive = ZipFile.OpenRead("external.zip"))
                        {
                            var zipSource = archive.Entries.Where(e => e.FullName.EndsWith(s)).FirstOrDefault();
                            if(zipSource != null)
                            {
                                zipSource.ExtractToFile(file);
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        LogSystem.AddLogMessage(ex.ToString());
                    }
                }

                //Zip展開後ファイルが存在しているならば
                if(File.Exists(file))
                {
                    switch(s)
                    {
                        case "k1.mp3"://遠征
                            Config.SoundMissionFileName = file;
                            break;
                        case "k2.mp3"://大破
                            Config.SoundDamageFileName = file;
                            Config.SoundDamageSortieFileName = file;
                            break;
                        case "k3.mp3"://入渠
                            Config.SoundNdockFileName = file;
                            break;
                    }
                }
            }
        }
    }
}
