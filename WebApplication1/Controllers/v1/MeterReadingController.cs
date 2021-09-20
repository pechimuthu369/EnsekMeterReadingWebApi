using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApplication1.Class;

namespace WebApplication1.Controllers
{
    /// <summary>
    /// App controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]    
    public class MeterReadingController : BaseController
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="MeterReadingController"/> class.
        /// </summary>        
        /// <param name="settings">The settings identifier.</param>        
        /// <param name="httpContextAccessor">The http contect accessor.</param>        
        public MeterReadingController(AppSettings settings)
          : base(settings)
        {
           
        }

        // GET api/app/values/id
        [HttpGet("{id}")]
        public ActionResult<int> Get(int id)
        {
            return id;
        }

        /// <summary>
        /// Compare.
        /// </summary>        
        /// <returns>List of test accounts.</returns>
        [HttpPost("SaveTestAccounts")]
        public ActionResult<List<TestAccounts>> SaveTestAccounts()
        {
            // populate meter reading list from excel            
            var file = Request.Form.Files[0];

            // read excel file.
            var testAccounts = ReadFromFile.TestAcccountValues(file);

            // insert into sqlite 
            this.SaveTestAccounts(testAccounts);


            return Ok(testAccounts);            
        }


        /// <summary>
        /// Compare.
        /// </summary>        
        /// <returns>List of meter reading.</returns>
        [HttpPost("CompareMeterReadingAndTestAccounts")]
        public ActionResult<List<MeterReading>> CompareMeterReadingAndTestAccounts()
        {
            // populate meter reading list from excel            
            var file = Request.Form.Files[0];
            
            // read excel file.
            var meterReadings = ReadFromFile.MeterReadingValues(file);

            // order by date
            meterReadings = meterReadings.OrderBy(x=>x.AccountId).ToList();
            
            // find invalid meter reading values
            // remove non gigits
            double dummy = -1;
            meterReadings = meterReadings.Where(x => double.TryParse(x.MeterReadValue, out dummy)).ToList();

            // filter values less than 0
            var zeroValue = meterReadings.Where(x => Convert.ToInt32(x.MeterReadValue) < 0).ToList();

            //exclude them 
            meterReadings = meterReadings.Except(zeroValue).ToList();

            // get test account from db
            var testAccounts = this.GetTestAccounts();

            if ((testAccounts.Count > 0) && (meterReadings.Count > 0))
            {
                testAccounts.OrderBy(x=>x.AccountId).ToList();

                //COMPARE test accounts and meter readings
                var newList = from a in meterReadings
                              from b in testAccounts
                              where a.AccountId == b.AccountId
                              select a;

                // insert into sqlite
                this.SaveMeterReading(newList.ToList());
            }

            return Ok(meterReadings);            
        }
    }
}
