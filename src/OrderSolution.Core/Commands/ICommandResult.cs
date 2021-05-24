namespace OrderSolution.Core.Commands
{
    public interface ICommandResult
    {
        bool Success { get; set; }
        public int Code { get; set; }
        object Data { get; set; }
    }
}
