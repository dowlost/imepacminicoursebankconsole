using IMEPAC.Curso.Console.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMEPAC.Curso.Console.Services
{
    public class LoanService
    {
        private static object _syncRoot = new Object();
        private static LoanService _instance;
        
        public async Task<bool> RequestLoan(string firstname, string lastname, int loanAmount)
        {
            var approved = await ValidateLoanRequest(firstname, lastname, loanAmount);

            return approved;
        }

        private async Task<bool> ValidateLoanRequest(string firstname, string lastname, int loanAmount)
        {
            var personStatus = new PersonStatusService(firstname, lastname);

            if (!personStatus.NameIsClear())
            {
                ConsoleUtils.PrintMessage("Empréstimo recusado: encontramos problemas financeiros relacionados este nome");
                return false;
            }

            if (personStatus.HasCrimeHistory())
            {
                ConsoleUtils.PrintMessage("Empréstimo recusado: encontramos histórico criminal relacionado este nome");
                return false;
            }

            var account = await BankService.GetInstance().TryGetAccount(firstname, lastname);

            if (account == null)
            {
                ConsoleUtils.PrintMessage("Empréstimo recusado: O cliente não possui conta no nosso banco");
                return false;
            }


            if (loanAmount > account.Money)
            {
                ConsoleUtils.PrintMessage("Empréstimo recusado: Não fazemos empréstimos maiores que a quantia que o cliente possui em conta");
                return false;
            }

            return true;

        }

        public static LoanService GetInstance()
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    _instance = new LoanService();
                }
            }

            return _instance;
        }
    }
}
