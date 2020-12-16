using System;
using System.Linq;

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

    public class CustomerController
    {

        private const int START_NUMBER = 3000;

        private const int PAGE_SIZE = 10;

        public IHandlerBuilder Index(int page, string _)
        {
            if (page < 1) page = 1;

            using var context = Database.Create();

            var records = context.Customers
                                 .Skip((page - 1) * PAGE_SIZE)
                                 .Take(PAGE_SIZE)
                                 .OrderByDescending(c => c.Number)
                                 .ToList();

            var total = context.Customers.Count();

            var pages = (total + PAGE_SIZE - 1) / PAGE_SIZE;

            var paged = new PagedList<Customer>(records, page, pages, total);

            return ModRazor.Page(Resource.FromAssembly("Customer.List.cshtml"), (r, h) => new ViewModel<PagedList<Customer>>(r, h, paged))
                           .Title("Kunden");
        }

        public IHandlerBuilder Create()
        {
            return ModRazor.Page(Resource.FromAssembly("Customer.Creation.cshtml"))
                           .Title("Kunden anlegen");
        }

        [ControllerAction(RequestMethod.POST)]
        public IHandlerBuilder Create(Customer customer)
        {
            using var context = Database.Create();

            if (customer.Number == 0)
            {
                var highest = context.Customers.Max(c => (int?)c.Number);

                customer.Number = (highest != null) ? highest.Value + 1 : START_NUMBER;
            }

            if (string.IsNullOrWhiteSpace(customer.FirstName))
            {
                customer.FirstName = null;
            }

            customer.Created = DateTime.UtcNow;
            customer.Modified = DateTime.UtcNow;

            context.Customers.Add(customer);

            context.SaveChanges();

            return Redirect.To($"{{controller}}/details/{customer.Number}/", true);
        }

        public IHandlerBuilder? Details([FromPath] int number)
        {
            using var context = Database.Create();

            var customer = context.Customers
                                  .Where(c => c.Number == number)
                                  .FirstOrDefault();

            if (customer == null)
            {
                return null;
            }

            return ModRazor.Page(Resource.FromAssembly("Customer.Details.cshtml"), (r, h) => new ViewModel<Customer>(r, h, customer))
                           .Title($"{customer.FirstName} {customer.Name}");
        }

        public IHandlerBuilder? Edit([FromPath] int number)
        {
            using var context = Database.Create();

            var customer = context.Customers
                                  .Where(c => c.Number == number)
                                  .FirstOrDefault();

            if (customer == null)
            {
                return null;
            }

            return ModRazor.Page(Resource.FromAssembly("Customer.Editor.cshtml"), (r, h) => new ViewModel<Customer>(r, h, customer))
                           .Title($"{customer.FirstName} {customer.Name}");
        }

        [ControllerAction(RequestMethod.POST)]
        public IHandlerBuilder? Edit([FromPath] int number, Customer customer)
        {
            using var context = Database.Create();

            var existing = context.Customers
                                  .Where(c => c.Number == number)
                                  .FirstOrDefault();

            if (existing == null)
            {
                return null;
            }

            existing.Number = customer.Number;

            existing.Name = customer.Name;
            existing.FirstName = OrNull(customer.FirstName);
            existing.Phone = OrNull(customer.Phone);
            existing.Mail = OrNull(customer.Mail);
            existing.Street = OrNull(customer.Street);
            existing.Zip = OrNull(customer.Zip);
            existing.City = OrNull(customer.City);

            existing.Modified = DateTime.UtcNow;

            context.SaveChanges();

            return Redirect.To($"{{controller}}/details/{customer.Number}/", true);
        }

        public IHandlerBuilder? Delete([FromPath] int number)
        {
            using var context = Database.Create();

            var customer = context.Customers
                                  .Where(c => c.Number == number)
                                  .FirstOrDefault();

            if (customer == null)
            {
                return null;
            }

            context.Remove(customer);

            context.SaveChanges();

            return Redirect.To("{controller}/", true);
        }

        private static string? OrNull(string? value)
        {
            return (!string.IsNullOrWhiteSpace(value)) ? value : null;
        }

    }

}
