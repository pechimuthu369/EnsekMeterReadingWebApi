using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace WebApplication1.Class
{
    /// <summary>
    /// The Data Manager.
    /// </summary>
    public class DBManager
    {
        private string Connection { get; set; }

        /// <summary>
        /// The Data Manager.
        /// </summary>
        /// <param name="ConnectionString"></param>
        public DBManager(string ConnectionString)
        {
            Connection = ConnectionString;
        }


        public DataTable ExecCommand(string statement)
        {
            var SqliteCmd = new SqliteCommand();
            DataTable dt = new DataTable();
            using (SqliteConnection sqlcon = new SqliteConnection(Connection))
            {
                SQLitePCL.Batteries.Init();
                sqlcon.Open();
                SqliteCmd.Connection = sqlcon;
                SqliteCmd.CommandText = statement;                
                SqliteDataReader sqr = SqliteCmd.ExecuteReader();
               
                if (sqr.HasRows)
                {                                     
                    dt.Load(sqr);                    
                }
                return dt;
            }            
        }

        /// <summary>
        /// call a stored procedure without a return and send it a list of paramaeters
        /// if the list exists it takes the list and converts it over to a sql parameter to be used in the stored procedure
        /// </summary>
        /// <param name="testAccountsList">list of sql parameters</param>        
        public void SaveTestAccounts(List<TestAccounts> testAccountsList)
        {
            var SqliteCmd = new SqliteCommand();
            using (SqliteConnection sqlcon = new SqliteConnection(Connection))
            {
                SQLitePCL.Batteries.Init();
                sqlcon.Open();                
                using (SqliteTransaction myTransaction = sqlcon.BeginTransaction())
                {
                    SqliteCmd = sqlcon.CreateCommand();
                    SqliteCmd.Connection = sqlcon;                    
                    foreach (var list in testAccountsList)
                    {
                        SqliteCmd.CommandText = "insert into testaccounts(accountid,firstname,lastname) values (" + list.AccountId + ",'" + list.FirstName + "','" + list.LastName + "')";
                        SqliteCmd.ExecuteNonQuery();
                    }
                    myTransaction.Commit();
                }               
            }
        }

        /// <summary>
        /// call a stored procedure without a return and send it a list of paramaeters
        /// if the list exists it takes the list and converts it over to a sql parameter to be used in the stored procedure
        /// </summary>
        /// <param name="meterReadingList">list of sql parameters</param>        
        public void SaveMeterReadings(List<MeterReading> meterReadingList)
        {
            var SqliteCmd = new SqliteCommand();
            using (SqliteConnection sqlcon = new SqliteConnection(Connection))
            {
                SQLitePCL.Batteries.Init();
                sqlcon.Open();
                using (SqliteTransaction myTransaction = sqlcon.BeginTransaction())
                {
                    SqliteCmd = sqlcon.CreateCommand();
                    SqliteCmd.Connection = sqlcon;
                    foreach (var list in meterReadingList)
                    {
                        SqliteCmd.CommandText = "insert into meterreading(accountid,meterreadingdatetime,meterreadingvalue) values (" + list.AccountId + ",'" + list.MeterReadingDateTime + "','" + list.MeterReadValue + "')";
                        SqliteCmd.ExecuteNonQuery();
                    }
                    myTransaction.Commit();
                }
            }
        }
    }
}
