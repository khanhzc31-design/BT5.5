# BÁO CÁO BÀI TẬP 5.5: HỆ THỐNG QUẢN LÝ QUY TRÌNH XUẤT - NHẬP KHO NGUYÊN VẬT LIỆU (INVENTORY MANAGEMENT SYSTEM)

## MỤC 1: THÔNG TIN SINH VIÊN
- **Họ và tên sinh viên:** LÊ ĐỨC KHÁNH
- **Mã sinh viên:** 24810310438
- **Tên Đề tài / Bài tập:** Bài tập 5.5 - Hệ thống Quản lý Quy trình Xuất - Nhập Kho Nguyên Vật Liệu
- **Mô hình kiến trúc:** 3-Tier Architecture (GUI - BUS - DAL - DTO)

---

## MỤC 2: HƯỚNG DẪN CÀI ĐẶT & CHẠY DỰ ÁN

### Các bước cài đặt:
1. Mở **SQL Server Management Studio (SSMS)**, chạy file `Database/ScriptDB.sql` để khởi tạo CSDL `InventoryDB` và dữ liệu mẫu.
2. Mở file `InventoryManagement.sln` bằng **Visual Studio 2022**.
3. Mở file `DAL/DatabaseHelper.cs`, cập nhật lại chuỗi kết nối `ConnectionString` cho phù hợp với SQL Server máy cá nhân.
4. Cài đặt gói `Microsoft.Data.SqlClient`.
5. Bấm `F5` hoặc gõ `dotnet run` trong Terminal để **Build & Run** dự án.

---

## MỤC 3: HÌNH ẢNH KẾT QUẢ CÁC MÀN HÌNH CHỨC NĂNG

1. **Màn hình Danh mục Vật tư & Nhà cung cấp (Tra cứu chống SQL Injection):**
<img width="1050" height="636" alt="image" src="https://github.com/user-attachments/assets/36cb4a67-eaf5-44ae-b9be-24cda556c595" />


2. **Màn hình Quản lý Danh mục (Thêm / Sửa / Xóa):**
<img width="1050" height="638" alt="image" src="https://github.com/user-attachments/assets/55ee582c-9507-4dfe-8286-bb6fa5f41b8f" />


3. **Màn hình Lập Phiếu Nhập / Xuất Kho:**
<img width="1052" height="639" alt="image" src="https://github.com/user-attachments/assets/d78af16a-5f1e-42ba-b2a0-5caae349ccf2" />


4. **Màn hình Cảnh báo Tồn kho & Tính tổng giá trị kho (ExecuteScalar):**
<img width="1051" height="639" alt="image" src="https://github.com/user-attachments/assets/a58a1da4-57db-413f-a049-bea6d35c9713" />

5. **Minh họa Báo lỗi Giao dịch Rollback (Không đủ hàng xuất kho):**
<img width="1055" height="637" alt="image" src="https://github.com/user-attachments/assets/2111b0b9-191f-4b65-9cfd-41ef70a384f9" />
