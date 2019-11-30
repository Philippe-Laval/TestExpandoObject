using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TestExpandoObject;
using Xunit;

namespace TestExpandoObjectTests
{
    public class MyRectangleTest
    {
        [Fact]
        public void GetCustomAttributesTest()
        {
            MyRectangle r = new MyRectangle(4.5, 7.5);
            r.Display();
            
            Type type = typeof(MyRectangle);

            //iterating through the attributes of the Rectangle class
            foreach (object attribute in type.GetCustomAttributes(false))
            {
                DeBugInfo dbi = attribute as DeBugInfo;
                if (dbi != null)
                {
                    Console.WriteLine("Bug no: {0}", dbi.BugNo);
                    Console.WriteLine("Developer: {0}", dbi.Developer);
                    Console.WriteLine("Last Reviewed: {0}", dbi.LastReview);
                    Console.WriteLine("Remarks: {0}", dbi.Message);
                }
            }

            //iterating through the method attribtues
            foreach (MethodInfo methodInfo in type.GetMethods())
            {
                foreach (object attribute in methodInfo.GetCustomAttributes(true))
                {
                    DeBugInfo dbi = attribute as DeBugInfo;
                    if (dbi != null)
                    {
                        Console.WriteLine("Bug no: {0}, for Method: {1}", dbi.BugNo, methodInfo.Name);
                        Console.WriteLine("Developer: {0}", dbi.Developer);
                        Console.WriteLine("Last Reviewed: {0}", dbi.LastReview);
                        Console.WriteLine("Remarks: {0}", dbi.Message);
                    }
                }
            }
           
        }

   
    }
}
