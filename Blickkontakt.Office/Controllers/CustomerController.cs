using GenHTTP.Api.Content;
using GenHTTP.Modules.Placeholders;

namespace Blickkontakt.Office.Controllers
{

    public class CustomerController
    {

        public IHandlerBuilder Index()
        {
            return Page.From("Customers", "This is the customers page");
        }

    }

}
