using GenHTTP.Api.Content;

using GenHTTP.Modules.Placeholders;

namespace Blickkontakt.Office.Controllers
{
    
    public class LetterController
    {

        public IHandlerBuilder Index()
        {
            return Page.From("Letters", "This is the letters page");
        }

    }

}
