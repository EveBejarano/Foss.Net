using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hoteles.Models
{
    public class Estrellas
    {
        [Key]
        public string IDEstrellas { get; set; }
        public byte[] Imagen_estrella { get; set; }

        public ICollection<Hotel> Hoteles { get; set; }
    }
}
