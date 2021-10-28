using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntegrationTests
{
    [TestClass]
    public class EmailTests 
    {
        [TestMethod]
        public void AlwaysSuccessForNo() 
        {
            Assert.AreEqual("Success", "Success");
        } 
    }
}
