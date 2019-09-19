using CommandLine;
using System;

namespace NwRfcNet.Sample
{
    class Options
    {
        [Option('u', "username", Required = true, HelpText = "RFC User Name")]
        public string UserName { get; set; }

        [Option('p', "password", Required = true, HelpText = "RFC User Password")]
        public string Password { get; set; }

        [Option('h', "hostname", Required = true, HelpText = "RFC Server Hostname")]
        public string Hostname { get; set; }

        [Option('c', "client", Required = true, HelpText = "RFC Server Client Id")]
        public string Client { get; set; }

        [Option('a', "action", Required = true, HelpText = "Action : 1 - List Companies, 2 - List Customers")]
        public BapiActions Action { get; set; }

        public enum BapiActions
        {
            ListCompanies = 1,
            ListCustomers,
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   {
                        //try
                        {
                           var version = RfcConnection.GetLibVersion();
                           Console.WriteLine($"currently loaded sapnwrfc library version : Major {version.MajorVersion}, Minor {version.MinorVersion}, patchLevel {version.PatchLevel}");

                           using (var conn = new RfcConnection(userName: o.UserName, password: o.Password, hostname: o.Hostname, client: o.Client))
                           {
                               conn.Open();
                               var bapi = new BapiDemo(conn);
                               bapi.Ping(o.Hostname);
                               switch (o.Action)
                               {

                                   case Options.BapiActions.ListCompanies:
                                       bapi.ListCompanies();
                                       break;

                                   case Options.BapiActions.ListCustomers:
                                       bapi.ListCustomers();
                                       break;
                               }
                           }
                        } 
                        //catch (Exception e) 
                        {
                            //Console.WriteLine(e.Message);
                        } 
                   });
        }
    }
}
