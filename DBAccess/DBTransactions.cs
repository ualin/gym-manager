using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace Gym_Manager.DBAccess
{
    public static class DBTransactions
    {
        private static OleDbConnection connEB = new OleDbConnection();

        public static OleDbConnection GetDBConnection()
        {
            if (connEB.State != System.Data.ConnectionState.Open)
            {
                try
                {
                    ////String connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=DB/GymDB.accdb; Jet OLEDB:Database Password='fitnesschef'";
                    String connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=DB/GymDB.accdb;";
                    connEB.ConnectionString = connectionString;
                    connEB.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return connEB;
        }
        public static string GetPass()
        {
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText="SELECT pass FROM  Table1 ";
            cmd.Connection=GetDBConnection();
            string pass="";

            try
            {
                pass=cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return pass;
        }
        public static DataTable GetKudinName()
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT ID, Name+' '+Vorname AS Name FROM Kunden ORDER BY Name", GetDBConnection());
            List<string> nameList = new List<string>();

            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return ds.Tables[0] ;
        }

        public static DataTable GetSports()
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT ID, Name FROM Sports ", GetDBConnection());

            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return ds.Tables[0];
        }

        public static DBTables.Kunden GetKundenDetails(int ID)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DBTables.Kunden kunden = new DBTables.Kunden();

            OleDbDataAdapter da = new OleDbDataAdapter(
           @"SELECT ID, Name, Vorname, Photo, HandyNr, Strasse,Plz,Ort,GebDatum, Active
            FROM Kunden
            WHERE ID=" + ID, GetDBConnection());
            try
            {
                da.Fill(ds);
                dt=ds.Tables[0];

                foreach (DataRow rows in dt.Rows)
                {
                    kunden.ID = int.Parse(rows["ID"].ToString().Trim());
                    kunden.Name = rows["Name"].ToString().Trim();
                    kunden.Vorname = rows["Vorname"].ToString().Trim();
                    kunden.Photo = rows["Photo"].ToString().Trim();
                    kunden.HandyNr = rows["HandyNr"].ToString().Trim();
                    kunden.Active = rows["Active"].ToString().Trim();
                    kunden.Strasse = rows["Strasse"].ToString().Trim();
                    kunden.Ort = rows["Ort"].ToString().Trim();
                    kunden.Plz = rows["Plz"].ToString().Trim();
                    kunden.GebDatum =rows["GebDatum"].ToString()==""?DateTime.Now:DateTime.Parse(rows["GebDatum"].ToString());
              }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return kunden;
        }
        public static List<DBTables.VertragDetailed> GetKundenVertragList(int ID)
        {
            List<DBTables.VertragDetailed> vertragList = new List<DBTables.VertragDetailed>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DBTables.VertragDetailed vertrag;
            OleDbDataAdapter da = new OleDbDataAdapter(
            @"SELECT k.ID, k.Name, k.Vorname, s.Name AS Sport, v.SportID, v.Anfang, v.Schluss, v.ID AS VertragID, k.Photo 
            FROM 
            ((Vertrag AS v 
            INNER JOIN Sports AS s ON v.SportID=s.ID) 
            INNER JOIN Kunden AS k ON v.KundenID=k.ID) 
            WHERE v.KundenID=" + ID, GetDBConnection());

            try
            {
                da.Fill(ds);
                dt = ds.Tables[0];

                foreach (DataRow rows in dt.Rows)
                {
                    vertrag = new DBTables.VertragDetailed();

                    vertrag.KundenID = int.Parse(rows["ID"].ToString().Trim());
                    vertrag.Name = rows["Name"].ToString().Trim();
                    vertrag.Vorname = rows["Vorname"].ToString().Trim();
                    vertrag.Photo = rows["Photo"].ToString().Trim();
                    vertrag.Sport = rows["Sport"].ToString().Trim();
                    vertrag.SportID = int.Parse(rows["SportID"].ToString());
                    vertrag.Anfang = DateTime.Parse(rows["Anfang"].ToString());
                    vertrag.Schluss = DateTime.Parse(rows["Schluss"].ToString());
                    vertrag.VertragID = int.Parse(rows["VertragID"].ToString());

                    vertragList.Add(vertrag);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return vertragList ;
        }
        public static DataTable GetCheckinList(int ID)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(
            @"SELECT c.KundenID,k.Active, k.Name, k.Vorname,k.Strasse, k.Ort,k.Plz,k.HandyNr, c.LockerKey AS Schlussel, c.CheckinTime
            FROM 
            (Checkin AS c 
            INNER JOIN Kunden AS k ON c.KundenID=k.ID)
             
            WHERE c.KundenID=" + ID + " ORDER BY c.CheckinTime", GetDBConnection());

            try
            {
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public static DataTable GetCheckinList(string schlussel)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(
            @"SELECT c.KundenID,k.Active, k.Name, k.Vorname,k.Strasse, k.Ort,k.Plz,k.HandyNr, c.LockerKey AS Schlussel, c.CheckinTime
            FROM 
            (Checkin AS c 
            INNER JOIN Kunden AS k ON c.KundenID=k.ID)
             
            WHERE c.LockerKey=@schlussel ORDER BY c.CheckinTime", GetDBConnection());
            da.SelectCommand.Parameters.Add("@schlussel", OleDbType.VarChar).Value=schlussel;

            try
            {
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public static DataTable GetCheckinList(DateTime start, DateTime end)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            OleDbCommand cmd = new OleDbCommand();
            OleDbDataAdapter da = new OleDbDataAdapter(
            @"SELECT c.KundenID, k.Active, k.Name, k.Vorname,k.Strasse, k.Ort,k.Plz,k.HandyNr, c.LockerKey AS Schlussel, c.CheckinTime
            FROM 
            (Checkin AS c 
            INNER JOIN Kunden AS k ON c.KundenID=k.ID)
             
            WHERE c.CheckinTime BETWEEN ? AND ? ORDER BY c.CheckinTime", GetDBConnection());

            //cmd.Parameters.Add("start", OleDbType.DBDate).Value=start;
            //cmd.Parameters.Add("end", OleDbType.DBDate).Value=end;
            da.SelectCommand.Parameters.Add("start", OleDbType.Date).Value = new DateTime(start.Year, start.Month,start.Day,00,00,01);
            da.SelectCommand.Parameters.Add("end", OleDbType.Date).Value = new DateTime(end.Year, end.Month, end.Day, 23, 59, 59); ;

            try
            {
                da.Fill(ds);
                dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dt;
        }
        public static List< DBTables.Kunden> GetKundenList()
        {
            List<DBTables.Kunden> kundenList = new List<DBTables.Kunden>();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DBTables.Kunden kunden = new DBTables.Kunden();
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT ID, Name, Vorname, Photo FROM Kunden", GetDBConnection());

            try
            {
                da.Fill(ds);
                dt=ds.Tables[0];

                foreach (DataRow rows in dt.Rows)
                {
                    kunden.ID = int.Parse(rows["ID"].ToString().Trim());
                    kunden.Name = rows["Name"].ToString().Trim();
                    kunden.Vorname = rows["Vorname"].ToString().Trim();
                    kunden.Photo = rows["Photo"].ToString().Trim();
                    //kunden.Anfang =DateTime.Parse(rows["Anfang"].ToString());
                    //kunden.Schluss = DateTime.Parse(rows["Schluss"].ToString());

                    kundenList.Add(kunden);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return kundenList;
        }

        public static DataTable GetVertragListTable()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            OleDbDataAdapter da = new OleDbDataAdapter(
           @"SELECT k.ID, k.Name, k.Vorname, s.Name AS Sport, k.LockerKey AS Schlussel, v.Anfang, v.Schluss, v.ID AS VertragID
            FROM 
            ((Vertrag AS v 
            INNER JOIN Sports AS s ON v.SportID=s.ID) 
            INNER JOIN Kunden AS k ON v.KundenID=k.ID)", GetDBConnection());

            try
            {
                da.Fill(ds);
                dt = ds.Tables[0];

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dt;
        }

        //******** insert update delete operations************

        public static bool UpdateVertragDetails(DBTables.Vertrag vertrag)
        {
            OleDbCommand cmd = new OleDbCommand("UPDATE Vertrag SET Schluss=? WHERE ID=?", GetDBConnection());
            //OleDbTransaction transaction = connEB.BeginTransaction();
            //OleDbCommand cmd = new OleDbCommand("insert into Vertrag (Schluss) values(?) ", GetDBConnection());
            try
            {
                //cmd.Transaction = transaction;

                cmd.Parameters.Add("schluss", OleDbType.DBDate).Value = vertrag.Schluss;
                cmd.Parameters.Add("ID", OleDbType.Integer).Value = vertrag.ID;
                cmd.ExecuteNonQuery();

                //cmd.Transaction.Commit();
                return true;
            }
            catch(Exception e)
            {
                //cmd.Transaction.Rollback();
                return false;
            }
        }

        public static bool DeleteVertrag(DBTables.Vertrag vertrag)
        {
            OleDbCommand cmd = new OleDbCommand("DELETE FROM Vertrag WHERE ID=?", GetDBConnection());
            //OleDbTransaction transaction = connEB.BeginTransaction();
            try
            {
                //cmd.Transaction = transaction;
                cmd.Parameters.Add("ID", OleDbType.Integer).Value = vertrag.ID;

                cmd.ExecuteNonQuery();
                //transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                //transaction.Rollback();
                return false;
            }
        }
        public static bool InsertVertrag(DBTables.Vertrag vertrag)
        {
            OleDbCommand cmd = new OleDbCommand("INSERT INTO Vertrag(KundenID,SportID,Anfang,Schluss) VALUES(?,?,?,?)", GetDBConnection());
            //OleDbTransaction transaction = connEB.BeginTransaction();
            try
            {
                //cmd.Transaction = transaction;
                cmd.Parameters.Add("KundenID", OleDbType.Integer).Value = vertrag.KundenID;
                cmd.Parameters.Add("SportID", OleDbType.Integer).Value = vertrag.SportID;
                cmd.Parameters.Add("Anfang", OleDbType.Date).Value = vertrag.Anfang;
                cmd.Parameters.Add("Schluss", OleDbType.Date).Value = vertrag.Schluss;

                cmd.ExecuteNonQuery();
                //transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                //transaction.Rollback();
                return false;
            }
        }

        public static bool InsertKunden(DBTables.Kunden kunden)
        {
            OleDbCommand cmd = new OleDbCommand("INSERT INTO Kunden(Name,Vorname,Photo,HandyNr,Active,Strasse,Ort,Plz,GebDatum) VALUES(?,?,?,?,?,?,?,?,?)", GetDBConnection());
            //OleDbTransaction transaction = connEB.BeginTransaction();
            try
            {
                //cmd.Transaction = transaction;
                cmd.Parameters.Add("Name", OleDbType.VarChar).Value = kunden.Name;
                cmd.Parameters.Add("Vorname", OleDbType.VarChar).Value = kunden.Vorname;
                cmd.Parameters.Add("Photo", OleDbType.VarChar).Value = kunden.Photo==null?"":kunden.Photo+".jpg";
                cmd.Parameters.Add("HandyNr", OleDbType.VarChar).Value = kunden.HandyNr;
                cmd.Parameters.Add("Active", OleDbType.VarChar).Value = "Aktiv";

                cmd.Parameters.Add("Strasse", OleDbType.VarChar).Value = kunden.Strasse;
                cmd.Parameters.Add("Ort", OleDbType.VarChar).Value = kunden.Ort;
                    cmd.Parameters.Add("Plz", OleDbType.VarChar).Value = kunden.Plz;
                    cmd.Parameters.Add("GebDatum", OleDbType.VarChar).Value = kunden.GebDatum;

                cmd.ExecuteNonQuery();
                //transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                //transaction.Rollback();
                return false;
            }
        }

        public static bool InsertCheckin(DBTables.Checkin checkin)
        {
            OleDbCommand cmd = new OleDbCommand("INSERT INTO Checkin(KundenID,KundenName,CheckinTime,LockerKey) VALUES(?,?,?,?)", GetDBConnection());
            //OleDbTransaction transaction = connEB.BeginTransaction();
            try
            {
                //cmd.Transaction = transaction;
                cmd.Parameters.Add("KundenID", OleDbType.Integer).Value = checkin.KundenID;
                cmd.Parameters.Add("KundenName", OleDbType.VarChar).Value = checkin.KundenName;
                cmd.Parameters.Add("CheckinTime", OleDbType.Date).Value = checkin.CheckinTime;
                cmd.Parameters.Add("LockerKey", OleDbType.VarChar).Value = checkin.LockerKey;

                cmd.ExecuteNonQuery();
                //transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                //transaction.Rollback();
                return false;
            }
        }

        public static bool UpdateKundenPhoto(DBTables.Kunden kunden)
        {
            OleDbCommand cmd = new OleDbCommand("UPDATE Kunden SET Photo=? WHERE ID=?", GetDBConnection());
            //OleDbTransaction transaction = connEB.BeginTransaction();
            //OleDbCommand cmd = new OleDbCommand("insert into Vertrag (Schluss) values(?) ", GetDBConnection());
            try
            {
                //cmd.Transaction = transaction;

                cmd.Parameters.Add("photo", OleDbType.VarChar).Value = kunden.Photo+".jpg";
                cmd.Parameters.Add("ID", OleDbType.Integer).Value = kunden.ID;
                cmd.ExecuteNonQuery();

                //cmd.Transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                //cmd.Transaction.Rollback();
                return false;
            }
        }
        public static bool UpdateKunden(DBTables.Kunden kunden)
        {
            OleDbCommand cmd = new OleDbCommand("UPDATE Kunden SET Name=?,Vorname=?,Photo=?,HandyNr=?,Active=?,Strasse=?,Ort=?,Plz=?,GebDatum=? WHERE ID=?", GetDBConnection());
            //OleDbTransaction transaction = connEB.BeginTransaction();
            try
            {
                //cmd.Transaction = transaction;
                cmd.Parameters.Add("Name", OleDbType.VarChar).Value = kunden.Name;
                cmd.Parameters.Add("Vorname", OleDbType.VarChar).Value = kunden.Vorname;
                cmd.Parameters.Add("Photo", OleDbType.VarChar).Value = kunden.Photo == null ? "" : kunden.Photo + ".jpg";
                cmd.Parameters.Add("HandyNr", OleDbType.VarChar).Value = kunden.HandyNr;
                cmd.Parameters.Add("Active", OleDbType.VarChar).Value = "Aktiv";

                cmd.Parameters.Add("Strasse", OleDbType.VarChar).Value = kunden.Strasse;
                cmd.Parameters.Add("Ort", OleDbType.VarChar).Value = kunden.Ort;
                cmd.Parameters.Add("Plz", OleDbType.VarChar).Value = kunden.Plz;
                cmd.Parameters.Add("GebDatum", OleDbType.Date).Value = kunden.GebDatum;
                cmd.Parameters.Add("ID", OleDbType.Integer).Value = kunden.ID;

                cmd.ExecuteNonQuery();
                //transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                //transaction.Rollback();
                return false;
            }
        }
        public static bool ActivateDeactivateKunden(int ID, string state)
        {
            OleDbCommand cmd = new OleDbCommand("UPDATE Kunden SET Active=@state WHERE ID=@ID", GetDBConnection());
            //OleDbTransaction transaction = connEB.BeginTransaction();

            try
            {
                //cmd.Transaction = transaction;

                cmd.Parameters.Add("@state", OleDbType.VarChar).Value = state;
                cmd.Parameters.Add("@ID", OleDbType.Integer).Value = ID;
                
                cmd.ExecuteNonQuery();

                //cmd.Transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                //cmd.Transaction.Rollback();
                return false;
            }
        }
    }
}
