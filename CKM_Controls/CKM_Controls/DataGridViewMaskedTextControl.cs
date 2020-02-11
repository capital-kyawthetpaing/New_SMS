using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace SMS.CustomControls
{
    [System.Drawing.ToolboxBitmap(typeof(TextBox))]
    public class DataGridViewMaskedTextControl : MaskedTextBox, IDataGridViewEditingControl
    {
        public string CheckFormat="";
        //編集コントロールが表示されているDataGridView
        DataGridView dataGridView;
        //編集コントロールが表示されている行
        int rowIndex;
        //編集コントロールの値とセルの値が違うかどうか
        bool valueChanged;
        string txt = "";

        //コンストラクタ
        public DataGridViewMaskedTextControl()
    {
        this.TabStop = false;
        this.TextChanged += (o, e) => NotifyDataGridViewOfValueChange();
    }

        #region IDataGridViewEditingControl メンバ

    //編集コントロールで変更されたセルの値
    public object GetEditingControlFormattedValue(
        DataGridViewDataErrorContexts context)
    {
        if (this.Text != null && ((this.Text.Replace(" ", "").Trim() == "//") || (this.Text.Replace(" ", "").Trim() == "//::")))
        {
            txt = (this.Text.Replace("/", "").Replace(":", ""));
        }
        else
        {
            txt = this.Text;
        }
        return txt;
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
            this.SelectAll();
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

    //protected override void OnLeave(EventArgs e)
    //{
    //    base.OnLeave(e);
    //    if(IsComplete(Text)){
    //        EditingControlDataGridView.CurrentCell.Value = Text;
    //                //EditingControlDataGridView.NotifyCurrentCellDirty(true);//generate new row
    //            }
    //            else
    //                if (!string.IsNullOrWhiteSpace(CheckFormat))
    //                {
    //                    EditingControlDataGridView.CurrentCell.Value = string.Empty;
    //                    //EditingControlDataGridView.NotifyCurrentCellDirty(false);
    //                }
    //}

        //値が変更された時
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            //EditingControlValueChanged = true;
            //if (EditingControlDataGridView != null)
            //{
            //    if (IsComplete(Text))
            //    {
            //        EditingControlDataGridView.CurrentCell.Value = Text;
            //        //EditingControlDataGridView.NotifyCurrentCellDirty(true);//generate new row
            //    }
            //    else
            //        if (!string.IsNullOrWhiteSpace(CheckFormat))
            //        {
            //            EditingControlDataGridView.CurrentCell.Value = string.Empty;
            //            //EditingControlDataGridView.NotifyCurrentCellDirty(false);
            //        }
            //}
        }

        private void NotifyDataGridViewOfValueChange()
        {
            if (!EditingControlValueChanged)
            {
                EditingControlValueChanged = true;
                EditingControlDataGridView.NotifyCurrentCellDirty(true);
            }
        }

        public bool IsValid()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.Text.ToString().Replace("-", "").Replace("/", "")))
                {
                    DateTime.ParseExact(this.Text.ToString().Replace("-", "/"), CheckFormat, CultureInfo.InvariantCulture);
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected bool IsComplete(string text)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(text.ToString().Replace("-", "").Replace("/", "")))
                {
                    DateTime.ParseExact(text.ToString().Replace("-", "/"), CheckFormat, CultureInfo.InvariantCulture);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
