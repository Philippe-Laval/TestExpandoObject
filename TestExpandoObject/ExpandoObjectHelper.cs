using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text;

// https://www.oreilly.com/learning/building-c-objects-dynamically

namespace TestExpandoObject
{
    public static class ExpandoObjectHelper
    {
        // For both AddEvent and AddProperty, you might be asking why didn’t we use extension methods for AddProperty and AddEvent? 
        // They both could hang off of ExpandoObject and make the syntax even cleaner, right? 
        // Unfortunately, that is not possible as extension methods work by the compiler doing a search on all classes that might be a match for the extended class. 
        // This means that the DLR would have to know all of this information at runtime as well (since ExpandoObject is handled by the DLR) 
        // and currently all of that information is not encoded into the call site for the class and methods.

        public static void AddProperty(ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }

        public static void AddEvent(ExpandoObject expando, string eventName, Action<object, EventArgs> handler)
        {
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(eventName))
                expandoDict[eventName] = handler;
            else
                expandoDict.Add(eventName, handler);
        }

        public static ExpandoObject ToExpandoObject(this object obj)
        {
            // https://sebnilsson.com/blog/convert-c-anonymous-or-any-types-into-dynamic-expandoobject/

            // Null-check

            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(obj.GetType()))
            {
                expando.Add(property.Name, property.GetValue(obj));
            }

            return (ExpandoObject)expando;
        }

        /*
         string strCust = JsonConvert.SerializeObject(cust, new ExpandoObjectConverter());

To convert that string back to your original ExpandoObject, you'll use code like this:

cust = JsonConvert.DeserializeObject<ExpandoObject>(res, new ExpandoObjectConverter());

         */


    }
}
