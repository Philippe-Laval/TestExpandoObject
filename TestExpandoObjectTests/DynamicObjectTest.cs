using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TestExpandoObject;
using Xunit;
using Xunit.Abstractions;

namespace TestExpandoObjectTests
{
    public class DynamicObjectTest
    {
        private readonly ITestOutputHelper output;

        public DynamicObjectTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void DeserializeJsonToExpandoObjectTest()
        {
            // Note : We will accpet only 1 and not "2" since it is not a long
            string json = "{\"a\":1,\"b\":\"2\"}";
            var dynObj = JsonConvert.DeserializeObject<TypedDynamicJson<long>>(json) as dynamic;

            output.WriteLine($"dynObj.a = {dynObj?.a}, type of {dynObj?.a.GetType()}");

            var members = string.Join(",", dynObj?.GetDynamicMemberNames());
            output.WriteLine($"dynObj member names: {members}");

            Assert.NotNull(dynObj);
            Assert.Equal("a", members);
            Assert.Equal(1L, dynObj.a);
        }
    }
}
