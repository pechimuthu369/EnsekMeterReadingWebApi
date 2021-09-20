using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApplication1.Class;

/// <summary>
/// Class BaseController.
/// Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />       
namespace WebApplication1.Controllers
{
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>        
        /// <param name="settings">The settings.</param>        
        /// <param name="httpContextAccessor">The http contect accessor.</param>        
        public BaseController(AppSettings settings)
        {            
            this.AppSettings = settings;

            this.Dal = new DBManager(AppSettings.StorageConnectionString);
        }        

        /// <summary>
        /// Gets or sets the application settings.
        /// </summary>
        /// <value>The application settings.</value>
        public AppSettings AppSettings { get; set; }       

        /// <summary>
        /// Data access manager
        /// </summary>
        protected DBManager Dal { get; set; }

        /// <summary>
        /// Compare meter reading and test accounts.
        /// </summary>
        /// <param name="meterReadings">The Meter Reading.</param>
        /// <param name="testAccounts">The Test Accounts.</param>
        public List<MeterReading> CompareMeterReadingAndTestAccounts(List<MeterReading> meterReadings, List<TestAccounts> testAccounts)
        {
            var result = new List<MeterReading>();

            // order by date.
            meterReadings = meterReadings.OrderBy(x => x.MeterReadingDateTime).ToList();


            // compare meter reading against test accounts.
            var filter = from m in meterReadings
                       from t in testAccounts
                       where m.AccountId == t.AccountId
                       select m;

            result = filter.ToList();

            return result;
        }

        /// <summary>
        /// Insert Test Accounts.
        /// </summary>
        /// <returns>list of test accounts.</returns>
        public void SaveTestAccounts(List<TestAccounts> testAccounts)
        {
            try
            {
                this.Dal.SaveTestAccounts(testAccounts);
            }
            catch (Exception ex)
            {
                Logger.Error("Erors in SaveTestAccounts", ex);
            }
        }


        /// <summary>
        /// Insert Test Accounts.
        /// </summary>
        /// <returns>list of test accounts.</returns>
        public void SaveMeterReading(List<MeterReading> meterReading)
        {
            try
            {
                this.Dal.SaveMeterReadings(meterReading);
            }
            catch (Exception ex)
            {
                Logger.Error("Erors in SaveMeterReading", ex);
            }
        }

        /// <summary>
        /// Insert Test Accounts.
        /// </summary>
        /// <returns>list of test accounts.</returns>
        public List<TestAccounts> GetTestAccounts()
        {
            var list = new List<TestAccounts>();
            try
            {
                // var testAccounts = this.Dal.ExecCommand("SELECT SQLITE_VERSION();");
                var dt = this.Dal.ExecCommand("select * from testaccounts;");                

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TestAccounts testAccounts = new TestAccounts();
                    testAccounts.AccountId = Convert.ToInt32(dt.Rows[i]["accountid"]);
                    testAccounts.FirstName = dt.Rows[i]["firstname"].ToString();
                    testAccounts.LastName = dt.Rows[i]["lastname"].ToString();

                    list.Add(testAccounts);
                }
                
            }
            catch (Exception ex)
            {
                Logger.Error("Erors in GetTestAccounts", ex);
            }
            return list;
        }
    }
}
