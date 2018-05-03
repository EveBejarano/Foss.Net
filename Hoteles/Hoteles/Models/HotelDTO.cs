using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hoteles.Models
{
    public class HotelDTO
    {
        public int IDHotel { get; set; }
        public string nombre_hotel { get; set; }
        public string direccion_hotel { get; set; }
        public string ciudad_hotel { get; set; }
        public string nombre_pais { get; set; }
        public byte[] Imagen_estrella { get; set; }

    }
}
