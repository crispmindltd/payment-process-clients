using Tectum.PublicPaymentProcessClient.Responses.Dtos;

namespace Tectum.PublicPaymentProcessClient.Responses;

public class GetTransactionResponse : BaseApiResponse
{
    public TransactionDto Transaction { get; set; }
}
