using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS.CustomControls
{
    public class dgvInventoryControl : TextBox, IDataGridViewEditingControl
    {
        //編集コントロールが表示されているDataGridView
        DataGridView dataGridView;
        //編集コントロールが表示されている行
        int rowIndex;
        //編集コントロールの値とセルの値が違うかどうか
        bool valueChanged;

        public Type txt_Type { get; set; }
        public enum Type
        {
            Normal = 0,
            IntegerOnly = 1,
            Hiragana = 2,
            Decimal = 3
        }

        bool useThousandSeparator = false;
        public bool UseThousandSeparator
        {
            get { return useThousandSeparator; }
            set { useThousandSeparator = value; }
        }


        //protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        //{
        //    return true;
        //}
        public dgvInventoryControl()
        {
            this.Enter += txt_Enter;
            this.KeyPress += txt_KeyPress;
            this.KeyUp += txt_KeyUp;
            this.TextChanged += new EventHandler(SMS_TextBox_TextChanged);

            this.TextChanged += (o, e) => NotifyDataGridViewOfValueChange();
        }

        private void txt_Enter(object sender, EventArgs e)
        {
            foreach (InputLanguage lang in InputLanguage.InstalledInputLanguages)
            {
                if (lang.LayoutName.Equals("Japanese"))
                {
                    InputLanguage.CurrentInputLanguage = lang;
                    break;
                }
            }

            if (this.txt_Type == Type.Hiragana)
                this.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            if (this.txt_Type == Type.Decimal)
                this.TextAlign = HorizontalAlignment.Right;
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.txt_Type == Type.IntegerOnly)
            {
                //if (e.KeyChar == 46)
                //{
                //    return;
                //}
                if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46 && e.KeyChar != 45) || e.KeyChar == 46)
                {
                    e.Handled = true;
                }
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '/';

                if (e.KeyChar == '/' && this.Text.Count(f => f == '/') >= 2)
                    e.Handled = true;

                if (char.IsDigit(e.KeyChar))
                {
                    if (this.SelectionLength == 0)
                    {
                        if (this.Text.Replace("/", "").Count() == 8)
                            e.Handled = true;
                    }
                }
            }
            if (this.txt_Type == Type.Decimal)
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                {
                    e.Handled = true;
                }

                // only allow one decimal point
                if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
                {
                    e.Handled = true;
                }
            }
            if (this.txt_Type == Type.IntegerOnly && this.useThousandSeparator == true)
            {
                if (e.KeyChar == 13)
                {
                    if ((sender as TextBox).Text.Contains("-"))
                    {

                    }

                }
            }
            //}
        }

        private void txt_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.txt_Type == Type.IntegerOnly || this.txt_Type == Type.Decimal)
            {
                if (e.KeyData == (Keys.Control | Keys.V))
                {
                    (sender as TextBox).Paste();
                }
                else if (e.KeyData == (Keys.Control | Keys.C))
                {
                    (sender as TextBox).Copy();
                }

                
                if (e.KeyData == Keys.Enter)
                { 
                }
            }
        }

        private void SMS_TextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.txt_Type == Type.Decimal && useThousandSeparator)
            {
                TextBox tb = sender as TextBox;
                string value = tb.Text.Replace(",", "");
                ulong ul;
                if (ulong.TryParse(value, out ul))
                {
                    tb.TextChanged -= SMS_TextBox_TextChanged;
                    if (!tb.Text.Equals("0"))
                        tb.Text = string.Format("{0:#,#}", ul);
                    tb.SelectionStart = tb.Text.Length;
                    tb.TextChanged += SMS_TextBox_TextChanged;
                }
            }
            else if  (this.txt_Type == Type.IntegerOnly && useThousandSeparator)
            {
                //var tb = this;
                ////string value = tb.Text.Replace(",", "");
                ////ulong ul;
                ////if (ulong.TryParse(value, out ul))
                ////{
                //    // tb.TextChanged -= SMS_TextBox_TextChanged;
                //    if (!tb.Text.Equals("0"))
                //        try
                //        {
                //          //  tb.Text = Convert.ToDecimal(tb.Text.ToString()).ToString("#,##0");
                //        }
                //        catch
                //        {

                //        }
                //    tb.SelectionStart = tb.Text.Length;
                    // tb.TextChanged += SMS_TextBox_TextChanged;
                //}
             //   this.Text = Convert.ToDecimal( this.Text).ToString("");
                //MessageBox.Show(this.Text);

                //TextBox tb = sender as TextBox;
                //tb.SelectionStart = tb.Text.Length;
                //tb.Text.Length.ToString("#,##0").ToString();
                //string value = tb.Text.Replace(",", "");
                //int ul;
                //if (int.TryParse(value, out ul))
                //{
                //    if (!tb.Text.Equals("0"))
                //    tb.SelectionStart = tb.Text.Length;
                //}
                //else
                //{
                //    if (tb.Text.Equals(""))
                //    tb.SelectionStart = tb.Text.Length;
                //}
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // dgvInventoryControl
            // 
            //this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvInventoryControl_KeyDown);
            this.Validated += new System.EventHandler(this.dgvInventoryControl_Validated);
            this.ResumeLayout(false);

        }

        private void dgvInventoryControl_Validated(object sender, EventArgs e)
        {

        }

        //private void override dgvInventoryControl_KeyDown(object sender, KeyEventArgs e)
        //{

        //}

       
        //private void virtual dgvInventoryControl_KeyDown(object sender, KeyEventArgs e)
        //{

        //}
    }
}
