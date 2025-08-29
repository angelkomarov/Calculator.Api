using Calculator.Api.Models;
using Calculator.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Xml.Serialization;

namespace Calculator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController(ICalculatorService calculator) : ControllerBase
    {
        [HttpPost("calculate")]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> Calculate()
        {
            Rootobject root = null;

            if (Request.ContentType.Contains("application/json"))
            {
                root = await JsonSerializer.DeserializeAsync<Rootobject>(Request.Body, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            else if (Request.ContentType.Contains("application/xml"))
            {
                string xmlContent;
                using (var reader = new StreamReader(Request.Body))
                {
                    xmlContent = await reader.ReadToEndAsync();
                }

                var serializer = new XmlSerializer(typeof(MathsRequest));
                using var stringReader = new StringReader(xmlContent);
                //TextReader is the abstract base class of StreamReader and StringReader
                var maths = (MathsRequest)serializer.Deserialize(stringReader);

                root = new Rootobject { Maths = maths };
            }
            else
            {
                return BadRequest("Unsupported Content-Type");
            }

            if (root?.Maths?.Operation == null)
                return BadRequest("Invalid input structure");

            var result = calculator.Evaluate(root.Maths.Operation);
            return new ObjectResult(new CalculatorResponse { Result = result });
        }

        /// <summary>
        /// Not supporting XML message serialization: 
        /// JSON expects Rootobject as root 
        /// XML expects Maths as root 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("calculatejson")]
        [Consumes("application/json", "application/xml")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> CalculateJsonOnly(Rootobject request)
        {
            var result = calculator.Evaluate(request.Maths.Operation);
            var response = new { Result = result };
            return Ok(response);
        }
    }

}
