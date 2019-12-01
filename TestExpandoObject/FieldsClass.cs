using System;
using System.Collections.Generic;
using System.Text;

namespace TestExpandoObject
{
    public class FieldsClass
    {
        public static String val = "test";
        
        public string fieldA;
        public string fieldB;

        public FieldsClass()
        {
            fieldA = "A public field";
            fieldB = "Another public field";
        }
    }
}
