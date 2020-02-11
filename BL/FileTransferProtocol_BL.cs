using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using DL;
using System.Data;

namespace BL
{
    public class FileTransferProtocol_BL :Base_BL
    {
        M_MultiPorpose_DL MmultiporposeDataDL;
        D_FTP_DL dftpdl;
        M_VendorFTP_DL mvdl;

        public FileTransferProtocol_BL()
        {
            MmultiporposeDataDL = new M_MultiPorpose_DL();
            dftpdl = new D_FTP_DL();
            mvdl = new M_VendorFTP_DL();
        }

        public DataTable M_MultiPorpose_SelectID(M_MultiPorpose_Entity MmultiporposeData)
        {
            return MmultiporposeDataDL.M_MultiPorpose_Select(MmultiporposeData);
        }
        public bool M_MultiPorpose_Update(M_MultiPorpose_Entity mme)
        {
            return dftpdl.M_MultiPorpose_Update(mme);
        }

        public DataTable D_FTP_SelectAll(string type)
        {
            return dftpdl.D_FTP_SelectAll(type);
        }

        public DataTable M_VendorFTP_ForSelectFile()
        {
            return mvdl.M_VendorFTP_ForSelectFile();
        }

        public bool InsertFiles(D_FTP_Entity dftpe)
        {
            return dftpdl.InsertFiles(dftpe);
        }
    }
}
