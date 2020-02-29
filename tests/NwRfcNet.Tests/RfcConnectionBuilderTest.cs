using Xunit;

namespace NwRfcNet.Tests
{
    public class RfcConnectionBuilderTest
    {
        [Fact]
        public void TestGetParms()
        {
            var builder = new RfcConnectionParameterBuilder()
                .UseConnectionHost("server_name")
                .UseLogonLanguage("en")
                .UseLogonUserName("user_name")
                .UseLogonPassword("complex_password");

            var rfcParms = builder.Build().Parameters;
            Assert.Equal(4, rfcParms.Count);

            Assert.Contains(rfcParms, r => r.Key == "ASHOST" && r.Value == "server_name");
            Assert.Contains(rfcParms, r => r.Key == "lang" && r.Value == "en");
            Assert.Contains(rfcParms, r => r.Key == "user" && r.Value == "user_name");
            Assert.Contains(rfcParms, r => r.Key == "passwd" && r.Value == "complex_password");
        }
    }
}
