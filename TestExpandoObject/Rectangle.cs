using System;
using System.Collections.Generic;
using System.Text;

// https://www.tutorialspoint.com/csharp/csharp_attributes.htm

namespace TestExpandoObject
{
    [DeBugInfo(45, "Zara Ali", "12/8/2012", Message = "Return type mismatch")]
    [DeBugInfo(49, "Nuha Ali", "10/10/2012", Message = "Unused variable")]
    public class MyRectangle
    {
        //member variables
        protected double length;
        protected double width;

        public MyRectangle(double l, double w)
        {
            length = l;
            width = w;
        }

        [DeBugInfo(55, "Zara Ali", "19/10/2012", Message = "Return type mismatch")]
        public double GetArea()
        {
            return length * width;
        }
        
        [DeBugInfo(56, "Zara Ali", "19/10/2012")]
        public void Display()
        {
            Console.WriteLine("Length: {0}", length);
            Console.WriteLine("Width: {0}", width);
            Console.WriteLine("Area: {0}", GetArea());
        }
    }
}
