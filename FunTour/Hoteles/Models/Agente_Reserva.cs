using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hoteles.Models
{
    public class Agente_Reserva
    {
        [Key]
        public int IDAgente_Reserva { get; set; }
        public int IDAgente { get; set; }
        public int total_huespedes { get; set; }
        public DateTime fecha_reserva { get; set; }

        public ICollection<Reserva> Reserva { get; set; }
        public Agente Agente { get; set; }
    }
}
