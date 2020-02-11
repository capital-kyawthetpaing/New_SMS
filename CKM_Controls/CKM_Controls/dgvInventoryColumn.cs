using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS.CustomControls
{
    public class dgvInventoryColumn : DataGridViewColumn
    {
        //CellTemplateとするDataGridViewMaskedTextBoxCellオブジェクトを指定して
    //基本クラスのコンストラクタを呼び出す
        public dgvInventoryColumn()
            : base(new dgvInventoryCell())
        {
        }
        private int maxInputLength = 32767;
        private Type txtType = Type.Normal;
        public Type TxtType
        {
            get { return this.txtType; }
            set { this.txtType = value; }
        }
        public enum Type
        {
            Normal = 0,
            IntegerOnly = 1,
            Hiragana = 2,
            Decimal = 3
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

        private bool usethousandSeparator = true;
        public bool UseThousandSeparator
        {
            get { return this.usethousandSeparator; }
            set { this.usethousandSeparator = value; }
        }
        //新しいプロパティを追加しているため、
    // Cloneメソッドをオーバーライドする必要がある
        public override object Clone()
        {
            dgvInventoryColumn col =
                (dgvInventoryColumn)base.Clone();
            col.maxInputLength = this.maxInputLength;
            col.UseThousandSeparator = this.usethousandSeparator;
            col.txtType = this.txtType;
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
                if (!(value is dgvInventoryCell))
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
