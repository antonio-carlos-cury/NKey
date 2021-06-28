using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Models
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "Faltou informar o códido da categoria")]
        [Display(Name = "Cod. Categoria")]
        public int Code { get; set; }

        [Required(ErrorMessage = "Faltou informar o nome da categoria")]
        [Display(Name = "Nome da categoria")]
        public string Name { get; set; }
    }
}
