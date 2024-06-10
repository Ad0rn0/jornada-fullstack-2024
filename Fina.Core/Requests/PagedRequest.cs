namespace Fina.Core.Requests
{
	public abstract class PagedRequest : Request
	{
		public int PageSize { get; set; } = Configuration.DefaultPageSize;
		public int PageNumber { get; set; } = Configuration.DefaultPageNumber;
		
		public int PageCount { get; set; } = (Configuration.DefaultPageNumber - 1) * Configuration.DefaultPageSize;
	}
}