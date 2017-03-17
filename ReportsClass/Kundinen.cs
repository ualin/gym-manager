using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WinForms;
using System.Data.OleDb;
using System.Data;
using Gym_Manager.DBAccess;

namespace Gym_Manager.ReportsClass
{
    public static class Kundinen
    {
        public static ReportDataSource KundinenDataSource()
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT Name,Vorname FROM Kunden",DBTransactions.GetDBConnection());

            try
            {
                da.Fill(ds);
            }
            catch(Exception ex)
            {

            }

            ReportDataSource reportDataSource = new ReportDataSource();

            reportDataSource.Name = "GymDBDataSet";
            reportDataSource.Value = ds.Tables[0];

            return reportDataSource;
            
        }
         
        
        
                    
            
    
       
    }
}
