using System;

namespace TestExpandoObject
{
    public class MyClass
    {
        public virtual int AddNumb(int number1, int number2)
        {
            int result = number1 + number2;
            return result;
        }

        //[Obsolete("Don't use OldMethod, use NewMethod instead", true)]
        [Obsolete("Don't use OldMethod, use NewMethod instead", false)]
        public int OldMethod()
        {
            return 2 * 21;
        }

        public int NewMethod()
        {
            SomeClass.Message("What is the answar ?");

            return 42;
        }

    }
}
