using System.Drawing;
using System.Windows.Forms;

namespace InventoryManagement
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private TabControl tabMain;
        private TabPage tabVatTu, tabPhieuKho, tabBaoCao;

        // Tab 1 Controls (Vật tư & NCC)
        private DataGridView dgvVatTu, dgvNCC;
        private TextBox txtSearchVatTu, txtMaVT, txtTenVT, txtDVT, txtSoLuongTon;
        private TextBox txtTenNCC, txtDienThoai, txtDiaChi;
        private Button btnSearchVT, btnAddVT, btnEditVT, btnDeleteVT;
        private Button btnAddNCC, btnEditNCC, btnDeleteNCC;

        // Tab 2 Controls (Lập Phiếu Kho)
        private ComboBox cboLoaiPhieu, cboNhaCungCap, cboVatTuSelect;
        private NumericUpDown nudSoLuong, nudDonGia;
        private Button btnAddDetail, btnRemoveDetail, btnSavePhieuKho;
        private DataGridView dgvChiTietPhieu;
        private Label lblTongTienPhieu;

        // Tab 3 Controls (Cảnh báo Tồn kho & Báo cáo)
        private NumericUpDown nudThreshold;
        private Button btnFilterLowStock, btnCalcTotalValue;
        private DataGridView dgvLowStock;
        private Label lblTotalStockValue;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabMain = new TabControl { Dock = DockStyle.Fill };
            this.tabVatTu = new TabPage("Quản Lý Danh Mục");
            this.tabPhieuKho = new TabPage("Lập Phiếu Nhập / Xuất Kho");
            this.tabBaoCao = new TabPage("Cảnh Báo & Báo Cáo Tồn Kho");

            // ================= Tab 1 Layout =================
            Label lblSearch = new Label { Text = "Tìm kiếm Vật tư:", Location = new Point(20, 15), AutoSize = true };
            this.txtSearchVatTu = new TextBox { Location = new Point(130, 12), Width = 200 };
            this.btnSearchVT = new Button { Text = "Tìm", Location = new Point(340, 10), Width = 75 };

            this.dgvVatTu = new DataGridView { Location = new Point(20, 45), Size = new Size(500, 300), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };

            Label lblMaVT = new Label { Text = "Mã VT:", Location = new Point(20, 360), AutoSize = true };
            this.txtMaVT = new TextBox { Location = new Point(80, 357), Width = 100 };
            Label lblTenVT = new Label { Text = "Tên VT:", Location = new Point(200, 360), AutoSize = true };
            this.txtTenVT = new TextBox { Location = new Point(260, 357), Width = 150 };
            Label lblDVT = new Label { Text = "ĐVT:", Location = new Point(420, 360), AutoSize = true };
            this.txtDVT = new TextBox { Location = new Point(460, 357), Width = 60 };

            this.btnAddVT = new Button { Text = "Thêm VT", Location = new Point(20, 400), Width = 100 };
            this.btnEditVT = new Button { Text = "Sửa VT", Location = new Point(130, 400), Width = 100 };
            this.btnDeleteVT = new Button { Text = "Xóa VT", Location = new Point(240, 400), Width = 100 };

            this.dgvNCC = new DataGridView { Location = new Point(550, 45), Size = new Size(480, 300), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };

            Label lblTenNCC = new Label { Text = "Tên NCC:", Location = new Point(550, 360), AutoSize = true };
            this.txtTenNCC = new TextBox { Location = new Point(620, 357), Width = 150 };
            Label lblSĐT = new Label { Text = "SĐT:", Location = new Point(780, 360), AutoSize = true };
            this.txtDienThoai = new TextBox { Location = new Point(820, 357), Width = 100 };

            this.btnAddNCC = new Button { Text = "Thêm NCC", Location = new Point(550, 400), Width = 100 };
            this.btnEditNCC = new Button { Text = "Sửa NCC", Location = new Point(660, 400), Width = 100 };
            this.btnDeleteNCC = new Button { Text = "Xóa NCC", Location = new Point(770, 400), Width = 100 };

            this.tabVatTu.Controls.AddRange(new Control[] {
                lblSearch, txtSearchVatTu, btnSearchVT, dgvVatTu, lblMaVT, txtMaVT, lblTenVT, txtTenVT, lblDVT, txtDVT,
                btnAddVT, btnEditVT, btnDeleteVT, dgvNCC, lblTenNCC, txtTenNCC, lblSĐT, txtDienThoai, btnAddNCC, btnEditNCC, btnDeleteNCC
            });

            // ================= Tab 2 Layout =================
            Label lblLoai = new Label { Text = "Loại phiếu:", Location = new Point(20, 20), AutoSize = true };
            this.cboLoaiPhieu = new ComboBox { Location = new Point(100, 17), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            this.cboLoaiPhieu.Items.AddRange(new object[] { "NHAP", "XUAT" });
            this.cboLoaiPhieu.SelectedIndex = 0;

            Label lblNCCSelect = new Label { Text = "Nhà cung cấp:", Location = new Point(250, 20), AutoSize = true };
            this.cboNhaCungCap = new ComboBox { Location = new Point(350, 17), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblVTSelect = new Label { Text = "Vật tư:", Location = new Point(20, 60), AutoSize = true };
            this.cboVatTuSelect = new ComboBox { Location = new Point(100, 57), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };

            Label lblSL = new Label { Text = "Số lượng:", Location = new Point(320, 60), AutoSize = true };
            this.nudSoLuong = new NumericUpDown { Location = new Point(390, 57), Width = 80, Minimum = 1, Maximum = 100000, Value = 1 };

            Label lblGia = new Label { Text = "Đơn giá:", Location = new Point(490, 60), AutoSize = true };
            this.nudDonGia = new NumericUpDown { Location = new Point(550, 57), Width = 120, Maximum = 100000000, Increment = 1000 };

            this.btnAddDetail = new Button { Text = "Thêm Dòng", Location = new Point(690, 55), Width = 90, Height = 28 };
            this.btnRemoveDetail = new Button { Text = "Xóa Dòng", Location = new Point(790, 55), Width = 90, Height = 28 };

            this.dgvChiTietPhieu = new DataGridView { Location = new Point(20, 100), Size = new Size(1000, 360), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };

            this.lblTongTienPhieu = new Label { Text = "TỔNG TIỀN PHIẾU: 0 VNĐ", Location = new Point(20, 480), Font = new Font("Arial", 12, FontStyle.Bold), ForeColor = Color.DarkBlue, AutoSize = true };
            this.btnSavePhieuKho = new Button { Text = "LƯU PHIẾU KHO (SQL TRANSACTION)", Location = new Point(700, 475), Width = 320, Height = 45, BackColor = Color.LightGreen, Font = new Font("Arial", 10, FontStyle.Bold) };

            this.tabPhieuKho.Controls.AddRange(new Control[] {
                lblLoai, cboLoaiPhieu, lblNCCSelect, cboNhaCungCap, lblVTSelect, cboVatTuSelect, lblSL, nudSoLuong, lblGia, nudDonGia,
                btnAddDetail, btnRemoveDetail, dgvChiTietPhieu, lblTongTienPhieu, btnSavePhieuKho
            });

            // ================= Tab 3 Layout =================
            Label lblThresh = new Label { Text = "Ngưỡng tồn kho an toàn:", Location = new Point(20, 20), AutoSize = true };
            this.nudThreshold = new NumericUpDown { Location = new Point(180, 17), Width = 100, Maximum = 10000, Value = 10 };
            this.btnFilterLowStock = new Button { Text = "Lọc Tồn Kho Dưới Ngưỡng", Location = new Point(300, 15), Width = 200, Height = 28 };

            this.dgvLowStock = new DataGridView { Location = new Point(20, 60), Size = new Size(1000, 360), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };

            this.btnCalcTotalValue = new Button { Text = "Tính Tổng Giá Trị Kho (ExecuteScalar)", Location = new Point(20, 440), Width = 280, Height = 40, BackColor = Color.LightSkyBlue, Font = new Font("Arial", 9, FontStyle.Bold) };
            this.lblTotalStockValue = new Label { Text = "Tổng giá trị hàng tồn: 0 VNĐ", Location = new Point(320, 450), Font = new Font("Arial", 12, FontStyle.Bold), ForeColor = Color.DarkRed, AutoSize = true };

            this.tabBaoCao.Controls.AddRange(new Control[] {
                lblThresh, nudThreshold, btnFilterLowStock, dgvLowStock, btnCalcTotalValue, lblTotalStockValue
            });

            this.tabMain.Controls.AddRange(new Control[] { tabVatTu, tabPhieuKho, tabBaoCao });
            this.ClientSize = new Size(1050, 600);
            this.Controls.Add(tabMain);
            this.Name = "Form1";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Hệ Thống Quản Lý Quy Trình Xuất - Nhập Kho (Inventory Management System)";
        }
    }
}