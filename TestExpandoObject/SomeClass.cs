using System;
using System.Diagnostics;

namespace TestExpandoObject
{
    public class SomeClass
    {
        [Conditional("DEBUG")]
        public static void Message(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
