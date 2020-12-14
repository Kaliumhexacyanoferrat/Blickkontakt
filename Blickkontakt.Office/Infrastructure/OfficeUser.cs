using GenHTTP.Api.Content.Authentication;

using Blickkontakt.Office.Model;

namespace Blickkontakt.Office.Infrastructure
{

    public class OfficeUser : IUser
    {

        #region Get-/Setters

        public Account User { get; }

        public string DisplayName => User.DisplayName ?? User.Name ?? $"User #{User.ID}";

        #endregion

        #region Initialization

        public OfficeUser(Account user)
        {
            User = user;
        }

        #endregion

    }

}
