using Calculator.Api.Services.Interfaces;

namespace Calculator.Api.Services
{
    public abstract class BaseOperation : IOperation
    {
        public abstract double Calculate(List<double> values);
    }
}
