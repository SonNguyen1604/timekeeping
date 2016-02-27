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
    public partial class ucListEmployees : UserControl
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-733PMIV;Initial Catalog=FaceDatabase;Integrated Security=True");
        string id;
        rpListEmpl rpListEmp = new rpListEmpl();
        public ucListEmployees()
        {
            InitializeComponent();
            loadData();
           
            repositoryItemButtonEdit1.Click+=repositoryItemButtonEdit1_Click;
        }

        public void loadData()
        {
            con.Open();
            string sql = "select * from Employees";
            SqlCommand com = new SqlCommand(sql, con);
            com.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();

            gridControl1.DataSource = dt;
        }

        private void repositoryItemButtonEdit1_Click(object sender, EventArgs e)
        {
            FormDetailEmployees detail = new FormDetailEmployees(id);
            detail.Show();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            FormEditEmpl editEmpl = new FormEditEmpl(id);
            editEmpl.Show();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            
            try
            {
                DialogResult confirm = System.Windows.Forms.MessageBox.Show("Xóa nhân viên cũng sẽ xóa tất cả thông tin về giờ làm việc của nhân viên đó. Bạn có chắc chắn muốn xóa nhân viên có ID là " + id + " ?", "Xác nhận!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    con.Open();
                    string sql1 = "delete from TimeWorking where EmployeesID=" +id;
                    SqlCommand com = new SqlCommand(sql1, con);
                    com.ExecuteNonQuery();
                    string sql2 = "delete from Employees where ID=" + id;
                    com = new SqlCommand(sql2, con);
                    com.ExecuteNonQuery();
                    System.IO.File.Delete(Application.StartupPath + "/PicNV/id" + id + ".bmp");
                    con.Close();
                    MessageBox.Show("Đã xóa tất cả thông tin về nhân viên có ID là " + id + " !");
                }
            }
            catch
            {
                MessageBox.Show("Bạn chưa chọn nhân viên nào!", "Lỗi", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
          
           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {   
            using (SaveFileDialog saveDialog = new SaveFileDialog()) 
            {
                saveDialog.Filter = "Excel (2003)(.xls)|*.xls|Excel (2010) (.xlsx)|*.xlsx|Pdf File (.pdf)|*.pdf";
                if(saveDialog.ShowDialog()!= DialogResult.Cancel)
                {
                    string exportFilePath = saveDialog.FileName;
                    string fileExtension = new FileInfo(exportFilePath).Extension;

                    switch(fileExtension)
                    {
                        case ".xls":
                            rpListEmp.ExportToXls(exportFilePath);
                            break;
                        case ".xlsx":
                            rpListEmp.ExportToXlsx(exportFilePath);
                            break;
                        case ".pdf":
                            rpListEmp.ExportToPdf(exportFilePath);
                            break;
                    }
                    MessageBox.Show("Đã xuất file vào " + saveDialog.FileName + " thành công!");
                    if(File.Exists(exportFilePath))
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
                        MessageBox.Show("Không thể xuất file!", "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            rpListEmp.PrintPre();
        }
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                var rowHandle = gridView1.FocusedRowHandle;
                id = gridView1.GetRowCellValue(rowHandle, colID).ToString();
            }
           catch
            { }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
