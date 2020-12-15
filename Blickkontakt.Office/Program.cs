using GenHTTP.Engine;
using GenHTTP.Modules.Practices;

using Blickkontakt.Office;
using Blickkontakt.Office.Infrastructure;

Migrations.Perform();

var project = Project.Create();

return Host.Create()
           .Handler(project)
           .Console()
           .Defaults(secureUpgrade: false, strictTransport: false)
           .Development()
           .Run();