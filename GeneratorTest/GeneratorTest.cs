using FunctionsGenerator;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneratorTest
{
    [TestClass]
    public class GeneratorTest
    {
        [TestMethod]
        public void GetFunctionTest()
        {
            // Arrange
            FunctionsGenerator.FunctionsGenerator functionsGenerator = new FunctionsGenerator.FunctionsGenerator();
            // Act
            Delegate resultDelegate = functionsGenerator.GetFunction();
            // Assert
            Assert.IsNotNull(resultDelegate);            
           
        }
    }
}
