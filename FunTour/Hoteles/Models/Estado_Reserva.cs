using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hoteles.Models
{
    public class Estado_Reserva
    {
        [Key]
        public string IDEstado { get; set; }
        public string descripcion_estado { get; set; }

        public ICollection<Reserva> Reservas { get; set; }
    }
}
