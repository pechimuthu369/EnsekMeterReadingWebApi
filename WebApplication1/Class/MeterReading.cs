using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Class
{
    /// <summary>
    /// Meter Reading
    /// </summary>
    public class MeterReading
    {
        /// <summary>
        /// Account Id
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// Meter Reaading Date
        /// </summary>
        public DateTime MeterReadingDateTime { get; set; }

        /// <summary>
        /// Meter Read Value
        /// </summary>
        public string MeterReadValue { get; set; }       
    }
}
