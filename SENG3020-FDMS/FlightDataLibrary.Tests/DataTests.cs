using NUnit.Framework;

namespace FlightDataLibrary.Tests
{
    [TestFixture]
    public class DataTests
    {
        [Test]
        public void DateTimeConversion()
        {
            // Should be able to convert even if there is whitespace
            string inputDate = " 7_8_2018 19:34:3";
            string expectedDate = "2018-07-08 19:34:03";
            string convertedDate = TimeConverter.ConvertToDatabaseFormat(inputDate);

            Assert.AreEqual(expectedDate, convertedDate);
        }
    }
}
