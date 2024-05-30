using System.ComponentModel.DataAnnotations;
namespace Fina.Core.Requests.Categories
{
	public class DeleteCategoryRequest : Request
	{
		[Required(ErrorMessage = "Id inválido")]
		public long Id { get; set; }
	}
}