using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class FormDetailEmployees : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-733PMIV;Initial Catalog=FaceDatabase;Integrated Security=True");
        public FormDetailEmployees(string idnv)
        {
            InitializeComponent();
            con.Open();
            string sql = "select * from Employees where ID="+idnv;
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);

            lbID.DataBindings.Add("Text", dt, "ID");
            lbHo.DataBindings.Add("Text", dt, "FirstName");
            lbTen.DataBindings.Add("Text", dt, "LastName");
            lbNS.DataBindings.Add("Text", dt, "Birthday");
            lbGT.DataBindings.Add("Text", dt, "Sex");
            lbSDT.DataBindings.Add("Text", dt, "Phone");
            lbDiachi.DataBindings.Add("Text", dt, "Address");
            lbCV.DataBindings.Add("Text", dt, "Position");

          //  FileStream f = new FileStream("imgnv.bmp", FileMode.Create);
          //  byte[] buff = (byte[]) dt.Rows[0]["Image"];
          //  f.Write(buff, 0, buff.Length);
           // f.Close();
            pictureBox1.ImageLocation = Application.StartupPath + "/PicNV/id" + idnv + ".bmp";
            
        }
    }
}
