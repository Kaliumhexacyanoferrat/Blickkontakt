using GenHTTP.Api.Content;
using GenHTTP.Api.Protocol;

using GenHTTP.Modules.Authentication;
using GenHTTP.Modules.IO;
using GenHTTP.Modules.Controllers;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.Websites;

using GenHTTP.Themes.AdminLTE;

using Blickkontakt.Office.Controllers;
using Blickkontakt.Office.Infrastructure;

namespace Blickkontakt.Office
{

    public static class Project
    {

        public static IHandlerBuilder Create()
        {
            var auth = BasicAuthentication.Create(AccessControl.AuthenticateAsync);

            var resources = Resources.From(ResourceTree.FromAssembly("Resources"));

            var logo = Content.From(Resource.FromAssembly("eye.png"));

            var theme = Theme.Create()
                             .Title("Blickkontakt")
                             .FooterRight(RenderFooterRight)
                             .UserProfile(RenderUser)
                             .Logo(logo);

            var content = Layout.Create()
                                .Fallback(Controller.From<DashboardController>())
                                .AddController<CustomerController>("customers")
                                .AddController<AnnounceController>("announces")
                                .AddController<LetterController>("letters")
                                .AddController<AccountController>("accounts")
                                .Add("static", resources)
                                .Authentication(auth);

            var menu = Menu.Empty()
                           .Add("{website}", "Übersicht")
                           .Add("/customers/", "Kunden")
                           .Add("/announces/", "Anzeigen")
                           .Add("/letters/", "Infobriefe")
                           .Add("/accounts/", "Mitarbeiter");

            return Website.Create()
                          .Theme(theme)
                          .Content(content)
                          .Menu(menu)
                          .AddScript("jquery-validate.js", Resource.FromAssembly("jquery.validate.min.js"))
                          .AddStyle("project.css", Resource.FromAssembly("project.css"));
        }

        private static UserProfile? RenderUser(IRequest request, IHandler handler)
        {
            var user = request.GetUser<OfficeUser>();

            if (user != null)
            {
                return new UserProfile(user.DisplayName, "/static/user.png", $"/accounts/details/{user.User.ID}/");
            }

            return null;
        }

        private static string RenderFooterRight(IRequest request, IHandler handler)
        {
            return "view <a href=\"https://github.com/Kaliumhexacyanoferrat/blickkontakt\">on GitHub</a>";
        }

    }

}
