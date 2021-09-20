using CsvHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Class
{
    public static class ReadFromFile
    {
        public static List<MeterReading> MeterReadingValues(IFormFile file)
        {                        
            var meterReadings = new List<MeterReading>();

            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var parser = new CsvParser(reader, System.Globalization.CultureInfo.CurrentCulture))
            {
                meterReadings = new CsvReader(parser).GetRecords<MeterReading>().ToList();
            }

            return meterReadings;
        }

        public static List<TestAccounts> TestAcccountValues(IFormFile file)
        {
            var testAccounts = new List<TestAccounts>();

            using (var stream = file.OpenReadStream())
            using (var reader = new StreamReader(stream))
            using (var parser = new CsvParser(reader, System.Globalization.CultureInfo.CurrentCulture))
            {
                testAccounts = new CsvReader(parser).GetRecords<TestAccounts>().ToList();
            }

            return testAccounts;
        }      
    }
}
