namespace Fina.Core.Requests
{
	public abstract class PagedRequest : Request
	{
		public int PageSize { get; set; } = Configurations.DefaultPageSize;
		public int PageNumber { get; set; } = Configurations.DefaultPageNumber;
		
	}
}