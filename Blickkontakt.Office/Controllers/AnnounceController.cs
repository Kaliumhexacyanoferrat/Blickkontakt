using GenHTTP.Api.Content;
using GenHTTP.Modules.Placeholders;

namespace Blickkontakt.Office.Controllers
{

    public class AnnounceController
    {

        public IHandlerBuilder Index()
        {
            return Page.From("Announces", "This is the announces page");
        }

    }

}
