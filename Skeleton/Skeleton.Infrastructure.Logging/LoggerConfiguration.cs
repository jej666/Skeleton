﻿using log4net.Config;
using System.IO;
using System.Reflection;

namespace Skeleton.Infrastructure.Logging
{
    public static class LoggerConfiguration
    {
        public static void Configure()
        {
            XmlConfigurator.ConfigureAndWatch(
                new FileInfo(
                    Path.GetDirectoryName(
                        Assembly.GetAssembly(typeof(LoggerConfiguration)).Location)
                                + @"\" + "log4net.config"));
        }
    }
}
