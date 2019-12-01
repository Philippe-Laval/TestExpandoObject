using Moq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using TestExpandoObject;
using Xunit;

// https://www.red-gate.com/simple-talk/dotnet/c-programming/working-with-the-dynamic-type-in-c/

namespace TestExpandoObjectTests
{
    public class MessageServiceTests
    {
        private readonly Mock<IMessageBus> _messageBus;
        private readonly MessageService _service;

        public MessageServiceTests()
        {
            _messageBus = new Mock<IMessageBus>();

            _service = new MessageService(_messageBus.Object);
        }

        /*
        [Fact]
        public void SendsWithExpandoObject_Fails()
        {
            // arrange
            const string json = "{\"a\":1}";
            dynamic message = null;

            // Error : “An expression tree may not contain a dynamic operation.”
            _messageBus.Verify(m => m.Send(It.Is<ExpandoObject>(
                o => o != null && (o as dynamic).a == 1)));

            // act
            _service.SendRawJson<ExpandoObject>(json);

            // assert
            Assert.NotNull(message);
            Assert.Equal(1, message.a);
        }
        */

        [Fact]
        public void SendsWithExpandoObject()
        {
            // arrange
            const string json = "{\"a\":1}";
            dynamic message = null;

            _messageBus.Setup(m => m.Send(It.IsAny<ExpandoObject>()))
              .Callback<object>(o => message = o);

            // act
            _service.SendRawJson<ExpandoObject>(json);

            // assert
            Assert.NotNull(message);
            Assert.Equal(1, message.a);
        }

        [Fact]
        public void SendsWithDynamicObject()
        {
            // arrange
            const string json = "{\"a\":1,\"b\":\"1\"}";
            dynamic message = null;

            _messageBus.Setup(m => m.Send(It.IsAny<TypedDynamicJson<long>>()))
              .Callback<object>(o => message = o);

            // act
            _service.SendRawJson<TypedDynamicJson<long>>(json);

            // assert
            Assert.NotNull(message);
            Assert.Equal(1, message.a);
            Assert.Equal("a", string.Join(",", message.GetDynamicMemberNames()));
        }

    }
}
