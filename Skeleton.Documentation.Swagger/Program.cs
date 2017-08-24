using Skeleton.Tests.Common;
using System;
using System.Diagnostics;

namespace Skeleton.Documentation.Swagger
{
    internal class Program
    {
        private const string Started = "Owin WebServer Started... Press any key to exit.";
        private const string SwaggerUri = "http://localhost:8081/swagger";

        private static void Main()
        {
            using (var server = new OwinTestServer())
            {
                server.Start(AppConfiguration.BaseAddress);
                Console.WriteLine(Started);

                Process.Start(SwaggerUri);

                Console.ReadKey();
            }
        }
    }
}