using System.Data;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS
{
    public class NhaCungCapBUS
    {
        private NhaCungCapDAL dal = new NhaCungCapDAL();

        public DataTable GetAll() => dal.GetAll();
        public bool Insert(NhaCungCapDTO ncc) => dal.Insert(ncc);
        public bool Update(NhaCungCapDTO ncc) => dal.Update(ncc);
        public bool Delete(int maNCC) => dal.Delete(maNCC);
    }
}