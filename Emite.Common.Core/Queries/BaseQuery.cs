namespace Emite.Common.Core.Queries;

/// <summary>
/// A base class for queries.
/// </summary>
public abstract record BaseQuery
{
    /// <summary>
    /// The page to retrieve.
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// The number of records to retrieve.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// The column by which to sort the records with.
    /// </summary>
    public string? SortColumn { get; set; }

    /// <summary>
    /// ASC or DESC
    /// </summary>
    public string? SortOrder { get; set; }

    /// <summary>
    /// Columns to use for filtering the results.
    /// </summary>
    public string[]? SearchColumns { get; set; }

    /// <summary>
    /// The value to search for.
    /// </summary>
    public string? SearchValue { get; set; }
    public string CacheQueryParameter
    {
        get
        {
            // Handle null or empty SearchColumns
            string searchColumns = (SearchColumns != null && SearchColumns.Any())
                ? string.Join("_", SearchColumns.OrderBy(c => c))
                : "All";

            // Handle null or empty SortColumn and SortOrder
            string sortColumn = string.IsNullOrEmpty(SortColumn) ? "DefaultSort" : SortColumn;
            string sortOrder = string.IsNullOrEmpty(SortOrder) ? "ASC" : SortOrder.ToUpper();

            // Handle null SearchValue
            string searchValue = SearchValue ?? "None";

            // Construct the cache key
            return $"PageNumber_{PageNumber}_PageSize_{PageSize}_SortColumn_{sortColumn}_SortOrder_{sortOrder}_SearchColumns_{searchColumns}_SearchValue_{searchValue}";
        }
    }

    /// <summary>
    /// Initializes an instance of <see cref="BaseQuery"/>
    /// with default page number and page size.
    /// </summary>
    public BaseQuery()
    {
        PageNumber = 1;
        PageSize = 10;
    }

    /// <summary>
    /// Initializes an instance of <see cref="BaseQuery"/>
    /// with the specified page number and page size.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    public BaseQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize > 10 ? 10 : pageSize;
    }
}
