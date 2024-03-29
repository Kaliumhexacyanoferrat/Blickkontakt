﻿using System;

using Blickkontakt.Office.Model;

using EvolveDb;
using Npgsql;

namespace Blickkontakt.Office.Infrastructure
{

    public static class Migrations
    {

        public static void Perform()
        {
            using var connection = new NpgsqlConnection(Database.ConnectionString);

            var evolve = new Evolve(connection, msg => Console.WriteLine(msg))
            {
                Locations = new[] { "Schema" },
                IsEraseDisabled = true
            };

            evolve.Migrate();
        }

    }

}
