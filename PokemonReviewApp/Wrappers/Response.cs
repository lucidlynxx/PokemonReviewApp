namespace PokemonReviewApp.Wrappers;

public class Response<T>
{
    public T Data { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
    public int Code { get; set; }

    public Response(T data)
    {
        Code = 200;
        Status = "success";
        Message = "data retrieved successfully";
        Data = data;
    }
}
