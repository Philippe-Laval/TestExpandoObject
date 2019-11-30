using System;
using System.Collections.Generic;
using System.Text;
using TestExpandoObject;
using Xunit;

namespace TestExpandoObjectTests
{
    public class MyClassTest
    {
        [Fact]
        public void AddNumbTest()
        {
            MyClass c = new MyClass();
            Assert.Equal(42, c.AddNumb(40, 2));
        }
        
        [Fact]
        public void NewMethodTest()
        {
            MyClass c = new MyClass();
            Assert.Equal(42, c.NewMethod());
        }

        [Fact]
        public void OldMethodTest()
        {
            MyClass c = new MyClass();
            Assert.Equal(42, c.OldMethod());
        }
    }
}
