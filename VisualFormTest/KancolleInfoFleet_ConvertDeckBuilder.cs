using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Codeplex.Data;

namespace VisualFormTest
{
    public partial class KancolleInfoFleet_ConvertDeckBuilder : Form
    {
        CheckBox[] cbs;

        public KancolleInfoFleet_ConvertDeckBuilder(int deckMax, int selectedDeck)
        {
            InitializeComponent();

            cbs = new CheckBox[]
            {
                checkBox1, checkBox2, checkBox3, checkBox4
            };

            for(int i=0; i<Math.Min(deckMax, cbs.Length); i++)
            {
                cbs[i].Enabled = true;
            }
            if(selectedDeck >= 0 && selectedDeck < cbs.Length)
            {
                cbs[selectedDeck].Checked = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(linkLabel1.Text);
        }

        private void button_selectall_Click(object sender, EventArgs e)
        {
            bool isAllSelected = checkBox1.Checked && checkBox2.Checked && checkBox3.Checked && checkBox4.Checked;
            foreach (var x in cbs) x.Checked = !isAllSelected;
        }

        private async void button_convert_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked || checkBox2.Checked || checkBox3.Checked || checkBox4.Checked) { }
            else return;
            //ボタンを無効にする
            button_convert.Enabled = false;
            //どれか選択されている場合
            int end = cbs.Length;
            for(int i=cbs.Length-1; i>=0; i--)
            {
                if(cbs[i].Checked)
                {
                    end = i;
                    break;
                }
            }
            List<bool> checks = new List<bool>();
            for(int i=0; i<=end; i++)
            {
                checks.Add(cbs[i].Checked);
            }
            //変換後の文字の取得
            string convert = KancolleInfoFleet.context_ConvertDeckBuilderLogic(checks);
            if (convert != null)
            {
                textBox1.Text = convert;
                //リンクラベルに変換
                string encode = Uri.EscapeUriString(convert);
                string url = @"http://www.kancolle-calc.net/deckbuilder.html?predeck=" + encode;
                //2艦隊以上出力する場合は短縮URLに変換
                if(checks.Where(x => x).Count() >= 2)
                {
                    await Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                //is.gdを使用
                                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://is.gd/create.php?format=simple&url="+url);
                                httpWebRequest.Method = "POST";

                                //レスポンスを読む
                                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                string responseText = null;
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    responseText = streamReader.ReadToEnd();
                                }

                                //is.gdのレスポンス
                                if(responseText != null)
                                {
                                    if(responseText.Contains("error"))
                                    {
                                        MessageBox.Show(responseText);
                                    }
                                    else
                                    {
                                        url = responseText;
                                    }
                                }
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        });
                }
                linkLabel2.Text = url;
            }
            //ボタンを有効に戻す
            button_convert.Enabled = true;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLabel2.Text != "Link")
            {
                Process.Start(linkLabel2.Text);
            }
        }

    }
}
