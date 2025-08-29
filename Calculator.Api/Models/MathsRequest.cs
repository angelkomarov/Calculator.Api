namespace Calculator.Api.Models
{
    using System.Xml.Serialization;
    using System.Text.Json.Serialization;

    public class Rootobject
    {
        [XmlElement("Maths")]
        [JsonPropertyName("Maths")]
        public MathsRequest Maths { get; set; }
    }


    [XmlRoot("Maths")]
    public class MathsRequest
    {
        [XmlElement("Operation")]
        [JsonPropertyName("Operation")]
        public OperationElement Operation { get; set; }
    }
}
