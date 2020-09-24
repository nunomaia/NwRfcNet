# .NET client library for SAP NetWeaver RFC
An easy way of making SAP RFC calls from .NET. Libray is supported in Windows, Linux and macOS.

## Supported Platforms & Prerequisites

* Requires .NET Framework ( [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) or higher ) 
  -  .NET Framework 4.6.1 or higher
  -  .NET Core 2.0 or higher

* OS versions
  - Windows x64.
  - Redhat Linux 7 or other Linux distribution that is supported simultaneously by .NET Core and SAP NetWeaver RFC. 
  - macOS 10.12+.

* SAP NetWeaver RFC Library 7.50 SDK C++ binaries must be installed locally. For download and installation instructions check [SAP Note 2573790](https://launchpad.support.sap.com/#/notes/2573790)


## Using this package

Add the package using the `dotnet` cli:

```
$ dotnet add package NwRfcNet
```

Create a class to match SAP RFC parameters

```C#
    public class BapiCompanyOutputParameters
    {
        public CompanyDetails[] Details { get; set; }
    }

    public class CompanyDetails
    {
        public string CompanyCode { get; set; }

        public string Name { get; set; }
    }
```

Map RFC mapameters to class 

```C#
    RfcMapper mapper = new RfcMapper();

    mapper.Parameter<BapiCompanyOutputParameters>().Property(x => x.Details)
        .HasParameterName("COMPANYCODE_LIST")
        .HasParameterType(RfcFieldType.Table);

    mapper.Parameter<CompanyDetails>().Property(x => x.CompanyCode)
        .HasParameterName("COMP_CODE")
        .MaxLength(4)
        .HasParameterType(RfcFieldType.Char);

    mapper.Parameter<CompanyDetails>().Property(x => x.Name)
        .HasParameterName("COMP_NAME")
        .MaxLength(25)
        .HasParameterType(RfcFieldType.Char);
```

Open a connection to server and invoke a BAPI 

```C#
    using (var conn = new RfcConnection(builder => builder
        .UseConnectionHost("hostname")
        .UseLogonUserName("user")
        .UseLogonPassword("password")
        .UseLogonClient("cln")))
    {
        conn.Open();
        using(var func = conn.CallRfcFunction("BAPI_COMPANYCODE_GETLIST"))
        {
            func.Invoke();
        }
    }
```

or 

```C#
    using (var conn = new RfcConnection("Server=server_name;lang=en;user=testUser;pwd=secret"))
    {
        conn.Open();
        using(var func = _conn.CallRfcFunction("BAPI_COMPANYCODE_GETLIST"))
        {
            func.Invoke();
        }
    }
```

Get result and display to Console

```C#
    var returnValue =  func.GetOutputParameters<BapiCompanyOutputParameters>();
    Console.WriteLine(String.Format("|{0,-20}|{1,-10}", "Company Code", "Company Name"));
    foreach (var row in returnValue.Details)
    {
        Console.WriteLine(String.Format("|{0,-20}|{1,-10}", row.CompanyCode, row.Name));
    }
```

Output should be 

| Company Code  | Company Name |
| ------------- | -------------|
| C001          | company 1    |
| C002          | Company 2    |

## Samples

Included samples in project

* List FI Companies
* List FI Customers
* Get Details of a FI Customer
* FI General Ledger Account
* RFC Read Table

## Connection String

Example : Server=server_name;lang=en;user=testUser;pwd=secret

| Value             | Alias                                                                 |
| ----------------- | ----------------------------------------------------------------------|
| User Name         | "userName", "userId", "uid", "user", "u"                              |
| password          | "password", "passwd", "pass", "pwd", "p"                              |
| Host              | "target_host", "targetHost", "host", "server", "h"                    |
| Logon Language    | "language", "lang", "l"                                               |
| System Client     | "client", "cl", "c"                                                   |
| SystemNumber      | "system_number", "systemnumber", "sysnr"                              |
| SystemId          | "system_id", "systemid", "sysid"                                      |
| Trace             | "trace", "tr", "RfcSdkTrace"                                          |
| Snc Mode          | "snc_mode", "sncmode", "UseSnc", "snc"                                |
|  Snc Qop          | "snc_partnername", "sncpartnername", "snc_partner", "sncpartner"      |
| Snc Lib           | "snc_library", "snc_lib", "snclib"                                    |