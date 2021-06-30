using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Notifications;
using FluentValidation;
using FluentValidation.Results;

namespace BookStore.Service.Core
{
    public abstract class BaseService
    {
        public readonly INotificator _notificator;

        protected BaseService(INotificator notificador)
        {
            _notificator = notificador;
        }

        protected void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }

        protected void Notify(string mensagem)
        {
            _notificator.Handle(new Notification(mensagem));
        }

        protected bool ValidateEntity<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE> where TE : Entity
        {
            var validator = validacao.Validate(entidade);

            if (validator.IsValid) return true;

            Notify(validator);

            return false;
        }
    }
}
