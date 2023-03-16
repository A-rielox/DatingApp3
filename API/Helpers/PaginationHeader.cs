namespace API.Helpers;

// se manda en la HttpResponse
// se usa en HttpExtensions - AddPaginationHeader
public class PaginationHeader
{
    public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
    {
        CurrentPage = currentPage;
        ItemsPerPage = itemsPerPage;
        TotalItems = totalItems;
        TotalPages = totalPages;
    }

    public int CurrentPage { get; set; }
    public int ItemsPerPage { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}
