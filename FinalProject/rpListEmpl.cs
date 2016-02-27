using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace FinalProject
{
    public partial class rpListEmpl : DevExpress.XtraReports.UI.XtraReport
    {
        public rpListEmpl()
        {
            InitializeComponent();
        }
        public void PrintPre()
        {
            this.ShowPreview();
        }

    }
}
