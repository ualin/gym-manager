using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace Gym_Manager
{
    public partial class ReportForm : Form
    {
        public ReportForm(string reportName, ReportDataSource reportDataSource )
        {
            InitializeComponent();

            reportViewer1.LocalReport.ReportEmbeddedResource = "Gym_Manager.Reports." + reportName + ".rdlc";
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            reportViewer1.RefreshReport();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
