using System;
using System.Collections.Generic;
using System.Text;

namespace IMEPAC.Curso.Console.Utilities
{
    public static class ConsoleUtils
    {
        private static string _endSectionMessage = "\n---------------------------------------------------------------------------------------------------------------\n\n";

        public static string GetInput(string message)
        {
            PrintMessage(message);
            var input = System.Console.ReadLine();
            EndSection();

            return input;
        }

        public static int GetIntegerInput(string message)
        {
            PrintMessage(message);

            var integerInput = 0;
            Int32.TryParse(System.Console.ReadLine(), out integerInput);

            EndSection();

            return integerInput;
        }

        public static void PrintMessage(string message)
        {
            System.Console.WriteLine(message);
        }

        public static void Spacer()
        {
            System.Console.WriteLine(string.Empty);
        }

        public static void EndSection()
        {
            System.Console.WriteLine(_endSectionMessage);
        }
    }
}
