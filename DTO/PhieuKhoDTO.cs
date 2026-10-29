using System;

namespace InventoryManagement.DTO
{
    public class PhieuKhoDTO
    {
        public int MaPhieu { get; set; }
        public string LoaiPhieu { get; set; } = "NHAP"; // "NHAP" hoặc "XUAT"
        public DateTime NgayLap { get; set; } = DateTime.Now;
        public int? MaNCC { get; set; }
        public decimal TongTien { get; set; }
    }
}