namespace API.Infrastructure.RequestDTOs.Shared;

public class Pager
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }

    public Pager()
    {
        PageNumber = 1;
        PageSize = 10;
    }
}
