using System;
using System.Linq;
using Blickkontakt.Office.Infrastructure;
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

    #region View Models

    public record AccountList(Account User, PagedList<Account> List);

    public record AccountDetails(Account User, Account Account);

    #endregion

    public class AccountController
    {
        private const int PAGE_SIZE = 10;

        public IHandlerBuilder Index(int page, bool inactive, IRequest request)
        {
            if (page < 1) page = 1;

            using var context = Database.Create();

            IQueryable<Account> query = context.Accounts;

            if (!inactive)
            {
                query = query.Where(u => u.Active);
            }

            var total = query.Count();

            var records = query.Skip((page - 1) * PAGE_SIZE)
                               .Take(PAGE_SIZE)
                               .OrderBy(u => u.DisplayName)
                               .ToList();

            var pages = (total + PAGE_SIZE - 1) / PAGE_SIZE;

            var paged = new PagedList<Account>(records, page, pages, total);

            var user = AccessControl.GetAccount(request);
            
            var list = new AccountList(user, paged);

            return ModRazor.Page(Resource.FromAssembly("Account.List.cshtml"), (r, h) => new ViewModel<AccountList>(r, h, list))
                           .Title("Mitarbeiter");
        }

        public IHandlerBuilder Create()
        {
            return ModRazor.Page(Resource.FromAssembly("Account.Create.cshtml"))
                           .Title("Mitarbeiter anlegen");
        }

        [ControllerAction(RequestMethod.POST)]
        public IHandlerBuilder Create(Account account, IRequest request)
        {
            EnsureAdmin(request);

            using var context = Database.Create();

            account.Password = AccessControl.Hash(account.Password);
            account.Active = true;

            account.Created = DateTime.UtcNow;
            account.Modified = DateTime.UtcNow;

            context.Accounts.Add(account);

            context.SaveChanges();

            return Redirect.To($"/accounts/", true);
        }

        public IHandlerBuilder? Details([FromPath] int id, IRequest request)
        {
            var user = AccessControl.GetAccount(request);

            using var context = Database.Create();

            var account = context.Accounts
                                 .Where(c => c.ID == id)
                                 .FirstOrDefault();

            if (account == null)
            {
                return null;
            }

            var viewModel = new AccountDetails(user, account);

            return ModRazor.Page(Resource.FromAssembly("Account.Details.cshtml"), (r, h) => new ViewModel<AccountDetails>(r, h, viewModel))
                           .Title($"{account.DisplayName}");
        }

        public IHandlerBuilder? Edit([FromPath] int id, IRequest request)
        {
            var user = AccessControl.GetAccount(request);

            if (!user.Admin && !(user.ID == id))
            {
                throw new ProviderException(ResponseStatus.Forbidden, "Your are not allowed to edit this user.");
            }

            using var context = Database.Create();

            var account = context.Accounts
                                  .Where(c => c.ID == id)
                                  .FirstOrDefault();

            if (account == null)
            {
                return null;
            }

            var viewModel = new AccountDetails(user, account);

            return ModRazor.Page(Resource.FromAssembly("Account.Editor.cshtml"), (r, h) => new ViewModel<AccountDetails>(r, h, viewModel))
                           .Title($"{account.DisplayName}");
        }

        [ControllerAction(RequestMethod.POST)]
        public IHandlerBuilder? Edit([FromPath] int id, Account account, IRequest request)
        {
            var user = AccessControl.GetAccount(request);

            if (!user.Admin && !(user.ID == id))
            {
                throw new ProviderException(ResponseStatus.Forbidden, "Your are not allowed to edit this user.");
            }

            using var context = Database.Create();

            var existing = context.Accounts
                                  .Where(c => c.ID == id)
                                  .FirstOrDefault();

            if (existing == null)
            {
                return null;
            }

            existing.Name = account.Name.Trim();
            existing.DisplayName = account.DisplayName.Trim();

            if (user.Admin)
            {
                existing.Admin = account.Admin;
            }

            if (!string.IsNullOrEmpty(account.Password))
            {
                existing.Password = AccessControl.Hash(account.Password);
            }

            existing.Modified = DateTime.UtcNow;

            context.SaveChanges();

            return Redirect.To($"{{controller}}/details/{id}/", true);
        }
        
        public IHandlerBuilder? Deactivate([FromPath] int id, IRequest request)
        {
            var user = AccessControl.GetAccount(request);

            if (!user.Admin)
            {
                throw new ProviderException(ResponseStatus.Forbidden, "Your are not allowed to deactivate this user.");
            }

            using var context = Database.Create();

            var account = context.Accounts
                                 .Where(c => c.ID == id)
                                 .FirstOrDefault();

            if (account == null)
            {
                return null;
            }

            account.Active = false;

            context.SaveChanges();

            return Redirect.To($"{{controller}}/details/{id}/", true);
        }

        public IHandlerBuilder? Activate([FromPath] int id, IRequest request)
        {
            var user = AccessControl.GetAccount(request);

            if (!user.Admin)
            {
                throw new ProviderException(ResponseStatus.Forbidden, "Your are not allowed to activate this user.");
            }

            using var context = Database.Create();

            var account = context.Accounts
                                 .Where(c => c.ID == id)
                                 .FirstOrDefault();

            if (account == null)
            {
                return null;
            }

            account.Active = true;

            context.SaveChanges();

            return Redirect.To($"{{controller}}/details/{id}/", true);
        }

        private static void EnsureAdmin(IRequest request)
        {
            if (!request.GetAccount().Admin)
            {
                throw new ProviderException(ResponseStatus.Forbidden, "You are not allowed to perform this action");
            }
        }

    }

}
