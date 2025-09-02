using Tectum.PublicPaymentProcessClient.Responses.Dtos;

namespace Tectum.PublicPaymentProcessClient.Responses;

public class GetTransactionsResponse : BaseApiResponse
{
    public List<TransactionDto> Transactions { get; set; }
}
