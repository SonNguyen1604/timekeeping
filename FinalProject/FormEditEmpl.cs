using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class FormEditEmpl : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-733PMIV;Initial Catalog=FaceDatabase;Integrated Security=True");
        public FormEditEmpl(string idnv)
        {
            InitializeComponent();
            con.Open();
            string sql = "select * from Employees where ID=" + idnv;
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            txtID.DataBindings.Add("Text", dt, "ID");
            txtHo.DataBindings.Add("Text", dt, "FirstName");
            txtTen.DataBindings.Add("Text", dt, "LastName");
            txtDate.DataBindings.Add("Text", dt, "Birthday");
            if (dt.Rows[0]["Sex"].ToString() == "Nam")
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;          
            txtSDT.DataBindings.Add("Text", dt, "Phone");
            txtDC.DataBindings.Add("Text", dt, "Address");
            txtCV.DataBindings.Add("Text", dt, "Position");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string sex;
            if (radioButton1.Checked==true)
            sex ="Nam";
            else
                sex="Nữ";
            string sql1 = "update Employees set FirstName='"+txtHo.Text+"',LastName='"+txtTen.Text+"',Birthday='"+txtDate.Text+"',Sex='"+sex+"',Position='"+txtCV.Text+"',Phone='"+txtSDT.Text+"',Address='"+txtDC.Text+"' where ID='"+txtID.Text+"'";
            SqlCommand com = new SqlCommand(sql1, con);
            com.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Thay đổi thông tin nhân viên thành công!");
            this.Close();
        }

     
    }
}
