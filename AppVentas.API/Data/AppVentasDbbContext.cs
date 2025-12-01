using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppVentas.Modelos;

    public class AppVentasDbbContext : DbContext
    {
        public AppVentasDbbContext (DbContextOptions<AppVentasDbbContext> options)
            : base(options)
        {
        }

        public DbSet<AppVentas.Modelos.Product> Products { get; set; } = default!;

        public DbSet<AppVentas.Modelos.Order> Orders { get; set; } = default!;

        public DbSet<AppVentas.Modelos.User> Users { get; set; } = default!;
    }
