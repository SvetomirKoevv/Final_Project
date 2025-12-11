namespace API.Infrastructure.RequestDTOs.Shared;
#nullable enable
public class BaseFilterModel
{
    public Pager? Pager { get; set; }
    public string? SortProperty { get; set; }
    public bool SortAscending { get; set; }
}
