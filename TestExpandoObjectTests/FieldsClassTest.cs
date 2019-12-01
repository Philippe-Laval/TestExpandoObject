using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TestExpandoObject;
using Xunit;
using Xunit.Abstractions;

namespace TestExpandoObjectTests
{
    public class FieldsClassTest
    {
        private readonly ITestOutputHelper output;

        public FieldsClassTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void StaticFieldTest()
        {
            FieldInfo fld = typeof(FieldsClass).GetField("val");

            Assert.True(fld.IsStatic);
            Assert.True(fld.IsPublic);

            // Get the value of the class field
            var result = fld.GetValue(null);

            output.WriteLine("{0}", result);
            Assert.Equal("test", result);

            // Change the class field
            FieldsClass.val = "hi";

            // Get the value of the class field
            result = fld.GetValue(null);
            output.WriteLine("{0}", result);
            Assert.Equal("hi", result);
        }


        [Fact]
        public void InstanceFieldTest()
        {
            FieldsClass instance = new FieldsClass();

            // Get the type of FieldsClass.
            Type fieldsType = typeof(FieldsClass);

            // Get an array of FieldInfo objects.
            FieldInfo[] fields = fieldsType.GetFields(BindingFlags.Public
                | BindingFlags.Instance);
            // Display the values of the fields.
            output.WriteLine("Displaying the values of the fields of {0}:", fieldsType);

            for (int i = 0; i < fields.Length; i++)
            {
                // Get the value of the instance field
                output.WriteLine("   {0}:\t'{1}'", fields[i].Name, fields[i].GetValue(instance));
            }

            FieldInfo fld = typeof(FieldsClass).GetField("fieldA");
            Assert.Equal("A public field", fld.GetValue(instance));
            Assert.False(fld.IsStatic);
            Assert.True(fld.IsPublic);
        }

    }
}
