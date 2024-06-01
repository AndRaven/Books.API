
public class ResponseDto
{
    public object? Data { get; set; }

    public bool IsSuccess { get; set; } = true;

    public int StatusCode { get; set; } = 200;

    public string Message
    { get; set; } = string.Empty;

}