using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using System.Data;

namespace FinalProject
{
    public partial class rpLED : DevExpress.XtraReports.UI.XtraReport
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-733PMIV;Initial Catalog=FaceDatabase;Integrated Security=True");
        public rpLED(DateTime d)
        {
            InitializeComponent();
            
            xrLabel9.Text = "DANH SÁCH NHÂN VIÊN TRONG NGÀY " + d.ToString("dd/MM/yyyy");
            con.Open();
            string sql = "select EmployeesID, Date, TimeIn, TimeOut from TimeWorking where Date='" + d + "'";
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            xrLabel5.DataBindings.Add("Text", dt, "EmployeesID");
            xrLabel6.Text = d.ToString("dd/MM/yyyy");
            xrLabel7.DataBindings.Add("Text", dt, "TimeIn");
            xrLabel8.DataBindings.Add("Text", dt, "TimeOut");
        }

    }
}
