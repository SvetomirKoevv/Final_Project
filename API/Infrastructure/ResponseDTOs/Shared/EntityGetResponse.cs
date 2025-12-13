namespace API.Infrastructure.ResponseDTOs.Shared;

public class EntityGetResponse<E, EGetRequest>
{
    public List<E> Items { get; set; }
    public EGetRequest FilterInfo { get; set; }
}
