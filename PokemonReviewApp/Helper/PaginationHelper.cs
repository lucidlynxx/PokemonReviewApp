using PokemonReviewApp.Filter;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Wrappers;

namespace PokemonReviewApp.Helper;

public class PaginationHelper
{
    public static PagedResponse<List<T>> CreatedPagedResponse<T>(List<T> pagedData, PaginationFilter validFilter, int TotalRecords, IUriService uriService, string route)
    {
        var response = new PagedResponse<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize);

        var TotalPages = (double)TotalRecords / validFilter.PageSize;

        int roundedTotalPages = Convert.ToInt32(Math.Ceiling(TotalPages));

        response.Pages.NextPage =
            validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
            ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
            : null;

        response.Pages.PreviousPage =
            validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
            ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
            : null;

        response.Pages.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route);


        response.Pages.LastPage = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.PageSize), route);

        response.Pages.TotalPages = roundedTotalPages;

        response.Pages.TotalRecords = TotalRecords;

        return response;
    }
}
