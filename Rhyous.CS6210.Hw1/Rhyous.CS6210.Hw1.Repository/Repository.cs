using Rhyous.CS6210.Hw1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rhyous.CS6210.Hw1.Repository
{
    public class Repository<T> : IRepository<T>, IDisposable
        where T : class, IEntity
    {
        GenericDbContext<T> Context = new GenericDbContext<T>();

        public T Create(T entity)
        {
            var addedRecord = Context.Entities.Add(entity);
            Context.SaveChanges();
            return addedRecord;
        }

        public IEnumerable<T> Create(IEnumerable<T> entities)
        {
            var addedRecords = Context.Entities.AddRange(entities);
            Context.SaveChanges();
            return addedRecords;
        }

        public bool Delete(T entity)
        {
            var removedRecord = Context.Entities.Remove(entity);
            Context.SaveChanges();
            return removedRecord != null;
        }

        public IQueryable<T> Read()
        {
            return Context.Entities;
        }

        public T Read(int id)
        {
            return Context.Entities.FirstOrDefault(e => e.Id == id);
        }

        public T Update(T entity)
        {
            var existingEntity = Context.Entities.First(f => f.Id == entity.Id);
            foreach (var propInfo in typeof(T).GetProperties())
            {
                propInfo.SetValue(existingEntity, propInfo.GetValue(entity, null));
            }
            Context.SaveChanges();
            return existingEntity;
        }

        #region IDisposable

        private bool _IsDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_IsDisposed)
            {
                _IsDisposed = true;
                if (disposing)
                {
                    Context.Dispose();
                }
            }
        }

        #endregion
    }
}