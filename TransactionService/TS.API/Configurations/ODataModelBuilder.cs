using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using TS.Domain.Entities;

namespace TS.API.Configurations;

public class ODataModelBuilder
{
    public IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();

        //MAke sure Ykun el esem abel el controller huwwer zeito!!!
        builder.EntitySet<Account>(nameof(Account));
        builder.EntitySet<Currency>(nameof(Currency));
        builder.EntitySet<Transaction>(nameof(Transaction));
        builder.EntitySet<Transactiontype>(nameof(Transactiontype));
        builder.EntitySet<RecurrentTransaction>(nameof(RecurrentTransaction));
        
        return builder.GetEdmModel();
    }
}