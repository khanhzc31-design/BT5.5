using System.Collections.Generic;
using InventoryManagement.DAL;
using InventoryManagement.DTO;

namespace InventoryManagement.BUS
{
    public class PhieuKhoBUS
    {
        private PhieuKhoDAL dal = new PhieuKhoDAL();

        public bool SavePhieuKhoWithTransaction(PhieuKhoDTO phieu, List<ChiTietPhieuKhoDTO> listChiTiet, out string errorMessage)
        {
            return dal.SavePhieuKhoWithTransaction(phieu, listChiTiet, out errorMessage);
        }
    }
}