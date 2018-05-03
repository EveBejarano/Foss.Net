using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hoteles.Models;
using Microsoft.EntityFrameworkCore;

namespace Hoteles.Data
{
    public class HotelContext : DbContext
    {
        public HotelContext(DbContextOptions<HotelContext> options) : base(options)
        {
        }

        public DbSet<Agente> Agente { get; set; }
        public DbSet<Agente_Reserva> Agente_Reserva { get; set; }
        public DbSet<Cadena_Hotel> Cadena_Hotel { get; set; }
        public DbSet<Estado_Reserva> Estado_Reserva { get; set; }
        public DbSet<Estrellas> Estrellas { get; set; }
        public DbSet<Habitacion> Habitacion { get; set; }
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Huesped> Huesped { get; set; }
        public DbSet<Pais> Pais { get; set; }
        public DbSet<Reserva> Reserva { get; set; }
        public DbSet<Tipo_Hab> Tipo_Hab { get; set; }

    }
}

