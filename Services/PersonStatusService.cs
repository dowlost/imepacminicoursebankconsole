using System;
using System.Collections.Generic;
using System.Text;

namespace IMEPAC.Curso.Console.Services
{
    public interface IPersonStatusService
    {
        bool NameIsClear();

        bool IsForeign();

        bool HasCrimeHistory();
    }

    public class PersonStatusService : IPersonStatusService
    {
        private readonly string _firstname;
        private readonly string _lastname;

        public PersonStatusService(string firstname, string lastname)
        {
            _firstname = firstname;
            _lastname = lastname;
        }

        /// <summary>
        /// Checa se o cliente possui histórico de crimes
        /// </summary>
        /// <returns></returns>
        public bool HasCrimeHistory()
        {
            return false;
        }

        /// <summary>
        /// Checa se o cliente é estrangeiro
        /// </summary>
        /// <returns></returns>
        public bool IsForeign()
        {
            return false;
        }

        /// <summary>
        /// Checa se o cliente está com o nome limpo
        /// </summary>
        /// <returns></returns>
        public bool NameIsClear()
        {
            return true;
        }
    }
}
