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
    public partial class CKMShop_GridView : DataGridView
    {
        private bool Setting = false;

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
        public bool HeaderVisible_ = true;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Header Visible")]
        [DisplayName("Header Visible")]
        public bool HeaderVisible
        {
            get => HeaderVisible_;
            set => HeaderVisible_ =  ColumnHeadersVisible = value;
        }
        public int RowHeight = 20;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Row Height")]
        [DisplayName("Row Height")]
        public int RowHeight_
        {
            get => RowHeight;
            set => RowHeight = RowTemplate.Height = value;
        }
        public int gv_height=200 ;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Height")]
        [DisplayName("Height")]
        public int Height_
        {
            get => gv_height;
            set => gv_height = Height = value;
        }
        public int gvheader_height = 22;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Header Height")]
        [DisplayName("Header Height")]
        public int HeaderHeight_
        {
            get => gvheader_height;
            set => gvheader_height =   ColumnHeadersHeight =value ;
             
        }

        public int gv_width=200;
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Width")]
        [DisplayName("Width")]
        public int Width_
        {
            get => gv_width;
            set => gv_width = Width = value;
        }
        private AltBackcolor AltBackColor { get; set; }
        public enum AltBackcolor
        {
            White = 0,
            Green = 1,
            DarkGreen = 2,
            Control = 3,
            Window=4
        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Select AlterBackColor")]
        [DisplayName("AlterBack Color")]
        public AltBackcolor AlterBackColor
        {
            get { return AltBackColor; }
            set
            {
                AltBackColor = value;
                switch (AltBackColor)
                {
                    case AltBackcolor.White:
                        this.AlternatingRowsDefaultCellStyle.BackColor =Color.White ;
                        break;
                    case AltBackcolor.Green:
                        this.AlternatingRowsDefaultCellStyle.BackColor =  Color.FromArgb(226, 239, 218);
                        break;
                    case AltBackcolor.DarkGreen:
                        this.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
                        break;
                    case AltBackcolor.Control:
                        this.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Control;
                        break;
                    case AltBackcolor.Window:
                        this.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.Window;
                        break;
                }
            }
        }
        private DBackcolor BackColor_ { get; set; }
        public enum DBackcolor
        {
            White = 0,
            Green = 1,
            DarkGreen = 2,
            Control = 3,
            Window = 4
        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Select BackColor")]
        [DisplayName("Back Color")]
        public DBackcolor BackgroungColor
        {
            get { return BackColor_; }
            set
            {
                BackColor_ = value;
                switch (BackColor_)
                {
                    case DBackcolor.White:
                        this.DefaultCellStyle.BackColor = Color.White;
                        break;
                    case DBackcolor.Green:
                        this.DefaultCellStyle.BackColor = Color.FromArgb(226, 239, 218);
                        break;
                    case DBackcolor.DarkGreen:
                        this.DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
                        break;
                    case DBackcolor.Control:
                        this.DefaultCellStyle.BackColor = SystemColors.Control;
                        break;
                    case DBackcolor.Window:
                        this.DefaultCellStyle.BackColor = SystemColors.Window;
                        break;
                }
            }
        }
        string DisablecolName = string.Empty;
        string EnablecolName = string.Empty;

        private  FontStyle_ GVFontStyle { get; set; }
        public enum FontStyle_
        {
            Regular = 0,
            Italic = 1,
            Bold = 2,
            Strikeout = 3,
            Underline =4
        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Font Style")]
        [DisplayName("Font Style")]

        public FontStyle_ GVFontstyle
        {
            get { return GVFontStyle; }
            set
            {
                GVFontStyle = value;
                modifiedFontStyle();
            }
        }
        public void modifiedFontStyle()
        {

            switch (GVFontstyle)
            {
                case FontStyle_.Regular:
                    Font = new Font("MS Gothic", Font.Size, FontStyle.Regular);
                    break;
                case FontStyle_.Italic:
                    Font = new Font("MS Gothic",  Font.Size,  FontStyle.Italic);
                    break;
                case FontStyle_.Bold:
                    Font = new Font("MS Gothic",  Font.Size,  FontStyle.Bold);
                    break;
                case FontStyle_.Strikeout:
                    Font = new Font("MS Gothic",Font.Size, FontStyle.Strikeout);
                    break;
                case FontStyle_.Underline:
                    Font = new Font("MS Gothic", Font.Size, FontStyle.Underline);
                    break;
            }
        }
        private Font_ FontS_{ get; set; }
        public enum Font_
        {
            Normal = 0,
            Medium = 1,
            Large = 2,
            ExtraLarge = 3,
            
        }
        
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Font Size")]
        [DisplayName("Font Size")]
        public Font_ ShopFontSize
        {
            get { return  FontS_; }
            set
            {
                FontS_ = value;
                modifiedFont();
            }
        }
        public void modifiedFont() {

            switch (ShopFontSize)
            {
                case Font_.Normal:
                   Font = new Font("MS Gothic", 10, Font.Style);
                    break;
                case Font_.Medium:
                    Font = new Font("MS Gothic", 26, Font.Style);
                    break;
                case Font_.Large:
                    Font = new Font("MS Gothic", 24, Font.Style);
                    break;
                case Font_.ExtraLarge:
                    Font = new Font("MS Gothic", 30,   Font.Style);

                    break;
            }
        }

        private DGVBackcolor DGVBackColor { get; set; }
        public enum DGVBackcolor
        {
            White = 0,
            Green = 1,
            DarkGreen = 2,
            Control = 3,
            Window = 4
        }
        [Browsable(true)]
        [Category("CKM Properties")]
        [Description("Select DGVBackColor")]
        [DisplayName("DGVBackground Color")]
        public DGVBackcolor DGVback
        {
            get { return DGVBackColor; }
            set
            {
                DGVBackColor = value;
                switch (DGVBackColor)
                {
                    case DGVBackcolor.White:
                        BackgroundColor = Color.White;
                        break;
                    case DGVBackcolor.Green:
                        BackgroundColor = Color.FromArgb(226, 239, 218);
                        break;
                    case DGVBackcolor.DarkGreen:
                        BackgroundColor = Color.FromArgb(198, 224, 180);
                        break;
                    case DGVBackcolor.Control:
                        BackgroundColor = SystemColors.Control;
                        break;
                    case DGVBackcolor.Window:
                        BackgroundColor = SystemColors.Window;
                        break;
                }
            }
        }


        public CKMShop_GridView()
        {
           // this.DefaultCellStyle.Font = new Font("MS Gothic", 9, FontStyle.Regular);
    
        }
        public void DisableHeaderRowResize()
        {
            this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;

            for (int i = 0; i < this.Columns.Count; i++)
            {
                this.Columns[i].Resizable = DataGridViewTriState.False;
            }


        }

        public void DisabledSorting()
        {
            foreach (DataGridViewColumn column in this.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
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

            return true;
            //if (keyData == Keys.Tab && this[this.CurrentCell.ColumnIndex + 1, this.CurrentCell.RowIndex].ReadOnly == true) //Skip Tab order the readonly column
            //{
            //    this.CurrentCell = this[this.CurrentCell.ColumnIndex + 2, this.CurrentCell.RowIndex];
            //    return true;
            //}
            //else
            //{
            //    return base.ProcessCmdKey(ref msg, keyData);
            //}

            //if (keyData == Keys.Tab && this.EditingControl != null && msg.HWnd == this.EditingControl.Handle)
            //{
            //    return true;
            //}
            //return base.ProcessCmdKey(ref msg, keyData);

        }

        public void frmKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

            }
        }

        private void MyProcessCmdKey(Keys keyData)
        {
            // MessageBox.Show(this.CurrentCell.EditedFormattedValue.ToString());
            if (this.CurrentCell.EditedFormattedValue.ToString().Contains("-") && Name == "dgvPETC0302I")
            {
                //if (this.Name == "dgvPETC0302I")
                //{
                    ProcessLeftKey(keyData);
                    ProcessRightKey(keyData);
                //}
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
        } // MyProcessCmdKey
        private void CKM_GridDesign()
        {
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            if (UseSetting)
            {
                DisabledSorting();
                DisableHeaderRowResize();
                this.EnableHeadersVisualStyles = false;
                this.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(191, 191, 191);
               // this.RowHeadersVisible = true;
                this.AutoGenerateColumns = false;
                this.ScrollBars = System.Windows.Forms.ScrollBars.Both;
                this.AllowUserToDeleteRows = false;
                this.AllowUserToResizeRows = false;
            }
        }

        public void DisabledColumn(string DisablecolumnName)
        {
            this.DisablecolName = DisablecolumnName;
            this.EnablecolName = string.Empty;
            SetReadOnly();
        }

        public void EnabledColumn(string EnablecolumnName)
        {
            this.EnablecolName = EnablecolumnName;
            this.DisablecolName = string.Empty;
            SetReadOnly();
        }

        private void SetReadOnly()
        {
            foreach (DataGridViewColumn col in Columns)
            {
                ArrayList arrlst = new ArrayList();
                if (!string.IsNullOrWhiteSpace(DisablecolName))
                {
                    if (DisablecolName.Equals("*"))
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

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
        }
    }
}
