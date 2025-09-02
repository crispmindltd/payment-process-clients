namespace Tectum.PublicPaymentProcessClient.Responses;

[Serializable]
public class ErrorResponse
{
    public int? Code { get; set; }

    public string Msg { get; set; }
}
