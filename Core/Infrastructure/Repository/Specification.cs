using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Repository
{
    public class Specification<T> : BaseSpecification<T>
    {
        public Specification(Expression<Func<T, bool>> criteria) : base(criteria) { }
        public Specification() : base() { }

    }
}
