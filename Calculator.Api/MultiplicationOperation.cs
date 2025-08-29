using Calculator.Api.Services;

namespace Calculator.Api
{
    public class MultiplicationOperation : BaseOperation
    {
        public override double Calculate(List<double> values) => 
            values.Aggregate(1.0, (acc, val) => acc * val);
    }
}
