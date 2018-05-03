using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hoteles.Models
{
    public class Cadena_Hotel
    {
        [Key]
        public string IDCadena { get; set; }
        public string nombre_cadena { get; set; }

        public ICollection<Hotel> Hoteles { get; set; }
    }
}
