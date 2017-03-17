using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_Manager.DBAccess
{
    public class DBTables
    {
        public class Kunden
        {
            public int ID;
            public string Name;
            public string Vorname;
            public string Photo;
            public string HandyNr;
            public string Ort;
            public string Strasse;
            public string Plz;
            public DateTime GebDatum;
            public string Active;
        }
        public class VertragDetailed
        {
            public int KundenID;
            public string Name;
            public string Vorname;
            public string Photo;
            
            public string Sport;
            public int SportID;
            public DateTime Anfang;
            public DateTime Schluss;
            public int VertragID;
        }
        public class Vertrag
        {
            public int ID;
            public int KundenID;
            public int SportID;
            public DateTime Anfang;
            public DateTime Schluss;
        }
        public class Sports
        {
            public int ID;
            public string Name;
        }
        public class Checkin
        {
            public int KundenID;
            public string KundenName;
            public string LockerKey;
            public DateTime CheckinTime;
        }
        public class CheckinDetailed
        {
            public int KundenID;
            public string LockerKey;
            public DateTime CheckinTime;

            public string Name;
            public string Vorname;
            public string Photo;
        }
    }
}
