namespace Calculator.Api.Services
{
    public class PlusOperation : BaseOperation
    {
        public override double Calculate(List<double> values) => 
            values.Sum();
    }   
}
