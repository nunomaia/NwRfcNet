using Xunit;

namespace NwRfcNet.Tests
{
    public class RfcConnectionBuilderTest
    {
        [Fact]
        public void TestGetParms()
        {
            var builder = new RfcConnectionBuilder
            {
                Hostname = "server_name",
                Language = "en",
                UserName = "user_name",
                Password = "complex_password"
            };

            var rfcParms = builder.GetParms();
            Assert.Equal(4, rfcParms.Length);

            Assert.Contains(rfcParms, r => r.Name == "ASHOST" && r.Value == "server_name");
            Assert.Contains(rfcParms, r => r.Name == "lang" && r.Value == "en");
            Assert.Contains(rfcParms, r => r.Name == "user" && r.Value == "user_name");
            Assert.Contains(rfcParms, r => r.Name == "passwd" && r.Value == "complex_password");
        }
    }
}
