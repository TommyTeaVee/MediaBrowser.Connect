using System;
using ServiceStack;

namespace MediaBrowser.Connect
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string address = args.Length == 0 ? "http://*:8095/" : args[0];
            
            ServiceStackHost appHost = new AppHost();
            appHost.Init().Start(address);

            Console.WriteLine("Media Browser Connect started and listening on {0}", address);
            Console.ReadKey();
        }
    }
}