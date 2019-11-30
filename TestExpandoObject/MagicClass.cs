using System;
using System.Collections.Generic;
using System.Text;

namespace TestExpandoObject
{
    [Help("Information on the class MyClass", Topic="Topic1")]
    public class MagicClass
    {
        private int magicBaseValue;

        public MagicClass()
        {
            magicBaseValue = 9;
        }

        public int ItsMagic(int preMagic)
        {
            return preMagic * magicBaseValue;
        }
    }
}
