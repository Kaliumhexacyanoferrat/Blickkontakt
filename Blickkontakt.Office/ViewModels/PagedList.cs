using System.Collections.Generic;

namespace Blickkontakt.Office.ViewModels
{

    public record PagedList<T>(List<T> Records, int CurrentPage, int PageCount, int Total);

}
