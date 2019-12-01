using System;
using System.Collections.Generic;
using System.Text;
using TestExpandoObject;
using Xunit;
using Xunit.Abstractions;

namespace TestExpandoObjectTests
{
    public class DynamicDictionaryTests
    {
        private readonly ITestOutputHelper output;

        public DynamicDictionaryTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TrySetMemberAndTryGetMemberTest()
        {
            // Creating a dynamic dictionary.
            dynamic person = new DynamicDictionary();

            // Adding new dynamic properties. 
            // The TrySetMember method is called.
            person.FirstName = "Ellen";
            person.LastName = "Adams";

            // Getting values of the dynamic properties.
            // The TryGetMember method is called.
            // Note that property names are case-insensitive.
            output.WriteLine(person.firstname + " " + person.lastname);

            // Getting the value of the Count property.
            // The TryGetMember is not called, 
            // because the property is defined in the class.
            output.WriteLine(
                "Number of dynamic properties:" + person.Count);

            // The following statement throws an exception at run time.
            // There is no "address" property,
            // so the TryGetMember method returns false and this causes a
            // RuntimeBinderException.
            Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() =>
            {
                output.WriteLine(person.address);
            });
            
        }

        [Fact]
        public void TryInvokeMemberTest()
        {
            // Creating a dynamic dictionary.
            dynamic person = new DynamicDictionary();

            // Adding new dynamic properties.
            // The TrySetMember method is called.
            person.FirstName = "Ellen";
            person.LastName = "Adams";

            // Calling a method defined in the DynamicDictionary class.
            // The Print method is called.
            person.Print();

            Console.WriteLine(
                "Removing all the elements from the dictionary.");

            // Calling a method that is not defined in the DynamicDictionary class.
            // The TryInvokeMember method is called.
            person.Clear();

            // Calling the Print method again.
            person.Print();

            // The following statement throws an exception at run time.
            // There is no Sample method 
            // in the dictionary or in the DynamicDictionary class.
            Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() =>
            {
                person.Sample();
            });

        }



    }
}
