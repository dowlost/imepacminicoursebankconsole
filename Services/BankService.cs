using IMEPAC.Curso.Console.Entities;
using IMEPAC.Curso.Console.Repositories;
using IMEPAC.Curso.Console.Repositories.Interfaces;
using IMEPAC.Curso.Console.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMEPAC.Curso.Console.Services
{

    public class BankService
    {
        private readonly IRepository<Account> _repository;
        private readonly IAccountBuilder _builder;
        private static BankService _instance;

        private static object _syncRoot = new Object();

        private BankService(IRepository<Account> accountRepository,
            IAccountBuilder accountBuilder)
        {

            _repository = accountRepository;
            _builder = accountBuilder;
        }

        /// <summary>
        /// Valida os dados do cliente e caso passe na validação, grava os dados da conta para futuras consultas e gerencia.
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="money"></param>
        public async Task TryOpenAccount(string firstname, string lastname, int money)
        {
            var account = _builder.SetFirstName(firstname)
                .SetLastName(lastname)
                .SetMoney(money)
                .GetResult();

            if (ValidateAccount(account))
            {
                await _repository.Insert(account);
                ConsoleUtils.PrintMessage("Conta aberta com sucesso!");
            }
        }

        /// <summary>
        /// Checa se o usuário tem todos os requisitos apra abertura de conta.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private bool ValidateAccount(Account account)
        {
            if (account.Money < 1000)
            {
                ConsoleUtils.PrintMessage("Falha: não abrimos conta com menos de 1000");
                return false;
            }

            var personStatus = new PersonStatusService(account.FirstName, account.LastName);
            
            if (!personStatus.NameIsClear())
            {
                ConsoleUtils.PrintMessage("Falha: encontramos problemas financeiros relacionados este nome");
                return false;
            }

            if (personStatus.IsForeign())
            {
                ConsoleUtils.PrintMessage("Falha: cliente estrangeiro");
                return false;
            }

            if (personStatus.HasCrimeHistory())
            {
                ConsoleUtils.PrintMessage("Falha: encontramos histórico criminal relacionado este nome");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Busca e retorna a conta do usuário, se existir.
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <returns></returns>
        public async Task<Account> TryGetAccount(string firstname, string lastname)
        {
            return await _repository.GetById(firstname.ToLower() + lastname.ToLower());
        }

        /// <summary>
        /// Retorna todas as contas salvas
        /// </summary>
        /// <returns></returns>
        public async Task<List<Account>> GetAllAccounts()
        {
            return await _repository.GetAll();
        }

        public static BankService GetInstance()
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    _instance = new BankService(new AccountRepository(),
                        new AccountBuilder());
                }
            }

            return _instance;
        }
    }
}
