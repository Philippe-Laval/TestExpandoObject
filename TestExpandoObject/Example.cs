using System;
using System.Collections.Generic;
using System.Text;

namespace TestExpandoObject
{
    public class Example
    {
        private int factor;
        public Example(int f)
        {
            factor = f;
        }

        public int SampleMethod(int x)
        {
            Console.WriteLine("\nExample.SampleMethod({0}) executes.", x);
            return x * factor;
        }
    }
}
