using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<T> CreateQuery<T>(IQueryable<T> inputQuery , ISpecifications<T> specifications) where T : class
        {
            //[input query => _storeDbContext.Set<TEntity>()].Where(specifications.Criteria).Include(specifications.IncludeExpressions[])
            var query = inputQuery;

            if(specifications.Criteria is not null) // if filter exits
               query = query.Where(specifications.Criteria);

            if(specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);
            else if(specifications.OrderByDesc is not null)
                query=query.OrderByDescending(specifications.OrderByDesc);

            if(specifications.IsPaginated)
                query = query.Skip(specifications.Skip).Take(specifications.Take);

            //foreach (var item in specifications.IncludeExpressions)
            //    query.Include(item);

            query = specifications.IncludeExpressions
                .Aggregate(query, (currentQuery, include) => currentQuery.Include(include));

            return query;
        }
    }
}
