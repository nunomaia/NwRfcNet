using System;
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

        [Fact]
        public void TestConnectionString()
        {
            var connection = new RfcConnection(connectionString: "Server=server_name; lang=en; user=testUser;pwd=secret");

            var rfcParms = connection.ConnectionParameters;
            Assert.Equal(4, rfcParms.Count);

            Assert.Contains(rfcParms, r => r.Key == "ASHOST" && r.Value == "server_name");
            Assert.Contains(rfcParms, r => r.Key == "lang" && r.Value == "en");
            Assert.Contains(rfcParms, r => r.Key == "user" && r.Value == "testUser");
            Assert.Contains(rfcParms, r => r.Key == "passwd" && r.Value == "secret");
        }

        [Fact]
        public void TestConnectionUri()
        {
            var connection = new RfcConnection(connectionUri: new Uri($"sap://user=testUser;pass=secret;l=EN;cl=001@A/test_server?rfcsdktrace=ON"));

            var rfcParms = connection.ConnectionParameters;
            Assert.Equal(6, rfcParms.Count);

            Assert.Contains(rfcParms, r => r.Key == "ASHOST" && r.Value == "test_server");
            Assert.Contains(rfcParms, r => r.Key == "lang" && r.Value == "EN");
            Assert.Contains(rfcParms, r => r.Key == "user" && r.Value == "testUser");
            Assert.Contains(rfcParms, r => r.Key == "passwd" && r.Value == "secret");
            Assert.Contains(rfcParms, r => r.Key == "client" && r.Value == "001");
            Assert.Contains(rfcParms, r => r.Key == "trace" && r.Value == "1");
        }

        [Theory]
        [InlineData("1")]
        [InlineData("0")]
        public void UseSecureNetworkCommunicationMode_ShouldStoreSncMode(string sncMode)
        {
            var builder = new RfcConnectionParameterBuilder()
                .UseSecureNetworkCommunicationMode(sncMode);

            var rfcParms = builder.Build().Parameters;
            Assert.Equal(1, rfcParms.Count);
            Assert.Contains(rfcParms, r => r.Key == "snc_mode" && r.Value == sncMode);
        }

        [Theory]
        [InlineData("2")]
        [InlineData("-1")]
        [InlineData("324234")]
        [InlineData("test")]
        public void UseSecureNetworkCommunicationMode_ShouldThrowArgumentException(string invalidSncMode)
        {
            var builder = new RfcConnectionParameterBuilder();
            Assert.Throws<ArgumentException>(() => builder.UseSecureNetworkCommunicationMode(invalidSncMode));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        [InlineData("8")]
        [InlineData("9")]
        public void UseSecureNetworkCommunicationQop_ShouldStoreSncQop(string sncQop)
        {
            var builder = new RfcConnectionParameterBuilder()
                .UseSecureNetworkCommunicationQop(sncQop);

            var rfcParms = builder.Build().Parameters;
            Assert.Equal(1, rfcParms.Count);
            Assert.Contains(rfcParms, r => r.Key == "snc_qop" && r.Value == sncQop);
        }

        [Theory]
        [InlineData("4")]
        [InlineData("-1")]
        [InlineData("test")]
        public void UseSecureNetworkCommunicationQop_ShouldThrowArgumentException(string invalidSncQop)
        {
            var builder = new RfcConnectionParameterBuilder();
            Assert.Throws<ArgumentException>(() => builder.UseSecureNetworkCommunicationQop(invalidSncQop));
        }
    }
}
