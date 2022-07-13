using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAFE_CALL;

namespace SAFE_CALL_Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SC.SAFE_CALL(typeof(Console), "WriteLine", "Hello World!");
            SC.SAFE_CALL(typeof(System.Threading.Thread), "Sleep", 2000);
            Console.ReadKey();
        }
    }
}
