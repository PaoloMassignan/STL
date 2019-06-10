using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace STL.Test
{
    [TestClass]
    public class Errors
    {

        private TestContext m_testContext;

        public TestContext TestContext

        {

            get { return m_testContext; }

            set { m_testContext = value; }

        }

        [TestMethod]
        public void Expecting()
        {
            STCompiler compiler = new STCompiler();
            var result = compiler.Compile("");

            List<string> list = compiler.GetExpectedTokens();
            

            Assert.IsTrue(list.Count==3);
            
        }
    }
}
