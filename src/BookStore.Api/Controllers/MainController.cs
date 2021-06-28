using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Domain.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace BookStore.Api.Controllers
{
    [ApiController]
    public abstract class MainController : Controller
    {
        private readonly INotificator _notificator;
        public readonly IUser _appUser;

        protected Guid UserId { get; set; }
        protected bool IsAuthenticated { get; set; }

        protected MainController(INotificator notificator, IUser appUser)
        {
            _notificator = notificator;
            _appUser = appUser;

            if (appUser.IsAuthenticated())
            {
                UserId = appUser.GetUserId();
                IsAuthenticated = true;
            }
        }

        protected bool IsValid()
        {
            return !_notificator.HasNotifications();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (IsValid())
            {
                return Ok(new DefaultApiResponseViewModel
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new DefaultApiResponseViewModel
            {
                success = false,
                errors = _notificator.GetAll().Select(n => n.Message)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
                InvalidModelErrorNotify(modelState);

            return CustomResponse();
        }

        protected void InvalidModelErrorNotify(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                ErrorNotify(errorMsg);
            }
        }

        protected void ErrorNotify(string mensagem)
        {
            _notificator.Handle(new Notification(mensagem));
        }
    }
}
