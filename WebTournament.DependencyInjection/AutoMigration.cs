﻿using Infrastructure.DataAccess.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DependencyInjection
{
    public static class AutoMigration
    {
        public static void AutoMigrateDatabase (this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                 dbContext.Database.Migrate();
            }
        }
    }
}
