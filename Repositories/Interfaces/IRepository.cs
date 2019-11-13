using IMEPAC.Curso.Console.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IMEPAC.Curso.Console.Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task Insert(T item);
        Task<T> GetById(string id);
        Task<List<T>> GetAll();
    }
}
