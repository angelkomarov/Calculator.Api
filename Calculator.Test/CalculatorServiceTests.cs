using Calculator.Api.Models;
using Calculator.Api.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Calculator.Test
{
    [TestClass]
    public class CalculatorServiceTests
    {
        private CalculatorService calculator;

        [TestInitialize]
        public void Setup()
        {
            calculator = new CalculatorService();
        }

        [TestMethod]
        public void Evaluate_PlusOperation_ReturnsCorrectSum()
        {
            var operation = new OperationElement
            {
                ID = "Plus",
                Values = new List<string> { "2", "3" }
            };

            var result = calculator.Evaluate(operation);

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Evaluate_MultiplicationOperation_ReturnsCorrectProduct()
        {
            var operation = new OperationElement
            {
                ID = "Multiplication",
                Values = new List<string> { "4", "5" }
            };

            var result = calculator.Evaluate(operation);

            Assert.AreEqual(20, result);
        }

        [TestMethod]
        public void Evaluate_NestedOperation_EvaluatesRecursively()
        {
            var operation = new OperationElement
            {
                ID = "Plus",
                Values = new List<string> { "2", "3" },
                NestedOperation = new OperationElement
                {
                    ID = "Multiplication",
                    Values = new List<string> { "4", "5" }
                }
            };

            var result = calculator.Evaluate(operation);

            Assert.AreEqual(25, result); // 2 + 3 + (4 * 5)
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void Evaluate_UnknownOperation_ThrowsException()
        {
            var operation = new OperationElement
            {
                ID = "Divide",
                Values = new List<string> { "10", "2" }
            };

            calculator.Evaluate(operation);
        }

        [TestMethod]
        public void Evaluate_HandlesEmptyValuesAndNestedOnly()
        {
            var operation = new OperationElement
            {
                ID = "Plus",
                Values = new List<string>(), // Empty list
                NestedOperation = new OperationElement
                {
                    ID = "Multiplication",
                    Values = new List<string> { "3", "3", "3" }
                }
            };

            var result = calculator.Evaluate(operation);

            Assert.AreEqual(27, result); // Only result from nested operation
        }
    }
}
