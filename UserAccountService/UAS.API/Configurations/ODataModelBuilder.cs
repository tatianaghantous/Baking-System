using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using UAS.Domain.Entities;

namespace UAS.API.Configurations;

public class ODataModelBuilder
{
    public IEdmModel GetEdmModel()
    {
        var builder = new ODataConventionModelBuilder();

        //MAke sure Ykun el esem abel el controller huwwer zeito!!!
        builder.EntitySet<Account>(nameof(Account));
        builder.EntitySet<Branch>(nameof(Branch));
        builder.EntitySet<Location>(nameof(Location));
        builder.EntitySet<Role>(nameof(Role));
        builder.EntitySet<User>(nameof(User));
        
        return builder.GetEdmModel();
    }
}
