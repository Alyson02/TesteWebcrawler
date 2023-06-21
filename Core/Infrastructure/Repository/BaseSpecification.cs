using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.Infrastructure.Repository
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        protected BaseSpecification()
        {

        }
        public Expression<Func<T, bool>> Criteria { get; }
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; } = new List<string>();
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public Expression<Func<T, object>> GroupBy { get; private set; }

        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; } = false;

        public virtual BaseSpecification<T> AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
            return this;
        }

        public virtual BaseSpecification<T> AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
            return this;
        }

        public virtual BaseSpecification<T> ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
            return this;
        }

        public virtual BaseSpecification<T> ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
            return this;
        }

        public virtual BaseSpecification<T> ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
            return this;
        }

        public virtual BaseSpecification<T> ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
        {
            GroupBy = groupByExpression;
            return this;
        }
    }
}
