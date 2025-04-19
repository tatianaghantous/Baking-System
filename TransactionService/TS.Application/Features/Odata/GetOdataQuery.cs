using Foxera.Keycloak.Contracts;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Contracts.Persistence;
using TS.Domain.Entities;

namespace TS.Persistence.Features.Odata;

public class GetOdataQuery : IRequest<IQueryable>
{
    public Type Type { get; set; }
}

 public class GetOdataQueryHandler(ITransactionsDbContext context, ICurrentUser _currentUser)
     : IRequestHandler<GetOdataQuery, IQueryable>
 {
    
     public async Task<IQueryable> Handle(GetOdataQuery request, CancellationToken cancellationToken)
     {
         var query= (IQueryable)context.GetType().GetMethods()
             .FirstOrDefault(x => x.Name == nameof(DbContext.Set) && x.GetParameters().Length == 0)?
             .MakeGenericMethod(request.Type)
             .Invoke(context, null)!;
    
    
         // Apply additional filtering for Accounts type
         if (request.Type == typeof(Domain.Entities.Account))
         {
             var predicate = PredicateBuilder.New<Domain.Entities.Account>( true);
        
             if (_currentUser.IsInRole("Customer"))//a customer can see only his accounts of all branches
             {
                 predicate = predicate.And(x => x.Guid == _currentUser.Id);
             }

             query = ((IQueryable<Domain.Entities.Account>)query).Where(predicate);
         }

         return query;
     }
 }