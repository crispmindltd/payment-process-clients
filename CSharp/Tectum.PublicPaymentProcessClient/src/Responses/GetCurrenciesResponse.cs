using Tectum.PublicPaymentProcessClient.Responses.Dtos;

namespace Tectum.PublicPaymentProcessClient.Responses;

public class GetCurrenciesResponse : BaseApiResponse
{
    public List<CurrencyDto> Currencies { get; set; }
}
