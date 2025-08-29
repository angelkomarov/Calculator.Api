namespace Calculator.Api.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using System.Text.Json.Serialization;

    public class OperationElement
    {
        [XmlAttribute("ID")]
        [JsonPropertyName("@ID")]
        public string ID { get; set; }

        [XmlElement("Value")]
        [JsonPropertyName("Value")]
        public List<string> Values { get; set; } = new();

        [XmlElement("Operation")]
        [JsonPropertyName("Operation")]
        public OperationElement? NestedOperation { get; set; }
    }

}
