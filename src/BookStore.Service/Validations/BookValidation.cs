using FluentValidation;

namespace BookStore.Service.Validations
{
    public class BookValidation : AbstractValidator<Domain.Entities.Book>
    {
        public BookValidation()
        {
            RuleFor(f => f.Name)
                .NotEmpty().WithMessage("O campo Nome do livro precisa ser informado")
                .Length(2, 100)
                .WithMessage("O campo Nome do livro precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(f => f.Code)
                .NotEmpty().WithMessage("O campo Código do livro precisa ser informado")
                .GreaterThanOrEqualTo(1)
                .WithMessage("O campo Código do livro precisa ser maior que 1")
                .LessThanOrEqualTo(int.MaxValue)
                .WithMessage("O campo Código do livro precisa ser menor que " + int.MaxValue);

            RuleFor(f => f.Isbn)
                .NotEmpty().WithMessage("O campo ISBN precisa ser informado")
                .Length(13)
                .WithMessage("O campo ISBN precisa ter 13 caracteres");

            RuleFor(f => f.Author)
                .NotNull().WithMessage("O autor do livro precisa ser informado");

            RuleFor(f => f.Category)
                .NotNull().WithMessage("A categoria do livro precisa ser informada");
        }
    }
}
