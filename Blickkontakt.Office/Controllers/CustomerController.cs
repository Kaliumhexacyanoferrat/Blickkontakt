using Blickkontakt.Office.Model;
using GenHTTP.Api.Content;
using GenHTTP.Api.Content.Templating;
using GenHTTP.Modules.IO;
using GenHTTP.Modules.Placeholders;
using GenHTTP.Modules.Razor;
using System.Collections.Generic;
using System.Linq;

namespace Blickkontakt.Office.Controllers
{

    public class CustomerController
    {

        public IHandlerBuilder Index()
        {
            using var context = Database.Create();

            var customers = context.Customers.ToList();

            return ModRazor.Page(Resource.FromAssembly("CustomerList.cshtml"), (r, h) => new ViewModel<List<Customer>>(r, h, customers))
                           .Title("Kunden");
        }

    }

}
