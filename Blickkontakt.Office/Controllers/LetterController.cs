using System.Linq;

using Microsoft.EntityFrameworkCore;

using GenHTTP.Api.Content;
using GenHTTP.Api.Content.Templating;
using GenHTTP.Modules.IO;
using GenHTTP.Modules.Razor;

using Blickkontakt.Office.Model;
using Blickkontakt.Office.ViewModels;

namespace Blickkontakt.Office.Controllers
{

    #region View Models

    public record LetterListEntry(Letter Letter, int Announces);

    #endregion

    public class LetterController
    {

        private const int PAGE_SIZE = 10;

        public IHandlerBuilder Index(int page)
        {
            if (page < 1) page = 1;

            using var context = Database.Create();

            IQueryable<Letter> query = context.Letters
                                              .Include(l => l.Announces);

            var total = query.Count();

            var records = query.Skip((page - 1) * PAGE_SIZE)
                               .Take(PAGE_SIZE)
                               .OrderByDescending(l => l.Created)
                               .Select(l => new LetterListEntry(l, l.Announces.Count))
                               .ToList();

            var pages = (total + PAGE_SIZE - 1) / PAGE_SIZE;

            var paged = new PagedList<LetterListEntry>(records, page, pages, total);

            return ModRazor.Page(Resource.FromAssembly("Letter.List.cshtml"), (r, h) => new ViewModel<PagedList<LetterListEntry>>(r, h, paged))
                           .Title("Infobriefe");
        }

    }

}
