using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Person : Entity
    {
        public int Code { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public PersonType Type { get; set; }

    }

    public enum PersonType
    {
        Customer,
        Administrator
    }
}
