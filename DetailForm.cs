using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baitap04_QuanLiSinhVien
{
    public partial class DetailForm : Form
    {
      
        private string MaSV { get; set; }
        public DetailForm(string m)
        {
            InitializeComponent();
            MaSV = m;
            SetCBBLSH();
            if (MaSV != null)
            {
                setGUI1();
            }
        }
        public List<string> GetLSH()
        {

            List<string> data = new List<string>();
            string query = "Select  Distinct Malop from LopSH";
            SqlConnection cnn = new SqlConnection("Data Source=LAPTOP-JDFPBP30\\SQLEXPRESS;Initial Catalog=QuanLySinhVien;Integrated Security=True");
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, cnn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                data.Add(row["Malop"].ToString());
            }
            return data;
        }

        public void SetCBBLSH()
        {
            
            cbbLSH.Items.AddRange(GetLSH().ToArray());

        }
        public void setGUI1()
        {
            DataTable dt = new DataTable();
            string query = "select * from SinhVien as S inner join LopSH as L on S.Malop = L.Malop where MaSV=@MaSV";
            SqlParameter p = new SqlParameter("@MaSV", MaSV);
            dt = DBHelper.Instance.GetRecords(query, p);
            DataRow r = dt.Rows[0];
            txtMSV.Text = r["MaSV"].ToString();
            txtMSV.ReadOnly = true;
            txtName.Text = r["NameSV"].ToString();
            cbbLSH.Text = r["Tenlop"].ToString();
            txtDTB.Text = r["DiemTB"].ToString();
            Birthday.Value = DateTime.Parse(r["NgaySinh"].ToString());
            if (Convert.ToBoolean(r["Gender"].ToString()))
                btnMale.Checked = true;
            else btnFeMale.Checked = true;
        }
        public void GetCBBLSH()
        {
            DataTable dt = DBHelper.Instance.GetRecords("select * from SinhVien as S inner join LopSH as L on S.Malop = L.Malop ");
            foreach (DataRow row in dt.Rows)
            {
                cbbLSH.Items.Add(row["Tenlop"]);
            }
        }
        public void SetGUI(string MaSV)
        {
            if (MaSV != null)
            {    
            DataTable dt = new DataTable();
            string query = "select  MaSV ,NameSV,Tenlop ,NgaySinh, DiemTB, Gender  from SinhVien as S inner join LopSH as L on S.Malop = L.Malop AND MaSV = '" + MaSV + "'";
            dt = DBHelper.Instance.GetRecords(query);
            txtMSV.Text = dt.Rows[0]["MaSV"].ToString();
            txtName.Text = dt.Rows[0]["NameSV"].ToString();
            if (cbbLSH != null) cbbLSH.Items.Clear();
            GetCBBLSH();
            cbbLSH.SelectedIndex = cbbLSH.Items.IndexOf(dt.Rows[0]["Tenlop"].ToString());
            Birthday.Value = Convert.ToDateTime(dt.Rows[0]["NgaySinh"].ToString());
            txtDTB.Text = dt.Rows[0]["DiemTB"].ToString();
            if (Convert.ToBoolean(dt.Rows[0]["DiemTB"].ToString()))
                     btnMale.Checked = true;
            else btnFeMale.Checked = true;
              }     
          
        }
        
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlParameter[] p = new SqlParameter[6];

            p[0] = new SqlParameter("@MaSV", txtMSV.Text);
            p[1] = new SqlParameter("@NameSV", txtName.Text);
            p[2] = new SqlParameter("@Tenlop", cbbLSH.Text);
            p[3] = new SqlParameter("@NgaySinh", Birthday.Value.ToString());
            p[4] = new SqlParameter("@DiemTB", Convert.ToDecimal(txtDTB.Text));
            p[5] = new SqlParameter("@Gender", btnMale.Checked);
           // string selectedLSH = cbbLSH.SelectedItem.ToString();

            if (MaSV != null)
            {    

                string query = "update SinhVien set NameSV = @NameSV,  NgaySinh =@NgaySinh, DiemTB = @DiemTB, Gender = @Gender where MaSV = @MaSV";
                
                DBHelper.Instance.ExecuteDB(query, p);
                
                this.Dispose();
            }
            else
            {
                List<string> list = new List<string>();
                list = DBHelper.Instance.Get_MSSV();
                foreach (string s in list)
                {
                    if (s == txtMSV.Text)
                    {
                        MessageBox.Show("MSSV bị trùng, Moi ban nhap lai");

                    }
                }
                string query2 = "insert into SinhVien(MaSV,NameSV,NgaySinh,DiemTB,Gender) values (@MaSV, @NameSV, @NgaySinh, @DiemTB, @Gender)";
                DBHelper.Instance.ExecuteDB(query2, p);
                this.Dispose();
            } 
        }
    }
}
