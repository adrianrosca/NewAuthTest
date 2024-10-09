using System;
using System.IO;
using Microsoft.Extensions.Configuration;

public static class ConfigUtil
{
    public static int GetServicePort()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        return configuration.GetValue<int>("ServicePort");
    }
}