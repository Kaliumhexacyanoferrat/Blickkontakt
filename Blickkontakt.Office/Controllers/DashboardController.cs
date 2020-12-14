using GenHTTP.Api.Content;

using GenHTTP.Modules.Placeholders;

namespace Blickkontakt.Office.Controllers
{

    public class DashboardController
    {

        public IHandlerBuilder Index()
        {
            return Page.From("Dashboard", "This is the dashboard");
        }

    }

}
