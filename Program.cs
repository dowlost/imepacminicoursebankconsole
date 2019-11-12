using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IMEPAC.Curso.Console
{
    class Program
    {
        private static List<Account> accounts = new List<Account>();
        private static string filename = Path.Combine(Path.GetTempPath(), "impec_curso_accounts.json");

        private static string next = "\n---------------------------------------------------------------------------------------------------------------\n\n";

        static void Main(string[] args)
        {

            if (File.Exists(filename))
                using (StreamReader sr = File.OpenText(filename))
                {
                    string fullText = "";
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                        fullText += s;

                    if (!string.IsNullOrEmpty(fullText))
                        accounts = JsonConvert.DeserializeObject<List<Account>>(fullText);
                }

            System.Console.WriteLine("Bem vindo ao nosso banco! \n\n");
            var operationType = 0;

            do
            {
                System.Console.WriteLine("1 - Abrir conta \n" +
                    "2 - Consultar Contas\n" +
                    "3 - Abrir pedido de empréstimo\n" +
                    "4 - Sair");

                var userInput = System.Console.ReadLine();


                Int32.TryParse(userInput, out operationType);

                if (operationType <= 0 || operationType > 4)
                {
                    System.Console.WriteLine("Operação inválida! A aplicação será finalizada...\n");
                    System.Console.ReadLine();

                    System.Console.WriteLine(next);
                }
                else if (operationType == 1)
                {
                    var account = new Account();

                    System.Console.WriteLine("Iniciando processo de abertura de conta. Por favor, responda abaixo: \n\n");

                    System.Console.WriteLine("Qual o seu primeiro nome? \n");
                    account.FirstName = System.Console.ReadLine();
                    System.Console.WriteLine(next);

                    System.Console.WriteLine("Qual o seu sobrenome? \n");
                    account.LastName = System.Console.ReadLine();
                    System.Console.WriteLine(next);

                    System.Console.WriteLine("Qual o valor inicial para depósito? \n");
                    var accountMoney = 0;
                    Int32.TryParse(System.Console.ReadLine(), out accountMoney);
                    account.Money = accountMoney;

                    System.Console.WriteLine(next);

                    accounts.Add(account);
                }
                else if (operationType == 2)
                {
                    var count = 1;
                    foreach (var account in accounts)
                    {
                        System.Console.WriteLine($"Nome: {account.FirstName} {account.LastName} | Saldo: {account.Money}");
                        count++;
                    }

                    System.Console.WriteLine(next);
                }
            } while (operationType != 4);


            var savedAccounts = JsonConvert.SerializeObject(accounts);

            if (File.Exists(filename))
                File.Delete(filename);
    
            using (FileStream fs = File.Create(filename))
            { 
                Byte[] title = new UTF8Encoding(true).GetBytes(savedAccounts);
                fs.Write(title, 0, title.Length);
            }
        }
    }

    public class Account
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Money { get; set; }
    }
}
