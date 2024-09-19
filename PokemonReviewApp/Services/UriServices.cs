using Microsoft.AspNetCore.WebUtilities;
using PokemonReviewApp.Filter;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Services;

public class UriServices : IUriService
{
    private readonly string _baseUri;

    public UriServices(string baseUri)
    {
        _baseUri = baseUri;
    }

    public Uri GetPageUri(PaginationFilter filter, string route)
    {
        var _endpointUri = new Uri(string.Concat(_baseUri, route));
        var modifiedUri = QueryHelpers.AddQueryString(_endpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
        return new Uri(modifiedUri);
    }
}
