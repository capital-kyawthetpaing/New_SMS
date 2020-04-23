﻿using BL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
        MasterTouroku_Souko_BL mtsbl;
        string DisablecolName = string.Empty;
        string EnablecolName = string.Empty;
        string columnName = string.Empty;
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
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)

        {
            CheckDuplicate();
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
                    case Keys.Enter:
                        direction = Keys.Tab;
                        reverseKey = Keys.Shift | Keys.Tab;
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

                                    if (this.Columns[currColIndex].ReadOnly == true)
                                    {
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
                            case Keys.Shift | Keys.Tab:
                                while (!found)
                                {
                                    if (currColIndex == 0)
                                    {
                                        if (currRowIndex == 0)
                                        {
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

                                    if (this.Columns[currColIndex].ReadOnly == true)
                                    {
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

        public void frmKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

            }


        }

        private void MyProcessCmdKey(Keys keyData)
        {
                if (this.CurrentCell.EditedFormattedValue.ToString().Contains("-"))
                {
                    if (this.Name == "dgvPETC0302I")
                    {
                        ProcessLeftKey(keyData);
                        ProcessRightKey(keyData);
                    }
                }
                else
                {
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
                }
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
            this.RowHeadersVisible = true;

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
            if (UseRow)
            { 
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

        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            base.OnCellFormatting(e);
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
        
        private bool TanaCheck( string colname)
        {
            int e = 0;
            foreach (DataGridViewRow gr in Rows)
            {
                if ((gr.Cells[colname].EditedFormattedValue != null))
                    if (gr.Cells[colname].EditedFormattedValue.ToString() ==CurrentCell.EditedFormattedValue.ToString())
                    {
                        e++;
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
    }
}
