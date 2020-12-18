using System.Linq;

using GenHTTP.Api.Content;
using GenHTTP.Api.Content.Templating;
using GenHTTP.Modules.IO;
using GenHTTP.Modules.Razor;

using Blickkontakt.Office.Model;

namespace Blickkontakt.Office.Controllers
{

    #region View Models

    public record DashboardViewModel(int Customers, int AnnouncesToPrepare, int AnnouncesToPublish, int ActiveAnnounces,
                                     int ActiveAccounts);

    #endregion

    public class DashboardController
    {

        public IHandlerBuilder Index()
        {
            using var context = Database.Create();

            var model = new DashboardViewModel
            (
                Customers: context.Customers.Count(),
                AnnouncesToPrepare: context.Announces.Where(a => a.Status == AnnounceStatus.New).Count(),
                AnnouncesToPublish: context.Announces.Where(a => a.Status == AnnounceStatus.Prepared).Count(),
                ActiveAnnounces: context.Announces.Where(a => a.Status == AnnounceStatus.Published).Count(),
                ActiveAccounts: context.Accounts.Where(a => a.Active).Count()
            );

            return ModRazor.Page(Resource.FromAssembly("Dashboard.cshtml"), (r, h) => new ViewModel<DashboardViewModel>(r, h, model))
                           .Title("Übersicht");
        }

    }

}
