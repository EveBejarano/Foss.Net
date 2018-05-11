using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hoteles.Models
{
    public class Reserva
    {
        [Key]
        public int IDReserva { get; set; }
        public int IDAgenteReserva { get; set; }
        public string IDEstado { get; set; }
        public int IDHuesped { get; set; }
        public int IDHabitacion { get; set; }
        public DateTime fecha_desde { get; set; }
        public DateTime fecha_hasta { get; set; }

        public Habitacion Habitacion { get; set; }
        public Agente_Reserva Agente_Reserva { get; set; }
        public Estado_Reserva Estado_Reserva { get; set; }
        public Huesped Huesped { get; set; }
    }
}
