using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Way2.BusinessRules;

namespace Way2.BusinessRulesTests
{
    [TestClass]
    public class SearchTest
    {
        [TestMethod]
        public void TestEmptyWordSearch()
        {
            Way2Dictionary objDictionary = new Way2Dictionary();
            objDictionary.GetDataFromWebService(5);
        }
    }
}
