using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using SAFE_CALL;

namespace SAFE_CALL_Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            _.SAFE_CALL(typeof(Console), "WriteLine", "Hello World!");

            Console.ReadKey();
        }
    }
}
