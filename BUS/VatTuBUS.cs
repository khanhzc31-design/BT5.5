using System.Data;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS
{
    public class VatTuBUS
    {
        private VatTuDAL dal = new VatTuDAL();

        public DataTable GetAll() => dal.GetAll();
        public DataTable Search(string keyword) => dal.Search(keyword);
        public bool Insert(VatTuDTO vt) => dal.Insert(vt);
        public bool Update(VatTuDTO vt) => dal.Update(vt);
        public bool Delete(string maVatTu) => dal.Delete(maVatTu);
        public DataTable GetLowStock(decimal threshold) => dal.GetLowStock(threshold);
        public decimal GetTotalStockValue() => dal.GetTotalStockValue();
    }
}