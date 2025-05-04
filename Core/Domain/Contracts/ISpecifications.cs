using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ISpecifications<T> where T : class
    {
        //_storeDbContext.Set<T>().Select(Experssion<Func<T,Object>>)
        //_storeDbContext.Set<T>().Where(Experssion<Func<T,bool>>)
        Expression<Func<T, bool>> Criteria { get; }   // for filtering

        //Include
        List<Expression<Func<T, Object>>> IncludeExpressions { get; }    //for eager loading

        //_storeDbContext.Set<T>().Where(Specifications.Criteria).Include(IncludeExpressions[0])
    }
}
