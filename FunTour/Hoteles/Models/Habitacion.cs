using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hoteles.Models
{
    public class Habitacion
    {
        [Key]
        public int IDHabitacion { get; set; }
        public int IDHotel { get; set; }
        public string IDTipohab { get; set; }
        public string piso_hab { get; set; }
        public string numero_hab { get; set; }
        public string detalles_hab { get; set; }

        public Hotel Hotel { get; set; }
        public Tipo_Hab Tipo_Hab { get; set; }
        public ICollection<Reserva> Reserva { get; set; }
    }
}
