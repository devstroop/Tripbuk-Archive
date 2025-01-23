namespace Tripbuk.Client.Models;

public class SearchFreeTextRequest 
{
    public string SearchTerm { get; set; }
    public string Currency { get; set; }
}

public class FreetextSearchProductFiltering 
{
    public string Destination { get; set; }
    public string DateRange { get; set; }
}

public class FreetextSearchProductSorting 
{
    public string Sort { get; set; }
    public string Order { get; set; }
}

public class FreetextSearchType
{
    public string SearchType { get; set; }
    public FreetextSearchTypePagination Pagination { get; set; }
}

public class FreetextSearchTypePagination
{
    public int Start { get; set; }
    public int Count { get; set; }
}