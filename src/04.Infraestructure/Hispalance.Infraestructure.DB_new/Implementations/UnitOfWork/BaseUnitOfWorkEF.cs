using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Hispalance.Infraestructure.DB.UnitOfWork
{
    public abstract class BaseUnitOfWorkEF : IDisposable
    {
        private TransactionScope transactionScope;

     //   private DbContextTransaction transaction { get; set; }

        protected DbContext _context;

        public BaseUnitOfWorkEF(DbContext context)
        {
            _context = context;
        }

        
        public void StartTransactionScope()
        {
            transactionScope = new TransactionScope();
        }

        public void CommitTransactionScope()
        {
            transactionScope.Complete();
            
        }

        //public void StartTransaction()
        //{
        //    transaction = ((DbContext)_context).Database.BeginTransaction();
        //}

        //public void CommitTransaction()
        //{
        //    transaction.Commit();
        //}

        //public void RollBackTransaction()
        //{
        //    transaction.Rollback();
        //}

        public void Commit()
        {
           _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            if (transactionScope != null)
                transactionScope.Dispose();

            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}