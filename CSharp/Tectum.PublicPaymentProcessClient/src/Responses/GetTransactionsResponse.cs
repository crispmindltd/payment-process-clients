using Tectum.PublicPaymentProcessClient.Responses.Dtos;

namespace Tectum.PublicPaymentProcessClient.Responses;

public class GetTransactionsResponse : BaseResponse
{
    public List<TransactionDto> Transactions { get; set; }
}
