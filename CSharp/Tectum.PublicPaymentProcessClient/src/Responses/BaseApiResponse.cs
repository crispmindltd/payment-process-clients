
using Newtonsoft.Json;

namespace Tectum.PublicPaymentProcessClient.Responses;

public class BaseApiResponse
{
    public int ErrorCode { get; set; }

    public IList<string> ErrorMsgs { get; set; }

    [JsonIgnore]
    public ErrorResponse? Error { get; set; }

    [JsonIgnore]
    public virtual bool HasError
    {
        get
        {
            if (!ErrorMsgs.Any() || ErrorCode == 0)
            {
                if (Error != null)
                {
                    return Error.Code != 0;
                }

                return false;
            }

            return true;
        }
    }

    public BaseApiResponse()
    {
        ErrorMsgs = new List<string>();
    }

    public void AddErrorMsg(string msg)
    {
        if (ErrorMsgs == null)
        {
            IList<string> list2 = (ErrorMsgs = new List<string>());
        }

        ErrorMsgs.Add(msg);
    }

    public void AddErrorMsg(int code, string msg)
    {
        ErrorCode = code;
        AddErrorMsg(msg);
    }

    public BaseApiResponse WithError(int code, string msg)
    {
        ErrorCode = code;
        AddErrorMsg(msg);
        return this;
    }

    public override string ToString()
    {
        return $"Code: {ErrorCode}, message: {string.Join(",", ErrorMsgs)}";
    }

    public string GetErrorStr()
    {
        return $"Code: {Error.Code}, message: {Error.Msg}";
    }
}
