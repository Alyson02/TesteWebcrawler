using Core.Context;
using Core.Infrastructure.Repository;

namespace Core.Business.Services.Base
{
    public class ServiceBase<T> : ServiceBaseAbstract<T, TesteElawDbContext>, IServiceBase<T> where T : class
    {
        public ServiceBase(IUnitOfWork<TesteElawDbContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
