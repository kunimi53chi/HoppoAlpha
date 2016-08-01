using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HoppoAlpha.DataLibrary.RawApi.ApiReqRanking;

namespace VisualFormTest.UserControls
{
    public partial class TabSenka_ForAnalyzeViewCalc : Form
    {
        private Form1 form1;

        public TabSenka_ForAnalyzeViewCalc(Form1 form1)
        {
            InitializeComponent();

            this.form1 = form1;
        }

        private void TabSenka_ForAnalyzeViewCalc_Load(object sender, EventArgs e)
        {
            //提督経験値の計算機
            if(HistoricalData.LogSenka != null && HistoricalData.LogSenka.Count > 0 && APIPort.Basic != null)
            {
                var data = HistoricalData.LogSenka[HistoricalData.LogSenka.Count - 1];
                textBox_calc_exp.Text = (APIPort.Basic.api_experience - data.StartExp).ToString();
            }
            //(1)の計算方法
            switch(Config.SenkaCalcForAnalyzeFirstMode)
            {
                case 0: radioButton_1_plus.Checked = true; break;
                case 1: radioButton_2_minus.Checked = true; break;
                case 2: radioButton_3_divide.Checked = true; break;
                case 3: radioButton_4_mod.Checked = true; break;
                default: radioButton_1_plus.Checked = true; break;
            }
            radioButton_1_plus.CheckedChanged += new EventHandler(RadioButtonCommon_CheckedChanged);
            radioButton_2_minus.CheckedChanged += new EventHandler(RadioButtonCommon_CheckedChanged);
            radioButton_3_divide.CheckedChanged += new EventHandler(RadioButtonCommon_CheckedChanged);
            radioButton_4_mod.CheckedChanged += new EventHandler(RadioButtonCommon_CheckedChanged);

            #region dataGridView設定
            dataGridView1.AutoGenerateColumns = false;
            //表示用カラム
            var textColumnRank = new DataGridViewTextBoxColumn();
            textColumnRank.Width = 50;
            textColumnRank.DataPropertyName = "Rank";            
            textColumnRank.HeaderText = "順位";
            textColumnRank.ReadOnly = true;

            var textColumnName = new DataGridViewTextBoxColumn();
            textColumnName.Width = 90;
            textColumnName.DataPropertyName = "Name";
            textColumnName.HeaderText = "提督名";
            textColumnName.ReadOnly = true;

            var textMeasuredValue = new DataGridViewTextBoxColumn();
            textMeasuredValue.DataPropertyName = "MeasuredValue";
            textMeasuredValue.Width = 75;
            textMeasuredValue.HeaderText = "測定値";
            textMeasuredValue.ReadOnly = true;

            var textViewValue = new DataGridViewTextBoxColumn();
            textViewValue.DataPropertyName = "ViewValue";
            textViewValue.HeaderText = "表示値";
            textViewValue.Width = 70;

            var checkIsSet = new DataGridViewCheckBoxColumn();
            checkIsSet.Width = 35;
            checkIsSet.DataPropertyName = "IsSet";
            checkIsSet.HeaderText = "入力済";

            dataGridView1.Columns.Add(textColumnRank);
            dataGridView1.Columns.Add(textColumnName);
            dataGridView1.Columns.Add(textMeasuredValue);
            dataGridView1.Columns.Add(textViewValue);
            dataGridView1.Columns.Add(checkIsSet);
            #endregion

            LoadRanking();
       }

        private void LoadRanking()
        {
            //セクションが異なる場合は読み込まない
            if (APIPort.Basic == null) return;
            if(APIReqRanking.GetFileName(DateTime.Now) != APIReqRanking.LastSavedFileName) return;
            if(APIReqRanking.Rankings == null || APIReqRanking.Rankings.Count == 0) return;

            var bind = APIReqRanking.Rankings.ToDatatable();
            dataGridView1.DataSource = bind;
        }


        private void AutoValueSet(int radioMode, bool isMult, int multDivisor, int startRank, int endRank)
        {
            foreach(var row in dataGridView1.Rows.OfType<DataGridViewRow>())
            {
                int rank;
                if (row.Cells[0].Value == null || !int.TryParse(row.Cells[0].Value.ToString().Replace("位", ""), out rank)) continue;
                if (rank < startRank || rank > endRank) continue;

                int measure;
                if(row.Cells[2].Value == null || !int.TryParse(row.Cells[2].Value.ToString(), out measure)) continue;

                //(1)の計算
                int val;
                switch(radioMode)
                {
                    case 0://足し算
                        val = measure + rank;
                        break;
                    case 1://引き算
                        val = Math.Max(measure - rank, 0);
                        break;
                    case 2://割り算
                        val = measure / rank;
                        break;
                    case 3://剰余
                        val = measure % rank;
                        break;
                    default:
                        val = 0;
                        break;
                }

                //(2)の計算
                if(isMult)
                {
                    //掛け算の場合
                    val = val * multDivisor;
                }
                else
                {
                    //割り算の場合
                    val = val / multDivisor;
                }

                //値の更新
                row.Cells[3].Value = val;
                if (val > 0) row.Cells[4].Value = true;
                else row.Cells[4].Value = false;
            }
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //3列目と4列目変更
            if (e.ColumnIndex != 3 && e.ColumnIndex != 4) return;
            //新しい行のセルでなく、セルの内容が変更されている時だけ検証する
            if (e.RowIndex == dataGridView1.NewRowIndex || !dataGridView1.IsCurrentCellDirty) return;
            //3列目の値のフォーマット
            if(e.ColumnIndex == 3)
            {
                int inttry;
                if (e.FormattedValue.ToString() == "")
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "値が入力されていません";
                    dataGridView1.CancelEdit();
                    e.Cancel = true;
                }
                else if(!int.TryParse(e.FormattedValue.ToString(), out inttry))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "数値以外が入力されています";
                    dataGridView1.CancelEdit();
                    e.Cancel = true;
                }
                else if(inttry < 0)
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "正の値を入力してください";
                    dataGridView1.CancelEdit();
                    e.Cancel = true;
                }
            }
        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].ErrorText = null;

            if (dataGridView1.Rows[e.RowIndex].Cells[3].Value == null) return;

            var setval = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            int intval;
            if (!int.TryParse(setval, out intval)) return;

            if (intval != 0) dataGridView1.Rows[e.RowIndex].Cells[4].Value = true;
            else dataGridView1.Rows[e.RowIndex].Cells[4].Value = false;
        }

        private void ValueSetCommonButton_Click(object sender, EventArgs e)
        {
            int firstmode; 
            string firstmode_str;
            //(1)の計算のモード
            if (radioButton_1_plus.Checked)
            {
                firstmode = 0;
                firstmode_str = radioButton_1_plus.Text;
            }
            else if (radioButton_2_minus.Checked) 
            {
                firstmode = 1;
                firstmode_str = radioButton_2_minus.Text;
            }
            else if (radioButton_3_divide.Checked) 
            {
                firstmode = 2;
                firstmode_str = radioButton_3_divide.Text;
            }
            else if (radioButton_4_mod.Checked) 
            {
                firstmode = 3;
                firstmode_str = radioButton_4_mod.Text.Replace("\r\n", "\n").Split('\n').First();
            }
            else return;
            //(2)の計算モード
            var but = sender as Button;
            bool isMult;
            string secondmode_str;
            if (but.Name.Contains("mult")) isMult = true;
            else if (but.Name.Contains("div")) isMult = false;
            else return;
            secondmode_str = but.Text;
            //(2)の係数
            int k;
            if (!int.TryParse((string)but.Tag, out k)) return;
            //10以上の数で割る場合
            if (!isMult && k > 10)
            {
                k = (int)numericUpDown_div.Value;
                secondmode_str = "÷ " + k;
            }

            //メッセージボックスの表示テキスト
            var sb = new StringBuilder();
            sb.AppendLine("戦果の表示値を");
            sb.AppendLine();
            sb.AppendFormat("　　表示値 = {0} {1}", firstmode_str, secondmode_str).AppendLine();
            sb.AppendFormat("　　対象 : {0}位 ～ {1}位", numericUpDown_start.Value, numericUpDown_end.Value).AppendLine();
            sb.AppendLine();
            sb.AppendLine("で一括計算します？よろしいですか？");
            sb.AppendLine("※入力済の値は置き換えられます");
            if (firstmode == 1) sb.AppendLine("※値がマイナスになった場合は0とします");
            if (!isMult) sb.AppendLine("※割り算の余りや小数点以下は切り捨てられます");

            if (MessageBox.Show(sb.ToString(), "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            {
                AutoValueSet(firstmode, isMult, k, (int)numericUpDown_start.Value, (int)numericUpDown_end.Value);
            }
        }

        bool isSenkaCalcEventStop = false;
        private void textBox_calc_exp_TextChanged(object sender, EventArgs e)
        {
            if (isSenkaCalcEventStop) return;

            isSenkaCalcEventStop = true;

            int expval;
            if (!int.TryParse(textBox_calc_exp.Text, out expval)) return;

            double senka = (double)expval / 10000.0 * 7.0;
            textBox_calc_senka.Text = senka.ToString("N1");

            isSenkaCalcEventStop = false;
        }

        private void textBox_calc_senka_TextChanged(object sender, EventArgs e)
        {
            if (isSenkaCalcEventStop) return;

            isSenkaCalcEventStop = true;

            double senkaval;
            if (!double.TryParse(textBox_calc_senka.Text, out senkaval)) return;

            int exp = (int)(senkaval * 10000.0 / 7.0);
            textBox_calc_exp.Text = exp.ToString();

            isSenkaCalcEventStop = false;
        }

        private void RadioButtonCommon_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_1_plus.Checked) Config.SenkaCalcForAnalyzeFirstMode = 0;
            else if (radioButton_2_minus.Checked) Config.SenkaCalcForAnalyzeFirstMode = 1;
            else if (radioButton_3_divide.Checked) Config.SenkaCalcForAnalyzeFirstMode = 2;
            else if (radioButton_4_mod.Checked) Config.SenkaCalcForAnalyzeFirstMode = 3;
        }

        //変更を適用する
        private void Apply()
        {
            if (HistoricalData.LogSenka == null || HistoricalData.LogSenka.Count == 0) return;
            //セクションが異なる場合は閉じる
            if (APIPort.Basic == null) return;
            if (APIReqRanking.GetFileName(DateTime.Now) != APIReqRanking.LastSavedFileName)
            {
                if(MessageBox.Show("セクションが変更されため終了します", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.OK)
                {
                    this.Close();
                    return;
                }
            }

            var senka = HistoricalData.LogSenka[HistoricalData.LogSenka.Count - 1];
            if (senka.TopForAnalyzeSenka == null) return;

            //自分の位置の登録
            var mysenkaList = new List<int>();
            foreach(var row in dataGridView1.Rows.OfType<DataGridViewRow>())
            {
                if(row.Cells[1].Value != null && row.Cells[1].Value.ToString() == APIPort.Basic.api_nickname)
                {
                    int rank;
                    if (row.Cells[0].Value == null || !int.TryParse(row.Cells[0].Value.ToString().Replace("位", ""), out rank)) continue;
                    mysenkaList.Add(rank);
                }
            }
            if(mysenkaList.Count == 1 && APIReqRanking.MyRank == 0)
            {
                APIReqRanking.MyRank = mysenkaList[0];
            }

            foreach (var row in dataGridView1.Rows.OfType<DataGridViewRow>())
            {
                //順位
                int rank;
                if (row.Cells[0].Value == null || !int.TryParse(row.Cells[0].Value.ToString().Replace("位", ""), out rank)) continue;

                //表示値
                int view;
                if (row.Cells[3].Value == null || !int.TryParse(row.Cells[3].Value.ToString(), out view)) continue;

                //--Ranking側
                ApiRanking.ApiList ranking;
                if (!APIReqRanking.Rankings.TryGetValue(rank, out ranking)) continue;
                ranking.ForAnalyzeSenkaValue = view;
                ranking.IsForAnalyzeSenkaValueSet = view > 0;

                APIReqRanking.Rankings[rank] = ranking;

                //--Senka側
                //自分の戦果があった場合
                if (APIReqRanking.MyRank == rank)
                {
                    senka.StartSenka = view;
                    senka.CalcEstimateMySenka();
                }

                if(rank <= 0 || rank > HoppoAlpha.DataLibrary.DataObject.SenkaRecord.MaxArraySize) continue;
                senka.TopForAnalyzeSenka[rank - 1] = view;
            }

            //戦果のアップデート
            form1.UpdateSenka();

        }

        private void button_apply_Click(object sender, EventArgs e)
        {
            Apply();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            Apply();
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    
    }
}
