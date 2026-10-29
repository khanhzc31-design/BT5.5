using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL
{
    public class PhieuKhoDAL
    {
        public bool SavePhieuKhoWithTransaction(PhieuKhoDTO phieu, List<ChiTietPhieuKhoDTO> listChiTiet, out string errorMessage)
        {
            errorMessage = string.Empty;
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    // 1. Chèn bản ghi vào bảng PhieuKho
                    string sqlPhieu = @"INSERT INTO PhieuKho (LoaiPhieu, NgayLap, MaNCC, TongTien) 
                                        OUTPUT INSERTED.MaPhieu 
                                        VALUES (@LoaiPhieu, GETDATE(), @MaNCC, @TongTien)";
                    SqlCommand cmdPhieu = new SqlCommand(sqlPhieu, conn, trans);
                    cmdPhieu.Parameters.Add(new SqlParameter("@LoaiPhieu", SqlDbType.NVarChar) { Value = phieu.LoaiPhieu });
                    cmdPhieu.Parameters.Add(new SqlParameter("@MaNCC", SqlDbType.Int) { Value = (object?)phieu.MaNCC ?? DBNull.Value });
                    cmdPhieu.Parameters.Add(new SqlParameter("@TongTien", SqlDbType.Decimal) { Value = phieu.TongTien });

                    int maPhieu = Convert.ToInt32(cmdPhieu.ExecuteScalar());

                    // 2 & 3. Chèn ChiTietPhieuKho & Cập nhật kho
                    foreach (var ct in listChiTiet)
                    {
                        // Chèn CTPK
                        string sqlCT = @"INSERT INTO ChiTietPhieuKho (MaPhieu, MaVatTu, SoLuong, DonGia) 
                                         VALUES (@MaPhieu, @MaVatTu, @SoLuong, @DonGia)";
                        SqlCommand cmdCT = new SqlCommand(sqlCT, conn, trans);
                        cmdCT.Parameters.Add(new SqlParameter("@MaPhieu", SqlDbType.Int) { Value = maPhieu });
                        cmdCT.Parameters.Add(new SqlParameter("@MaVatTu", SqlDbType.VarChar) { Value = ct.MaVatTu });
                        cmdCT.Parameters.Add(new SqlParameter("@SoLuong", SqlDbType.Decimal) { Value = ct.SoLuong });
                        cmdCT.Parameters.Add(new SqlParameter("@DonGia", SqlDbType.Decimal) { Value = ct.DonGia });
                        cmdCT.ExecuteNonQuery();

                        // Cập nhật kho
                        if (phieu.LoaiPhieu == "NHAP")
                        {
                            string sqlUpdate = "UPDATE VatTu SET SoLuongTon = SoLuongTon + @SoLuong WHERE MaVatTu = @MaVatTu";
                            SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn, trans);
                            cmdUpdate.Parameters.Add(new SqlParameter("@SoLuong", SqlDbType.Decimal) { Value = ct.SoLuong });
                            cmdUpdate.Parameters.Add(new SqlParameter("@MaVatTu", SqlDbType.VarChar) { Value = ct.MaVatTu });
                            cmdUpdate.ExecuteNonQuery();
                        }
                        else if (phieu.LoaiPhieu == "XUAT")
                        {
                            // Kiểm tra số lượng tồn kho trước khi xuất
                            string sqlCheck = "SELECT SoLuongTon FROM VatTu WHERE MaVatTu = @MaVatTu";
                            SqlCommand cmdCheck = new SqlCommand(sqlCheck, conn, trans);
                            cmdCheck.Parameters.Add(new SqlParameter("@MaVatTu", SqlDbType.VarChar) { Value = ct.MaVatTu });
                            object result = cmdCheck.ExecuteScalar();

                            decimal currentStock = result != null ? Convert.ToDecimal(result) : 0;
                            if (currentStock < ct.SoLuong)
                            {
                                trans.Rollback();
                                errorMessage = $"Không đủ hàng trong kho cho vật tư '{ct.MaVatTu}'! Tồn hiện tại: {currentStock}, yêu cầu xuất: {ct.SoLuong}.";
                                return false;
                            }

                            string sqlUpdate = "UPDATE VatTu SET SoLuongTon = SoLuongTon - @SoLuong WHERE MaVatTu = @MaVatTu";
                            SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn, trans);
                            cmdUpdate.Parameters.Add(new SqlParameter("@SoLuong", SqlDbType.Decimal) { Value = ct.SoLuong });
                            cmdUpdate.Parameters.Add(new SqlParameter("@MaVatTu", SqlDbType.VarChar) { Value = ct.MaVatTu });
                            cmdUpdate.ExecuteNonQuery();
                        }
                    }

                    // 4. Thành công -> Commit
                    trans.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    errorMessage = "Lỗi trong quá trình giao dịch: " + ex.Message;
                    return false;
                }
            }
        }
    }
}