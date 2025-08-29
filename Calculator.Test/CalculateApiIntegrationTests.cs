using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Test
{
    [TestClass]
    public class CalculateApiIntegrationTests
    {
        private static WebApplicationFactory<Program> factory;
        private static HttpClient client;

        [ClassInitialize]
        public static void Init(TestContext context)
        {
            factory = new WebApplicationFactory<Program>();
            client = factory.CreateClient();
        }

        [TestMethod]
        public async Task PostJsonRequest_ReturnsJsonResponse()
        {
            // Arrange
            string json = """
            {
              "Maths": {
                "Operation": {
                  "@ID": "Plus",
                  "Value": ["2", "3"],
                  "Operation": {
                    "@ID": "Multiplication",
                    "Value": ["4", "5"]
                  }
                }
              }
            }
            """;

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/api/Calculator/calculate", content);
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            StringAssert.Contains(responseString, "25");
            StringAssert.Contains(response.Content.Headers.ContentType.ToString(), "application/json");
        }

        [TestMethod]
        public async Task PostXmlRequest_ReturnsXmlResponse()
        {
            var xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <Maths>
                  <Operation ID=""Plus"">
                    <Value>2</Value>
                    <Value>3</Value>
                    <Operation ID=""Multiplication"">
                      <Value>4</Value>
                      <Value>5</Value>
                    </Operation>
                  </Operation>
                </Maths>";

            var content = new StringContent(xml, Encoding.UTF8, "application/xml");

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/Calculator/calculate");
            request.Content = content;

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var resultString = await response.Content.ReadAsStringAsync();

            Console.WriteLine("XML Response:");
            Console.WriteLine(resultString);

            Assert.IsTrue(resultString.Contains("<Result>")); // simple check
        }


    }
}
