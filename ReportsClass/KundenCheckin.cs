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
    class KundenCheckin
    {
        public static ReportDataSource ReportDataSource()
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter(
@"SELECT c.KundenID,k.Name +' '+ k.Vorname AS Name,c.LockerKey AS Schlussel, c.CheckinTime 
FROM (Checkin AS c
INNER JOIN Kunden AS k 
ON c.KundenID = k.ID) ORDER BY c.CheckinTime", DBTransactions.GetDBConnection());

            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {

            }

            ReportDataSource reportDataSource = new ReportDataSource();

            reportDataSource.Name = "GymDBDataSet";
            reportDataSource.Value = ds.Tables[0];
            

            return reportDataSource;

        }
         
    }
}
