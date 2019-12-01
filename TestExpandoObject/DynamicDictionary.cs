using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;

// https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.dynamicobject?view=netframework-4.8
// DynamicObject.TryInvokeMember
// https://docs.microsoft.com/en-us/dotnet/api/system.dynamic.dynamicobject.tryinvokemember?view=netframework-4.8

namespace TestExpandoObject
{
    public class DynamicDictionary : DynamicObject
    {
        // The inner dictionary.
        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        // This property returns the number of elements
        // in the inner dictionary.
        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        // If you try to get a value of a property 
        // not defined in the class, this method is called.
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            string name = binder.Name.ToLower();

            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.
            return dictionary.TryGetValue(name, out result);
        }

        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            dictionary[binder.Name.ToLower()] = value;

            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }

        // Calling a method.
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            Type dictType = typeof(Dictionary<string, object>);
            try
            {
                result = dictType.InvokeMember(
                             binder.Name,
                             BindingFlags.InvokeMethod,
                             null, dictionary, args);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        // This methods prints out dictionary elements.
        public void Print()
        {
            foreach (var pair in dictionary)
                Console.WriteLine(pair.Key + " " + pair.Value);
            if (dictionary.Count == 0)
                Console.WriteLine("No elements in the dictionary.");
        }

    }
}
