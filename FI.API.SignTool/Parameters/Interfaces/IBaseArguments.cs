namespace FI.API.SignTool.Parameters.Interfaces
{
    public interface IBaseArguments
    {
        string Data { get; set; }
        string DataFileName { get; set; }
        string InputData { get; }
    }
}