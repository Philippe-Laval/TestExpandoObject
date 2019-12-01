using System;
using System.Collections.Generic;
using System.Text;

namespace TestExpandoObject
{
    public interface IMessageBus
    {
        void Send(dynamic message);
    }
}
