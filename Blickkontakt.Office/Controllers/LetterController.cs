using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using Microsoft.EntityFrameworkCore;

using GenHTTP.Api.Content;
using GenHTTP.Api.Content.Templating;
using GenHTTP.Api.Protocol;
using GenHTTP.Modules.Basics;
using GenHTTP.Modules.Controllers;
using GenHTTP.Modules.IO;
using GenHTTP.Modules.Razor;

using Blickkontakt.Office.Model;
using Blickkontakt.Office.ViewModels;

namespace Blickkontakt.Office.Controllers
{

    #region View Models

    public record LetterListEntry(Letter Letter, int Announces);

    public record LetterDetail(Letter Letter, List<Announce> Announces, List<Announce> AvailableAnnounces);

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

        public IHandlerBuilder Create()
        {
            using var context = Database.Create();

            var announces = context.Announces
                                   .Include(a => a.Customer)
                                   .Where(a => a.Status == AnnounceStatus.Prepared || a.Status == AnnounceStatus.Published)
                                   .OrderBy(a => a.Status)
                                   .ThenByDescending(a => a.ID)
                                   .Select(a => a)
                                   .ToList();

            return ModRazor.Page(Resource.FromAssembly("Letter.Create.cshtml"), (r, h) => new ViewModel<List<Announce>>(r, h, announces))
                           .Title("Infobrief erstellen");
        }

        [ControllerAction(RequestMethod.POST)]
        public IHandlerBuilder Create(string title, IRequest request)
        {
            using var context = Database.Create();

            var letter = new Letter()
            {
                Title = title
            };

            letter.Status = LetterStatus.New;

            letter.Created = DateTime.UtcNow;
            letter.Modified = DateTime.UtcNow;

            context.Letters.Add(letter);

            var announces = ExtractAnnounces(request, context);

            var order = 0;

            foreach (var announce in announces)
            {
                var letterAnnounce = new LetterAnnounce()
                {
                    Letter = letter,
                    Announce = announce,
                    Order = order++
                };

                context.LetterAnnounces.Add(letterAnnounce);
            }

            context.SaveChanges();

            return Redirect.To("/letters/", true);
        }

        public IHandlerBuilder? Details([FromPath] int id)
        {
            using var context = Database.Create();

            var letter = context.Letters
                                .Where(c => c.ID == id)
                                .FirstOrDefault();

            if (letter == null)
            {
                return null;
            }

            var announces = context.LetterAnnounces
                                   .Include(la => la.Announce)
                                   .Include(la => la.Announce.Customer)
                                   .Where(la => la.Letter == letter)
                                   .OrderBy(la => la.Order)
                                   .Select(la => la.Announce)
                                   .ToList();

            var availableAnnounces = context.Announces
                                            .Include(a => a.Customer)
                                            .Where(a => !announces.Contains(a))
                                            .Where(a => a.Status == AnnounceStatus.Prepared || a.Status == AnnounceStatus.Published)
                                            .OrderBy(a => a.Status)
                                            .ThenByDescending(a => a.ID)
                                            .Select(a => a)
                                            .ToList();

            var viewModel = new LetterDetail(letter, announces.ToList(), availableAnnounces);

            return ModRazor.Page(Resource.FromAssembly("Letter.Details.cshtml"), (r, h) => new ViewModel<LetterDetail>(r, h, viewModel))
                           .Title($"Infobrief {letter.Title}");
        }

        public IHandlerBuilder? Edit([FromPath] int id)
        {
            using var context = Database.Create();

            var letter = context.Letters
                                .Include(l => l.Announces)
                                .Where(c => c.ID == id)
                                .FirstOrDefault();

            if (letter == null)
            {
                return null;
            }

            return ModRazor.Page(Resource.FromAssembly("Letter.Editor.cshtml"), (r, h) => new ViewModel<Letter>(r, h, letter))
                           .Title($"Infobrief bearbeiten");
        }

        [ControllerAction(RequestMethod.POST)]
        public IHandlerBuilder? Edit([FromPath] int id, string title)
        {
            using var context = Database.Create();

            var letter = context.Letters
                                .Include(l => l.Announces)
                                .Where(c => c.ID == id)
                                .FirstOrDefault();

            if (letter == null)
            {
                return null;
            }

            letter.Title = title;

            context.SaveChanges();

            return Redirect.To($"/letters/details/{id}/", true);
        }

        public IHandlerBuilder? Delete([FromPath] int id)
        {
            using var context = Database.Create();

            var letter = context.Letters
                                .Where(c => c.ID == id)
                                .FirstOrDefault();

            if (letter == null)
            {
                return null;
            }

            context.Letters.Remove(letter);

            context.SaveChanges();

            return Redirect.To("/letters/");
        }

        [ControllerAction(RequestMethod.POST)]
        public IHandlerBuilder? RemoveAnnounces([FromPath] int id, IRequest request)
        {
            using var context = Database.Create();

            var announces = ExtractAnnounces(request, context);

            var letterAnnounces = context.LetterAnnounces
                                         .Where(la => la.Letter.ID == id)
                                         .Where(la => announces.Contains(la.Announce))
                                         .Select(la => la)
                                         .ToList();

            context.LetterAnnounces.RemoveRange(letterAnnounces);

            context.SaveChanges();

            return Redirect.To($"/letters/details/{id}/", true);
        }

        [ControllerAction(RequestMethod.POST)]
        public IHandlerBuilder? AddAnnounces([FromPath] int id, IRequest request)
        {
            using var context = Database.Create();

            var letter = context.Letters
                                .Include(l => l.Announces)
                                .Where(c => c.ID == id)
                                .FirstOrDefault();

            if (letter == null)
            {
                return null;
            }

            var highest = letter.Announces.Max(c => (int?)c.Order);

            int newOrder = (highest != null) ? (int)highest + 1 : 0;

            var announces = ExtractAnnounces(request, context);

            foreach (var announce in announces)
            {
                var letterAnnounce = new LetterAnnounce()
                {
                    Letter = letter,
                    Announce = announce,
                    Order = newOrder++
                };

                context.LetterAnnounces.Add(letterAnnounce);
            }

            context.SaveChanges();

            return Redirect.To($"/letters/details/{id}/", true);
        }

        public IHandlerBuilder? Publish([FromPath] int id)
        {
            using var context = Database.Create();

            var letter = context.Letters
                                .Where(c => c.ID == id)
                                .FirstOrDefault();

            if (letter == null)
            {
                return null;
            }

            // mark all previous announces as expired ...
            var letterAnnounces = context.LetterAnnounces
                                         .Where(la => la.Letter == letter)
                                         .Select(la => la.Announce)
                                         .ToList();

            var previous = context.Announces
                                  .Where(a => a.Status == AnnounceStatus.Published)
                                  .Where(a => !letterAnnounces.Contains(a))
                                  .ToList();

            foreach (var announce in previous)
            {
                announce.Status = AnnounceStatus.Expired;
            }

            // publish the letter ...
            letter.Status = LetterStatus.Published;
            letter.Published = DateTime.UtcNow;

            // and it's announces ...
            foreach (var announce in letterAnnounces)
            {
                announce.Status = AnnounceStatus.Published;
            }

            context.SaveChanges();

            return Redirect.To($"/letters/details/{id}/", true);
        }

        private List<Announce> ExtractAnnounces(IRequest request, Database context)
        {
            using var reader = new StreamReader(request.Content!, leaveOpen: true);

            var content = reader.ReadToEnd();

            var query = HttpUtility.ParseQueryString(content);

            var ids = new List<int>();

            foreach (var key in query.AllKeys)
            {
                if (key != null && key.StartsWith("a_") && query[key] == "1")
                {
                    if (int.TryParse(key[2..], out var id))
                    {
                        ids.Add(id);
                    }
                }
            }

            var announces = context.Announces
                                   .Where(a => ids.Contains(a.Number))
                                   .Select(a => a)
                                   .OrderBy(a => a.Title)
                                   .ToList();

            return announces;
        }

    }

}
