using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data.SqlClient;
using System.Data;

namespace FinalProject
{
    public partial class rpListEmpMonth : DevExpress.XtraReports.UI.XtraReport
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-733PMIV;Initial Catalog=FaceDatabase;Integrated Security=True");
        public rpListEmpMonth(DateTime d)
        {
            InitializeComponent();
            xrLabel9.Text = "DANH SÁCH NHÂN VIÊN TRONG THÁNG " + d.Month+"/"+d.Year;
            con.Open();
            string sql = "select EmployeesID, Date, TimeIn, TimeOut from TimeWorking where Month(Date)='" + d.Month + "'";
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            xrTable1.DataBindings.Add("Text", dt, "EmployeesID");
            

           // ReportPrintTool printtool = new DevExpress.XtraReports.UI.ReportPrintTool(this);
          //  printtool.ShowPreviewDialog();

           
        }

    }
}
