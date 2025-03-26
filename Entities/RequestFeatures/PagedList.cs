namespace Entities.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        // Nesneye yönelik programlama bilgisi gerekiyor.
        public PagedList(List<T> items , int count , int pageNumber , int pageSize)
        {
            MetaData = new MetaData()
            {
                TotalCount = count,
                PageSize = pageSize,
                TotalPage = (int)Math.Ceiling(count/(double)pageSize),
                
            };
            AddRange(items);
        }
        public static PagedList<T> ToPagedList(IEnumerable<T> source ,
            int pageNumber ,
            int pageSize)
            {
                var count = source.Count();
                var items = source
                    .Skip((pageNumber -1 ) * pageSize)
                    .Take(pageSize)
                    .ToList();
             return new PagedList<T>(items ,count , pageNumber , pageSize);
            }
    }
}