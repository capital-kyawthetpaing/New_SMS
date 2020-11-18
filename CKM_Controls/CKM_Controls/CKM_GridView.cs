using BL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CKM_Controls
{
    public class CKM_GridView : DataGridView
    {
        private bool Setting = false;
        ArrayList hiraganaColumn = new ArrayList();

        private ArrayList checkCol = new ArrayList();
        public ArrayList CheckCol 
        {
            get { return checkCol; } 
            set { checkCol = value; }
        }

        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Use CKM Setting")]
        [DisplayName("CKM Setting")]
        public bool UseSetting
        {
            get { return Setting; }
            set
            {
                Setting = value;
                CKM_GridDesign();
            }
        }

        public bool UseRow = true;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Use RowNo")]
        [DisplayName("Use RowNo")]
        public bool UseRowNo
        {
            get => UseRow;
            set => UseRow = RowHeadersVisible = value;
        }

        public int RowHeight = 20;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Row Height")]
        [DisplayName("Row Height")]
        public int RowHeight_
        {
            get { return RowHeight; }
            set { RowHeight = value;
                RowTemplate.Height = RowHeight;
                //AllowUserToAddRows = false;
                //AllowUserToAddRows = true;
                //this.Refresh();
             //   this.RefreshEdit();
                Invalidate();
            }
           

        }

        MasterTouroku_Souko_BL mtsbl;
        string DisablecolName = string.Empty;
        string EnablecolName = string.Empty;
        string columnName = string.Empty;

        public bool lastKey = false;

        public CKM_GridView()
        {
            mtsbl = new MasterTouroku_Souko_BL();
        }
        private bool isAllColumnReadOnly()
        {
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].ReadOnly == false)
                    return false;
            }
            return true;
        }

        protected void Check_SubFormGrid(Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                try
                {
                    Control ctrl = this;
                    do
                    {
                        ctrl = ctrl.Parent;
                    } while (!(ctrl is Form));
                    if (ctrl.GetType().BaseType.Name.Contains("FrmSubForm"))
                    {
                        Button btn12 = ctrl.Controls.Find("BtnF12", true).FirstOrDefault() as Button;
                        btn12.PerformClick();
                    }
                }
                catch
                {

                }
            }

        }
      

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)

        {
            Check_SubFormGrid(keyData);
            CheckDuplicate();
            MasterTouroku_YuubinBangou_BL YuubinBangouBL = new MasterTouroku_YuubinBangou_BL();  DataTable dtDisplay = DataSource as DataTable; var dgvYuubinBangou = this;
            if (IsSkipped && ( keyData == Keys.Enter || keyData == Keys.Up || keyData == Keys.Down))
            {
                mtsbl.ShowMessage("E105");
                BeginEdit(true);
            }
            else
            {
                // Choose a direction based on the key the user entered
                Keys direction = Keys.None;
                Keys reverseKey = Keys.None;
                
                switch (keyData)
                {
                    case Keys.Left:              // Handle Left inside the user's text (if any)
                        if (this.EditingControl != null)
                            if (this.EditingControl.GetType() == typeof(DataGridViewTextBoxEditingControl))

                                if (((TextBox)this.EditingControl).SelectionStart > 0)
                                    return base.ProcessCmdKey(ref msg, keyData);
                        direction = Keys.Shift | Keys.Tab;
                        reverseKey = Keys.Tab;
                        break;
                    case Keys.Down:
                        direction = keyData;
                        reverseKey = Keys.Up;
                        break;
                    case Keys.Up:
                        direction = keyData;
                        reverseKey = Keys.Down;
                        break;
                    case Keys.Right:
                        // Handle Right inside the user's text (if any)
                        if (this.EditingControl != null)
                            if (this.EditingControl.GetType() == typeof(DataGridViewTextBoxEditingControl))
                                if (((TextBox)this.EditingControl).SelectionStart < this.EditingControl.Text.Length)
                                    return base.ProcessCmdKey(ref msg, keyData);
                        direction = Keys.Tab;
                        reverseKey = Keys.Shift | Keys.Tab;
                        break;
                    case Keys.Tab:
                        direction = keyData;
                        reverseKey = Keys.Shift | Keys.Tab;
                        break;
                    case Keys.Shift | Keys.Tab:
                        direction = keyData;
                        reverseKey = Keys.Tab;
                        break;
                    case Keys.Enter:   //// PTK Added not still Confirm
                        if (CurrentCell != null)
                        {
                            if (!checkCol.Contains(this.Columns[this.CurrentCell.ColumnIndex].Name))
                            {
                                if (this.Name == "dgvYuubinBangou")
                                {
                                    if (CurrentCell.OwningColumn.Name == "colZipCD2")
                                    {
                                        if (String.IsNullOrWhiteSpace(Rows[CurrentRow.Index].Cells["colZipCD1"].Value.ToString()))
                                        {
                                            direction = Keys.Shift | Keys.Tab;
                                            YuubinBangouBL.ShowMessage("E102");
                                            //CurrentCell.Selected = true;
                                            CurrentCell.Selected = true;
                                            NotifyCurrentCellDirty(true);
                                            BeginEdit(true);
                                            break;
                                        }
                                        if ((!string.IsNullOrWhiteSpace(Rows[CurrentRow.Index].Cells["colZipCD1"].EditedFormattedValue.ToString()) && string.IsNullOrWhiteSpace(Rows[CurrentRow.Index].Cells["colZipCD2"].EditedFormattedValue.ToString())))
                                        {
                                            YuubinBangouBL.ShowMessage("E102");
                                            BeginEdit(true); break;
                                        }
                                        if (!string.IsNullOrWhiteSpace(Rows[CurrentRow.Index].Cells["colZipCD1"].EditedFormattedValue.ToString()) && !string.IsNullOrWhiteSpace(Rows[CurrentRow.Index].Cells["colZipCD2"].EditedFormattedValue.ToString()))
                                        {
                                            DataRow[] drarr = dtDisplay.Select("ZipCD1 = '" + Rows[CurrentRow.Index].Cells["colZipCD1"].EditedFormattedValue.ToString() + "' AND ZipCD2 = '" + Rows[CurrentRow.Index].Cells["colZipCD2"].EditedFormattedValue.ToString() + "'");
                                            if (drarr.Length > 1)
                                            {
                                                YuubinBangouBL.ShowMessage("E105");
                                                BeginEdit(true);
                                                break;
                                            }
                                            else if (TanaCheck("colZipCD1", Rows[CurrentRow.Index].Cells["colZipCD1"].EditedFormattedValue.ToString()) && TanaCheck("colZipCD2"))
                                            {
                                                YuubinBangouBL.ShowMessage("E105");
                                                BeginEdit(true);
                                                break;
                                            }
                                            else
                                            {
                                                direction = Keys.Tab;
                                                reverseKey = Keys.Shift | Keys.Tab;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            direction = Keys.Tab;
                                            reverseKey = Keys.Shift | Keys.Tab;
                                            break;
                                        }
                                    }
                                    else if (CurrentCell.OwningColumn.Name == "colAdd1")
                                    {
                                        if (!string.IsNullOrWhiteSpace(Rows[CurrentRow.Index].Cells["colZipCD1"].EditedFormattedValue.ToString()) && string.IsNullOrWhiteSpace(Rows[CurrentRow.Index].Cells["colAdd1"].EditedFormattedValue.ToString()))
                                        {
                                            YuubinBangouBL.ShowMessage("E102");
                                            CurrentCell = Rows[CurrentRow.Index].Cells["colAdd1"];
                                            break;
                                            // dgvYuubinBangou.BeginEdit(true);
                                        }
                                        else if (string.IsNullOrWhiteSpace(Rows[CurrentRow.Index].Cells["colZipCD1"].EditedFormattedValue.ToString()) && !string.IsNullOrWhiteSpace(Rows[CurrentRow.Index].Cells["colAdd1"].EditedFormattedValue.ToString()))
                                        {
                                            Rows[CurrentRow.Index].Cells["colAdd1"].Value = null;
                                            direction = Keys.Tab;
                                            reverseKey = Keys.Shift | Keys.Tab;
                                            break;
                                        }
                                        else
                                        {
                                            direction = Keys.Tab;
                                            reverseKey = Keys.Shift | Keys.Tab;
                                            break;
                                        }
                                    }
                                    else if (CurrentCell.OwningColumn.Name == "colAdd2") ///////Changed
                                    {
                                        if (string.IsNullOrWhiteSpace(Rows[CurrentRow.Index].Cells["colZipCD2"].EditedFormattedValue.ToString()) && !string.IsNullOrWhiteSpace(Rows[CurrentRow.Index].Cells["colAdd2"].EditedFormattedValue.ToString()))
                                        {
                                            Rows[CurrentRow.Index].Cells["colAdd2"].Value = null;
                                        }
                                        direction = Keys.Tab;
                                        reverseKey = Keys.Shift | Keys.Tab;
                                        break;
                                    }
                                    else
                                    {
                                        direction = Keys.Tab;
                                        reverseKey = Keys.Shift | Keys.Tab;
                                        break;
                                    }
                                }
                                else if (this.Name == "GV_item")
                                {
                                    Base_BL bb = new Base_BL();
                                    if (CurrentCell.RowIndex != -1)
                                    {
                                        if (CurrentCell.ColumnIndex == Columns["掛率"].Index)
                                        {
                                            string ratevlue = Rows[CurrentCell.RowIndex].Cells["掛率"].Value.ToString();
                                            string editvalue = Rows[CurrentCell.RowIndex].Cells["掛率"].EditedFormattedValue.ToString();
                                            if (!String.IsNullOrEmpty(editvalue))
                                            {
                                                if (!editvalue.Contains("."))
                                                {
                                                    var isNumeric = int.TryParse(editvalue, out int n);
                                                    if (isNumeric)
                                                    {
                                                        if (editvalue.Length > 3)
                                                        {

                                                            MessageBox.Show("enter valid no");
                                                            CurrentCell = this[CurrentCell.ColumnIndex, CurrentCell.RowIndex];
                                                            RefreshEdit();
                                                            //CurrentCell.Value = "0";
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            string priceouttax = Rows[CurrentCell.RowIndex].Cells["定価"].Value.ToString();
                                                            string rateproce = Rows[CurrentCell.RowIndex].Cells["掛率"].EditedFormattedValue.ToString();
                                                            decimal rate = Convert.ToDecimal(rateproce);
                                                            decimal con = (decimal)0.01;
                                                            decimal listprice = Convert.ToDecimal(priceouttax);
                                                            Rows[CurrentCell.RowIndex].Cells["発注単価"].Value = Math.Round(listprice * (rate * con)).ToString();
                                                            direction = Keys.Tab;
                                                            reverseKey = Keys.Shift | Keys.Tab;
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bb.ShowMessage("E118");
                                                        CurrentCell = this[CurrentCell.ColumnIndex, CurrentCell.RowIndex];
                                                        RefreshEdit();
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    Columns["掛率"].DefaultCellStyle.Format = "N2";
                                                    int x = editvalue.IndexOf('.');
                                                    int count = editvalue.Count(f => f == '.');
                                                    string charre = editvalue.Remove(x, count);
                                                    var isNumeric = int.TryParse(charre, out int n);
                                                    if (isNumeric)
                                                    {
                                                        if (count != 1 || x >= 4)
                                                        {
                                                            MessageBox.Show("enter valid no");
                                                            CurrentCell = this[CurrentCell.ColumnIndex, CurrentCell.RowIndex];
                                                            RefreshEdit();
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            string priceouttax = Rows[CurrentCell.RowIndex].Cells["定価"].Value.ToString();
                                                            string rateproce = Rows[CurrentCell.RowIndex].Cells["掛率"].EditedFormattedValue.ToString();
                                                            decimal rate = Convert.ToDecimal(rateproce);
                                                            decimal con = (decimal)0.01;
                                                            decimal listprice = Convert.ToDecimal(priceouttax);
                                                            Rows[CurrentCell.RowIndex].Cells["発注単価"].Value = Math.Round(listprice * (rate * con)).ToString();
                                                            direction = Keys.Tab;
                                                            reverseKey = Keys.Shift | Keys.Tab;
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bb.ShowMessage("E118");
                                                        CurrentCell = this[CurrentCell.ColumnIndex, CurrentCell.RowIndex];
                                                        RefreshEdit();
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                bb.ShowMessage("E102");
                                                CurrentCell = this[CurrentCell.ColumnIndex, CurrentCell.RowIndex];
                                                RefreshEdit();
                                                break;
                                            }
                                        }
                                        if (CurrentCell.ColumnIndex == Columns["改定日"].Index)
                                        {
                                            string editvalue = Rows[CurrentCell.RowIndex].Cells["改定日"].EditedFormattedValue.ToString();

                                            if (String.IsNullOrEmpty(editvalue))
                                            {
                                                bb.ShowMessage("E102");
                                                CurrentCell = this[CurrentCell.ColumnIndex, CurrentCell.RowIndex];
                                                RefreshEdit();
                                                break;
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrWhiteSpace(editvalue))
                                                {

                                                    if (bb.IsInteger(editvalue.Replace("/", "").Replace("-", "")))
                                                    {
                                                        string day = string.Empty, month = string.Empty, year = string.Empty;
                                                        string corvalue = string.Empty;
                                                        if (editvalue.Contains("/"))
                                                        {
                                                            string[] date = editvalue.Split('/');
                                                            day = date[date.Length - 1].PadLeft(2, '0');
                                                            month = date[date.Length - 2].PadLeft(2, '0');

                                                            if (date.Length > 2)
                                                                year = date[date.Length - 3];

                                                            Rows[CurrentCell.RowIndex].Cells["改定日"].Value = year + month + day;//  this.Text.Replace("/", "");
                                                            corvalue = year + month + day;
                                                        }
                                                        else if (editvalue.Contains("-"))
                                                        {
                                                            string[] date = editvalue.Split('-');
                                                            day = date[date.Length - 1].PadLeft(2, '0');
                                                            month = date[date.Length - 2].PadLeft(2, '0');

                                                            if (date.Length > 2)
                                                                year = date[date.Length - 3];

                                                            Rows[CurrentCell.RowIndex].Cells["改定日"].Value = year + month + day;//  this.Text.Replace("-", "");
                                                            corvalue = year + month + day;
                                                        }
                                                        string text;
                                                        if (!String.IsNullOrEmpty(corvalue))
                                                        {
                                                            text = corvalue;
                                                        }
                                                        else
                                                        {
                                                            text = editvalue;
                                                        }

                                                        text = text.PadLeft(8, '0');
                                                        day = text.Substring(text.Length - 2);
                                                        month = text.Substring(text.Length - 4).Substring(0, 2);
                                                        year = Convert.ToInt32(text.Substring(0, text.Length - 4)).ToString();

                                                        if (month == "00")
                                                        {
                                                            month = string.Empty;
                                                        }
                                                        if (year == "0")
                                                        {
                                                            year = string.Empty;
                                                        }

                                                        if (string.IsNullOrWhiteSpace(month))
                                                            month = DateTime.Now.Month.ToString().PadLeft(2, '0');//if user doesn't input for month,set current month

                                                        if (string.IsNullOrWhiteSpace(year))
                                                        {
                                                            year = DateTime.Now.Year.ToString();//if user doesn't input for year,set current year
                                                        }
                                                        else
                                                        {
                                                            if (year.Length == 1)
                                                                year = "200" + year;
                                                            else if (year.Length == 2)
                                                                year = "20" + year;
                                                        }
                                                        string strdate = year + "/" + month + "/" + day;
                                                        if (bb.CheckDate(strdate))
                                                        {
                                                            Rows[CurrentCell.RowIndex].Cells["改定日"].Value = strdate;
                                                            RefreshEdit();
                                                            direction = Keys.Tab;
                                                            reverseKey = Keys.Shift | Keys.Tab;
                                                            break;

                                                        }
                                                        else
                                                        {
                                                            bb.ShowMessage("E103");
                                                            CurrentCell = this[CurrentCell.ColumnIndex, CurrentCell.RowIndex];
                                                            RefreshEdit();
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bb.ShowMessage("E103");
                                                        CurrentCell = this[CurrentCell.ColumnIndex, CurrentCell.RowIndex];
                                                        RefreshEdit();
                                                        break;
                                                    }
                                                }

                                            }
                                        }

                                        if (CurrentCell.ColumnIndex == Columns["発注単価"].Index)
                                        {
                                            string editvalue = Rows[CurrentCell.RowIndex].Cells["発注単価"].EditedFormattedValue.ToString();

                                            if (String.IsNullOrEmpty(editvalue))
                                            {
                                                bb.ShowMessage("E102");
                                                CurrentCell = this[CurrentCell.ColumnIndex, CurrentCell.RowIndex];
                                                RefreshEdit();
                                                break;
                                            }
                                            var isNumeric = int.TryParse(editvalue, out int n);
                                            if (!isNumeric)
                                            {
                                                bb.ShowMessage("E118");
                                                //MessageBox.Show("enter valid no");
                                                CurrentCell = this[CurrentCell.ColumnIndex, CurrentCell.RowIndex];
                                                RefreshEdit();
                                                break;
                                            }
                                        }
                                    }
                                    //else

                                    //{
                                    direction = Keys.Tab;
                                    reverseKey = Keys.Shift | Keys.Tab;
                                    break;
                                    //}

                                }


                                else
                                {
                                    direction = Keys.Tab;
                                    reverseKey = Keys.Shift | Keys.Tab;

                                    break;
                                }//(!string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()) && string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"].Value.ToString()))
                            }
                            else
                            {
                                lastKey = true;
                                this.OnValidating(new CancelEventArgs(false));
                                if (!lastKey)
                                {
                                    direction = Keys.Tab;
                                    reverseKey = Keys.Shift | Keys.Tab;
                                }
                            }

                        }
                        else
                        {
                            direction = Keys.Tab;
                            reverseKey = Keys.Shift | Keys.Tab;
                        }
                        lastKey = false;
                        break;
                        
                    case Keys.ProcessKey:
                        return base.ProcessCmdKey(ref msg, keyData);
                        //direction = Keys.Tab;
                        //reverseKey = Keys.Shift | Keys.Tab;
                        //break;
                    default:
                        // Simply process all other keys normally
                        return base.ProcessCmdKey(ref msg, keyData);
                }

                if (isAllColumnReadOnly())
                {
                    MyProcessCmdKey(direction);
                }
                else
                {
                    if (this.CurrentCell == null && this.Rows.Count > 0)
                        this.CurrentCell = this.Rows[0].Cells[0];

                    if (this.CurrentCell != null)
                    {
                        int currRowIndex = this.CurrentCell.RowIndex;
                        int currColIndex = this.CurrentCell.ColumnIndex;
                        bool found = false;
                        int readonlyCount = 0;
                        switch (direction)
                        {
                            case Keys.Tab:
                                while (!found)
                                {
                                    if (currColIndex == this.Columns.Count - 1)
                                    {
                                        if (currRowIndex == this.Rows.Count - 1)
                                        {
                                            found = true;
                                            readonlyCount = -1;
                                        }
                                        else
                                        {
                                            currColIndex = 0;
                                        }
                                    }
                                    else
                                        currColIndex++;

                                    if (!found && this.Columns[currColIndex].ReadOnly == true)
                                    {
                                        if(this.Columns[currColIndex].Visible == true)
                                            readonlyCount++;
                                    }
                                    else
                                        found = true;
                                }
                                for (int i = 0; i <= readonlyCount; i++)
                                {
                                    MyProcessCmdKey(direction);
                                    
                                }

                                //最終行で行く先がない場合、GridViewのKeydownイベントに処理を任せる

                                if (currColIndex == this.Columns.Count - 1)
                                {
                                    if (currRowIndex == this.Rows.Count - 1)
                                    {
                                        if (readonlyCount <= 0)
                                            this.OnKeyDown(new KeyEventArgs(Keys.Enter));
                                    }
                                }
                                break;
                            case Keys.Shift | Keys.Tab:
                                while (!found)
                                {
                                    if (currColIndex == 0)
                                    {
                                        if (currRowIndex == 0)
                                        {
                                            if (!found && this.Columns[currColIndex].ReadOnly == true)
                                            {
                                                //先頭項目
                                                MyProcessCmdKey(direction);
                                                return true;
                                            }
                                            found = true;
                                            readonlyCount = 1;
                                        }
                                        else
                                        {
                                            currColIndex = this.ColumnCount - 1;
                                        }
                                    }
                                    else
                                        currColIndex--;

                                    if (!found && this.Columns[currColIndex].ReadOnly == true)
                                    {
                                        if (this.Columns[currColIndex].Visible == true)
                                            readonlyCount++;
                                    }
                                    else
                                        found = true;
                                }
                                for (int i = 0; i <= readonlyCount; i++)
                                {
                                    MyProcessCmdKey(direction);
                                }

                                break;
                            case Keys.Up:
                            case Keys.Down:
                                MyProcessCmdKey(direction);
                                break;
                        }
                    }
                }
            }

            return true;

        }
        protected bool MoreDuplicate()
        {
            return true;
        }
        protected  virtual void ErrorCheck()
        {
            

        }
        public void frmKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

            }


        }

        private void MyProcessCmdKey(Keys keyData)
        {
                //if (this.CurrentCell != null && this.CurrentCell.EditedFormattedValue.ToString().Contains("-"))
                //{
                //    if (this.Name == "dgvPETC0302I")
                //    {
                //        ProcessLeftKey(keyData);
                //        ProcessRightKey(keyData);
                //    }
                //}
                //else
                //{
                    switch (keyData)
                    {
                        case Keys.Enter:
                            ProcessTabKey(keyData);
                            break;
                        case Keys.Tab:
                       
                            ProcessDialogKey(keyData);
                            break;
                        
                        case Keys.Tab | Keys.Shift:
                            ProcessTabKey(keyData);
                            break;
                        case Keys.Left:

                            ProcessLeftKey(keyData);
                            break;
                        case Keys.Right:
                            ProcessRightKey(keyData);
                            break;
                        case Keys.Up:
                            ProcessUpKey(keyData);
                            break;
                        case Keys.Down:
                            ProcessDownKey(keyData);
                            break;
                        case Keys.None:
                            break;
                    }
            this.BeginEdit(true);
            //}
            //}
        } // MyProcessCmdKey

        protected override void OnCellValidated(DataGridViewCellEventArgs e)
        {
            if (DuplicateCheckCol != null)
            {
                if (DuplicateCheckCol.Contains(CurrentCell.OwningColumn.Name) && TanaCheck(CurrentCell.OwningColumn.Name))
                {
                    IsSkipped = true;
                }
                else
                {
                    IsSkipped = false;
                }
            }
            base.OnCellValidated(e);
        }
        private void CheckDuplicate()
        {
            if (DuplicateCheckCol != null)
            {
                if (DuplicateCheckCol.Contains(CurrentCell.OwningColumn.Name) && TanaCheck( CurrentCell.OwningColumn.Name))
                {
                    IsSkipped = true;
                }
                else
                {
                    IsSkipped = false;
                }


            }
        }
        private void CKM_GridDesign()
        {
            this.DefaultCellStyle.Font = new Font("MS Gothic", 9, FontStyle.Regular);

            this.EnableHeadersVisualStyles = false;
            this.ColumnHeadersDefaultCellStyle.Font = new Font(this.Font, FontStyle.Bold);
            this.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(191, 191, 191);
            this.BackgroundColor = this.GridColor = Color.FromArgb(198, 224, 180);

            this.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
           // this.RowHeadersVisible = true;

            this.AutoGenerateColumns = false;
            this.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.ColumnHeadersHeight = 25;



        }

        public void DisabledColumn(string DisablecolumnName)
        {
            this.DisablecolName = DisablecolumnName;
            this.EnablecolName = string.Empty;
            SetReadOnly();
        }

        public string[] DuplicateCheckCol = null;

        private bool IsSkipped = false;
        
        public void EnabledColumn(string EnablecolumnName)
        {
            this.EnablecolName = EnablecolumnName;
            this.DisablecolName = string.Empty;
            SetReadOnly();
        }

        private void SetReadOnly()
        {
            foreach(DataGridViewColumn col in Columns)
            {
                ArrayList arrlst = new ArrayList();
                if (!string.IsNullOrWhiteSpace(DisablecolName))
                {
                    if(DisablecolName.Equals("*"))
                        col.ReadOnly = true;
                    else
                    { 
                        string[] arr = this.DisablecolName.Split(',');
                        arrlst.AddRange(arr);

                        if (arrlst.Contains(col.Name))
                            col.ReadOnly = true;
                        else
                            col.ReadOnly = false;
                    }
                }
                if (!string.IsNullOrWhiteSpace(EnablecolName))
                {
                    if (EnablecolName.Equals("*"))
                        col.ReadOnly = false;
                    else
                    {
                        string[] arr = this.EnablecolName.Split(',');
                        arrlst.AddRange(arr);

                        if (arrlst.Contains(col.Name))
                            col.ReadOnly = false;
                        else
                            col.ReadOnly = true;
                    }
                }
            }
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (UseRow)
                {
                    if (e.ColumnIndex == -1 && e.RowIndex == -1)
                    {
                        //セルを描画する
                        e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

                        //行番号を描画する範囲を決定する
                        //e.AdvancedBorderStyleやe.CellStyle.Paddingは無視しています
                        Rectangle indexRect = e.CellBounds;
                        indexRect.Inflate(-2, -2);
                        //行番号を描画する
                        TextRenderer.DrawText(e.Graphics,
                        "No",
                        e.CellStyle.Font,
                        indexRect,
                        e.CellStyle.ForeColor,
                        TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                        //描画が完了したことを知らせる
                        e.Handled = true;
                    }
                    //列ヘッダーかどうか調べる
                    if (e.ColumnIndex < 0 && e.RowIndex >= 0)
                    {
                        //セルを描画する
                        e.Paint(e.ClipBounds, DataGridViewPaintParts.All);

                        //行番号を描画する範囲を決定する
                        //e.AdvancedBorderStyleやe.CellStyle.Paddingは無視しています
                        Rectangle indexRect = e.CellBounds;
                        indexRect.Inflate(-2, -2);
                        //行番号を描画する
                        TextRenderer.DrawText(e.Graphics,
                            (e.RowIndex + 1).ToString(),
                            e.CellStyle.Font,
                            indexRect,
                            e.CellStyle.ForeColor,
                            TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                        //描画が完了したことを知らせる
                        e.Handled = true;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        protected override void OnDataBindingComplete(DataGridViewBindingCompleteEventArgs e)
        {
            //NO1816
            //   if (DataSource != null)
            //    {
            //        this.Focus();
            //    }
            base.OnDataBindingComplete(e);
        }
        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            base.OnCellFormatting(e);

            try
            {
                if (e.CellStyle.BackColor != Color.Yellow)
                {
                    if (!string.IsNullOrWhiteSpace(DisablecolName))
                    {
                        if (DisablecolName.Equals("*"))
                        {
                            e.CellStyle.BackColor = Color.FromArgb(217, 217, 217);
                        }
                        else
                        {
                            string[] arr = this.DisablecolName.Split(',');
                            ArrayList arrlst = new ArrayList();
                            arrlst.AddRange(arr);

                            if (arrlst.Contains(this.Columns[e.ColumnIndex].Name))
                            {
                                e.CellStyle.BackColor = Color.FromArgb(217, 217, 217);
                            }
                            else
                            {
                                if (e.RowIndex % 2 != 0)
                                    e.CellStyle.BackColor = Color.FromArgb(221, 235, 247);
                                else e.CellStyle.BackColor = SystemColors.Window;
                            }
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(EnablecolName))
                    {
                        if (EnablecolName.Equals("*"))
                        {
                            if (e.RowIndex % 2 != 0)
                                e.CellStyle.BackColor = Color.FromArgb(221, 235, 247);
                            else e.CellStyle.BackColor = SystemColors.Window;
                        }
                        else
                        {
                            string[] arr = this.EnablecolName.Split(',');
                            ArrayList arrlst = new ArrayList();
                            arrlst.AddRange(arr);

                            if (arrlst.Contains(this.Columns[e.ColumnIndex].Name))
                            {
                                e.CellStyle.BackColor = SystemColors.Window;
                            }
                            else
                            {
                                e.CellStyle.BackColor = Color.FromArgb(217, 217, 217);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private bool TanaCheck( string colname, string Val= null)
        {
            int e = 0;
            if (Val == null)
            {
                foreach (DataGridViewRow gr in Rows)
                {
                    if ((gr.Cells[colname].EditedFormattedValue != null))
                    {
                        if (gr.Cells[colname].EditedFormattedValue.ToString() == CurrentCell.EditedFormattedValue.ToString())
                        {
                            e++;
                        }
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow gr in Rows)
                {
                    if ((gr.Cells[colname].EditedFormattedValue != null))
                    {
                        if (gr.Cells[colname].EditedFormattedValue.ToString() == Val)
                        {
                            e++;
                        }
                    }
                }
            }
            if (e > 1)
            {
                return true;
            }
            else
                return false;
        }

        public void Hiragana_Column(string columnName)
        {
            string[] arr = columnName.Split(',');
            hiraganaColumn.AddRange(arr);
        }
           
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // CKM_GridView
            // 
           
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        protected override void OnCellEnter(DataGridViewCellEventArgs e)
        {
            base.OnCellEnter(e);
            if (hiraganaColumn.Contains(this.Columns[e.ColumnIndex].Name))
            {
                this.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            }
            else this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
        }

        protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
        {
            // var dtDisplay = new DataTable();
            if (Name == "dgvYuubinBangou")
            {
                DataTable dtDisplay = DataSource as DataTable;
                MasterTouroku_YuubinBangou_BL YuubinBangouBL = new MasterTouroku_YuubinBangou_BL(); 

                Base_BL bb = new Base_BL();
                var dgvYuubinBangou = this;
                if (CurrentCell is DataGridViewTextBoxCell)
                {
                    if (dgvYuubinBangou.CurrentCell == dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"])
                    {
                        //if (!string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()) && string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"].Value.ToString()))
                        //{
                        //    YuubinBangouBL.ShowMessage("E102");
                        //    dgvYuubinBangou.CurrentCell = dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"];
                        //}

                        //if (!string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"].Value.ToString()) && string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()))
                        //{
                          //  YuubinBangouBL.ShowMessage("E102");
                            //try
                            //{
                            //   // dgvYuubinBangou.RefreshEdit();
                            //  //dgvYuubinBangou.CurrentCell = dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"];
                            //}
                            //catch (Exception ex)
                            //{
                            //    var ad = ex.Message;
                            //}
                           // ProcessLeftKey(Keys.Left);
                          //  MyProcessCmdKey(Keys.Shift | Keys.Tab);
                        //}

                        //if (!string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()) && !string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"].Value.ToString()))
                        //{
                        //    DataRow[] drarr = dtDisplay.Select("ZipCD1 = '" + dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value + "' AND ZipCD2 = '" + dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"].Value + "'");
                        //    if (drarr.Length > 1)
                        //    {
                        //        YuubinBangouBL.ShowMessage("E105");
                        //        dgvYuubinBangou.CurrentCell = dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD2"];
                        //    }
                        //}

                    }
                    else if (dgvYuubinBangou.CurrentCell == dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd1"])
                    {
                        //if (!string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()) && string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd1"].Value.ToString()))
                        //{
                        //    YuubinBangouBL.ShowMessage("E102");
                        //    dgvYuubinBangou.CurrentCell = dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd1"];

                        //    // dgvYuubinBangou.BeginEdit(true);
                        //}
                        //else if (string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()) && !string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd1"].Value.ToString()))
                        //{
                        //    dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd1"].Value = null;
                        //}
                    }
                    else if (dgvYuubinBangou.CurrentCell == dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd2"])
                    {
                        //if (string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colZipCD1"].Value.ToString()) && !string.IsNullOrWhiteSpace(dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd2"].Value.ToString()))
                        //{
                        //    dgvYuubinBangou.Rows[e.RowIndex].Cells["colAdd2"].Value = null;
                        //}
                    }

                }
            }
           
            else
            {
                base.OnCellEndEdit(e);
            }
        }
      
        protected override void OnCellValidating(DataGridViewCellValidatingEventArgs e)
        {
            base.OnCellValidating(e);
            //lastKey = false;
        }
    }
}
