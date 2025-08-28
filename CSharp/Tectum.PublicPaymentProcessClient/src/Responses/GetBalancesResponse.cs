using Tectum.PublicPaymentProcessClient.Responses.Dtos;

namespace Tectum.PublicPaymentProcessClient.Responses;

public class GetBalancesResponse : BaseResponse
{
    public List<BalanceDto> Balances { get; set; }
}
