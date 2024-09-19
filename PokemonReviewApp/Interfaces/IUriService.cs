using PokemonReviewApp.Filter;

namespace PokemonReviewApp.Interfaces;

public interface IUriService
{
    public Uri GetPageUri(PaginationFilter filter, string route);
}
