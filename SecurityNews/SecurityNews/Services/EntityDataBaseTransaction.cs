using Microsoft.EntityFrameworkCore.Storage;
using SecurityNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecurityNews.Services
{
    public class EntityDataBaseTransaction: IEntityDataBaseTransaction
    {

        private readonly IDbContextTransaction _transaction;



        public EntityDataBaseTransaction(ApplicationDbContext context)
        {
            _transaction = context.Database.BeginTransaction();

            
        }



        public void Commit()
        {

            _transaction.Commit();
        }

        public void RollBack()
        {

            _transaction.Rollback();
        }

        public void Dispose()
        {

            _transaction.Dispose();
        }
    }
}
