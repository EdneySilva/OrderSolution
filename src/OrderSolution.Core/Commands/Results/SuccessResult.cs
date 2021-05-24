namespace OrderSolution.Core.Commands.Results
{
    public class SuccessResult : ICommandResult
    {
        public SuccessResult(object data = null)
        {
            this.Success = true;
            this.Code = 200;
            this.Data = data;
        }

        public bool Success { get; set; }
        public int Code { get; set; }
        public object Data { get; set; }
    }
}
