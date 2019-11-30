using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TestExpandoObject;
using Xunit;

namespace TestExpandoObjectTests
{
    public class ExampleTest
    {
        [Fact]
        public void Tests1()
        {
            Assembly assem = typeof(Example).Assembly;

            Console.WriteLine("Assembly Full Name:");
            Console.WriteLine(assem.FullName);

            // The AssemblyName type can be used to parse the full name.
            AssemblyName assemName = assem.GetName();
            
            Console.WriteLine("\nName: {0}", assemName.Name);
            Console.WriteLine("Version: {0}.{1}",
                assemName.Version.Major, assemName.Version.Minor);

            Console.WriteLine("\nAssembly CodeBase:");
            Console.WriteLine(assem.CodeBase);

            // Create an object from the assembly, passing in the correct number
            // and type of arguments for the constructor.
            Object o = assem.CreateInstance("TestExpandoObject.Example", false,
                BindingFlags.ExactBinding,
                null, new Object[] { 2 }, null, null);

            // Make a late-bound call to an instance method of the object. 
            Type type = Type.GetType("TestExpandoObject.Example, TestExpandoObject");
            MethodInfo m = type.GetMethod("SampleMethod");
            Object ret = m.Invoke(o, new Object[] { 42 });
            Console.WriteLine("SampleMethod returned {0}.", ret);

            Console.WriteLine("\nAssembly entry point:");
            Console.WriteLine(assem.EntryPoint);
        }

        /*
        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
        */
    }
}
