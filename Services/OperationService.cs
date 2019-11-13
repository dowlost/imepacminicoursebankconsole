using IMEPAC.Curso.Console.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMEPAC.Curso.Console.Services
{
    public class OperationService
    {
        private Dictionary<Operation, string> _operations = new Dictionary<Operation, string>();
        private BankService _bankService;
        private LoanService _loanService;

        private static OperationService _instance;
        private static object _syncRoot = new Object();

        public OperationService()
        {
            _bankService = BankService.GetInstance();
            _loanService = LoanService.GetInstance();

            _operations.Add(Operation.CreateAccount, "1 - Abrir conta");
            _operations.Add(Operation.GetAccounts, "2 - Consultar Contas");
            _operations.Add(Operation.RequestLoan, "3 - Abrir pedido de empréstimo");
            _operations.Add(Operation.Leave, "4 - Sair");
        }

        /// <summary>
        /// Retorna todas as operações possíveis
        /// </summary>
        /// <returns></returns>
        public Dictionary<Operation, string> GetAllOperations()
        {
            return _operations;
        }

        /// <summary>
        /// Decide a ação para a operação escolhida
        /// </summary>
        /// <param name="operation"></param>
        public async Task RunOperation(Operation operation)
        {
            if (operation == Operation.CreateAccount)
                await RunCreateAccount();
            else if (operation == Operation.GetAccounts)
                await RunGetAccounts();
            else if (operation == Operation.RequestLoan)
                await RunRequestLoan();
        }

        private async Task RunCreateAccount()
        {
            ConsoleUtils.PrintMessage("Iniciando processo de abertura de conta. Por favor, responda abaixo: \n\n");

            //Pegando os dados necessários
            var firstname = ConsoleUtils.GetInput("Qual o primeiro nome do requisitor?");
            var lastname = ConsoleUtils.GetInput("Qual o sobrenome do requisitor?");
            var accountMoney = ConsoleUtils.GetIntegerInput("Qual o valor inicial para depósito?");

            //Tenta abrir a conta
            await _bankService.TryOpenAccount(firstname, lastname, accountMoney);

            ConsoleUtils.EndSection();
        }

        private async Task RunGetAccounts()
        {
            var accounts = await _bankService.GetAllAccounts();

            if (accounts.Count > 0)
                foreach (var account in accounts)
                    ConsoleUtils.PrintMessage($"Nome: {account.FirstName} {account.LastName} | Saldo: {account.Money}");
            else
                ConsoleUtils.PrintMessage("Não temos contas!");

            ConsoleUtils.EndSection();
        }

        private async Task RunRequestLoan()
        {
            ConsoleUtils.PrintMessage("Abrindo processo de requisição de empréstimo... \n\n");

            var firstname = ConsoleUtils.GetInput("Qual o primeiro nome do requisitor?");
            var lastname = ConsoleUtils.GetInput("Qual o sobrenome do requisitor?");
            var loanAmount = ConsoleUtils.GetIntegerInput("Qual o valor do empréstimo?");

            var approved = await _loanService.RequestLoan(firstname, lastname, loanAmount);

            if (approved)
                System.Console.WriteLine("Pedido aberto com sucesso, o requisitor possui todos os requisitos para o empréstimo.");
            else
                System.Console.WriteLine("Pedido de empréstimo recusado.");

            ConsoleUtils.EndSection();
        }
        
        public static OperationService GetInstance()
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    _instance = new OperationService();
                }
            }

            return _instance;
        }
    }

    public enum Operation
    {
        Unknow = 0,
        CreateAccount = 1,
        GetAccounts = 2,
        RequestLoan = 3,
        Leave = 4
    }
}
