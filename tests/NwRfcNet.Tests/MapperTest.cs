using NwRfcNet.TypeMapper;
using Xunit;

namespace NwRfcNet.Tests
{

    public class MapperTest
    {
        private class BapiCustomerParms
        {
            public int MaxRows { get; set; }
        }

        [Fact]
        public void ManualMapperTest()
        {
            RfcMapper builder = new RfcMapper();

            builder
                .Parameter<BapiCustomerParms>()
                .Property(x => x.MaxRows)
                .HasParameterName("Max_Rows")
                .HasParameterType(RfcFieldType.Int);

            PropertyMap map = builder[typeof(BapiCustomerParms)]["MaxRows"];

            Assert.Equal(RfcFieldType.Int, map.ParameterType);
            Assert.Equal("MaxRows", map.PropertyName);
            Assert.Equal("Max_Rows", map.RfcParameterName);
        }
    }
}
