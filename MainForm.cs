using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Baitap04_QuanLiSinhVien
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            SetCBBLSH();
            cbbShow.SelectedIndex = 0;// chọn item đầu tiên all
            SetCBBSORT();
        }

        // lay ra lop sinh hoat 
        public List<string> GetLSH()
        {

            List<string> data = new List<string>();
            string query = "SELECT  Distinct Tenlop FROM LopSH";
            SqlConnection cnn = new SqlConnection("Data Source=LAPTOP-JDFPBP30\\SQLEXPRESS;Initial Catalog=QuanLySinhVien;Integrated Security=True");
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                data.Add(row["Tenlop"].ToString());
            }
            return data;
        }
        
        public void SetCBBLSH()
        {
            cbbShow.Items.Add("All");
            cbbShow.Items.AddRange(GetLSH().ToArray());

        }
        public void SetCBBSORT()
        {
            cbbSort.Items.Add("DiemTB");
            cbbSort.Items.Add("MaSV");
            cbbSort.SelectedIndex = 0;
        }
        public void GetALLSV()
        {
            string query = "Select * from SinhVien";
            dtGridViewSv.DataSource = DBHelper.Instance.GetRecords(query);
        }
        // show ra dssv theo all, tung lop
        private void btnShow_Click(object sender, EventArgs e)
        {

            SqlConnection cnn = new SqlConnection("Data Source=LAPTOP-JDFPBP30\\SQLEXPRESS;Initial Catalog=QuanLySinhVien;Integrated Security=True");
            string query = "select * from SinhVien";
            SqlDataAdapter da = new SqlDataAdapter(query, cnn);
            DataSet ds = new DataSet();
            // lua chon lop tu ccb
            string selectedLSH = cbbShow.SelectedItem.ToString();
            // lua chon all de hien thi tat ca
            if (selectedLSH == "All")
            {
                
                GetALLSV();
            }
            // chi hien thi lop duoc chon 
            else
            {
                query = "select  MaSV,NameSV,Tenlop ,NgaySinh, DiemTB, Gender from SinhVien ,LopSH where SinhVien.Malop = LopSH.Malop AND Tenlop = '" + selectedLSH + "'";
             
            }
            da.SelectCommand = new SqlCommand(query, cnn);
            DataTable dt = new DataTable();
            cnn.Open();
            da.Fill(dt);
            cnn.Close();
            dtGridViewSv.DataSource = dt;
        }

        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "";
            SqlConnection cnn = new SqlConnection("Data Source=LAPTOP-JDFPBP30\\SQLEXPRESS;Initial Catalog=QuanLySinhVien;Integrated Security=True");
            SqlDataAdapter da = new SqlDataAdapter(query, cnn);
            if (dtGridViewSv.SelectedRows.Count > 0)
            {
                List<string> del = new List<string>();
                foreach (DataGridViewRow i in dtGridViewSv.SelectedRows)
                {
                    del.Add(i.Cells["MaSV"].Value.ToString());
                }
                foreach (string MaSV in del)
                {
                    query = "delete from SinhVien where MaSV= '" +MaSV + "'";
                    DBHelper.Instance.ExecuteDB(query);
                }
                query = "select * from SinhVien";
                da.SelectCommand = new SqlCommand(query, cnn);
                DataTable dt = new DataTable();
                cnn.Open();
                da.Fill(dt);
                cnn.Close();
                dtGridViewSv.DataSource = dt;
            }
        }

        public void GetSVsearch(string LSH, string MaSV)
        {
            DataTable dt = new DataTable();
            if (txtSearch.Text == "" && cbbShow.Items.IndexOf(LSH) == 0)
            {
                GetALLSV();
            }
            else if (txtSearch.Text == "" && cbbShow.Items.IndexOf(LSH) > 0)
            {
                 string query = "select  MaSV,NameSV,Tenlop ,NgaySinh, DiemTB, Gender from SinhVien ,LopSH where SinhVien.Malop = LopSH.Malop AND Tenlop = '" + LSH + "'";
                
                dtGridViewSv.DataSource = DBHelper.Instance.GetRecords(query);
            }
            else if (txtSearch.Text != "" && cbbShow.Items.IndexOf(LSH) == 0)
            {
                string query = "select  MaSV ,NameSV,Tenlop ,NgaySinh, DiemTB, Gender from SinhVien ,LopSH where SinhVien.Malop = LopSH.Malop AND MaSV = '" + MaSV + "'";
                dtGridViewSv.DataSource = DBHelper.Instance.GetRecords(query);
            }
            else {
                string query = "select  MaSV ,NameSV,Tenlop ,NgaySinh, DiemTB, Gender from SinhVien ,LopSH where SinhVien.Malop = LopSH.Malop AND MaSV = '" + MaSV + "' ,Tenlop = '" + LSH + "' ";
                dtGridViewSv.DataSource = DBHelper.Instance.GetRecords(query);

            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string Tenlop = cbbShow.SelectedItem.ToString(); // lay gia tri duoc chon tu cbb
            string masv = txtSearch.Text; // lay gia tri tu txt
            GetSVsearch(Tenlop, masv);
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            string s = cbbSort.SelectedItem.ToString();
            string query = "select MaSV,NameSV,Gender,DiemTB, Tenlop from SinhVien as sv inner join LopSH as sh on sv.Malop = sh.Malop order by " + s + " asc";
            dtGridViewSv.DataSource = DBHelper.Instance.GetRecords(query);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dtGridViewSv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Chon Sinh Vien can Update");
                return;
              }
            if (dtGridViewSv.SelectedRows.Count == 1)
            {
                string MaSV = dtGridViewSv.SelectedRows[0].Cells["MaSV"].Value.ToString();
                DetailForm f = new DetailForm(MaSV);
                f.ShowDialog();
            }   
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DetailForm f = new DetailForm(null);
            f.ShowDialog();
        }
    } 
}
