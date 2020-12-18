using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using GenHTTP.Api.Content.Authentication;
using GenHTTP.Api.Protocol;
using GenHTTP.Modules.Authentication;

using Blickkontakt.Office.Model;
using GenHTTP.Api.Content;

namespace Blickkontakt.Office.Infrastructure
{

    public static class AccessControl
    {
        private const string SALT = "faa39358-3e18-11eb-b378-0242ac130002";

        public static ValueTask<IUser?> AuthenticateAsync(string username, string password)
        {
            var hash = Hash(password);

            using var context = Database.Create();

            var query = from u in context.Users
                        where u.Name == username
                        select u;

            var found = query.FirstOrDefault();

            if (found != null)
            {
                if (found.Active && (found.Password == hash))
                {
                    return new ValueTask<IUser?>(new OfficeUser(found));
                }
            }

            return new ValueTask<IUser?>();
        }

        public static string Hash(string value)
        {
            using var sha256Hash = SHA256.Create();

            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(SALT + value));

            var builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }

        public static Account GetAccount(this IRequest request)
        {
            return request.GetUser<OfficeUser>()?.User ?? throw new ProviderException(ResponseStatus.Unauthorized, "Unable to retrieve the logged in user");
        }

    }

}
