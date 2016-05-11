using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Windows.Forms.DataVisualization.Charting;

namespace VisualFormTest
{
    public static class CallBacks
    {
        //Delegate
        public delegate void SetChartCallBack(System.Windows.Forms.DataVisualization.Charting.Chart chart, GraphInfo info);
        delegate void SetControlCheckedCallBacl(dynamic control, bool flag);
        delegate void SetControlEnabledCallBack(Control control, bool value);
        delegate void SetControlToolTipCallBack(System.Windows.Forms.ToolTip tooltip, System.Windows.Forms.Control control, string text);
        delegate void SetControlVisibleCallBack(Control control, bool value);
        delegate void SetLabelTextCallBack(System.Windows.Forms.Label label, string text);
        delegate void SetLabelTextAndColorCallBack(Label label, string text, Color foreColor);
        delegate void SetLabelTextAndColorDoubleCallBack(Label label, string text, Color foreColor, Color backColor);
        delegate void SetLabelTextColorDoubleAndBoldCallBack(Label label, string text, Color foreColor, Color backColor, bool isBold);
        delegate void SetLabelTextColorDoubleAndFontSizeCallBack(Label label, string text, Color foreColor, Color backColor, float fontSize);
        delegate void SetLabelTagCallBack(Label label, object tag);
        delegate void SetLabelColorCallBack(Label label, Color foreColor, Color backColor);
        delegate void SetLabelMouseEventCallBack(Label label, EventHandler mouseHoverHandler, EventHandler mouseLeaveHandler);
        delegate void SetLabelToolTipHideCallBack(Label label, ToolTip tooltip);
        delegate void SetLabelToolTipShowCallBack(Label label, string text, ToolTip tooltip);
        delegate void SetFormTextCallBack(Form form, string text);
        delegate void SetNumericUpDownValueCallBack(System.Windows.Forms.NumericUpDown nudown, int value);
        delegate void SetShipNumLabelCallBack(Label label, string text, Color foreColor, bool bold);
        delegate void SetShipNumPanelCallBack(Panel panel, Color backColor);
        delegate void SetTextBoxTextCallBack(TextBox textbox, string text);
        delegate void SetTextBoxTextAppendCallBack(TextBox textbox, string appendtext);
        delegate void SetTextBoxTextColorDoubleAndBorderStyleCallBack(TextBox textbox, string text, Color foreColor, Color backColor, BorderStyle borderStyle);
        delegate void SetToolStripDropDownButtonEnabledCallBack(ToolStripDropDownButton button, StatusStrip strip, bool value);
        delegate void SetToolStripMenuItemTextCallBack(ToolStripMenuItem item, ToolStrip strip, string text);
        delegate void SetToolStripMenuItemEnabledCallBack(ToolStripMenuItem item, ToolStrip strip, bool value);
        delegate void SetToolStripMenuItemCheckedCallBack(ToolStripMenuItem item, ToolStrip strip, bool value);
        delegate void EnableDoubleBufferingCallBack(Control control);


        #region コールバック～元KancolleInfo～
        //ラベルの更新+文字色
        public static void SetLabelTextAndColor(Label label, string text, Color foreColor)
        {
            if (!label.IsHandleCreated)
            {
                var form = label.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (label.Text == text && label.ForeColor == foreColor) return;
            if (label.InvokeRequired)
            {
                SetLabelTextAndColorCallBack d = new SetLabelTextAndColorCallBack(SetLabelTextAndColor);
                label.Invoke(d, new object[] { label, text, foreColor });
            }
            else
            {
                label.Text = text;
                label.ForeColor = foreColor;
            }
        }

        //ラベルの更新＋色×2
        public static void SetLabelTextAndColorDouble(Label label, string text, Color foreColor, Color backColor)
        {
            if (!label.IsHandleCreated)
            {
                var form = label.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (label.Text == text && label.ForeColor == foreColor && label.BackColor == backColor) return;
            if (label.InvokeRequired)
            {
                SetLabelTextAndColorDoubleCallBack d = new SetLabelTextAndColorDoubleCallBack(SetLabelTextAndColorDouble);
                label.Invoke(d, new object[] { label, text, foreColor, backColor });
            }
            else
            {
                label.Text = text;
                label.ForeColor = foreColor;
                label.BackColor = backColor;
            }
        }

        //ラベルの更新＋色×2＋太字
        public static void SetLabelTextColorDoubleAndBold(Label label, string text, Color foreColor, Color backColor, bool isBold)
        {
            if (!label.IsHandleCreated)
            {
                var form = label.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (label.InvokeRequired)
            {
                SetLabelTextColorDoubleAndBoldCallBack d = new SetLabelTextColorDoubleAndBoldCallBack(SetLabelTextColorDoubleAndBold);
                label.Invoke(d, new object[] { label, text, foreColor, backColor, isBold });
            }
            else
            {
                label.Text = text;
                label.ForeColor = foreColor;
                label.BackColor = backColor;
                if (isBold) label.Font = new Font(label.Font, FontStyle.Bold);
                else label.Font = new Font(label.Font, FontStyle.Regular);
            }
        }

        //ラベルの更新＋色×2＋フォントサイズ
        public static void SetLabelTextColorDoubleAndFontSize(Label label, string text, Color foreColor, Color backColor, float fontSize)
        {
            if (!label.IsHandleCreated)
            {
                var form = label.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (label.InvokeRequired)
            {
                SetLabelTextColorDoubleAndFontSizeCallBack d = new SetLabelTextColorDoubleAndFontSizeCallBack(SetLabelTextColorDoubleAndFontSize);
                label.Invoke(d, new object[]{label, text, foreColor, backColor, fontSize});
            }
            else
            {
                label.Text = text;
                label.ForeColor = foreColor;
                label.BackColor = backColor;
                if(label.Font.Size != fontSize) label.Font = new Font(label.Font.FontFamily, fontSize);
            }
        }

        //ラベルのタグ設定
        public static void SetLabelTag(Label label, object tag)
        {
            if (!label.IsHandleCreated)
            {
                var form = label.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (label.InvokeRequired)
            {
                SetLabelTagCallBack d = new SetLabelTagCallBack(SetLabelTag);
                label.Invoke(d, new object[] { label, tag });
            }
            else
            {
                label.Tag = tag;
            }

        }

        //色の設定
        public static void SetLabelColor(Label label, Color foreColor, Color backColor)
        {
            if (!label.IsHandleCreated)
            {
                var form = label.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (label.InvokeRequired)
            {
                SetLabelColorCallBack d = new SetLabelColorCallBack(SetLabelColor);
                label.Invoke(d, new object[] { label, foreColor, backColor });
            }
            else
            {
                label.ForeColor = foreColor;
                label.BackColor = backColor;
            }
        }

        //マウスイベント
        public static void SetLabelMouseEvent(Label label, EventHandler mouseHoverHandler, EventHandler mouseLeaveHandler)
        {
            if (!label.IsHandleCreated)
            {
                var form = label.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (label.InvokeRequired)
            {
                SetLabelMouseEventCallBack d = new SetLabelMouseEventCallBack(SetLabelMouseEvent);
                label.Invoke(d, new object[] { label, mouseHoverHandler, mouseLeaveHandler });
            }
            else
            {
                label.MouseHover += new EventHandler(mouseHoverHandler);
                label.MouseLeave += new EventHandler(mouseLeaveHandler);
            }
        }

        //ラベルのToolTipのHide
        public static void SetLabelToolTipHide(Label label, ToolTip tooltip)
        {
            if (!label.IsHandleCreated)
            {
                var form = label.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (label.InvokeRequired)
            {
                SetLabelToolTipHideCallBack d = new SetLabelToolTipHideCallBack(SetLabelToolTipHide);
                label.Invoke(d, new object[] { label, tooltip});
            }
            else
            {
                tooltip.Hide(label);
            }
        }

        //ラベルのToolTipのShow
        public static void SetLabelToolTipShow(Label label, string text, ToolTip tooltip)
        {
            if (!label.IsHandleCreated)
            {
                var form = label.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (label.InvokeRequired)
            {
                SetLabelToolTipShowCallBack d = new SetLabelToolTipShowCallBack(SetLabelToolTipShow);
                label.Invoke(d, new object[] { label, text, tooltip});
            }
            else
            {
                tooltip.Show(text, label, 30, 15, 10000);
            }
        }

        //フォームのテキスト
        public static void SetFormText(Form form, string text)
        {
            if (!form.IsHandleCreated)
            {
                var fform = form.FindForm();
                if (fform == null) return;
                if (!fform.IsHandleCreated) return;
                if (form.IsDisposed) return;
            }
            if (form.InvokeRequired)
            {
                SetFormTextCallBack d = new SetFormTextCallBack(SetFormText);
                form.Invoke(d, new object[] { form, text });
            }
            else
            {
                form.Text = text;
            }
        }

        //船の数専用のコールバック
        public static void SetShipNumLabel(Label label, string text, Color foreColor, bool bold)
        {
            if (!label.IsHandleCreated)
            {
                var form = label.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (label.InvokeRequired)
            {
                SetShipNumLabelCallBack d = new SetShipNumLabelCallBack(SetShipNumLabel);
                label.Invoke(d, new object[] { label, text, foreColor, bold });
            }
            else
            {
                label.Text = text;
                label.ForeColor = foreColor;
                if (bold) label.Font = new Font(label.Font, FontStyle.Bold);
                else label.Font = new Font(label.Font, FontStyle.Regular);
            }
        }

        public static void SetShipNumPanel(Panel panel, Color backColor)
        {
            if (!panel.IsHandleCreated)
            {
                var form = panel.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (panel.InvokeRequired)
            {
                SetShipNumPanelCallBack d = new SetShipNumPanelCallBack(SetShipNumPanel);
                panel.Invoke(d, new object[] { panel, backColor });
            }
            else
            {
                panel.BackColor = backColor;
            }
        }
        #endregion

        #region コールバック～元Form1
        public static void SetTextBoxText(TextBox textbox, string text)
        {
            if (!textbox.IsHandleCreated)
            {
                var form = textbox.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (textbox.InvokeRequired)
            {
                SetTextBoxTextCallBack d = new SetTextBoxTextCallBack(SetTextBoxText);
                textbox.Invoke(d, new object[] { textbox, text });
            }
            else
            {
                textbox.Text = text;
            }
        }

        public static void SetTextBoxTextAppend(TextBox textbox, string appendtext)
        {
            if (!textbox.IsHandleCreated)
            {
                var form = textbox.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (textbox.InvokeRequired)
            {
                SetTextBoxTextAppendCallBack d = new SetTextBoxTextAppendCallBack(SetTextBoxTextAppend);
                textbox.Invoke(d, new object[] { textbox, appendtext });
            }
            else
            {
                textbox.AppendText(appendtext);
            }
        }

        public static void SetControlEnabled(Control control, bool value)
        {
            if (!control.IsHandleCreated)
            {
                var form = control.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (control.InvokeRequired)
            {
                SetControlEnabledCallBack d = new SetControlEnabledCallBack(SetControlEnabled);
                control.Invoke(d, new object[] { control, value });
            }
            else
            {
                control.Enabled = value;
            }
        }

        public static void SetControlVisible(Control control, bool value)
        {
            if (!control.IsHandleCreated)
            {
                var form = control.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (control.InvokeRequired)
            {
                SetControlVisibleCallBack d = new SetControlVisibleCallBack(SetControlVisible);
                control.Invoke(d, new object[] { control, value });
            }
            else
            {
                control.Visible = value;
            }
        }

        public static void SetToolStripDropDownButtonEnabled(ToolStripDropDownButton button, StatusStrip strip, bool flag)
        {
            if (!strip.IsHandleCreated)
            {
                var form = strip.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (strip.InvokeRequired)
            {
                SetToolStripDropDownButtonEnabledCallBack d = new SetToolStripDropDownButtonEnabledCallBack(SetToolStripDropDownButtonEnabled);
                strip.Invoke(d, new object[] { button, strip, flag });
            }
            else
            {
                button.Enabled = flag;
            }
        }

        public static void SetToolStripMenuItemChecked(ToolStripMenuItem item, ToolStrip strip, bool value)
        {
            if (!strip.IsHandleCreated)
            {
                var form = strip.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (strip.InvokeRequired)
            {
                SetToolStripMenuItemCheckedCallBack d = new SetToolStripMenuItemCheckedCallBack(SetToolStripMenuItemChecked);
                strip.Invoke(d, new object[] { item, strip, value });
            }
            else
            {
                item.Checked = value;
            }
        }

        public static void SetToolStripMenuItemEnabled(ToolStripMenuItem item, ToolStrip strip, bool value)
        {
            if (!strip.IsHandleCreated)
            {
                var form = strip.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (strip.InvokeRequired)
            {
                SetToolStripMenuItemEnabledCallBack d = new SetToolStripMenuItemEnabledCallBack(SetToolStripMenuItemEnabled);
                strip.Invoke(d, new object[] { item, strip, value });
            }
            else
            {
                item.Enabled = value;
            }
        }

        public static void SetToolStripMenuItemText(ToolStripMenuItem item, ToolStrip strip, string text)
        {
            if (!strip.IsHandleCreated)
            {
                var form = strip.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (strip.InvokeRequired)
            {
                SetToolStripMenuItemTextCallBack d = new SetToolStripMenuItemTextCallBack(SetToolStripMenuItemText);
                strip.Invoke(d, new object[] { item, strip, text });
            }
            else
            {
                item.Text = text;
            }
        }
        #endregion

        #region コールバック～元KancolleUnitList～
        public static void SetControlChecked(dynamic control, bool flag)
        {
            if (!control.IsHandleCreated)
            {
                var form = control.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (control.InvokeRequired)
            {
                SetControlCheckedCallBacl d = new SetControlCheckedCallBacl(SetControlChecked);
                control.Invoke(d, new object[] { control, flag });
            }
            else
            {
                control.Checked = flag;
            }
        }

        public static void SetNumericUpDown(System.Windows.Forms.NumericUpDown nudown, int value)
        {
            if (!nudown.IsHandleCreated)
            {
                var form = nudown.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (nudown.InvokeRequired)
            {
                SetNumericUpDownValueCallBack d = new SetNumericUpDownValueCallBack(SetNumericUpDown);
                nudown.Invoke(d, new object[] { nudown, value });
            }
            else
            {
                nudown.Value = value;
            }
        }

        public static void SetLabelText(System.Windows.Forms.Label label, string text)
        {
            if (!label.IsHandleCreated)
            {
                var form = label.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (label.Text == text) return;
            if (label.InvokeRequired)
            {
                SetLabelTextCallBack d = new SetLabelTextCallBack(SetLabelText);
                label.Invoke(d, new object[] { label, text });
            }
            else
            {
                label.Text = text;
            }
        }

        //DoubleBuffer
        /// <summary>
        /// コントロールのDoubleBufferedプロパティをTrueにする
        /// </summary>
        /// <param name="control">対象のコントロール</param>
        public static void EnableDoubleBuffering(System.Windows.Forms.Control control)
        {
            if (!control.IsHandleCreated)
            {
                var form = control.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (control.InvokeRequired)
            {
                EnableDoubleBufferingCallBack d = new EnableDoubleBufferingCallBack(EnableDoubleBuffering);
                control.Invoke(d, new object[] { control });
            }
            else
            {
                control.GetType().InvokeMember(
                   "DoubleBuffered",
                   BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                   null,
                   control,
                   new object[] { true });
            }
        }
        #endregion

        #region コールバック～KancolleInfoGeneral～
        public static void SetTextBoxTextColorDoubleAndBorderStyle(TextBox textbox, string text, Color foreColor, Color backColor, BorderStyle borderStyle)
        {
            if (!textbox.IsHandleCreated)
            {
                var form = textbox.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (textbox.InvokeRequired)
            {
                SetTextBoxTextColorDoubleAndBorderStyleCallBack d = new SetTextBoxTextColorDoubleAndBorderStyleCallBack(SetTextBoxTextColorDoubleAndBorderStyle);
                textbox.Invoke(d, new object[] { textbox, text, foreColor, backColor, borderStyle });
            }
            else
            {
                textbox.Text = text;
                textbox.ForeColor = foreColor;
                textbox.BackColor = backColor;
                textbox.BorderStyle = borderStyle;
            }
        }

        public static void SetControlToolTip(System.Windows.Forms.ToolTip tooltip, System.Windows.Forms.Control control, string text)
        {
            if (!control.IsHandleCreated)
            {
                var form = control.FindForm();
                if (form == null) return;
                if (!form.IsHandleCreated) return;
            }
            if (control.InvokeRequired)
            {
                SetControlToolTipCallBack d = new SetControlToolTipCallBack(SetControlToolTip);
                control.Invoke(d, new object[] { tooltip, control, text });
            }
            else
            {
                tooltip.SetToolTip(control, text);
            }
        }
        #endregion
    }
}
