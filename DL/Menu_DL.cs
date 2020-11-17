using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Entity;
namespace DL
{
   public class Menu_DL:Base_DL
    {

        public Menu_DL()
        {

        }
        public DataTable SettingGetAllPermission(string val, string admin, string setting, string def)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>() {
            { "@StaffCD",new ValuePair { value1 = SqlDbType.VarChar, value2 = val }},

                 { "@Admin",new ValuePair { value1 = SqlDbType.TinyInt, value2 = admin }},
                 { "@Setting",new ValuePair { value1 = SqlDbType.TinyInt, value2 = setting }},
                 { "@Default",new ValuePair { value1 = SqlDbType.TinyInt, value2 =def }} };
            UseTransaction = true;
            return SelectData(dic, "SettingGetAllPermission");
        }
        public DataTable D_MenuMessageSelect(string Staff_CD)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();

            dic.Add("@Staff_CD", new ValuePair { value1 = SqlDbType.VarChar, value2 = Staff_CD });

            UseTransaction = true;
            return SelectData(dic, "D_MenuMessageSelect");
        }

        public DataTable getMenuNo(string Staff_CD,string IsStored)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>();

            dic.Add("@Staff_CD", new ValuePair { value1 = SqlDbType.VarChar, value2 = Staff_CD });
            dic.Add("@IsStore", new ValuePair { value1 = SqlDbType.TinyInt, value2 =  IsStored });
            UseTransaction = true;
            return SelectData(dic, "HMENU");
        }
        public bool SettingSave(M_Setting ms)
        {
           
            string sp = "SettingSave";
            Dictionary<string, ValuePairBinary> dic = new Dictionary<string, ValuePairBinary>
            {
                //StaffCD,IconName,L_LogoName,M_LogoName,ThemeKBN,MenuKBN,FSKBN,FWKBN,M_HoverKBN,M_NormalKBN,HTopic,Setting_Path,PC,DeleteFlg,InsertDateTime,InsertOperator,UpdateDateTime,UpdateOperator 
                { "@StaffCD",new ValuePairBinary{value1=SqlDbType.VarChar,value2=ms.StaffCD}},

                 { "@IName",new ValuePairBinary{value1=SqlDbType.VarChar,value2=ms.Local_Icon}},
                 { "@LName",new ValuePairBinary{value1=SqlDbType.VarChar,value2=ms.Local_LoginLogo}},
                 { "@MName",new ValuePairBinary{value1=SqlDbType.VarChar,value2=ms.Local_MenuLogo}},
                 { "@IconName",new ValuePairBinary{value1=SqlDbType.VarBinary,value3=ms.Icon}},
                 { "@L_LogoName",new ValuePairBinary{value1=SqlDbType.VarBinary,value3=ms.LoginLogo}},
                 { "@M_LogoName",new ValuePairBinary{value1=SqlDbType.VarBinary,value3=ms.MenuLogo}},
                 { "@ThemeKBN",new ValuePairBinary{value1=SqlDbType.VarChar,value2= ms.Theme}},
                 { "@MenuKBN",new ValuePairBinary{value1=SqlDbType.TinyInt,value2=ms.MenuKBN}},
                 { "@FSKBN",new ValuePairBinary{value1=SqlDbType.TinyInt,value2=ms.FSKBN }},
                 { "@FWKBN",new ValuePairBinary{value1=SqlDbType.TinyInt,value2= ms.FWKBN}},
                 { "@M_HoverKBN",new ValuePairBinary{value1=SqlDbType.TinyInt,value2= ms.HKBN}},
                 { "@M_NormalKBN",new ValuePairBinary{value1=SqlDbType.TinyInt,value2= ms.MKBN}},
                 { "@HTopic",new ValuePairBinary{value1=SqlDbType.VarChar,value2= ms.HText}},
                 { "@Setting_Path",new ValuePairBinary{value1=SqlDbType.VarChar,value2="Setting/"+ Base_DL.iniEntity.DatabaseName}},
                 { "@PC",new ValuePairBinary{value1=SqlDbType.VarChar,value2= ms.PC}},
            };
            return InsertUpdateDeleteBinary(dic, sp);
        }
        public bool SettingDefault(string SCD, string MCD)
        {

            string sp = "SettingDefault";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD",new ValuePair{value1=SqlDbType.VarChar,value2= SCD}},
                  { "@MenuCD",new ValuePair{value1=SqlDbType.VarChar,value2= MCD}},
            };
             return InsertUpdateDeleteData(dic, sp);
        }
        public bool SettingPermissionUpdate(string sc, string date, string xml)
        {

            string sp = "SettingPermissionUpdate";
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@StaffCD",new ValuePair{value1=SqlDbType.VarChar,value2= sc}},
                  { "@Date",new ValuePair{value1=SqlDbType.VarChar,value2= date}},
                  { "@Setting_Path",new ValuePair{value1=SqlDbType.VarChar,value2="Setting/"+ Base_DL.iniEntity.DatabaseName}},
                     { "@PC",new ValuePair { value1 = SqlDbType.VarChar, value2 = System.Environment.MachineName } },
            { "@Xml",new ValuePair{value1=SqlDbType.VarChar,value2= xml}} };
            
         
    
            return InsertUpdateDeleteData(dic, sp);
        }
        public DataTable CheckDefault(string mse, string StaffCD)
        {
            Dictionary<string, ValuePair> dic = new Dictionary<string, ValuePair>
            {
                { "@MenuCD", new ValuePair { value1 = SqlDbType.VarChar, value2 = mse } },
                    { "@StaffCD",new ValuePair{value1=SqlDbType.VarChar,value2= StaffCD}},
            };
            return SelectData(dic, "SettingCheckDefault");
        }
        //SettingDefault

    }
}
