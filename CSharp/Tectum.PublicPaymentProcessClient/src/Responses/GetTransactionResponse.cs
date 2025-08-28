using Tectum.PublicPaymentProcessClient.Responses.Dtos;

namespace Tectum.PublicPaymentProcessClient.Responses;

public class GetTransactionResponse : BaseResponse
{
    public TransactionDto Transaction { get; set; }
}
