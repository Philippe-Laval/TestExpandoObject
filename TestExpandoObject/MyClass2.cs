using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

//[assembly: AssemblyVersionAttribute("1.0.2000.0")]

namespace TestExpandoObject
{
    public class MyClass2
    {
        int answer;

        public MyClass2()
        {
            answer = 0;
        }

        public int AddNumb(int numb1, int numb2)
        {
            answer = numb1 + numb2;
            return answer;
        }
    }
}
