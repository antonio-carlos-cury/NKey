
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Models
{
    public class BookViewModel
    {

        [Required(ErrorMessage = "Faltou informar o códido do livro")]
        [Display(Name = "Cod. Livro")]
        public int Code { get; set; }

        [Required(ErrorMessage = "Faltou informar o nome do livro")]
        [Display(Name = "Nome do livro")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Faltou informar o nome do autor")]
        [Display(Name = "Nome do autor")]
        public string AuthorName { get; set; }

        [Required(ErrorMessage = "Faltou informar o ISBN do livro.")]
        [Display(Name = "Cod. ISBN")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Faltou informar o ano de lançamento do livro.")]
        [Display(Name = "Ano de lançamento")]
        public short ReleaseYear { get; set; }

        [Display(Name = "Prefácio")]
        public string Preface { get; set; }

        [Display(Name = "Categoria do livro")]
        public IEnumerable<CategoryViewModel> Categories{ get; set; }

        [Display(Name = "Total de capítulos")]
        public short TotalChaptersNumbers { get; set; }

        [Display(Name = "Total de páginas")]
        public short TotalPagesNumbers { get; set; }


    }
}
