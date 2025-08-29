using Calculator.Api.Models;
using Calculator.Api.Services.Interfaces;

namespace Calculator.Api.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly Dictionary<string, IOperation> _operations = new();

        public CalculatorService()
        {
            _operations["Plus"] = new PlusOperation();
            _operations["Multiplication"] = new MultiplicationOperation();
        }

        public double Evaluate(OperationElement element)
        {
            if (!_operations.ContainsKey(element.ID))
                throw new InvalidOperationException($"Unsupported operation: {element.ID}");

            var values = new List<double>();

            foreach (var val in element.Values)
            {
                if (double.TryParse(val, out var d))
                    values.Add(d);
            }

            if (element.NestedOperation != null)
                values.Add(Evaluate(element.NestedOperation));

            return _operations[element.ID].Calculate(values);
        }
    }
}
