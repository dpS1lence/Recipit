namespace Recipit.Pagination
{
    using System.Collections;
    using System.Collections.Generic;
    using Recipit.Pagination.Contracts;

    public class Page<T>(IEnumerable<T> values, int currentPage, int pageSize, int totalCount) : IPage<T>
    {
        private readonly IEnumerable<T> _values = values;

        public int CurrentPage { get; } = currentPage;
        public int PageSize { get; } = pageSize;
        public int TotalCount { get; } = totalCount;
        public bool HasPreviousPage => (CurrentPage - 1 > 0);
        public bool HasNextPage => (CurrentPage + 1 <= Math.Ceiling((double)TotalCount / (double)PageSize));

        public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
    }
}
