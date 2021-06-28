using System;

namespace BookStore.Domain.Notifications
{
    public class Notification
    {
        public Notification(string msg) 
        { 
            Message = msg;
            CreatedDate = DateTime.Now;
        }

        public string Message { get; }

        public DateTime CreatedDate { get; }

        public bool Read { get; }
    }
}
