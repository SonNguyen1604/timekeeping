using Emgu.CV;
using Emgu.CV.Structure;
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
    public partial class FormMain : Form
    {
        ucCamera1 cam1;
      
        public FormMain()
        {
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Skin;
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Office 2010 Blue");

            InitializeComponent();
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            cam1 = new ucCamera1();
            panelControl1.Controls.Add(cam1);
        }
      
        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            ucCamera2 cam2 = new ucCamera2();
            panelControl1.Controls.Add(cam2);
        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            ucListEmployees listEmp = new ucListEmployees();
            panelControl1.Controls.Add(listEmp);
        }

        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            FormAddEmployees add = new FormAddEmployees();
            add.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            ucListEmpDay empDay = new ucListEmpDay();
            panelControl1.Controls.Add(empDay);
        }

        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            panelControl1.Controls.Clear();
            ucListEmpMonth empMonth = new ucListEmpMonth();
            panelControl1.Controls.Add(empMonth);
        }

        private void navBarItem8_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            ucEmpSearch empSearch = new ucEmpSearch();
            panelControl1.Controls.Clear();
            panelControl1.Controls.Add(empSearch);
        }
        
        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLogIn login = new FormLogIn();
            login.Show();
            this.Close();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Ngày:  " + DateTime.Now.Date.ToShortDateString() + "  Giờ hiện tại:  " + DateTime.Now.ToLongTimeString() + "  Trạng thái: SẴN SÀNG";
        }
    }
}
