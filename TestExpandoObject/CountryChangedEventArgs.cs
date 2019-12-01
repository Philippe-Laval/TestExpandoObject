using System;
using System.Collections.Generic;
using System.Text;

namespace TestExpandoObject
{
    public class CountryChangedEventArgs : EventArgs
    {
        public string Country { get; set; }
    }
}
