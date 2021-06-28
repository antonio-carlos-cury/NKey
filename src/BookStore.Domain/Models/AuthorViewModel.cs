using System;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Domain.Models
{
    public class AuthorViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Cod. do autor")]
        public int Code { get; set; }

        [Display(Name = "Ativo?")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Faltou informar o email do autor do livro")]
        [Display(Name = "Email do autor")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Faltou informar o nome do autor do livro")]
        [Display(Name = "Nome do autor")]
        public string Name { get; set; }

    }
}
