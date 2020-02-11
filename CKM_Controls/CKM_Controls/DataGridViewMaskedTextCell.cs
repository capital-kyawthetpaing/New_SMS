using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace SMS.CustomControls
{
    public class DataGridViewMaskedTextBoxCell : DataGridViewTextBoxCell
    { 
        //コンストラクタ
        public DataGridViewMaskedTextBoxCell()
    {
    }

        //編集コントロールを初期化する
    //編集コントロールは別のセルや列でも使いまわされるため、初期化の必要がある
        public override void InitializeEditingControl(
            int rowIndex, object initialFormattedValue,
            DataGridViewCellStyle dataGridViewCellStyle)
    {
        base.InitializeEditingControl(rowIndex,
            initialFormattedValue, dataGridViewCellStyle);

        //編集コントロールの取得
        DataGridViewMaskedTextControl maskedBox =
            this.DataGridView.EditingControl as
            DataGridViewMaskedTextControl;
        if (maskedBox != null)
        {
            //Textを設定
            string maskedText = initialFormattedValue as string;
            maskedBox.Text = maskedText != null ? maskedText : "";
            //カスタム列のプロパティを反映させる
            DataGridViewMaskedTextColumn column =
                this.OwningColumn as DataGridViewMaskedTextColumn;
            if (column != null)
            {
                maskedBox.Mask = column.Mask;
                maskedBox.CheckFormat = column.CheckFormat;
            }
        }
    }

        //編集コントロールの型を指定する
        public override Type EditType
    {
        get
        {
            return typeof(DataGridViewMaskedTextControl);
        }
    }

        //セルの値のデータ型を指定する
    //ここでは、Object型とする
    //基本クラスと同じなので、オーバーライドの必要なし
        public override Type ValueType
    {
        get
        {
            return typeof(object);
        }
    }

        //新しいレコード行のセルの既定値を指定する
        public override object DefaultNewRowValue
    {
        get
        {
            return base.DefaultNewRowValue;
        }
    }
		
    }
}
