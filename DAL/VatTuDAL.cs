using System;
using System.Data;
using Microsoft.Data.SqlClient;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL
{
    public class VatTuDAL
    {
        public DataTable GetAll()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT MaVatTu, TenVatTu, DVT, SoLuongTon FROM VatTu", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Tìm kiếm Vật tư chống SQL Injection bằng SqlParameter
        public DataTable Search(string keyword)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT MaVatTu, TenVatTu, DVT, SoLuongTon FROM VatTu WHERE MaVatTu LIKE @KW OR TenVatTu LIKE @KW";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@KW", SqlDbType.NVarChar) { Value = "%" + keyword + "%" });

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool Insert(VatTuDTO vt)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "INSERT INTO VatTu (MaVatTu, TenVatTu, DVT, SoLuongTon) VALUES (@Ma, @Ten, @DVT, @Ton)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar) { Value = vt.MaVatTu });
                cmd.Parameters.Add(new SqlParameter("@Ten", SqlDbType.NVarChar) { Value = vt.TenVatTu });
                cmd.Parameters.Add(new SqlParameter("@DVT", SqlDbType.NVarChar) { Value = vt.DVT });
                cmd.Parameters.Add(new SqlParameter("@Ton", SqlDbType.Decimal) { Value = vt.SoLuongTon });

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(VatTuDTO vt)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "UPDATE VatTu SET TenVatTu = @Ten, DVT = @DVT, SoLuongTon = @Ton WHERE MaVatTu = @Ma";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Ten", SqlDbType.NVarChar) { Value = vt.TenVatTu });
                cmd.Parameters.Add(new SqlParameter("@DVT", SqlDbType.NVarChar) { Value = vt.DVT });
                cmd.Parameters.Add(new SqlParameter("@Ton", SqlDbType.Decimal) { Value = vt.SoLuongTon });
                cmd.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar) { Value = vt.MaVatTu });

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(string maVatTu)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM VatTu WHERE MaVatTu = @Ma", conn);
                cmd.Parameters.Add(new SqlParameter("@Ma", SqlDbType.VarChar) { Value = maVatTu });

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Lọc vật tư có số lượng tồn dưới ngưỡng an toàn
        public DataTable GetLowStock(decimal threshold)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "SELECT MaVatTu, TenVatTu, DVT, SoLuongTon FROM VatTu WHERE SoLuongTon <= @Threshold";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@Threshold", SqlDbType.Decimal) { Value = threshold });

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Sử dụng ExecuteScalar để tính tổng giá trị hàng hóa tồn kho dựa trên đơn giá nhập gần nhất
        public decimal GetTotalStockValue()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = @"
            SELECT ISNULL(SUM(v.SoLuongTon * ISNULL(ct.DonGia, 0)), 0)
            FROM VatTu v
            OUTER APPLY (
                SELECT TOP 1 DonGia 
                FROM ChiTietPhieuKho c 
                WHERE c.MaVatTu = v.MaVatTu 
                ORDER BY MaPhieu DESC
            ) ct";
                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToDecimal(result) : 0;
            }
        }
    }
}