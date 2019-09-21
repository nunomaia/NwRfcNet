using NwRfcNet.RfcTypes;
using System;
using Xunit;

namespace NwRfcNet.Tests
{

    public class RfcTypesTest
    {
        [Fact]
        public void RfcDateTest_1()
        {
            RfcDate date = new RfcDate("20190921");
            Assert.Equal(2019, date.RfcValue?.Year);
            Assert.Equal(9, date.RfcValue?.Month);
            Assert.Equal(21, date.RfcValue?.Day);
        }

        [Fact]
        public void RfcDateTest_2() => 
            Assert.Throws<ArgumentException>(() => new RfcDate("2019092A"));

        [Fact]
        public void RfcDateTest_3()
        {
            RfcDate date = new RfcDate("20190921");
            Assert.Equal("20190921", date.ToString());
        }

        [Fact]
        public void RfcDateTest_4()
        {
            RfcDate date = new RfcDate("00000000");
            Assert.Null(date.RfcValue);
        }

        [Fact]
        public void RfcTimeTest_1()
        {
            var t = new RfcTime("140423");
            Assert.Equal(14, t.RfcValue?.Hours);
            Assert.Equal(4, t.RfcValue?.Minutes);
            Assert.Equal(23, t.RfcValue?.Seconds);
        }

        [Fact]
        public void RfcBcd_1()
        {
            var bcd = new RfcBcd("140423.2101");
            Assert.Equal(140423.2101M, bcd.RfcValue);
        }

        [Fact]
        public void RfcBcd_2()
        {
            var bcd = new RfcBcd(140423.2101M);
            Assert.Equal("140423.2101", bcd.ToString());
        }
    }
}
