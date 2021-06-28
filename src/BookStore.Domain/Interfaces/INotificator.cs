using BookStore.Domain.Notifications;
using System.Collections.Generic;

namespace BookStore.Domain.Interfaces
{
    public interface INotificator
    {
        bool HasNotifications();
        List<Notification> GetAll();
        void Handle(Notification notification);
    }
}
