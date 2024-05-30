using System.ComponentModel.DataAnnotations;

namespace Fina.Core.Requests.Categories
{
	public class UpdateCategoryRequest : Request
	{
		public long Id { get; set; }
		
		[Required(ErrorMessage = "Título inválido")]
		[MaxLength(80, ErrorMessage = "Título deve ter no maximo 80 caracteres")]
		public string Title { get; set; } = string.Empty;
		
		[Required(ErrorMessage = "Descricão inválida")]
		public string? Description { get; set; } = string.Empty;
	}
}