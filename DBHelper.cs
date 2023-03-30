using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baitap04_QuanLiSinhVien
{
    internal class DBHelper
    {
        private SqlConnection _cnn;
        private static DBHelper instance;

        public static DBHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBHelper("Data Source=LAPTOP-JDFPBP30\\SQLEXPRESS;Initial Catalog=QuanLySinhVien;Integrated Security=True");
                }
                return instance;
            }

            private set
            { }
        }
        // đồng bộ ngược qua executeDB
        private DBHelper(string s)
        {
            _cnn = new SqlConnection(s);
        }
        public DataTable GetRecords(string query)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(query, _cnn);
            _cnn.Open();
            da.Fill(dt);
            _cnn.Close();
            return dt;
        }
        public DataTable GetRecords(string query, SqlParameter p)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(query, _cnn);
            cmd.Parameters.Add(p);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            _cnn.Open();
            da.Fill(dt);
            _cnn.Close();
            return dt;
        }
        public List<string> Get_MSSV()
        {
            List<string> data = new List<string>();
            DataTable dt = GetRecords("select * from SinhVien");
            foreach (DataRow dr in dt.Rows)
            {
                data.Add(dr["MaSV"].ToString());
            }

            return data.Distinct().ToList();
        }
        public void ExecuteDB(string query)
        {
            SqlCommand cmd = new SqlCommand(query, _cnn);
            _cnn.Open();
            cmd.ExecuteNonQuery();
            _cnn.Close();
        }
        public void ExecuteDB(string query, SqlParameter p)
        {
            SqlCommand cmd = new SqlCommand(query, _cnn);
            cmd.Parameters.Add(p);
            _cnn.Open();
            cmd.ExecuteNonQuery();
            _cnn.Close();
        }
        public void ExecuteDB(string query, SqlParameter[] p)
        {
            SqlCommand cmd = new SqlCommand(query, _cnn);
            foreach (SqlParameter sp in p)
            {
                cmd.Parameters.Add(sp);
            }
            _cnn.Open();
            cmd.ExecuteNonQuery();
            _cnn.Close();
        }

    }
}
