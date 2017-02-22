using Skeleton.Tests.Web;
using System;
using System.Diagnostics;

namespace Skeleton.Tests.ConsoleApplication
{
    class Program
    {
        private const string Started = "Owin WebServer Started...";
        private const string SwaggerUri = "http://localhost:8081/swagger";

        static void Main(string[] args)
        {
            using (var server = new OwinServer())
            {
                server.Start();
                Console.WriteLine(Started);

                Process.Start(SwaggerUri);

                Console.ReadKey();
            }
        }
    }
}
