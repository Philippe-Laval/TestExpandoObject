using System;
using System.Collections.Generic;
using System.Text;

// https://www.tutorialspoint.com/csharp/csharp_attributes.htm

/*
[AttributeUsage (
   validon,
   AllowMultiple = allowmultiple,
   Inherited = inherited
)]

[AttributeUsage(
   AttributeTargets.Class |
   AttributeTargets.Constructor |
   AttributeTargets.Field |
   AttributeTargets.Method |
   AttributeTargets.Property, 
   AllowMultiple = true)]

[Conditional("DEBUG")]

[Obsolete("Don't use OldMethod, use NewMethod instead", true)]


*/


namespace TestExpandoObject
{
    [AttributeUsage(AttributeTargets.All)]
    public class HelpAttribute : System.Attribute
    {
        public readonly string Url;

        // Topic is a named parameter 
        public string Topic { get; set; }

        /// <summary>
        /// url is a positional parameter
        /// </summary>
        /// <param name="url"></param>
        public HelpAttribute(string url)
        {
            this.Url = url;
        }
    }
}
