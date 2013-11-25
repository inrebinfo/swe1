using System;
using System.Collections.Generic;
using EmbeddedSensorCloud;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmbeddedSensorCloud.Test
{
    [TestClass]
    public class CWebURLTest
    {
        [TestMethod]
        public void CWebURL_CreateParameters_Dictionary_is_correctly_filled()
        {
            Dictionary<string, string> _dictToCheck = new Dictionary<string, string>();

            _dictToCheck.Add("param1", "value1");
            _dictToCheck.Add("param2", "value2");

            string _adressToTest = "somefile.html";
            string _paramsToTest = "param1=value1&param2=value2";
            var _URL = new CWebURL(_adressToTest, _paramsToTest);

            Assert.AreEqual(_dictToCheck.Count, _URL.WebParameters.Count);
        }
    }
}
