using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hoteles.Models
{
    public class Agente
    {
        [Key]
        public int IDAgente { get; set; }
        public string nombre_agente { get; set; }
        public string email_agente { get; set; }

        public ICollection<Agente_Reserva> Agente_Reservas { get; set; }
    }
}
