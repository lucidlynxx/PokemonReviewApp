namespace PokemonReviewApp.Wrappers;

public class PagedResponse<T> : Response<T>
{
    public Pages Pages { get; set; }

    public PagedResponse(T data, int pageNumber, int pageSize) : base(data)
    {
        Pages = new Pages
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        Code = 200;
        Data = data;
        Status = "success";
        Message = "data retrieved successfully";
    }
}
