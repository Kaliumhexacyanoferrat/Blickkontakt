using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using GenHTTP.Api.Content.Authentication;

using Blickkontakt.Office.Model;

namespace Blickkontakt.Office.Infrastructure
{

    public static class AccessControl
    {
        private const string SALT = "faa39358-3e18-11eb-b378-0242ac130002";

        public static async ValueTask<IUser?> AuthenticateAsync(string username, string password)
        {
            var hash = await HashAsync(password);

            using var context = Database.Create();

            var query = from u in context.Users
                        where u.Name == username
                        select u;

            var found = query.FirstOrDefault();

            if (found != null)
            {
                if (found.Password == hash)
                {
                    return new OfficeUser(found);
                }
            }

            return null;
        }

        private static async ValueTask<string> HashAsync(string value)
        {
            using var sha256Hash = SHA256.Create();

            using var input = new MemoryStream(Encoding.UTF8.GetBytes(SALT + value));

            var bytes = await sha256Hash.ComputeHashAsync(input);

            var builder = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }

    }

}
