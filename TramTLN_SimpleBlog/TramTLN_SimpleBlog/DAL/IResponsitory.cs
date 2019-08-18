using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    interface IResponsitory<T> where T : class
    {
        IQueryable<T> GetAll(params string[] include);
        //IQueryable<T> GetAll();
        void Insert(T item);
        void Update(T item);
        void Delete(int id);
        T GetById(int id, params string[] include);
        //T GetById(int id);
        void Save();
    }
}
