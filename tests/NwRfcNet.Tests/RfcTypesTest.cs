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
            Assert.Equal(2019, date.Date?.Year);
            Assert.Equal(9, date.Date?.Month);
            Assert.Equal(21, date.Date?.Day);
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
            Assert.Null(date.Date);
        }

        [Fact]
        public void RfcTimeTest_1()
        {
            var t = new RfcTime("140423");
            Assert.Equal(14, t.Time?.Hours);
            Assert.Equal(4, t.Time?.Minutes);
            Assert.Equal(23, t.Time?.Seconds);
        }
    }
}
