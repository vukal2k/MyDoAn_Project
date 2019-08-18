using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public abstract class AllResponsitory<T> : IResponsitory<T> where T : class
    {
        public  DbBlogData Context;
        private IDbSet<T> dbEntity;

        DbContextTransaction transaction;

        public AllResponsitory()
        {
            Context = new DbBlogData();
            dbEntity = Context.Set<T>();
        }
        public void Delete(int id)
        {
            T model = dbEntity.Find(id);
            dbEntity.Remove(model);
        }

        public IQueryable<T> GetAll(params string[] include)
        {
            IQueryable<T> entities = dbEntity;
            if (include != null)
            {
                for (int i = 0; i < include.Length; i++)
                {
                    entities = entities.Include(include[i]);
                }
            }
            return entities;
        }

        //public IQueryable<T> GetAll()
        //{
        //    return dbEntity;
        //}

        public T GetById(int id, params string[] include)
        {
            if (include != null)
            {
                for (int i = 0; i < include.Length; i++)
                {
                    dbEntity = (DbSet<T>) dbEntity.Include(include[i]);
                }
            }
            T model = dbEntity.Find(id);
            return model;
        }

        //public T GetById(int id)
        //{
        //    T model = dbEntity.Find(id);
        //    return model;
        //}


        public void BeginTransaction()
        {
            transaction= Context.Database.BeginTransaction();
        }

        public void RollBackTransaction()
        {
            transaction.Rollback();
        }

        public void CommitTransaction()
        {
            transaction.Commit();
        }

        public void Insert(T item)
        {
            dbEntity.Add(item);
        }
        
        public void Save()
        {
            Context.SaveChanges();
        }

        public void Update(T item)
        {
            dbEntity.AddOrUpdate(item);
        }
    }
}
