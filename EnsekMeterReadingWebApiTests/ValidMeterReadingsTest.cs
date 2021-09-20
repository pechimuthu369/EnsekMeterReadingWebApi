using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Class;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;

namespace EnsekMeterReadingWebApiTests
{
    [TestClass]
    public class ValidMeterReadingsTest
    {
        private Mock<AppSettings> mockRepoAppSettings = new Mock<AppSettings>();        
        private List<MeterReading> mockMeterReadings = new List<MeterReading>();
        private List<TestAccounts> mockTestAccounts = new List<TestAccounts>();

        [TestInitialize]
        public void TestInit()
        {
            this.mockMeterReadings = this.CreateMockMeterReading(10);
            this.mockTestAccounts = this.CreateMockTestAccounts(10);
        }
        
        [TestMethod]
        public void CompareMeterReadingAndTestAccounts()
        {
            // Arrange            
            var controller = new BaseController(mockRepoAppSettings.Object );

            // Act
            var result = controller.CompareMeterReadingAndTestAccounts(this.mockMeterReadings, this.mockTestAccounts);

            // Assert            
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// The create Mock Meter Reading.
        /// </summary>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        private List<MeterReading> CreateMockMeterReading(int numberOfDays)
        {
            var list = new List<MeterReading>();
            for (int i = 0; i <= numberOfDays; i++)
            {
                list.Add(new MeterReading
                {
                    AccountId = 100 + i,
                    MeterReadingDateTime = DateTime.Now,
                    MeterReadValue = "100",
                }); 
            }
            return list;
        }

        /// <summary>
        /// The create Mock Test Accounts.
        /// </summary>
        /// <param name="numberOfDays"></param>
        /// <returns></returns>
        private List<TestAccounts> CreateMockTestAccounts(int numberOfDays)
        {
            var list = new List<TestAccounts>();
            for (int i = 0; i <= numberOfDays; i++)
            {
                list.Add(new TestAccounts
                {
                    AccountId = 100+i,
                    FirstName = "User ",
                    LastName = "Test" + i,
                }); ;
            }
            return list;
        }
    }
}
