using System;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using Blickkontakt.Office.Model;
using Blickkontakt.Office.ViewModels;

using GenHTTP.Api.Content;
using GenHTTP.Api.Content.Templating;
using GenHTTP.Api.Protocol;

using GenHTTP.Modules.Basics;
using GenHTTP.Modules.Controllers;
using GenHTTP.Modules.IO;
using GenHTTP.Modules.Razor;

namespace Blickkontakt.Office.Controllers
{

    public class AnnounceController
    {
        private const int PAGE_SIZE = 10;

        private const int START_NUMBER = 3000;

        public IHandlerBuilder Index(int page, string status)
        {
            if (page < 1) page = 1;

            using var context = Database.Create();

            IQueryable<Announce> query = context.Announces
                                                .Include(a => a.Customer);

            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<AnnounceStatus>(status, true, out var announceStatus))
                {
                    query = query.Where(a => a.Status == announceStatus);
                }
            }

            var total = query.Count();

            var records = query.Skip((page - 1) * PAGE_SIZE)
                               .Take(PAGE_SIZE)
                               .OrderByDescending(c => c.Number)
                               .ToList();

            var pages = (total + PAGE_SIZE - 1) / PAGE_SIZE;

            var paged = new PagedList<Announce>(records, page, pages, total);

            return ModRazor.Page(Resource.FromAssembly("Announce.List.cshtml"), (r, h) => new ViewModel<PagedList<Announce>>(r, h, paged))
                           .Title("Anzeigen");
        }

        public IHandlerBuilder? Details([FromPath] int number)
        {
            using var context = Database.Create();

            var announce = context.Announces
                                  .Include(a => a.Customer)
                                  .Where(c => c.Number == number)
                                  .FirstOrDefault();

            if (announce == null)
            {
                return null;
            }

            return ModRazor.Page(Resource.FromAssembly("Announce.Details.cshtml"), (r, h) => new ViewModel<Announce>(r, h, announce))
                           .Title("Anzeige");
        }

        public IHandlerBuilder? Create([FromPath] int number)
        {
            using var context = Database.Create();

            var customer = context.Customers
                                  .Where(c => c.Number == number)
                                  .FirstOrDefault();

            if (customer == null)
            {
                return null;
            }

            return ModRazor.Page(Resource.FromAssembly("Announce.Create.cshtml"), (r, h) => new ViewModel<Customer>(r, h, customer))
                           .Title("Neue Anzeige");
        }

        [ControllerAction(RequestMethod.POST)]
        public IHandlerBuilder? Create([FromPath] int number, Announce announce)
        {
            using var context = Database.Create();

            var customer = context.Customers
                                  .Where(c => c.Number == number)
                                  .FirstOrDefault();

            if (customer == null)
            {
                return null;
            }

            if (announce.Number == 0)
            {
                var highest = context.Announces.Max(c => (int?)c.Number);

                announce.Number = (highest != null) ? highest.Value + 1 : START_NUMBER;
            }

            announce.Customer = customer;
            announce.Status = AnnounceStatus.New;

            announce.Notes = OrNull(announce.Notes);
            announce.Message = OrNull(announce.Message);
            announce.Title = OrNull(announce.Title);

            announce.Created = DateTime.UtcNow;
            announce.Modified = DateTime.UtcNow;

            context.Announces.Add(announce);

            context.SaveChanges();

            return Redirect.To($"/announces/details/{announce.Number}/", true);
        }

        public IHandlerBuilder? Prepare([FromPath] int number)
        {
            using var context = Database.Create();

            var announce = context.Announces
                                  .Include(a => a.Customer)
                                  .Where(c => c.Number == number)
                                  .FirstOrDefault();

            if (announce == null)
            {
                return null;
            }

            announce.Status = AnnounceStatus.Prepared;

            context.SaveChanges();

            return Redirect.To("/announces/", true);
        }

        private static string? OrNull(string? value)
        {
            return (!string.IsNullOrWhiteSpace(value)) ? value.Trim() : null;
        }

    }

}
