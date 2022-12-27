namespace Search.API.Models;

public class SearchFilter
{
    public string Keyword { get; set; }
    public int? MinQuantity { get; set; }
    public int? MaxQuantity { get; set; }
}