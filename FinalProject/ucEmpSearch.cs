using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace FinalProject
{
    public partial class ucEmpSearch : UserControl
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-733PMIV;Initial Catalog=FaceDatabase;Integrated Security=True");
        string sql;
        
        DataTable dt = new DataTable();
        public ucEmpSearch()
        {
            InitializeComponent();
            radioButton1.Checked = true;
            dateEdit1.Text = DateTime.Now.ToShortDateString();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            rpListEmployeesSearch rpListEmS = new rpListEmployeesSearch(txtID.Text, dateEdit1.DateTime);

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx|Pdf File (.pdf)|*.pdf";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtension = new FileInfo(exportFilePath).Extension;

                    switch (fileExtension)
                    {
                        case ".xls":
                            gridControl1.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            gridControl1.ExportToXlsx(exportFilePath);
                            break;
                        case ".pdf":
                            gridControl1.ExportToPdf(exportFilePath);
                            break;
                    }
                    MessageBox.Show("Đã xuất file vào " + saveDialog.FileName + " thành công!");
                    if (File.Exists(exportFilePath))
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(exportFilePath);
                        }
                        catch
                        {
                            MessageBox.Show("Không thể mở file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không thể xuất file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            gridControl1.ShowPrintPreview();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
                sql = "select EmployeesID, Date, TimeIn, TimeOut from TimeWorking where EmployeesID='" + txtID.Text + "' and Date='" + dateEdit1.DateTime.Date + "'";
            else
            {
                sql = "select EmployeesID, Date, TimeIn, TimeOut from TimeWorking where EmployeesID='" + txtID.Text + "' and Month(Date)='" + dateEdit1.DateTime.Month + "'";
            }

            try
            {
                con.Open();

                SqlCommand com = new SqlCommand(sql, con);
                com.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dt);
                con.Close();

                gridControl1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
