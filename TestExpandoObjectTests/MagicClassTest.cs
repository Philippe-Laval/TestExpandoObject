using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TestExpandoObject;
using Xunit;

namespace TestExpandoObjectTests
{
    public class MagicClassTest
    {
        [Fact]
        public void GetConstructorTest()
        {
            // Get the constructor and create an instance of MagicClass

            // Type magicType = Type.GetType("MagicClass");
            Type magicType = Type.GetType("TestExpandoObject.MagicClass, TestExpandoObject");


            ConstructorInfo magicConstructor = magicType.GetConstructor(Type.EmptyTypes);
            object magicClassObject = magicConstructor.Invoke(new object[] { });

            // Get the ItsMagic method and invoke with a parameter value of 100
            MethodInfo magicMethod = magicType.GetMethod("ItsMagic");
            object magicValue = magicMethod.Invoke(magicClassObject, new object[] { 100 });

            Console.WriteLine("MethodInfo.Invoke() Example\n");
            Console.WriteLine("MagicClass.ItsMagic() returned: {0}", magicValue);
        }

        [Fact]
        public void GetCustomAttributesTest()
        {
            Type type = typeof(MagicClass);
            object[] attributes = type.GetCustomAttributes(true);

            for (int i = 0; i < attributes.Length; i++)
            {
                HelpAttribute helpAttribute = attributes[i] as HelpAttribute;
                //System.Console.WriteLine(attributes[i]);

                Assert.NotNull(helpAttribute);
                Assert.Equal("Topic1", helpAttribute.Topic);
                Assert.Equal("Information on the class MyClass", helpAttribute.Url);
            }
        }

    }
}
