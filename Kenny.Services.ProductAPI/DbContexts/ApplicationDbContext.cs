﻿using Microsoft.EntityFrameworkCore;

namespace Kenny.Services.ProductAPI.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        const int abc = 2;       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
