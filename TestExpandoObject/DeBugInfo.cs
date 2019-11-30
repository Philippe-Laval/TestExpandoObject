using System;
using System.Collections.Generic;
using System.Text;

namespace TestExpandoObject
{
    // A custom attribute BugFix to be assigned to a class and its members
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Constructor |
        AttributeTargets.Field |
        AttributeTargets.Method |
        AttributeTargets.Property,
        AllowMultiple = true)]
    public class DeBugInfo : System.Attribute
    {
        public string message;

        public DeBugInfo(int bg, string dev, string d)
        {
            this.BugNo = bg;
            this.Developer = dev;
            this.LastReview = d;
        }
        public int BugNo { get; }
        public string Developer { get; }
        public string LastReview { get; }

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
    }
}
