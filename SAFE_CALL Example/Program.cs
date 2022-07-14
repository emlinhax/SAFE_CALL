using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using SAFE_CALL;

namespace SAFE_CALL_Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WebClient webClient = new WebClient();
            string downloaded = (string)_.SAFE_CALL(webClient, typeof(WebClient), "DownloadString", "https://pastebin.com/raw/9j2PYc2u");
            _.SAFE_CALL(typeof(Console), "WriteLine", downloaded);

            Console.ReadKey();
        }
    }
}
