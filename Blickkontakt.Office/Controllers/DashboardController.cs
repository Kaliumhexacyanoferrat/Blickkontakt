using System.Linq;

using GenHTTP.Api.Content;
using GenHTTP.Api.Content.Templating;
using GenHTTP.Modules.IO;
using GenHTTP.Modules.Razor;

using Blickkontakt.Office.Model;

namespace Blickkontakt.Office.Controllers
{

    #region View Models

    public record DashboardViewModel(int Customers);

    #endregion

    public class DashboardController
    {

        public IHandlerBuilder Index()
        {
            using var context = Database.Create();

            var model = new DashboardViewModel
            (
                Customers: context.Customers.Count()
            );

            return ModRazor.Page(Resource.FromAssembly("Dashboard.cshtml"), (r, h) => new ViewModel<DashboardViewModel>(r, h, model))
                           .Title("Übersicht");
        }

    }

}
