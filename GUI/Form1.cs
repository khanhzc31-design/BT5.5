using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using InventoryManagement.BUS;
using InventoryManagement.DTO;

namespace InventoryManagement
{
    public partial class Form1 : Form
    {
        private VatTuBUS vatTuBUS = new VatTuBUS();
        private NhaCungCapBUS nccBUS = new NhaCungCapBUS();
        private PhieuKhoBUS phieuKhoBUS = new PhieuKhoBUS();

        private List<ChiTietPhieuKhoDTO> listChiTiet = new List<ChiTietPhieuKhoDTO>();
        private BindingSource bsChiTiet = new BindingSource();

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadVatTuData();
            LoadNCCData();
            LoadPhieuKhoCombos();

            bsChiTiet.DataSource = listChiTiet;
            dgvChiTietPhieu.DataSource = bsChiTiet;

            // Events Tab 1
            btnSearchVT.Click += BtnSearchVT_Click;
            btnAddVT.Click += BtnAddVT_Click;
            btnEditVT.Click += BtnEditVT_Click;
            btnDeleteVT.Click += BtnDeleteVT_Click;
            btnAddNCC.Click += BtnAddNCC_Click;

            // Events Tab 2
            btnAddDetail.Click += BtnAddDetail_Click;
            btnRemoveDetail.Click += BtnRemoveDetail_Click;
            btnSavePhieuKho.Click += BtnSavePhieuKho_Click;
            cboLoaiPhieu.SelectedIndexChanged += CboLoaiPhieu_SelectedIndexChanged;

            // Events Tab 3
            btnFilterLowStock.Click += BtnFilterLowStock_Click;
            btnCalcTotalValue.Click += BtnCalcTotalValue_Click;
        }

        #region Tab 1: Danh Mục Vật Tư & Nhà Cung Cấp
        private void LoadVatTuData()
        {
            dgvVatTu.DataSource = vatTuBUS.GetAll();
        }

        private void LoadNCCData()
        {
            dgvNCC.DataSource = nccBUS.GetAll();
        }

        private void BtnSearchVT_Click(object sender, EventArgs e)
        {
            string kw = txtSearchVatTu.Text.Trim();
            dgvVatTu.DataSource = vatTuBUS.Search(kw);
        }

        private void BtnAddVT_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaVT.Text) || string.IsNullOrEmpty(txtTenVT.Text))
            {
                MessageBox.Show("Vui lòng điền đủ Mã VT và Tên VT!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            VatTuDTO vt = new VatTuDTO
            {
                MaVatTu = txtMaVT.Text.Trim(),
                TenVatTu = txtTenVT.Text.Trim(),
                DVT = txtDVT.Text.Trim(),
                SoLuongTon = 0
            };

            if (vatTuBUS.Insert(vt))
            {
                MessageBox.Show("Thêm vật tư thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadVatTuData();
                LoadPhieuKhoCombos();
            }
        }

        private void BtnEditVT_Click(object sender, EventArgs e)
        {
            VatTuDTO vt = new VatTuDTO
            {
                MaVatTu = txtMaVT.Text.Trim(),
                TenVatTu = txtTenVT.Text.Trim(),
                DVT = txtDVT.Text.Trim()
            };

            if (vatTuBUS.Update(vt))
            {
                MessageBox.Show("Cập nhật vật tư thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadVatTuData();
            }
        }

        private void BtnDeleteVT_Click(object sender, EventArgs e)
        {
            string maVT = txtMaVT.Text.Trim();
            if (vatTuBUS.Delete(maVT))
            {
                MessageBox.Show("Xóa vật tư thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadVatTuData();
            }
        }

        private void BtnAddNCC_Click(object sender, EventArgs e)
        {
            NhaCungCapDTO ncc = new NhaCungCapDTO
            {
                TenNCC = txtTenNCC.Text.Trim(),
                DienThoai = txtDienThoai.Text.Trim(),
                DiaChi = txtDiaChi.Text.Trim()
            };

            if (nccBUS.Insert(ncc))
            {
                MessageBox.Show("Thêm Nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadNCCData();
                LoadPhieuKhoCombos();
            }
        }
        #endregion

        #region Tab 2: Lập Phiếu Kho & SqlTransaction
        private void LoadPhieuKhoCombos()
        {
            cboNhaCungCap.DataSource = nccBUS.GetAll();
            cboNhaCungCap.DisplayMember = "TenNCC";
            cboNhaCungCap.ValueMember = "MaNCC";

            cboVatTuSelect.DataSource = vatTuBUS.GetAll();
            cboVatTuSelect.DisplayMember = "TenVatTu";
            cboVatTuSelect.ValueMember = "MaVatTu";
        }

        private void CboLoaiPhieu_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboNhaCungCap.Enabled = (cboLoaiPhieu.SelectedItem?.ToString() == "NHAP");
        }

        private void BtnAddDetail_Click(object sender, EventArgs e)
        {
            if (cboVatTuSelect.SelectedValue == null) return;

            string maVT = cboVatTuSelect.SelectedValue.ToString()!;
            string tenVT = cboVatTuSelect.Text;
            decimal sl = nudSoLuong.Value;
            decimal gia = nudDonGia.Value;

            ChiTietPhieuKhoDTO item = listChiTiet.Find(x => x.MaVatTu == maVT);
            if (item != null)
            {
                item.SoLuong += sl;
                item.DonGia = gia;
            }
            else
            {
                listChiTiet.Add(new ChiTietPhieuKhoDTO
                {
                    MaVatTu = maVT,
                    TenVatTu = tenVT,
                    SoLuong = sl,
                    DonGia = gia
                });
            }

            bsChiTiet.ResetBindings(false);
            CalculateTotalPhieu();
        }

        private void BtnRemoveDetail_Click(object sender, EventArgs e)
        {
            if (dgvChiTietPhieu.CurrentRow != null && dgvChiTietPhieu.CurrentRow.DataBoundItem is ChiTietPhieuKhoDTO item)
            {
                listChiTiet.Remove(item);
                bsChiTiet.ResetBindings(false);
                CalculateTotalPhieu();
            }
        }

        private void CalculateTotalPhieu()
        {
            decimal total = 0;
            foreach (var item in listChiTiet)
            {
                total += item.ThanhTien;
            }
            lblTongTienPhieu.Text = $"TỔNG TIỀN PHIẾU: {total:#,##0} VNĐ";
        }

        private void BtnSavePhieuKho_Click(object sender, EventArgs e)
        {
            if (listChiTiet.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất 1 vật tư vào phiếu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string loai = cboLoaiPhieu.SelectedItem?.ToString() ?? "NHAP";
            int? maNCC = null;
            if (loai == "NHAP" && cboNhaCungCap.SelectedValue != null)
            {
                maNCC = Convert.ToInt32(cboNhaCungCap.SelectedValue);
            }

            decimal totalMoney = 0;
            foreach (var item in listChiTiet) totalMoney += item.ThanhTien;

            PhieuKhoDTO phieu = new PhieuKhoDTO
            {
                LoaiPhieu = loai,
                MaNCC = maNCC,
                TongTien = totalMoney
            };

            bool success = phieuKhoBUS.SavePhieuKhoWithTransaction(phieu, listChiTiet, out string errorMsg);
            if (success)
            {
                MessageBox.Show("Lưu phiếu kho và cập nhật số lượng tồn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listChiTiet.Clear();
                bsChiTiet.ResetBindings(false);
                CalculateTotalPhieu();

                LoadVatTuData();
            }
            else
            {
                MessageBox.Show($"Giao dịch thất bại (SqlTransaction Rollback):\n{errorMsg}", "Lỗi Giao Dịch", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Tab 3: Cảnh Báo Tồn Kho & ExecuteScalar
        private void BtnFilterLowStock_Click(object sender, EventArgs e)
        {
            decimal threshold = nudThreshold.Value;
            dgvLowStock.DataSource = vatTuBUS.GetLowStock(threshold);
        }

        private void BtnCalcTotalValue_Click(object sender, EventArgs e)
        {
            decimal totalVal = vatTuBUS.GetTotalStockValue();
            lblTotalStockValue.Text = $"Tổng giá trị hàng tồn: {totalVal:#,##0} VNĐ";
        }
        #endregion
    }
}