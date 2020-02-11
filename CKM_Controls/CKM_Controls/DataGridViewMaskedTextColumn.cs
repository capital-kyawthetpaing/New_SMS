using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace SMS.CustomControls
{
    public class DataGridViewMaskedTextColumn : DataGridViewColumn
    {
        //CellTemplateとするDataGridViewMaskedTextBoxCellオブジェクトを指定して
    //基本クラスのコンストラクタを呼び出す
        public DataGridViewMaskedTextColumn()
            : base(new DataGridViewMaskedTextBoxCell())
    {
    }

        private string checkFormat=string.Empty;
        public string CheckFormat
        {
            get { return this.checkFormat; }
            set { this.checkFormat = value; }
        }

        private string maskValue = "";
        /// <summary>
    /// MaskedTextBoxのMaskプロパティに適用する値
    /// </summary>
        public string Mask
    {
        get
        {
            return this.maskValue;
        }
        set
        {
            this.maskValue = value;
        }
    }

        //新しいプロパティを追加しているため、
    // Cloneメソッドをオーバーライドする必要がある
        public override object Clone()
    {
        DataGridViewMaskedTextColumn col =
            (DataGridViewMaskedTextColumn)base.Clone();
        col.Mask = this.Mask;
        col.CheckFormat = this.CheckFormat;
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
            if (!(value is DataGridViewMaskedTextBoxCell))
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
