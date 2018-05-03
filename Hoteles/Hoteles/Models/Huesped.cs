using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hoteles.Models
{
    public class Huesped
    {
        [Key]
        public int IDHuesped { get; set; }
        public string nombre_huesped { get; set; }
        public string direccion_huesped { get; set; }
        public string email_huesped { get; set; }

        public ICollection<Reserva> Reservas { get; set; }
    }
}
