using System;
using System.Collections.Generic;
using System.Text;

namespace IMEPAC.Curso.Console.Entities
{
    public class Account : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Money { get; set; }
    }

    public interface IAccountBuilder
    {
        IAccountBuilder SetFirstName(string firstname);
        IAccountBuilder SetLastName(string firstname);
        IAccountBuilder SetMoney(int money);
        Account GetResult();
    }

    public class AccountBuilder : IAccountBuilder
    {
        private Account _account = new Account();

        public Account GetResult()
        {
            return _account;
        }

        public IAccountBuilder SetFirstName(string firstname)
        {
            _account.FirstName = firstname;
            return this;
        }

        public IAccountBuilder SetLastName(string lastname)
        {
            _account.LastName = lastname;
            return this;
        }

        public IAccountBuilder SetMoney(int money)
        {
            _account.Money = money;
            return this;
        }
    }
}
