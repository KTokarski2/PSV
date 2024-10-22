namespace PSV.Models.DTOs;

public class PaginationViewModel<T>
{
    public List<T> Items { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);

    public PaginationViewModel(List<T> items, int totalItems, int pageNumber, int pageSize)
    {
        Items = items;
        TotalItems = totalItems;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}