using IMEPAC.Curso.Console.Entities;
using IMEPAC.Curso.Console.Repositories.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMEPAC.Curso.Console.Repositories
{
    /// <summary>
    /// Repositório para as contas bancárias dos usuários
    /// </summary>
    public class AccountRepository : IRepository<Account>
    {

        private List<Account> _repo;
        private readonly string filename = Path.Combine(Path.GetTempPath(), "impec_curso_accounts.json");

        /// <summary>
        /// Ao instanciar esse repositório, as contas existentes serão carregadas para memória
        /// </summary>
        public AccountRepository()
        {
            _repo = new List<Account>();

            if (File.Exists(filename))
                using (StreamReader sr = File.OpenText(filename))
                {
                    string fullText = "";
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                        fullText += s;

                    if (!string.IsNullOrEmpty(fullText))
                        _repo = JsonConvert.DeserializeObject<List<Account>>(fullText);
                }
        }

        /// <summary>
        /// Retorna a conta bancário com o id informado.
        /// </summary>
        /// <param name="id">O id deve ser composto com o nome + sobrenome(sem espaços) do dono da conta bancária</param>
        /// <returns></returns>
        public async Task<Account> GetById(string id)
        {
            return _repo
                .Where(x => x.Id == id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Salva a conta bancária informada
        /// </summary>
        /// <param name="item">Objeto com os dados da conta bancária</param>
        /// <returns></returns>
        public async Task Insert(Account item)
        {
            item.Id = string.Concat(item.FirstName.ToLower(), item.LastName.ToLower());
            _repo.Add(item);
            Save();
        }

        /// <summary>
        /// Salva os dados contidos em memória para um arquivo json
        /// </summary>
        private void Save()
        {
            var toSave = JsonConvert.SerializeObject(_repo);

            //se o arquivo existe, será deletado
            if (File.Exists(filename))
                File.Delete(filename);

            //gravando o arquivo 
            using (FileStream fs = File.Create(filename))
            {
                Byte[] repoBytes = new UTF8Encoding(true).GetBytes(toSave);
                fs.Write(repoBytes, 0, repoBytes.Length);
            }
        }

        public async Task<List<Account>> GetAll()
        {
            return _repo;
        }
    }
}
