using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SMS.CustomControls
{
    public class DataGridViewDecimalColumn : DataGridViewColumn
    {
        //CellTemplateとするDataGridViewMaskedTextBoxCellオブジェクトを指定して
    //基本クラスのコンストラクタを呼び出す
        public DataGridViewDecimalColumn()
            : base(new DataGridViewDecimalCell())
        {
        }

        private byte decimalPlace = 0;
        private bool usethousandSeparator = true;
        private int maxInputLength = 32767;
        private bool useminus = false;
        /// <summary>
        /// DecimalBoxプロパティに適用する値
        /// </summary>
        public byte DecimalPlace
        {
            get
            {
                return this.decimalPlace;
            }
            set
            {
                this.decimalPlace = value;
            }
        }

        public int MaxInputLength
        {
            get
            {
                return this.maxInputLength;
            }
            set
            {
                this.maxInputLength = value;
            }
        }

        public bool UseMinus
        {
            get { return this.useminus; }
            set { this.useminus = value; }
        }

        public bool UseThousandSeparator
        {
            get { return this.usethousandSeparator; }
            set { this.usethousandSeparator = value; }
        }
        //新しいプロパティを追加しているため、
    // Cloneメソッドをオーバーライドする必要がある
        public override object Clone()
        {
            DataGridViewDecimalColumn col =
                (DataGridViewDecimalColumn)base.Clone();
            col.decimalPlace = this.decimalPlace;
            col.maxInputLength = this.maxInputLength;
            col.usethousandSeparator = this.usethousandSeparator;
            col.useminus = this.useminus;
            return col;
        }

        //CellTemplateの取得と設定
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                //DataGridViewMaskedTextBoxCellしか
                // CellTemplateに設定できないようにする
                if (!(value is DataGridViewDecimalCell))
                {
                    throw new InvalidCastException(
                        "DataGridViewMaskedTextBoxCellオブジェクトを" +
                        "指定してください。");
                }
                base.CellTemplate = value;
            }
        }
    }
}
