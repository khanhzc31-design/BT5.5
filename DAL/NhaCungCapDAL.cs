using System;
using System.Data;
using Microsoft.Data.SqlClient;
using InventoryManagement.DTO;

namespace InventoryManagement.DAL
{
    public class NhaCungCapDAL
    {
        public DataTable GetAll()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT MaNCC, TenNCC, DienThoai, DiaChi FROM NhaCungCap", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public bool Insert(NhaCungCapDTO ncc)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "INSERT INTO NhaCungCap (TenNCC, DienThoai, DiaChi) VALUES (@TenNCC, @DienThoai, @DiaChi)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@TenNCC", SqlDbType.NVarChar) { Value = ncc.TenNCC });
                cmd.Parameters.Add(new SqlParameter("@DienThoai", SqlDbType.VarChar) { Value = ncc.DienThoai });
                cmd.Parameters.Add(new SqlParameter("@DiaChi", SqlDbType.NVarChar) { Value = ncc.DiaChi });

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Update(NhaCungCapDTO ncc)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string sql = "UPDATE NhaCungCap SET TenNCC = @TenNCC, DienThoai = @DienThoai, DiaChi = @DiaChi WHERE MaNCC = @MaNCC";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@TenNCC", SqlDbType.NVarChar) { Value = ncc.TenNCC });
                cmd.Parameters.Add(new SqlParameter("@DienThoai", SqlDbType.VarChar) { Value = ncc.DienThoai });
                cmd.Parameters.Add(new SqlParameter("@DiaChi", SqlDbType.NVarChar) { Value = ncc.DiaChi });
                cmd.Parameters.Add(new SqlParameter("@MaNCC", SqlDbType.Int) { Value = ncc.MaNCC });

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int maNCC)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM NhaCungCap WHERE MaNCC = @MaNCC", conn);
                cmd.Parameters.Add(new SqlParameter("@MaNCC", SqlDbType.Int) { Value = maNCC });

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}