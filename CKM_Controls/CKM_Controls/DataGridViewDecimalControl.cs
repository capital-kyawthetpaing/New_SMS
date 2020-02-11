using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SMS.CustomControls
{
    public class DataGridViewDecimalControl : TextBox, IDataGridViewEditingControl
    {
        public byte DecimalPlace = 0;
        //編集コントロールが表示されているDataGridView
        DataGridView dataGridView;
        //編集コントロールが表示されている行
        int rowIndex;
        //編集コントロールの値とセルの値が違うかどうか
        bool valueChanged;

        public bool UseThousandSeperator = false;

        public bool UseMinus = false;

        //コンストラクタ
        public DataGridViewDecimalControl()
        {
            this.TabStop = false;
            this.TextChanged += new EventHandler(DecimalTextBox_TextChanged);
            this.KeyPress += new KeyPressEventHandler(DecimalTextBox_KeyPress);
            this.TextAlign = HorizontalAlignment.Right;

            this.TextChanged += (o, e) => NotifyDataGridViewOfValueChange();
        }

        #region IDataGridViewEditingControl メンバ

        //編集コントロールで変更されたセルの値
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
    {
        return this.Text;
    }

        //編集コントロールで変更されたセルの値
        public object EditingControlFormattedValue
    {
        get
        {
            return this.GetEditingControlFormattedValue(
                DataGridViewDataErrorContexts.Formatting);
        }
        set
        {
            this.Text = (string)value;
        }
    }

        //セルスタイルを編集コントロールに適用する
    //編集コントロールの前景色、背景色、フォントなどをセルスタイルに合わせる
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
    {
        this.Font = dataGridViewCellStyle.Font;
        this.ForeColor = dataGridViewCellStyle.ForeColor;
        this.BackColor = dataGridViewCellStyle.BackColor;
        switch (dataGridViewCellStyle.Alignment)
        {
            case DataGridViewContentAlignment.BottomCenter:
            case DataGridViewContentAlignment.MiddleCenter:
            case DataGridViewContentAlignment.TopCenter:
                this.TextAlign = HorizontalAlignment.Center;
                break;
            case DataGridViewContentAlignment.BottomRight:
            case DataGridViewContentAlignment.MiddleRight:
            case DataGridViewContentAlignment.TopRight:
                this.TextAlign = HorizontalAlignment.Right;
                break;
            default:
                this.TextAlign = HorizontalAlignment.Left;
                break;
        }
    }

        //編集するセルがあるDataGridView
        public DataGridView EditingControlDataGridView
    {
        get
        {
            return this.dataGridView;
        }
        set
        {
            this.dataGridView = value;
        }
    }

        //編集している行のインデックス
        public int EditingControlRowIndex
    {
        get
        {
            return this.rowIndex;
        }
        set
        {
            this.rowIndex = value;
        }
    }

        //値が変更されたかどうか
    //編集コントロールの値とセルの値が違うかどうか
        public bool EditingControlValueChanged
    {
        get
        {
            return this.valueChanged;
        }
        set
        {
            this.valueChanged = value;
        }
    }

        //指定されたキーをDataGridViewが処理するか、編集コントロールが処理するか
    //Trueを返すと、編集コントロールが処理する
    //dataGridViewWantsInputKeyがTrueの時は、DataGridViewが処理できる
        public bool EditingControlWantsInputKey(
            Keys keyData, bool dataGridViewWantsInputKey)
    {
        //Keys.Left、Right、Home、Endの時は、Trueを返す
        //このようにしないと、これらのキーで別のセルにフォーカスが移ってしまう
        switch (keyData & Keys.KeyCode)
        {
            case Keys.Right:
            case Keys.End:
            case Keys.Left:
            case Keys.Home:
                return true;
            default:
                return !dataGridViewWantsInputKey;
        }
    }

        //マウスカーソルがEditingPanel上にあるときのカーソルを指定する
    //EditingPanelは編集コントロールをホストするパネルで、
    //編集コントロールがセルより小さいとコントロール以外の部分がパネルとなる
        public Cursor EditingPanelCursor
    {
        get
        {
            return base.Cursor;
        }
    }

        //コントロールで編集する準備をする
    //テキストを選択状態にしたり、挿入ポインタを末尾にしたりする
        public void PrepareEditingControlForEdit(bool selectAll)
    {
        if (selectAll)
        {
            //選択状態にする
            //this.SelectAll();
        }
        else
        {
            //挿入ポインタを末尾にする
            this.SelectionStart = this.TextLength;
        }
    }

        //値が変更した時に、セルの位置を変更するかどうか
    //値が変更された時に編集コントロールの大きさが変更される時はTrue
        public bool RepositionEditingControlOnValueChange
    {
        get
        {
            return false;
        }
    }

        #endregion

        //値が変更された時
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
          
                EditingControlValueChanged = true;

                if (EditingControlDataGridView != null)
                {
                    try
                    {
                        if (Text == null)
                        {
                            EditingControlDataGridView.CurrentCell.Value = "";
                        }
                        else
                        {
                            EditingControlDataGridView.CurrentCell.Value = Text;
                        }
                        
                    }
                    catch
                    {

                    }
                    EditingControlDataGridView.NotifyCurrentCellDirty(true);

                }
            
           
        }

        private void NotifyDataGridViewOfValueChange()
        {
            if (!EditingControlValueChanged)
            {
                EditingControlValueChanged = true;
                EditingControlDataGridView.NotifyCurrentCellDirty(true);
            }
        }

        private void DecimalTextBox_TextChanged(object sender, EventArgs e)
        {
            if (UseThousandSeperator)
            {
                TextBox txt = sender as TextBox;
                string num = txt.Text;
                string dec = string.Empty;
                string minus = string.Empty;

                if (txt.Text.Contains("."))
                {
                    string[] str = txt.Text.Split('.');
                    num = str[0];
                    dec += "." + str[1];
                }

                if (txt.Text.Contains("-"))
                {
                    minus = "-";
                }

                string value = num.Replace(",", "").Replace("-", "");
                ulong ul;
                if (ulong.TryParse(value, out ul))
                {
                    txt.TextChanged -= DecimalTextBox_TextChanged;
                    int start = txt.SelectionStart;
                    int oldCount = txt.Text.Split(',').Length - 1;//comma count
                    txt.Text = string.Format("{0:#,#}", ul) == "" ? "0" : string.Format("{0:#,#}", ul);//format thousand seperator    
                    txt.Text += dec;
                    txt.Text = minus + txt.Text;
                    int newCount = txt.Text.Split(',').Length - 1;//comma count
                    if (!string.IsNullOrWhiteSpace(txt.Text))
                    {
                        if (oldCount < newCount)
                            txt.SelectionStart = start + 1;
                        else if (oldCount > newCount)
                            txt.SelectionStart = start - 1 > 0 ? start - 1 : 0;
                        else
                            txt.SelectionStart = start;
                    }
                    else txt.SelectionStart = 0;

                    txt.TextChanged += DecimalTextBox_TextChanged;
                }
            }
        }

        private void DecimalTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;
            bool handle = false;

            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46 && e.KeyChar != 45))
            {
                handle = true;
                goto l1;
            }
            else
            {
                if (e.KeyChar >= 48 && e.KeyChar <= 57)
                {
                    if (txt.Text.Contains("."))
                    {
                        int decIndex = txt.Text.IndexOf('.');
                        int curIndex = txt.SelectionStart;

                        string[] str = txt.Text.Split('.');
                        if (decIndex < curIndex)
                        {

                            if (str[1].Length >= DecimalPlace)
                            {
                                handle = true;
                                goto l1;
                            }
                        }
                        else
                        {
                            if (str[0].Length < this.MaxLength - (DecimalPlace + 1))
                            {
                                handle = true;
                                goto l1;
                            }
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(txt.Text) && DecimalPlace > 0)
                    {
                        string[] str = txt.Text.Split('.');
                        if (str[0].Length >= this.MaxLength - (DecimalPlace + 1))
                        {
                            handle = true;
                            goto l1;
                        }
                    }
                }
                else if (e.KeyChar == 8)
                {
                    if (txt.SelectionStart > 1)
                    {
                        char c = txt.Text[txt.SelectionStart - 1];
                        if (c == ',')
                            txt.SelectionStart -= 1;
                    }
                }
                else if (e.KeyChar == 46)
                {
                    if (DecimalPlace == 0)
                    {
                        handle = true;
                        goto l1;
                    }
                    else
                    {
                        if (txt.Text.Contains("."))
                        {
                            handle = true;
                            goto l1;
                        }
                    }
                }
                else if (e.KeyChar == 45)
                {
                    if (!UseMinus)
                    {
                        handle = true;
                        goto l1;
                    }
                    else if (txt.SelectionStart != 0 && txt.Text.Contains("-"))
                    {
                        handle = true;
                        goto l1;
                    }
                }
            }

        l1: e.Handled = handle;
        }
    }
}
