using GenHTTP.Api.Content;

using GenHTTP.Modules.Placeholders;

namespace Blickkontakt.Office.Controllers
{
    public class AccountController
    {

        public IHandlerBuilder Index()
        {
            return Page.From("Accounts", "This is the accounts page");
        }

    }

}
