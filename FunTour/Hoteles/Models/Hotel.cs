using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hoteles.Models
{
    public class Hotel
    {
        [Key]
        public int IDHotel { get; set; }
        public string IDCadena { get; set; }
        public string IDPais { get; set; }
        public string IDEstrellas { get; set; }
        public string nombre_hotel { get; set; }
        public string direccion_hotel { get; set; }
        public string email_hotel { get; set; }
        public string sitioweb_hotel { get; set; }
        public string detalles_hotel { get; set; }
        public string ciudad_hotel { get; set; }

        public Cadena_Hotel Cadena_Hotel { get; set; }
        public Estrellas Estrellas { get; set; }
        public Pais Pais { get; set; }
        public ICollection<Habitacion> Habitaciones { get; set; }
    }
}
