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
        static void Main(string[] args)
        {
            ConsoleUtils.PrintMessage("Bem vindo ao nosso banco!");
            ConsoleUtils.Spacer();
            ConsoleUtils.Spacer();

            Operation currentOperation = Operation.Unknow;
            var operationService = OperationService.GetInstance();

            do
            {
                try
                {
                    var operations = OperationService.GetInstance().GetAllOperations();

                    foreach (var operation in operations)
                        ConsoleUtils.PrintMessage(operation.Value);

                    ConsoleUtils.Spacer();

                    currentOperation = (Operation)ConsoleUtils.GetIntegerInput("Digite o nro da operação: ");

                    if (!Enum.IsDefined(typeof(Operation), currentOperation))
                    {
                        ConsoleUtils.PrintMessage("Operação inválida");
                        ConsoleUtils.EndSection();
                        continue;
                    }

                    var runOperation = operationService.RunOperation(currentOperation);
                    runOperation.Wait();
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
    }

}
