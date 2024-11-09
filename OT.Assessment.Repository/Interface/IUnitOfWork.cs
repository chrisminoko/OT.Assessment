using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Repository.Interface
{
    public interface IUnitOfWork
    {
        ICommandRepository Commands { get; }
        IQueryRepository Queries { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
