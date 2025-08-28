using Tectum.PublicPaymentProcessClient.Responses.Dtos;

namespace Tectum.PublicPaymentProcessClient.Responses;

public class GetCurrenciesResponse : BaseResponse
{
    public List<CurrencyDto> Currencies { get; set; }
}
