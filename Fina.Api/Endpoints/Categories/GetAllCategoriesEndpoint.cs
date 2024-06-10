using Microsoft.AspNetCore.Mvc;

using Fina.Api.Common.Api;
using Fina.Core;
using Fina.Core.Handlers;
using Fina.Core.Models;
using Fina.Core.Requests.Categories;
using Fina.Core.Responses;

namespace Fina.Api.Endpoints.Categories
{
	public class GetAllCategoriesEndpoint : IEndPoint
	{
		public static void Map(IEndpointRouteBuilder app)
		{
			app.MapGet("/", HandleAsync).WithName("Categories: Get All").WithSummary("Recupera todas as categorias").WithDescription("Recupera todas as categorias").WithOrder(5).Produces<PagedResponse<List<Category>?>>();
		}
		
		private static async Task<IResult> HandleAsync(
			ICategoryHandler handler,
			[FromQuery] int pageNumber = Configuration.DefaultPageNumber,
			[FromQuery] int pageSize = Configuration.DefaultPageSize)
		{
			var request = new GetAllCategoriesRequest();
			request.UserId = ApiConfiguration.UserId;
			request.PageNumber = pageNumber;
			request.PageSize = pageSize;
			
			var result = await handler.GetAllAsync(request);
			return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
		}
	}
}