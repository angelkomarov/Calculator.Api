using Calculator.Api.Models;

namespace Calculator.Api.Services.Interfaces
{
    public interface ICalculatorService
    {
        double Evaluate(OperationElement element);
    }
}
