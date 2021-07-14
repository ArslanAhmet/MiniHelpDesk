using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace IntegrationTests
{
    public class EmailTests 
    {
        [Fact]
        public void AlwaysSuccessForNo() 
        {
            Assert.Equal("Success", "Success1");
        } 
    }
}
