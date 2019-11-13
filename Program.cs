using IMEPAC.Curso.Console.Entities;
using IMEPAC.Curso.Console.Services;
using IMEPAC.Curso.Console.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IMEPAC.Curso.Console
{
    class Program
    {

        private static Dictionary<Operation, string> operations = new Dictionary<Operation, string>();

        static void Main(string[] args)
        {
            operations.Add(Operation.CreateAccount, "1 - Abrir conta");
            operations.Add(Operation.GetAccounts, "2 - Consultar Contas");
            operations.Add(Operation.RequestLoan, "3 - Abrir pedido de empréstimo");
            operations.Add(Operation.Leave, "4 - Sair");

            var currentOperation = Operation.Unknow;

            ConsoleUtils.PrintMessage("Bem vindo ao nosso banco!");
            ConsoleUtils.Spacer();
            ConsoleUtils.Spacer();
            
            do
            {
                try
                {
                    foreach (var operation in operations)
                        ConsoleUtils.PrintMessage(operation.Value);

                    ConsoleUtils.Spacer();

                    currentOperation = (Operation)ConsoleUtils.GetIntegerInput("Digite o nro da operação: ");
                    RunOperation(currentOperation);
                }
                catch (BankException be)
                {
                    ConsoleUtils.PrintMessage(be.Message);
                }
                catch
                {
                    ConsoleUtils.PrintMessage("Um erro inesperado aconteceu. Por favor, reinicie o sistema.");
                }

            }
            while (currentOperation != Operation.Leave);
        }

        /// <summary>
        /// Decide a ação para a operação escolhida
        /// </summary>
        /// <param name="operation"></param>
        private static void RunOperation(Operation operation)
        {
            if(operation == Operation.CreateAccount)
            {
                RunCreateAccount();
            }
            else if (operation == Operation.GetAccounts)
            {
                RunGetAccounts();
            }
            else if (operation == Operation.RequestLoan)
            {
                RunRequestLoan();
            }
        }


        private static void RunCreateAccount()
        {
            ConsoleUtils.PrintMessage("Iniciando processo de abertura de conta. Por favor, responda abaixo: \n\n");

            //Pegando os dados necessários
            var firstname = ConsoleUtils.GetInput("Qual o primeiro nome do requisitor?");
            var lastname = ConsoleUtils.GetInput("Qual o sobrenome do requisitor?");
            var accountMoney = ConsoleUtils.GetIntegerInput("Qual o valor inicial para depósito?");

            //Tenta abrir a conta
            BankService.GetInstance().TryOpenAccount(firstname, lastname, accountMoney);

            ConsoleUtils.EndSection();
        }


        private async static void RunGetAccounts()
        {
            var accounts = await BankService.GetInstance().GetAllAccounts();

            if (accounts.Count > 0)
                foreach (var account in accounts)
                    ConsoleUtils.PrintMessage($"Nome: {account.FirstName} {account.LastName} | Saldo: {account.Money}");
            else
                ConsoleUtils.PrintMessage("Não temos contas!");

            ConsoleUtils.EndSection();
        }


        private async static void RunRequestLoan()
        {
            ConsoleUtils.PrintMessage("Abrindo processo de requisição de empréstimo... \n\n");

            var firstname = ConsoleUtils.GetInput("Qual o primeiro nome do requisitor?");
            var lastname = ConsoleUtils.GetInput("Qual o sobrenome do requisitor?");
            var loanAmount = ConsoleUtils.GetIntegerInput("Qual o valor do empréstimo?");

            var approved = await LoanService.GetInstance().RequestLoan(firstname, lastname, loanAmount);

            if(approved)
                System.Console.WriteLine("Pedido aberto com sucesso, o requisitor possui todos os requisitos para o empréstimo.");
            else
                System.Console.WriteLine("Pedido de empréstimo recusado.");

            ConsoleUtils.EndSection();
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
