using Tectum.PublicPaymentProcessClient.Responses.Dtos;

namespace Tectum.PublicPaymentProcessClient.Responses;

public class GetBalancesResponse : BaseApiResponse
{
    public List<BalanceDto> Balances { get; set; }
}
