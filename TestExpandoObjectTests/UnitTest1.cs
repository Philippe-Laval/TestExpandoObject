using System;
using System.Reflection;
using TestExpandoObject;
using Xunit;

// https://dotnetcademy.net/Learn/4/Pages/1

namespace TestExpandoObjectTests
{
    public class UnitTest1
    {
        [Fact]
        public void GetExecutingAssemblyTest()
        {
            var assembly = Assembly.GetExecutingAssembly();
            Console.WriteLine("Assembly Name: " + assembly.GetName().Name);
            Console.WriteLine("Version: " + assembly.GetName().Version.ToString());
        }

        [Fact]
        public void Test1()
        {
            Console.WriteLine("\nReflection.MethodInfo");
            
            // Create MyClass object
            MyClass myClassObj = new MyClass();
            
            // Get the Type information.
            Type myTypeObj = myClassObj.GetType();
            
            // Get Method Information.
            MethodInfo myMethodInfo = myTypeObj.GetMethod("AddNumb");
            
            object[] mParam = new object[] { 5, 10 };

            // Get and display the Invoke method.
            //Console.Write("\nFirst method - " + myTypeObj.FullName + " returns " + myMethodInfo.Invoke(myClassObj, mParam) + "\n");
           
        }

        [Fact]
        public void Test2()
        { 
            Type myTypeObj = Type.GetType("TestExpandoObject.MyClass, TestExpandoObject");

            Assert.True(myTypeObj.IsClass);
        }

        [Fact]
        public void Test3()
        {
            Type type1 = typeof(MyClass2);
            object obj = Activator.CreateInstance(type1);
            object[] mParam = new object[] { 5, 10 };

            //invoke AddMethod, passing in two parameters
            int res = (int)type1.InvokeMember("AddNumb", BindingFlags.InvokeMethod,
                                               null, obj, mParam);
            Console.Write("Result: {0} \n", res);
        }

    }
}
