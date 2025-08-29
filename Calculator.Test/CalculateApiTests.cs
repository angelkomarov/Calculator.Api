using Calculator.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Test
{
    [TestClass]
    public class CalculateApiTests
    {
        private static CustomWebApplicationFactory factory;
        private static HttpClient client;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            factory = new CustomWebApplicationFactory();
            client = factory.CreateClient();
        }

        [TestMethod]
        public async Task PostJsonRequest_UsesMockedCalculator()
        {
            // Arrange - setup mock behavior
            factory.CalculatorMock
                .Setup(c => c.Evaluate(It.IsAny<OperationElement>()))
                .Returns(1234); // force the return value

        string json = """
        {
          "Maths": {
            "Operation": {
              "@ID": "Plus",
              "Value": ["2", "3"]
            }
          }
        }
        """;

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/calculator/calculate");
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            // Assert - verify mocked value returned
            response.EnsureSuccessStatusCode();
            StringAssert.Contains(body, "1234");

            // Verify Evaluate() was called exactly once
            factory.CalculatorMock.Verify(c => c.Evaluate(It.IsAny<OperationElement>()), Times.Once);
        }
    }
}