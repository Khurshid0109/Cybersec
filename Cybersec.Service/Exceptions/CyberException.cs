namespace Cybersec.Service.Exceptions;
public class CyberException:Exception
{
    public int StatusCode { get; set; }
    public CyberException(int code,string message):base(message)
    {
        StatusCode = code;
    }
}
