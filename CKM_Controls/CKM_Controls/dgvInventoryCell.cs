using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMS.CustomControls
{
    public class dgvInventoryCell : DataGridViewTextBoxCell
    {
        //コンストラクタ
        public dgvInventoryCell()
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
                dgvInventoryControl inventoryBox =
                this.DataGridView.EditingControl as
                dgvInventoryControl;
                if (inventoryBox != null)
                {
                    //Textを設定
                    string text = initialFormattedValue as string;
                    inventoryBox.Text = text != null ? text : "";
                    //カスタム列のプロパティを反映させる
                    dgvInventoryColumn column =
                    this.OwningColumn as dgvInventoryColumn;
                    if (column != null)
                    {
                        inventoryBox.MaxLength = column.MaxInputLength;
                        inventoryBox.UseThousandSeparator = column.UseThousandSeparator;
                        inventoryBox.txt_Type = (dgvInventoryControl.Type)column.TxtType;
                    }
                }
            }

        //編集コントロールの型を指定する
        public override Type EditType
        {
            get
            {
                return typeof(dgvInventoryControl);
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
